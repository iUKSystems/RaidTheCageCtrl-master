using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;
using WiseGuys.configs;
using WiseGuys.Settings;
using WiseGuysFrameWork2015.Settings;

namespace RaidTheCageCtrl.Settings
{
    public partial class ucMidiTextSettings : SettingsBase
    {
        private MainForm mParent;        

        private bool mIsUpdating = false;

        public ucMidiTextSettings(MainForm pParent, SettingsEditor pSetParent, int InNumber)
            : base(pParent, pSetParent, InNumber)
        {
            InitializeComponent();

            mSetParent.OnFormClosingMsg += OnFormClosingMsg;
            
            mParent = pParent;

            

            mIsUpdating = true;
            CreateControls();

            UpdateDlg();

            mIsUpdating = false;
        }

        private void UpdateDlg()
        {
            
        }

        private void OnFormClosingMsg(object sender)
        {
            if (MidiSettings.mChanged)
            {
                // save...
                //mSetParent.mChanged[(int)ESettingsForms.Midi] = true;
                MidiSettings.WriteXmlConfig(mParent.ExePath);
                MidiSettings.mChanged = false;
            }
        }

        private void CreateControls()
        {
            int cnt = 0;
            foreach (RaidTheCagesModes val in Enum.GetValues(typeof(RaidTheCagesModes)))
            {
                if (MultiLangualTexts.GetContinueTextEnglish(val) != "")
                {
                    Label lb = new Label();
                    lb.Text = MultiLangualTexts.GetContinueTextEnglish(val);
                    lb.Location = new Point(0, cnt * 22);
                    lb.Size = new Size(280, 20);
                    lb.TextAlign = ContentAlignment.MiddleRight;
                    this.panel1.Controls.Add(lb);

                    wgtextbox tb = new wgtextbox();
                    tb.rtcmode = val;
                    if (MidiSettings.mMidiNotes[(int)val] != -1)
                        tb.Text = MidiSettings.mMidiNotes[(int)val].ToString();
                    tb.Size = new Size(30, 20);
                    tb.TextChanged += tb_TextChanged;
                    tb.Location = new Point(290, cnt++ * 22);
                    this.panel1.Controls.Add(tb);
                }
            }

            
            cnt = 0;
            foreach (string str in MidiSettings.ExtraMidiNotes)
            {

                Label lb = new Label();
                lb.Text = MidiSettings.ExtraMidiNotes[cnt];
                lb.Location = new Point(0, cnt * 22);
                lb.Size = new Size(280, 20);
                lb.TextAlign = ContentAlignment.MiddleRight;
                this.panel2.Controls.Add(lb);

                wgtextbox2 tb = new wgtextbox2();
                tb.mTextnr = cnt;
                if (MidiSettings.mExtraMidiNotes[cnt] != -1)
                    tb.Text = MidiSettings.mExtraMidiNotes[cnt].ToString();
                tb.Size = new Size(30, 20);
                tb.TextChanged += tb_TextChanged2;
                tb.Location = new Point(290, cnt++ * 22);
                this.panel2.Controls.Add(tb);

            }
            

        }

        void tb_TextChanged(object sender, EventArgs e)
        {
            if (mIsUpdating)
                return;

            wgtextbox tb = (wgtextbox)sender;
            
            // ok.. we need to update the class... first set the mchanged to true, so everything will be saved
            MidiSettings.mChanged = true;

            if (tb.Text == "" || tb.Text == " ")
                MidiSettings.mMidiNotes[(int) tb.rtcmode] = -1;
            else
                int.TryParse(tb.Text, out MidiSettings.mMidiNotes[(int)tb.rtcmode]);
             
        }

        void tb_TextChanged2(object sender, EventArgs e)
        {
            if (mIsUpdating)
                return;

            wgtextbox2 tb = (wgtextbox2)sender;

            // ok.. we need to update the class... first set the mchanged to true, so everything will be saved
            MidiSettings.mChanged = true;

            if (tb.Text == "" || tb.Text == " ")
                MidiSettings.mExtraMidiNotes[tb.mTextnr] = -1;
            else
                int.TryParse(tb.Text, out MidiSettings.mExtraMidiNotes[tb.mTextnr]);
        }        
    }

    public static class MidiSettings
    {
        public static string[] ExtraMidiNotes =
            {
                "Closing Door in 3 Seconds",     
                "Door Closed",
                "Answer Locked",
                "Stuck in Cage",
                "Answer Correct",
                "Answer Wrong",
                "Lights Reset",
                };

        public static bool mChanged = false;

        public static MainForm mParent;

        private static IProfile m_profile;

        public static int[] mMidiNotes = new int[(int)RaidTheCagesModes.MODES + 1];
        public static int[] mExtraMidiNotes = new int[ExtraMidiNotes.Count()];

        public static void SetParent(MainForm pParent)
        {
            mParent = pParent;            

            ReadXmlConfig(mParent.ExePath);

        }

        public static void ReadXmlConfig(string inPath)
        {
            string filename = inPath + "\\ConfigFiles\\Settings\\MidiSettings.xmc";
            string filenamexml = inPath + "\\ConfigFiles\\Settings\\" +
                                 Path.GetFileNameWithoutExtension(filename) + ".xml";

            if (File.Exists(filename))
            {
                // convert to xml file.. later try away again!
                SettingsEncrypt.Decrypt(filename, filenamexml);
            }

            // read the xml file with all defaults.......
            m_profile = new Xml();
            m_profile.Name = filenamexml;

            //m_profile.Name = inPath + "\\ConfigFiles\\MidiSettings.xml";            

            foreach (RaidTheCagesModes val in Enum.GetValues(typeof(RaidTheCagesModes)))
            {
                mMidiNotes[(int)val] = m_profile.GetValue("MidiNotes", val.ToString(), -1);
            }

            int cnt = 0;
            foreach (string str in ExtraMidiNotes)
            {
                mExtraMidiNotes[cnt] = m_profile.GetValue("ExtraMidiNotes", ExtraMidiNotes[cnt], -1);
                cnt++;
            }
            if (File.Exists(filenamexml))
            {
                File.Delete(filenamexml);
            } 
        }

        public static void WriteXmlConfig(string inPath)
        {
            string filename = inPath + "\\ConfigFiles\\Settings\\MidiSettings.xmc";
            string filenamexml = inPath + "\\ConfigFiles\\Settings\\" +
                                 Path.GetFileNameWithoutExtension(filename) + ".xml";

            if (File.Exists(filename))
            {
                // convert to xml file.. later try away again!
                SettingsEncrypt.Decrypt(filename, filenamexml);
            }
            // read the xml file with all defaults.......
            m_profile = new Xml();
            m_profile.Name = filenamexml;

            //m_profile.Name = inPath + "\\ConfigFiles\\MidiSettings.xml";
            

            foreach (RaidTheCagesModes val in Enum.GetValues(typeof(RaidTheCagesModes)))
            {
                m_profile.SetValue("MidiNotes", val.ToString(), mMidiNotes[(int)val]);
            }

            int cnt = 0;
            foreach (string str in ExtraMidiNotes)
            {
                m_profile.SetValue("ExtraMidiNotes", ExtraMidiNotes[cnt], mExtraMidiNotes[cnt]);
                cnt++;
            }
            // now encrypt...
            SettingsEncrypt.Encrypt(filenamexml, filename);
            // and delete
            File.Delete(filenamexml);
        }
    }
}
