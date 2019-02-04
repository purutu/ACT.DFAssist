using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Advanced_Combat_Tracker;
using System.EnterpriseServices.Internal;
using System.IO;
using System.Collections.Concurrent;
using System.Collections;
using System.Xml;
using System.Diagnostics;
using System.Reflection;

namespace ACT.DFAssist
{
    public partial class MainControl : UserControl, IActPluginV1
    {
        private static string PluginPath = "";

        private static readonly string[] Dependencies =
        {
            "Newtonsoft.Json.dll",
        };

        //
        private bool _isFormLoaded;
        private bool _isInActInit;
        private bool _isPluginEnabled;
        private bool _isLockFates;
        private string _selectedFates;
        private Localization.Language _selectedUiLanguage;
        private Localization.Language _selectedGameLanguage;

        private readonly string _settingPath;
        private readonly ConcurrentDictionary<int, ProNet> _procaps;
        private readonly ConcurrentStack<string> _selectedFateStack;

        private Timer _timer;
        private ulong _tick_count;

        private SettingsSerializer _srset;

        //
        private Label _actLabelStatus;
        private TabPage _actTabPage;

        //
        public MainControl()
        {
            RegisterActAssemblies();

            InitializeComponent();

            ArrayList colors = new ArrayList();
            Type colortype = typeof(System.Drawing.Color);
            PropertyInfo[] pis = colortype.GetProperties(BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.Public);
            foreach (var p in pis)
                cboLogBackground.Items.Add(p.Name);

            cboLogBackground.SelectedValue = rtxLogger.BackColor.Name;

            _settingPath = Path.Combine(ActGlobals.oFormActMain.AppDataFolder.FullName, "Config", "ACT.DFAssist.config.xml");
            _procaps = new ConcurrentDictionary<int, ProNet>();
            _selectedFateStack = new ConcurrentStack<string>();

            foreach (var f in Application.OpenForms)
            {
                if (f != ActGlobals.oFormActMain)
                    continue;

                _isFormLoaded = true;
                break;
            }
        }

        private static void RegisterActAssemblies()
        {
            var pub = new Publish();

            var pin = ActGlobals.oFormActMain.ActPlugins.FirstOrDefault(x => x.pluginFile.Name.Equals("ACT.DFAssist.dll"));
            PluginPath = pin?.pluginFile.DirectoryName;

            if (PluginPath == null)
                return;

            foreach (var d in Dependencies)
            {
                var dll = Path.Combine(PluginPath, d);
                try
                {
                    pub.GacInstall(dll);
                }
                catch (Exception ex)
                {
                    ActGlobals.oFormActMain.WriteExceptionLog(ex, "ACT.DFAssist: cannot registry dependency dll");
                }
            }
        }

        public void InitPlugin(TabPage pluginScreenSpace, Label pluginStatusText)
        {
            _actLabelStatus = pluginStatusText;
            _actTabPage = pluginScreenSpace;

            if (_isFormLoaded)
                ActInitialize();
            else
                ActGlobals.oFormActMain.Shown += OFormActMain_Shown;
        }

        private void OFormActMain_Shown(object sender, EventArgs e)
        {
            _isFormLoaded = true;
            ActInitialize();
        }

        private void ActInitialize()
        {
            if (_isInActInit)
                return;

            _isInActInit = true;

            MsgLog.SetTextBox(rtxLogger);
            ActGlobals.oFormActMain.Shown -= OFormActMain_Shown;

            Localization.Language deflang = new Localization.Language { Name = "English", Code = "en" };
            ReadLanguage(deflang);

            MsgLog.Info("ui-dbg-msg", System.Environment.CurrentDirectory);
            MsgLog.Info("ui-dbg-msg", PluginPath);

            ReadGameData(deflang);

            _isPluginEnabled = true;

            cboUiLanguage.DataSource = new Localization.Language[]
            {
                new Localization.Language{Name="English", Code="en"},
                new Localization.Language{Name="にほんご", Code="ja"},
                new Localization.Language{Name="한국말", Code="ko"},
            }; 
            cboUiLanguage.DisplayMember = "Name";
            cboUiLanguage.ValueMember = "Code";

            cboGameLanguage.DataSource = new Localization.Language[]
            {
                new Localization.Language{Name="English", Code="en"},
                new Localization.Language{Name="にほんご", Code="ja"},
                new Localization.Language{Name="한국말", Code="ko"},
            };
            cboGameLanguage.DisplayMember = "Name";
            cboGameLanguage.ValueMember = "Code";

            Dock = DockStyle.Fill;

            _actLabelStatus.Text = "Initializing...";

            UpdateUiLanguage();

            _actLabelStatus.Text = Localization.GetText("l-plugin-started");
            _actTabPage.Text = Localization.GetText("app-name");
            _actTabPage.Controls.Add(this);

            _srset = new SettingsSerializer(this);
            ReadSettings();

            UpdateFates();

            UpdateProcesses();

            if (_timer==null)
            {
                _timer = new Timer { Interval = 10000 };
                _timer.Tick += _timer_Tick;
            }

            _timer.Enabled = true;

            _isInActInit = false;
        }

        public void DeInitPlugin()
        {
            _isPluginEnabled = false;

            SaveSettings();

            _actTabPage = null;

            if (_actLabelStatus!=null)
            {
                _actLabelStatus.Text = Localization.GetText("l-plugin-stopped");
                _actLabelStatus = null;
            }

            foreach (var e in _procaps)
                e.Value.Network.StopCapture();

            _timer.Enabled = false;

            MsgLog.SetTextBox(null);
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            if (!_isPluginEnabled)
                return;

            TimeSpan time = TimeSpan.FromSeconds(_tick_count * 10);
            label2.Text = string.Format("Running : {0}", time.ToString(@"hh\:mm\:ss\:fff"));

            _tick_count++;

            UpdateProcesses();
        }

        private void UpdateUiLanguage()
        {
            lblUiLanguage.Text = Localization.GetText("ui-language-display-text");
            lblGameLanguage.Text = Localization.GetText("ui-language-game-text");
            btnClearLogs.Text = Localization.GetText("ui-log-clear-display-text");
            label1.Text= Localization.GetText("app-description");
        }

        private void UpdateProcesses()
        {
            var ps = new List<Process>();
            ps.AddRange(Process.GetProcessesByName("ffxiv"));
            ps.AddRange(Process.GetProcessesByName("ffxiv_dx11"));

            foreach (var p in ps)
            {
                try
                {
                    if (_procaps.ContainsKey(p.Id))
                        continue;

                    var pc = new ProNet(p, new Network());
                    PacketFFXIV.OnEventReceived += PacketFFXIV_OnEventReceived;

                    _procaps.TryAdd(p.Id, pc);
                    MsgLog.Success("l-process-set-success", p.Id);
                }
                catch (Exception e)
                {
                    MsgLog.Exception(e, "l-process-set-failed");
                }
            }

            var dels = new List<int>();
            foreach (var e in _procaps)
            {
                if (e.Value.Process.HasExited)
                {
                    e.Value.Network.StopCapture();
                    dels.Add(e.Key);
                }
                else
                {
                    if (e.Value.Network.IsRunning)
                        e.Value.Network.UpdateGameConnections(e.Value.Process);
                    else
                        e.Value.Network.StartCapture(e.Value.Process);
                }
            }

            foreach (var u in dels)
            {
                try
                {
                    _procaps.TryRemove(u, out var _);
                    PacketFFXIV.OnEventReceived -= PacketFFXIV_OnEventReceived;
                }
                catch (Exception e)
                {
                    MsgLog.Exception(e, "l-process-remove-failed");
                }
            }
        }

        private void PacketFFXIV_OnEventReceived(int pid, GameEvents gameevent, int[] args)
        {
#if true
            var server = _procaps[pid].Process.MainModule.FileName.Contains("KOREA") ? "KOREA" : "GLOBAL";
            var text = pid + "|" + server + "|" + gameevent + "|";
#else
            var text = pid + "|GLOBAL|" + gameevent + "|";
#endif
            var pos = 0;
            var isFate = false;

            switch (gameevent)
            {
                case GameEvents.InstanceEnter:
                case GameEvents.InstanceLeave:
                    if (args.Length > 0)
                    {
                        text += GetInstanceName(args[0]) + "|";
                        pos++;
                    }
                    break;

                case GameEvents.FateBegin:
                case GameEvents.FateProgress:
                case GameEvents.FateEnd:
                    isFate = true;
                    text += GetFateName(args[0]) + "|" + GetAreaNameFromFate(args[0]) + "|";
                    pos++;
                    break;

                case GameEvents.MatchBegin:
                    text += (MatchType)args[0] + "|";
                    pos++;
                    switch ((MatchType)args[0])
                    {
                        case MatchType.Roulette:
                            text += GetRouletteName(args[1]) + "|";
                            pos++;
                            break;

                        case MatchType.Assignment:
                            text += args[1] + "|";
                            pos++;
                            var p = pos;
                            for (var i = p; i < args.Length; i++)
                            {
                                text += GetInstanceName(args[i]) + "|";
                                pos++;
                            }
                            break;
                    }
                    break;

                case GameEvents.MatchEnd:
                    text += (MatchResult)args[0] + "|";
                    pos++;
                    break;

                case GameEvents.MatchStatus:
                    text += GetInstanceName(args[0]) + "|";
                    pos++;
                    break;

                case GameEvents.MatchDone:
                    text += GetRouletteName(args[0]) + "|";
                    pos++;
                    text += GetInstanceName(args[1]) + "|";
                    pos++;
                    break;
            }

            for (var i = pos; i < args.Length; i++)
                text += args[i] + "|";

            if (isFate) text += args[0] + "|";

            ActGlobals.oFormActMain.ParseRawLogLine(false, DateTime.Now, "00|" + DateTime.Now.ToString("O") + "|0048|F|" + text);
        }

        private void ReadLanguage(Localization.Language uilang=null)
        {
            Localization.Language lang = uilang ?? (Localization.Language)cboUiLanguage.SelectedItem;

            if (_selectedUiLanguage == null || !lang.Code.Equals(_selectedUiLanguage.Code))
            {
                _selectedUiLanguage = lang;
                Localization.Initialize(PluginPath, lang.Code);
            }
        }

        private void ReadGameData(Localization.Language gamelang=null)
        {
            Localization.Language lang = gamelang ?? (Localization.Language)cboGameLanguage.SelectedItem;

            if (_selectedGameLanguage == null || !lang.Code.Equals(_selectedGameLanguage.Code))
            {
                _selectedGameLanguage = lang;
                GameData.Initialize(PluginPath, lang.Code);

                MsgLog.Info("ui-info-version",
                    GameData.Version, 
                    GameData.Areas.Count, GameData.Instances.Count, 
                    GameData.Roulettes.Count, GameData.Fates.Count);
            }
        }

        private void UpdateSelectedFates(IEnumerable nodes)
        {
            foreach (TreeNode n in nodes)
            {
                if (n.Checked)
                    _selectedFateStack.Push((string)n.Tag);

                UpdateSelectedFates(n.Nodes);
            }
        }

        private void UpdateFates()
        {
            trvFates.Nodes.Clear();

            var chks = new List<string>();
            if (!string.IsNullOrEmpty(_selectedFates))
            {
                var s = _selectedFates.Split('|');
                chks.AddRange(s);
            }

            _isLockFates = true;

            foreach (var a in GameData.Areas)
            {
                var n = trvFates.Nodes.Add(a.Value.Name);
                n.Tag = "AREA:" + a.Key;

                if (chks.Contains((string)n.Tag))
                    n.Checked = true;

                foreach (var f in a.Value.Fates)
                {
                    var name = f.Value.Name;
                    var node = n.Nodes.Add(name);
                    node.Tag = f.Key.ToString();

                    if (chks.Contains((string)node.Tag))
                        node.Checked = true;
                }
            }

            _selectedFateStack.Clear();
            UpdateSelectedFates(trvFates.Nodes);

            _isLockFates = false;
        }

        private void ReadSettings()
        {
            _srset.AddControlSetting(cboUiLanguage.Name, cboUiLanguage);
            _srset.AddControlSetting(cboGameLanguage.Name, cboGameLanguage);
            _srset.AddControlSetting(cboLogBackground.Name, cboLogBackground);
            _srset.AddStringSetting("SelectedFates");

            if (File.Exists(_settingPath))
            {
                using (var fs = new FileStream(_settingPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
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
                    catch(Exception ex)
                    {
                        _actLabelStatus.Text = string.Format("Setting read fail: {0}", ex.Message);
                    }

                    xr.Close();
                }
            }

            _selectedUiLanguage = (Localization.Language)cboUiLanguage.SelectedItem;
            _selectedGameLanguage= (Localization.Language)cboGameLanguage.SelectedItem;
        }

        private void SaveSettings()
        {
            try
            {
                _selectedFates = string.Empty;

                var fl = new List<string>();
                foreach (TreeNode a in trvFates.Nodes)
                {
                    if (a.Checked)
                        fl.Add((string)a.Tag);

                    foreach (TreeNode f in a.Nodes)
                    {
                        if (f.Checked)
                            fl.Add((string)f.Tag);
                    }
                }

                _selectedFates = string.Join("|", fl);

                using (var fs = new FileStream(_settingPath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
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
                MsgLog.Exception(ex, "Exception: save setting failed");
            }
        }

        private static string GetInstanceName(int code)
        {
            return GameData.GetInstance(code).Name;
        }

        private static string GetFateName(int code)
        {
            return GameData.GetFate(code).Name;
        }

        private static string GetAreaNameFromFate(int code)
        {
            return GameData.GetFate(code).Area.Name;
        }

        private static string GetRouletteName(int code)
        {
            return GameData.GetRoulette(code).Name;
        }

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

            _selectedFateStack.Clear();

            UpdateSelectedFates(trvFates.Nodes);
            SaveSettings();

            _isLockFates = false;
        }

        private void BtnClearLogs_Click(object sender, EventArgs e)
        {
            rtxLogger.Clear();
        }

        private void CboUiLanguage_SelectedValueChanged(object sender, EventArgs e)
        {
            ReadLanguage();
            UpdateUiLanguage();
        }

        private void CboGameLanguage_SelectedValueChanged(object sender, EventArgs e)
        {
            ReadGameData();
            UpdateFates();
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                linkLabel1.LinkVisited = true;
                System.Diagnostics.Process.Start("https://devunt.github.io/DFAssist/");
            }
            catch (Exception)
            {

            }
        }

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
                g.DrawString(n, f, Brushes.Black, r.X+32, r.Top);
            }
        }

        private void CboLogBackground_SelectionChangeCommitted(object sender, EventArgs e)
        {
            rtxLogger.BackColor = Color.FromName(cboLogBackground.Text);
        }
    }
}
