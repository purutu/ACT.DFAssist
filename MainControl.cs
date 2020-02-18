#define ENABLE_FATE

using Advanced_Combat_Tracker;
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
		private bool _isFormLoaded;
		private bool _isInActInit;
		private bool _isPluginEnabled;
		private bool _isLockFates;
		private bool _isInitSetting;

		//
		private Label _actLabelStatus;
		private TabPage _actTabPage;

		//
		private SettingsSerializer _srset;

		//
		private Localization.Locale _localeUi;
		private Localization.Locale _localeGame;

		private long _last_sound;
		private bool _use_notify = false;

		//
		private OverlayForm _frmOverlay;
		#endregion

		#region 클래스
		//
		public MainControl()
		{
			RegisterActAssemblies();

			InitializeComponent();

			// 색깔 선택 색깔만들기
			Type colortype = typeof(System.Drawing.Color);
			PropertyInfo[] pis = colortype.GetProperties(BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.Public);
			foreach (var p in pis)
				cboLogBackground.Items.Add(p.Name);

			cboLogBackground.SelectedValue = rtxLogger.BackColor.Name;

			// 설정 경로
			Settings.Path = Path.Combine(ActGlobals.oFormActMain.AppDataFolder.FullName, "Config", "ACT.DFAssist.config.xml");

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

			// 페이트 안되게하자 
			// 2020-1-10 페이트 찾았다
#if false
			tabLeft.TabPages.Remove(tabPageFates);
			chkWholeFates.Enabled = false;
#endif
		}
		#endregion

		#region ACT처리
		// ACT에 어셈블리 등록
		private static void RegisterActAssemblies()
		{
			var pub = new Publish();

			var pin = ActGlobals.oFormActMain.ActPlugins.FirstOrDefault(x => x.pluginFile.Name.Equals("ACT.DFAssist.dll"));
			Settings.PluginPath = pin?.pluginFile.DirectoryName;
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

			_isInActInit = true;

			MsgLog.SetTextBox(rtxLogger);
			ActGlobals.oFormActMain.Shown -= OFormActMain_Shown;

			Localization.Locale defaultlocale = Localization.DefaultLocale;
			ReadLocale(defaultlocale);

#if DEBUG && false
            MsgLog.D("ui-dbg", System.Environment.CurrentDirectory);
            MsgLog.D("ui-dbg", Settings.PluginPath);
#endif

			ReadGameData(defaultlocale);

			_isPluginEnabled = true;

			cboUiLanguage.DataSource = Localization.Locales.Clone();
			cboUiLanguage.DisplayMember = "Name";
			cboUiLanguage.ValueMember = "Code";

			cboGameLanguage.DataSource = Localization.Locales.Clone();
			cboGameLanguage.DisplayMember = "Name";
			cboGameLanguage.ValueMember = "Code";

			cboClientVersion.DataSource = GameData.ClientVersions.Clone();
			cboClientVersion.DisplayMember = "Name";
			cboClientVersion.ValueMember = "Value";
			cboClientVersion.SelectedIndex = 0;

			Dock = DockStyle.Fill;

			_actLabelStatus.Text = "Initializing...";

			UpdateUiLanguage();

			_actLabelStatus.Text = Localization.GetText("l-plugin-started");
			_actTabPage.Text = Localization.GetText("app-name");
			_actTabPage.Controls.Add(this);

			_srset = new SettingsSerializer(this);
			ReadSettings();

			UpdateFates();

			// 
			string tagname = Settings.GetTagNameForUpdate();
			if (!Settings.TagName.Equals(tagname))
			{
				MsgLog.I("i-client-updated", tagname);

				if (!txtUpdateSkip.Text.Equals(tagname))
				{
					Task.Run(() =>
					{
						var res = MessageBox.Show(
							Localization.GetText("i-visit-updated"),
							Localization.GetText("app-name"), MessageBoxButtons.YesNo, MessageBoxIcon.Information);
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
			PacketWorker.OnEventReceived += PacketWorker_OnEventReceived;
			PacketWorker.BeginMachina();

			_isInActInit = false;
		}

		//
		public void DeInitPlugin()
		{
			_isPluginEnabled = false;

			_frmOverlay.Hide();
			_frmOverlay = null;

			SaveSettings();

			_isInitSetting = false;

			_actTabPage = null;

			if (_actLabelStatus != null)
			{
				_actLabelStatus.Text = Localization.GetText("l-plugin-stopped");
				_actLabelStatus = null;
			}

			PacketWorker.EndMachina();

			MsgLog.SetTextBox(null);
		}
		#endregion

		#region UI 처리
		//
		private void UpdateUiLanguage()
		{
			tabPageFates.Text = Localization.GetText("ui-tab-1-text");
			tabPageSetting.Text = Localization.GetText("ui-tab-2-text");
			tabPageNotify.Text = Localization.GetText("ui-tab-3-text");

			lblClientVersion.Text = Localization.GetText("ui-client-version");
			lblUiLanguage.Text = Localization.GetText("ui-language");
			lblGameLanguage.Text = Localization.GetText("ui-in-game");
			lblBackColor.Text = Localization.GetText("ui-back-color");
			lblDisplayFont.Text = Localization.GetText("ui-display-font");
			btnClearLogs.Text = Localization.GetText("ui-clear-logs");
			btnReconnect.Text = Localization.GetText("ui-reconnect");
			chkWholeFates.Text = Localization.GetText("ui-log-whole-fate");
			chkUseOverlay.Text = Localization.GetText("ui-enable-overlay");
			chkUseSound.Text = Localization.GetText("ui-enable-sound");
			//btnSelectSound.Text = Localization.GetText("ui-find");
			label1.Text = Localization.GetText("app-description");

			btnTestNotify.Text = Localization.GetText("ui-notift-test");
			chkNtfUseLine.Text = Localization.GetText("ui-notify-use-line");
			lblNtfLineToken.Text = Localization.GetText("ui-token");
			chkNtfUseTelegram.Text = Localization.GetText("ui-notify-use-telegram");
			lblNtfTelegramId.Text = Localization.GetText("ui-id");
			lblNtfTelegramToken.Text = Localization.GetText("ui-token");

			_frmOverlay.SetInfoText("app-description");

			ttCtrls.SetToolTip(btnBlinkOverlay, Localization.GetText("tip-blink-overlay"));
			ttCtrls.SetToolTip(btnSelectSound, Localization.GetText("tip-select-sound-dialog"));
			ttCtrls.SetToolTip(btnSoundPlay, Localization.GetText("tip-sound-play"));

			btnLogFont.Text = $"{rtxLogger.Font.Name}, {rtxLogger.Font.Size}";
		}

		//
		private void TrvFates_AfterCheck(object sender, TreeViewEventArgs e)
		{
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
			rtxLogger.Clear();
		}

		//
		private void CboUiLanguage_SelectedValueChanged(object sender, EventArgs e)
		{
			ReadLocale();
			UpdateUiLanguage();
		}

		//
		private void CboGameLanguage_SelectedValueChanged(object sender, EventArgs e)
		{
			ReadGameData();
			UpdateFates();
		}

		//
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<보류 중>")]
		private void CboClientVersion_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboClientVersion.SelectedIndex < 0)
				return;

			try
			{
				var v = GameData.ClientVersions[cboClientVersion.SelectedIndex];
				//MsgLog.S("i-client-version", string.Format("이전={0}, 변경={1}", Settings.ClientVersion, v.Value));

				if (Settings.ClientVersion != v.Value)
				{
					Settings.ClientVersion = v.Value;
					txtClientVersion.Text = v.Value.ToString();

					PacketWorker.Codes = GameData.PacketCodes[v.Value];

#if true
					MsgLog.S("i-client-version", $"{v.Name}, {PacketWorker.Codes.FATE:X4}/{PacketWorker.Codes.Duty:X4}/{PacketWorker.Codes.Match:X4}");
#else
					MsgLog.S("i-client-version", v.Name);
#endif
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
				MsgLog.I("i-selected-color", cboLogBackground.Text);
			}
		}

		//
		private void BtnReconnect_Click(object sender, EventArgs e)
		{
			PacketWorker.BeginMachina();
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
				Title = Localization.GetText("ui-select-sound"),
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
#endregion

#region 자료 처리
		//
		private void ReadLocale(Localization.Locale uilang = null)
		{
			Localization.Locale lang = uilang ?? (Localization.Locale)cboUiLanguage.SelectedItem;

			if (_localeUi == null || !lang.Code.Equals(_localeUi.Code))
			{
				_localeUi = lang;

				string json;
				switch (lang.Index)
				{
					case 1: json = Properties.Resources.locale_ja; break;
					case 4: json = Properties.Resources.locale_ko; break;
					default: json = Properties.Resources.locale_en; break;
				}
				Localization.Initialize(json);
			}
		}

		//
		private void ReadGameData(Localization.Locale gamelang = null)
		{
			Localization.Locale lang = gamelang ?? (Localization.Locale)cboGameLanguage.SelectedItem;

			if (_localeGame == null || !lang.Code.Equals(_localeGame.Code))
			{
				_localeGame = lang;

				string json;
				switch (lang.Index)
				{
					case 1: json = Properties.Resources.gamedata_ja; break;
					case 2: json = Properties.Resources.gamedata_de; break;
					case 3: json = Properties.Resources.gamedata_fr; break;
					case 4: json = Properties.Resources.gamedata_ko; break;
					default: json = Properties.Resources.gamedata_en; break;
				}
				GameData.Initialize(json);

				MsgLog.I("i-data-version",
					GameData.Version,
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
						_actLabelStatus.Text = Localization.GetText("e-setting-load", ex.Message);
					}

					xr.Close();
				}
			}

			// game version
			int.TryParse(txtClientVersion.Text, out int clientversion);
			for (int i = 0; i < GameData.ClientVersions.Length; i++)
			{
				if (GameData.ClientVersions[i].Value == clientversion)
				{
					cboClientVersion.SelectedIndex = i;
					break;
				}
			}

			// locale
			_localeUi = (Localization.Locale)cboUiLanguage.SelectedItem;
			_localeGame = (Localization.Locale)cboGameLanguage.SelectedItem;

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
				MsgLog.Ex(ex, "Exception: save setting failed");
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

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<보류 중>")]
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

#region 게임 프로시져
		// 실제 데이터 처리 하는 곳
		private void PacketWorker_OnEventReceived(string pid, GameEvents gameevent, int[] args)
		{
			var text = pid + "|" + gameevent + "|";
			var pos = 0;

			var isfate = false;

			switch (gameevent)
			{
				case GameEvents.InstanceEnter:      // [0] = instance code
				case GameEvents.InstanceLeave:
					{
						if (args.Length > 0)
						{
							text += GameData.GetInstanceName(args[0]) + "|";
							pos++;
						}

						_frmOverlay.EventNone();
					}
					break;

				case GameEvents.FateOccur:          // [0] = fate code
					{
						var fate = GameData.GetFate(args[0]);
						text += fate.Name + "|" + fate.Area.Name + "|";
						pos++;

						// 모든 페이트를 골라도 목록에 있는것만 알려줌
						if (Settings.SelectedFates.Contains(args[0].ToString()))
						{
							_frmOverlay.EventFate(fate);
							if (_use_notify)
								NotifyFate(fate);
							PlayEffectSound();
						}

						isfate = true;
					}
					break;

				case GameEvents.MatchQueue:         // [0] = MatchType, [1] = code, [...] = instances
					{
						var type = (MatchType)args[0];

						text += type + "|";
						pos++;

						switch (type)
						{
							case MatchType.Roulette:
								var roulette = GameData.GetRoulette(args[1]);

								text += roulette.Name + "|";
								pos++;

								_frmOverlay.EventRoulette(roulette);

								break;

							case MatchType.Assignment:
								text += args[1] + "|";
								pos++;

								var p = pos;
								for (var i = p; i < args.Length; i++)
								{
									text += GameData.GetInstanceName(args[i]) + "|";
									pos++;
								}

								_frmOverlay.EventStatus(args[1]);

								break;
						}
					}
					break;

				case GameEvents.MatchDone:          // [0] = roulette code, [1] = instance code
					{
						var roulette = GameData.GetRoulette(args[0]);
						var instance = GameData.GetInstance(args[1]);

						text += roulette.Name + "|";
						pos++;
						text += instance.Name + "|";
						pos++;

						_frmOverlay.EventMatch(instance);
						if (_use_notify)
							NotifyDuty(instance);
						PlayEffectSound();
					}
					break;

				case GameEvents.MatchCancel:
					_frmOverlay.StopBlink();
					break;
			}

			for (var i = pos; i < args.Length; i++)
				text += args[i] + "|";

			if (isfate)
				text += args[0] + "|";

			ActGlobals.oFormActMain.ParseRawLogLine(false, DateTime.Now, "00|" + DateTime.Now.ToString("O") + "|0048|F|" + text);
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
			string s = Localization.GetText("l-fate-occured-info", fate.Name);
			SendNotify(s);
		}

		private void NotifyDuty(GameData.Instance instance)
		{
			string s = Localization.GetText("i-matched", instance.Name);
			SendNotify(s);
		}

		private async void BtnTestNotify_Click(object sender, EventArgs e)
		{
			string s = string.Format("{0} - {1}",
				Localization.GetText("ui-notift-test"),
				Localization.GetText("app-description"));

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
	}
}
