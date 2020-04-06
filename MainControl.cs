using Advanced_Combat_Tracker;
using FFXIV_ACT_Plugin.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.EnterpriseServices.Internal;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace ACT.DFAssist
{
	public partial class MainControl : UserControl, IActPluginV1
	{
		#region 변수
		//
		private Mesg.Locale _localeUi;
		private Mesg.Locale _localeGame;

		//
		private bool _isFormLoaded;
		private bool _isInActInit;
		private bool _isPluginEnabled;
		private bool _isLockFates;
		private bool _isInitSetting;

		private Label _actLabelStatus;
		private TabPage _actTabPage;

		private SettingsSerializer _srset;

		//
		private IActPluginV1 _FFXIVPlugin;
		private NetworkReceivedDelegate _fpgNetworkReceiveDelegete;
		private ZoneChangedDelegate _fpgZoneChangeDelegate;
		private bool _fpgConnect = false;

		//
		private bool _use_notify = false;
		private long _last_notify;
		private long _last_sound;

		//
		private OverlayForm _frmOverlay;
		#endregion

		#region
		//
		public bool IsAttached => _FFXIVPlugin != null && _fpgConnect;
		#endregion

		#region 클래스
		//
		public MainControl()
		{
			RegisterActAssemblies();

			InitializeComponent();
			InitializeUi();

			//
			foreach (var f in Application.OpenForms)
			{
				if (f != ActGlobals.oFormActMain)
					continue;

				_isFormLoaded = true;
				break;
			}

			//
			_frmOverlay = new OverlayForm();
		}
		#endregion

		#region ACT처리
		// ACT에 어셈블리 등록
		private void RegisterActAssemblies()
		{
			var pub = new Publish();

			var pin = ActGlobals.oFormActMain.ActPlugins.FirstOrDefault(x => x.pluginFile.Name.Equals("ACT.DFAssist.dll"));
			Settings.PluginPath = pin?.pluginFile.DirectoryName;

			// 설정 경로
			Settings.Path = Path.Combine(ActGlobals.oFormActMain.AppDataFolder.FullName, "Config", "ACT.DFAssist.config.xml");

			// FFXIV 플러그인용
			_fpgNetworkReceiveDelegete = new NetworkReceivedDelegate(OnFFXIVNetworkReceived);
			_fpgZoneChangeDelegate = new ZoneChangedDelegate(OnFFXIVZoneChanged);
		}

		//
		public void InitPlugin(TabPage pluginScreenSpace, Label pluginStatusText)
		{
			_actLabelStatus = pluginStatusText;
			_actTabPage = pluginScreenSpace;

			if (_isFormLoaded)
				ActInitialize();
			else
				ActGlobals.oFormActMain.Shown += OFormActMain_Shown;

			// FFXIV 플러그인용
			if (_FFXIVPlugin == null)
			{
				_FFXIVPlugin = ActGlobals.oFormActMain.ActPlugins.Where(x =>
					 x.pluginFile.Name.ToUpper().Contains("FFXIV_ACT_PLUGIN") &&
					 x.lblPluginStatus.Text.ToUpper().Contains("FFXIV PLUGIN STARTED."))
					.Select(x => x.pluginObj)
					.FirstOrDefault();
			}

			if (_FFXIVPlugin != null)
			{
				try
				{
					((FFXIV_ACT_Plugin.FFXIV_ACT_Plugin)_FFXIVPlugin).DataSubscription.NetworkReceived -= _fpgNetworkReceiveDelegete;
					((FFXIV_ACT_Plugin.FFXIV_ACT_Plugin)_FFXIVPlugin).DataSubscription.NetworkReceived += _fpgNetworkReceiveDelegete;
					((FFXIV_ACT_Plugin.FFXIV_ACT_Plugin)_FFXIVPlugin).DataSubscription.ZoneChanged -= _fpgZoneChangeDelegate;
					((FFXIV_ACT_Plugin.FFXIV_ACT_Plugin)_FFXIVPlugin).DataSubscription.ZoneChanged += _fpgZoneChangeDelegate;
					_fpgConnect = true;
				}
				catch
				{
					_fpgConnect = false;
				}
			}
		}

		//
		private void OFormActMain_Shown(object sender, EventArgs e)
		{
			_isFormLoaded = true;
			ActInitialize();
		}

		//
		private void ActInitialize()
		{
			if (_isInActInit)
				return;

			//
			_isInActInit = true;

			Mesg.SetTextBox(rtxLogger);
			ActGlobals.oFormActMain.Shown -= OFormActMain_Shown;

			ReadMesg();
			ReadGame();

			//
			_actLabelStatus.Text = "Initializing...";

			Dock = DockStyle.Fill;

			UpdateUiLanguage();

			_actLabelStatus.Text = Mesg.GetText("l-plugin-started");
			_actTabPage.Text = Mesg.GetText("app-name");
			_actTabPage.Controls.Add(this);

			//
			_isPluginEnabled = true;

			//
			_srset = new SettingsSerializer(this);
			ReadSettings();
			UpdateFates();

			// 
			string tagname = Settings.GetTagNameForUpdate();
			if (!Settings.TagName.Equals(tagname))
			{
				Mesg.I("i-client-updated", tagname);

				if (!txtUpdateSkip.Text.Equals(tagname))
				{
					Task.Run(() =>
					{
						var res = MessageBox.Show(
							Mesg.GetText("i-visit-updated"),
							Mesg.GetText("app-name"), MessageBoxButtons.YesNo, MessageBoxIcon.Information);
						if (res == DialogResult.Yes)
							Process.Start("https://github.com/purutu/ACT.DFAssist/releases/latest");
						else
						{
							txtUpdateSkip.Text = tagname;
							SaveSettings();
						}
					});
				}
			}

			//
			_isInActInit = false;
		}

		//
		public void DeInitPlugin()
		{
			//
			_isPluginEnabled = false;

			_frmOverlay.Hide();
			_frmOverlay = null;

			SaveSettings();

			//
			_isInitSetting = false;

			_actTabPage = null;

			if (_actLabelStatus != null)
			{
				_actLabelStatus.Text = Mesg.GetText("l-plugin-stopped");
				_actLabelStatus = null;
			}

			Mesg.SetTextBox(null);
		}
		#endregion

		#region UI 처리
		// 추가 ui 초기화
		private void InitializeUi()
		{
			// 색깔 선택 색깔만들기
			Type colortype = typeof(System.Drawing.Color);
			PropertyInfo[] pis = colortype.GetProperties(BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.Public);
			foreach (var p in pis)
				cboLogBackground.Items.Add(p.Name);

			cboLogBackground.SelectedValue = rtxLogger.BackColor.Name;

			// 콤보박스
			cboUiLanguage.DataSource = Mesg.Locales.Clone();
			cboUiLanguage.DisplayMember = "Name";
			cboUiLanguage.ValueMember = "Code";

			cboGameLanguage.DataSource = Mesg.Locales.Clone();
			cboGameLanguage.DisplayMember = "Name";
			cboGameLanguage.ValueMember = "Code";

			cboClientVersion.DataSource = GameVersion.Versions.Clone();
			cboClientVersion.DisplayMember = "Name";
			cboClientVersion.ValueMember = "Index";
			cboClientVersion.SelectedIndex = 0;
		}

		//
		private void UpdateUiLanguage()
		{
			tabPageFates.Text = Mesg.GetText("ui-tab-1-text");
			tabPageSetting.Text = Mesg.GetText("ui-tab-2-text");
			tabPageNotify.Text = Mesg.GetText("ui-tab-3-text");

			lblClientVersion.Text = Mesg.GetText("ui-client-version");
			lblUiLanguage.Text = Mesg.GetText("ui-language");
			lblGameLanguage.Text = Mesg.GetText("ui-in-game");
			lblBackColor.Text = Mesg.GetText("ui-back-color");
			lblDisplayFont.Text = Mesg.GetText("ui-display-font");
			btnShowLogSetting.Text = Mesg.GetText("ui-show-log-setting");
			btnClearLogs.Text = Mesg.GetText("ui-clear-logs");
			btnReconnect.Text = Mesg.GetText("ui-reconnect");
			chkWholeFates.Text = Mesg.GetText("ui-log-whole-fate");
			chkUseOverlay.Text = Mesg.GetText("ui-enable-overlay");
			chkUseSound.Text = Mesg.GetText("ui-enable-sound");
			//btnSelectSound.Text = Mesg.GetText("ui-find");
			label1.Text = Mesg.GetText("app-description");

			btnTestNotify.Text = Mesg.GetText("ui-notift-test");
			chkNtfUseLine.Text = Mesg.GetText("ui-notify-use-line");
			lblNtfLineToken.Text = Mesg.GetText("ui-token");
			chkNtfUseTelegram.Text = Mesg.GetText("ui-notify-use-telegram");
			lblNtfTelegramId.Text = Mesg.GetText("ui-id");
			lblNtfTelegramToken.Text = Mesg.GetText("ui-token");

			_frmOverlay.SetInfoText("app-description");

			ttCtrls.SetToolTip(btnBlinkOverlay, Mesg.GetText("tip-blink-overlay"));
			ttCtrls.SetToolTip(btnSelectSound, Mesg.GetText("tip-select-sound-dialog"));
			ttCtrls.SetToolTip(btnSoundPlay, Mesg.GetText("tip-sound-play"));

			btnLogFont.Text = $"{rtxLogger.Font.Name}, {rtxLogger.Font.Size}";
		}

		//
		private void TrvFates_AfterCheck(object sender, TreeViewEventArgs e)
		{
			if (!_isPluginEnabled)
				return;

			if (_isLockFates)
				return;

			_isLockFates = true;

			if (((string)e.Node.Tag).Contains("AREA:"))
			{
				foreach (TreeNode n in e.Node.Nodes)
					n.Checked = e.Node.Checked;
			}
			else
			{
				if (!e.Node.Checked)
					e.Node.Parent.Checked = false;
				else
				{
					var f = true;
					foreach (TreeNode n in e.Node.Parent.Nodes)
						f &= n.Checked;

					e.Node.Parent.Checked = f;
				}
			}

			BuildSelectedFates(true);
			SaveSettings();

			_isLockFates = false;
		}

		//
		private void BtnClearLogs_Click(object sender, EventArgs e)
		{
			if (!_isPluginEnabled)
				return;

			rtxLogger.Clear();
		}

		//
		private void CboUiLanguage_SelectedValueChanged(object sender, EventArgs e)
		{
			if (!_isPluginEnabled)
				return;

			ReadMesg();
			UpdateUiLanguage();
		}

		//
		private void CboGameLanguage_SelectedValueChanged(object sender, EventArgs e)
		{
			if (!_isPluginEnabled)
				return;

			ReadGame();
			UpdateFates();
		}

		//
		private void CboClientVersion_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!_isPluginEnabled)
				return;

			if (cboClientVersion.SelectedIndex < 0)
				return;

			try
			{
				var v = GameVersion.Versions[cboClientVersion.SelectedIndex];
				Mesg.I("i-client-version", string.Format("이전={0}, 변경={1}", Settings.ClientVersion, v.Index));

				if (Settings.ClientVersion != v.Index)
				{
					Settings.ClientVersion = v.Index;
					txtClientVersion.Text = v.Index.ToString();

					GamePacket.Current = GamePacket.Versions[v.Index];

					Mesg.I("i-client-version", $"{v.Name}, {GamePacket.Current.OpFate:X4}/{GamePacket.Current.OpDuty:X4}/{GamePacket.Current.OpMatch:X4}");
					SaveSettings();
				}
			}
			catch
			{
			}
		}

		//
		private void CboClientVersion_SelectedValueChanged(object sender, EventArgs e)
		{
		}

		//
		private void CboLogBackground_DrawItem(object sender, DrawItemEventArgs e)
		{
			Graphics g = e.Graphics;
			Rectangle r = e.Bounds;

			if (e.Index >= 0)
			{
				var n = ((ComboBox)sender).Items[e.Index].ToString();
				var f = new Font("Sego UI", 9, FontStyle.Regular);
				var c = Color.FromName(n);
				var b = new SolidBrush(c);
				g.FillRectangle(b, r.X + 4, r.Y + 3, r.X + 30, r.Height - 3);
				g.DrawString(n, f, Brushes.Black, r.X + 32, r.Top);
			}
		}

		//
		private void CboLogBackground_SelectedValueChanged(object sender, EventArgs e)
		{
			if (!string.IsNullOrWhiteSpace(cboLogBackground.Text) && !cboLogBackground.Text.Equals(Color.Transparent.Name))
			{
				rtxLogger.BackColor = Color.FromName(cboLogBackground.Text);
				Mesg.I("i-selected-color", cboLogBackground.Text);
			}
		}

		//
		private void BtnReconnect_Click(object sender, EventArgs e)
		{
		}

		//
		private void ChkWholeFates_CheckedChanged(object sender, EventArgs e)
		{
			Settings.LoggingWholeFates = chkWholeFates.Checked;
		}

		private void ChkUseOverlay_CheckedChanged(object sender, EventArgs e)
		{
			if (chkUseOverlay.Checked)
			{
				_frmOverlay.Show();
				btnBlinkOverlay.Enabled = true;
			}
			else
			{
				_frmOverlay.Hide();
				btnBlinkOverlay.Enabled = false;
			}

			Settings.UseOverlay = chkUseOverlay.Checked;
		}

		private void ChkUseSound_CheckedChanged(object sender, EventArgs e)
		{
			CheckSoundEnable();
		}

		private void BtnBlinkOverlay_Click(object sender, EventArgs e)
		{
			_frmOverlay.StartBlink();
		}

		private void BtnTest_Click(object sender, EventArgs e)
		{
			PlayEffectSound();
		}

		private void BtnSelectSound_Click(object sender, EventArgs e)
		{
			var dg = new OpenFileDialog
			{
				Title = Mesg.GetText("ui-select-sound"),
				DefaultExt = "wav",
				Filter = "Wave (*.wav)|*.wav|All (*.*)|*.*"
			};

			if (dg.ShowDialog() == DialogResult.OK)
			{
				txtSoundFile.Text = dg.FileName;

				SaveSettings();
			}
		}

		private void BtnSoundPlay_Click(object sender, EventArgs e)
		{
			PlayEffectSound(true);
		}

		private void BtnLogFont_Click(object sender, EventArgs e)
		{
			FontDialog dg = new FontDialog
			{
				Font = rtxLogger.Font,
				FontMustExist = true,
				AllowVerticalFonts = false
			};

			if (dg.ShowDialog() == DialogResult.OK)
			{
				rtxLogger.Font = dg.Font;

				var s = $"{rtxLogger.Font.Name}, {rtxLogger.Font.Size}";
				txtLogFont.Text = s;
				btnLogFont.Text = s;

				SaveSettings();
			}
		}

		private void btnShowLogSetting_Click(object sender, EventArgs e)
		{
			pnlLogSetting.Visible = !pnlLogSetting.Visible;
		}
		#endregion

		#region 자료 처리
		//
		private void ReadMesg(Mesg.Locale locale = null)
		{
			Mesg.Locale loc = locale ?? (Mesg.Locale)cboUiLanguage.SelectedItem;

			if (_localeUi == null || !loc.Code.Equals(_localeUi.Code))
			{
				_localeUi = loc;

				string json;
				switch (loc.Code)
				{
					case "ja": json = Properties.Resources.mesg_ja; break;
					case "ko": json = Properties.Resources.mesg_ko; break;
					default: json = Properties.Resources.mesg_en; break;
				}

				Mesg.Initialize(json);
			}
		}

		//
		private void ReadGame(Mesg.Locale locale = null)
		{
			Mesg.Locale loc = locale ?? (Mesg.Locale)cboGameLanguage.SelectedItem;

			if (_localeGame == null || !loc.Code.Equals(_localeGame.Code))
			{
				_localeGame = loc;

				string json;
				switch (loc.Code)
				{
					case "ja": json = Properties.Resources.dfas_ja; break;
					//case "de": json = Properties.Resources.dfas_de; break;
					//case "fr": json = Properties.Resources.dfas_fr; break;
					case "ko": json = Properties.Resources.dfas_ko; break;
					default: json = Properties.Resources.dfas_en; break;
				}
				GameData.Initialize(json);

				Mesg.I("i-data-version",
					GameData.Areas.Count, GameData.Instances.Count,
					GameData.Roulettes.Count, GameData.Fates.Count);
			}
		}

		#region 설정
		//
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<보류 중>")]
		private void ReadSettings()
		{
			_srset.AddControlSetting("LocaleUi", cboUiLanguage);
			_srset.AddControlSetting("LocaleGame", cboGameLanguage);
			_srset.AddControlSetting("LogBackColor", cboLogBackground);
			_srset.AddControlSetting("LoggingWholeFATEs", chkWholeFates);
			_srset.AddControlSetting("UseOverlay", chkUseOverlay);
			_srset.AddControlSetting("OverlayLocation", txtOverayLocation);
			_srset.AddControlSetting("SelectedFates", txtSelectedFates);
			_srset.AddControlSetting("UseSound", chkUseSound);
			_srset.AddControlSetting("SoundFile", txtSoundFile);
			_srset.AddControlSetting("LogFont", txtLogFont);
			_srset.AddControlSetting("ClientVersion", txtClientVersion);
			_srset.AddControlSetting("UpdateSkip", txtUpdateSkip);

			_srset.AddControlSetting("NotifyUseLine", chkNtfUseLine);
			_srset.AddControlSetting("NotifyLineToken", txtNtfLineToken);
			_srset.AddControlSetting("NotifyUseTelegram", chkNtfUseTelegram);
			_srset.AddControlSetting("NotifyTelegramId", txtNtfTelegramId);
			_srset.AddControlSetting("NotifyTelegramToken", txtNtfTelegramToken);

			if (File.Exists(Settings.Path))
			{
				using (var fs = new FileStream(Settings.Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
				using (var xr = new XmlTextReader(fs))
				{
					try
					{
						while (xr.Read())
						{
							if (xr.NodeType != XmlNodeType.Element)
								continue;

							if (xr.LocalName == "SettingsSerializer")
								_srset.ImportFromXml(xr);
						}
					}
					catch (Exception ex)
					{
						_actLabelStatus.Text = Mesg.GetText("e-setting-load", ex.Message);
					}

					xr.Close();
				}
			}

			// game version
			int.TryParse(txtClientVersion.Text, out int clientversion);
			for (int i = 0; i < GameVersion.Versions.Length; i++)
			{
				if (GameVersion.Versions[i].Index == clientversion)
				{
					cboClientVersion.SelectedIndex = i;
					break;
				}
			}

			// locale
			_localeUi = (Mesg.Locale)cboUiLanguage.SelectedItem;
			_localeGame = (Mesg.Locale)cboGameLanguage.SelectedItem;

			// fates
			Settings.LoggingWholeFates = chkWholeFates.Checked;

			try
			{
				var ss = txtOverayLocation.Text.Split(',');
				if (ss.Length == 2)
				{

					Settings.OverlayLocation = new Point(int.Parse(ss[0].Trim()), int.Parse(ss[1].Trim()));
					_frmOverlay.Location = Settings.OverlayLocation;
				}
			}
			catch
			{
			}

			// overlay
			if (chkUseOverlay.Checked)
				_frmOverlay.Show();
			else
				_frmOverlay.Hide();

			Settings.UseOverlay = chkUseOverlay.Checked;

			// sound 
			CheckSoundEnable();

			// font
			try
			{
				var ss = txtLogFont.Text.Split(',');
				if (ss.Length == 2)
				{
					var font = new Font(ss[0], float.Parse(ss[1]), FontStyle.Regular, GraphicsUnit.Point);
					if (font != null)
						rtxLogger.Font = font;
				}
			}
			catch
			{
			}
			finally
			{
				btnLogFont.Text = $"{rtxLogger.Font.Name}, {rtxLogger.Font.Size}";
			}

			//
			CheckUseNotify();

			// background color
			if (!string.IsNullOrWhiteSpace(cboLogBackground.Text))
			{
				Color c = Color.FromName(cboLogBackground.Text);
				if (c.Equals(Color.Transparent))
					rtxLogger.BackColor = c;
			}

			//
			_isInitSetting = true;
		}


		//
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<보류 중>")]
		private void SaveSettings()
		{
			if (!_isInitSetting)
				return;

			txtOverayLocation.Text = $"{Settings.OverlayLocation.X},{Settings.OverlayLocation.Y}";

			try
			{
				using (var fs = new FileStream(Settings.Path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
				using (var xw = new XmlTextWriter(fs, Encoding.UTF8) { Formatting = Formatting.Indented, Indentation = 1, IndentChar = '\t' })
				{
					xw.WriteStartDocument(true);
					xw.WriteStartElement("Config"); // <Config>
					xw.WriteStartElement("SettingsSerializer"); // <Config><SettingsSerializer>
					_srset.ExportToXml(xw); // Fill the SettingsSerializer XML
					xw.WriteEndElement(); // </SettingsSerializer>
					xw.WriteEndElement(); // </Config>
					xw.WriteEndDocument(); // Tie up loose ends (shouldn't be any)
					xw.Flush(); // Flush the file buffer to disk
					xw.Close();
				}
			}
			catch (Exception ex)
			{
				Mesg.Ex(ex, "Exception: save setting failed");
			}
		}
		#endregion
		#endregion

		#region FATE 처리
		//
		private void InternalBuildSelectedFates(IEnumerable node)
		{
			foreach (TreeNode n in node)
			{
				if (n.Checked)
					Settings.SelectedFates.Add((string)n.Tag);
				InternalBuildSelectedFates(n.Nodes);
			}
		}

		//
		private void BuildSelectedFates(bool maketext = false)
		{
			Settings.SelectedFates.Clear();
			InternalBuildSelectedFates(trvFates.Nodes);

			if (maketext)
				txtSelectedFates.Text = string.Join("|", Settings.SelectedFates);
		}

		//
		private void UpdateFates()
		{
			trvFates.Nodes.Clear();

			//
			Settings.SelectedFates.Clear();

			if (!string.IsNullOrWhiteSpace(txtSelectedFates.Text))
			{
				var ss = txtSelectedFates.Text.Split('|');

				foreach (var s in ss)
				{
					if (!string.IsNullOrWhiteSpace(s))
						Settings.SelectedFates.Add(s);
				}
			}

			_isLockFates = true;

			foreach (var a in GameData.Areas)
			{
				var n = trvFates.Nodes.Add(a.Value.Name);
				n.Tag = "AREA:" + a.Key;

				if (Settings.SelectedFates.Contains((string)n.Tag))
				{
					n.Checked = true;
					n.Expand();
				}

				foreach (var f in a.Value.Fates)
				{
					var name = f.Value.Name;
					var node = n.Nodes.Add(name);
					node.Tag = f.Key.ToString();

					if (Settings.SelectedFates.Contains((string)node.Tag))
					{
						node.Checked = true;

						if (!n.IsExpanded)
							n.Expand();
					}
				}
			}

			BuildSelectedFates();

			_isLockFates = false;
		}
		#endregion

		#region 소리 처리
		private void CheckSoundEnable()
		{
			txtSoundFile.Enabled = chkUseSound.Checked;
			btnSelectSound.Enabled = chkUseSound.Checked;
		}

		private void PlayEffectSound(bool force = false)
		{
			if (!force && !chkUseSound.Checked)
				return;

			if (string.IsNullOrWhiteSpace(txtSoundFile.Text) || !File.Exists(txtSoundFile.Text))
				return;

			long now = DateTime.Now.Ticks;

			if ((now - _last_sound) > 1000)
			{
				_last_sound = now;

				try
				{
					using (var sp = new SoundPlayer(txtSoundFile.Text))
						sp.Play();
				}
				catch
				{
				}
			}
		}
		#endregion

		#region 알림
		private void CheckUseNotify()
		{
			_use_notify = chkNtfUseLine.Checked || chkNtfUseTelegram.Checked;

			txtNtfLineToken.Enabled = chkNtfUseLine.Checked;
			txtNtfTelegramToken.Enabled = txtNtfTelegramId.Enabled = chkNtfUseTelegram.Checked;
		}

		private void SendNotify(string s)
		{
			if (chkNtfUseLine.Checked)
				InternalNotifyByLine(s).Wait();

			if (chkNtfUseTelegram.Checked)
				InternalNotifyByTelegram(s);
		}

		private void NotifyFate(GameData.Fate fate)
		{
			string s = Mesg.GetText("l-fate-occured-info", fate.Name);
			SendNotify(s);
		}

		private void NotifyMatch(string name)
		{
			string s = Mesg.GetText("i-matched", name);
			SendNotify(s);
		}

		private async void BtnTestNotify_Click(object sender, EventArgs e)
		{
			string s = string.Format("{0} - {1}",
				Mesg.GetText("ui-notift-test"),
				Mesg.GetText("app-description"));

			if (chkNtfUseLine.Checked)
				await InternalNotifyByLine(s);

			if (chkNtfUseTelegram.Checked)
				InternalNotifyByTelegram(s);
		}

		private void ChkNtfUseLine_CheckedChanged(object sender, EventArgs e)
		{
			CheckUseNotify();
			SaveSettings();
		}

		private void TxtNtfLineToken_TextChanged(object sender, EventArgs e)
		{
			SaveSettings();
		}

		private void LnklblLineNotify_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start("https://notify-bot.line.me/");
		}

		internal async Task InternalNotifyByLine(string mesg)
		{
			if (txtNtfLineToken.TextLength == 0)
				return;

			var hc = new HttpClient();
			hc.DefaultRequestHeaders.Add("Authorization", $"Bearer {txtNtfLineToken.Text}");

			var param = new Dictionary<string, string>
			{
				{ "message", mesg }
			};

			await hc.PostAsync(
				"https://notify-api.line.me/api/notify",
				new FormUrlEncodedContent(param)).ConfigureAwait(false);
		}

		private void ChkNtfUseTelegram_CheckedChanged(object sender, EventArgs e)
		{
			CheckUseNotify();
			SaveSettings();
		}

		internal void InternalNotifyByTelegram(string mesg)
		{
			if (txtNtfTelegramId.TextLength == 0 || txtNtfTelegramToken.TextLength == 0)
				return;

			var url = string.Format("https://api.telegram.org/bot{0}/sendMessage?chat_id={1}&text={2}",
				txtNtfTelegramToken.Text, txtNtfTelegramId.Text, mesg);
			var wr = WebRequest.Create(url);
			wr.GetResponse().GetResponseStream();
		}
		#endregion

		#region 게임 프로시져
		// FFXIV 플러그인: 장소 변경
		public void OnFFXIVZoneChanged(uint zoneId, string zoneName)
		{
#if DEBUG
			Mesg.I($"장소 바뀜: {zoneId}/{zoneName}");
#endif
			_frmOverlay.EventNone();
		}

		// FFXIV 플러그인: 메시지 받음
		public void OnFFXIVNetworkReceived(string connection, long epoch, byte[] message)
		{
			if (message.Length < 32)
				return;

			try
			{
				PacketHandler(connection, message);
			}
			catch
			{
			}
		}

		private void PacketHandler(string pid, byte[] message)
		{
			var opcode = BitConverter.ToUInt16(message, 18);

			if (opcode != GamePacket.Current.OpFate &&
				opcode != GamePacket.Current.OpDuty &&
				opcode != GamePacket.Current.OpMatch &&
				opcode != GamePacket.Current.OpInstance)
				return;

			var data = message.Skip(32).ToArray();

			// FATE
			if (opcode == GamePacket.Current.OpFate)
			{
				if (data[0] == GamePacket.Current.FateIndex)
				{
					var fcode = BitConverter.ToUInt16(data, 4);
					bool isselected = Settings.SelectedFates.Contains(fcode.ToString());

					if (Settings.LoggingWholeFates || isselected)
					{
						var fate = GameData.GetFate(fcode);

						Mesg.Fate("l-fate-occured-info", fate.Name);

						if (isselected)
						{
							PlayEffectSound();
							_frmOverlay.EventFate(fate);

							if (_use_notify)
								NotifyFate(fate);
						}
					}
				}
			}
			// 듀티
			else if (opcode == GamePacket.Current.OpDuty)
			{
				// 안쓴다
				// var status = data[0];
				// var reason = data[4];
				var rcode = data[GamePacket.Current.DutyRoulette];

				if (rcode != 0)
				{
					// 루렛
					var roulette = GameData.GetRoulette(rcode);

					Mesg.Duty("i-queue-roulette", roulette.Name);

					_frmOverlay.EventQueue(roulette.Name);
				}
				else
				{
					// 직접 골라 큐
					var insts = new List<int>();
					for (var i = 0; i < 5; i++)
					{
						var icode = BitConverter.ToUInt16(data, GamePacket.Current.DutyInstance + (i * 4));
						if (icode == 0)
							break;
					}

					if (insts.Any())
					{
						Mesg.Duty("i-queue-instance", string.Join(", ", insts.Select(x => GameData.GetInstance(x).Name).ToArray()));

						_frmOverlay.EventStatus(insts.Count);
					}
				}
			}
			// 매치
			else if (opcode == GamePacket.Current.OpMatch)
			{
				var rcode = BitConverter.ToUInt16(data, GamePacket.Current.MatchRoulette);
				var icode = BitConverter.ToUInt16(data, GamePacket.Current.MatchInstance);
				string name;

				if (icode == 0 && rcode != 0)
				{
					// 이것 루렛 매칭
					var roulette = GameData.GetRoulette(rcode);

					Mesg.Duty("i-matched", roulette.Name);
					name = roulette.Name;
				}
				else if (icode != 0)
				{
					// 이건 골라 매칭
					var instance = GameData.GetInstance(icode);

					Mesg.Duty("i-matched", instance.Name);
					name = instance.Name;
				}
				else
				{
					// 루렛도 인스도 아녀
					name = Mesg.GetText("l-unknown-instance", icode);
				}

				PlayEffectSound();
				_frmOverlay.EventMatch(name);

				if (_use_notify)
					NotifyMatch(name);

			}
			// 인스턴스 관련
			else if (opcode == GamePacket.Current.OpInstance && GamePacket.Current.OpInstance != 0)
			{
				if (data[4] == 0)
				{
					// 0은 최초 입장때만 나오므로 이거 쓰자
					var icode = BitConverter.ToUInt16(data, GamePacket.Current.InstanceInstance);
					var instance = GameData.GetInstance(icode);

					Mesg.Duty("l-instance-enter", instance.Name);

					_frmOverlay.EventMatch(Mesg.GetText("l-instance-enter", instance.Name));

					if (_use_notify)
						NotifyMatch(instance.Name);
				}
				else
				{
					// 조용히 시키자
					_frmOverlay.EventNone();
				}
			}
		}
		#endregion
	}
}
