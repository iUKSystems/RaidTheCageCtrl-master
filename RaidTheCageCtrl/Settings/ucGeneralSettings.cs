using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using RaidTheCageCtrl;
using WiseGuys;
using WiseGuys.configs;
using WiseGuysFrameWork2015.Settings;

namespace WiseGuys.Settings
{
    public partial class ucGeneralSettings : SettingsBase
    {
        private MainForm mParent;        
        
        private bool IsUpdating = false;



        public ucGeneralSettings(MainForm pParent, SettingsEditor pSetParent, int InNumber)
            : base(pParent, pSetParent, InNumber)
        {
            mSetParent.OnFormClosingMsg += OnFormClosingMsg;
            
            mParent = pParent;
            InitializeComponent();                       


            UpdateDlg();
        }

        private void Changed()
        {
            generalsettings.mChanged = true;
            mChanged = true;
        }

        private void OnFormClosingMsg(object sender)
        {
            if (generalsettings.mChanged)
            {
                // save...
                //mSetParent.mChanged[(int)ESettingsForms.Connections] = true;
                generalsettings.WriteXmlConfig(mParent.ExePath);
                generalsettings.mChanged = false;
            }
        }

       

        private void UpdateDlg()
        {
            IsUpdating = true;

            tbBetweenNames.Text = generalsettings.mTextbetweenNames;

            tbgensettingscurrencysign.Text = generalsettings.mCurrencysign;

            cbCurSignBehindAmount.Checked = generalsettings.mCurrencysignafteramount;

            tbTimebeforethedoorcloses.Text = generalsettings.mTimebeforethedoorcloses.ToString();

            tbTimebeforethetimerstart.Text = generalsettings.mTimebeforettimerstart.ToString();

        
            IsUpdating = false;
        }


        private void tbgensettingscurrencysign_TextChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                generalsettings.mCurrencysign = tbgensettingscurrencysign.Text;
                Changed();
            }
        }

        private void cbCurSignBehindAmount_CheckedChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                generalsettings.mCurrencysignafteramount = cbCurSignBehindAmount.Checked;
                Changed();
            }
        }

        private void tbTimebeforethedoorcloses_TextChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                int.TryParse(tbTimebeforethedoorcloses.Text, out generalsettings.mTimebeforethedoorcloses);
                Changed();
            }
        }

        private void tbTimebeforethetimerstart_TextChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                int.TryParse(tbTimebeforethetimerstart.Text, out generalsettings.mTimebeforettimerstart);
                Changed();
            }
        }

        private void tbBetweenNames_TextChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                generalsettings.mTextbetweenNames = tbBetweenNames.Text;
                Changed();
            }
        }                                      
    }

    public class wgtextbox : TextBox
    {
        public RaidTheCagesModes rtcmode;

    }

    public class wgtextbox2 : TextBox
    {
        public int mTextnr;

    }

    public static class generalsettings
    {
        private static IProfile m_profile;

        public static bool mChanged = false;

        public static MainForm mParent;


        public static string mCurrencysign;

        public static string mTextbetweenNames;
        public static bool mCurrencysignafteramount;

        public static int mTimebeforethedoorcloses;
        public static int mTimebeforettimerstart;

        public static void SetParent(MainForm pParent)
        {
            mParent = pParent;

            ReadXmlConfig(mParent.ExePath);

        }

        public static void ReadXmlConfig(string inPath)
        {
            string filename = inPath + "\\ConfigFiles\\Settings\\GeneralSettings.xmc";
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
          
          

            mCurrencysign = m_profile.GetValue("GeneralSettings", "Currencysign", "$");
            mCurrencysignafteramount = m_profile.GetValue("GeneralSettings", "Currencysignafteramount", false);

            mTimebeforethedoorcloses = m_profile.GetValue("GeneralSettings", "TimeBeforeDoorCloses", 3000);
            mTimebeforettimerstart = m_profile.GetValue("GeneralSettings", "Timebeforettimerstart", 0);

            mTextbetweenNames = m_profile.GetValue("GeneralSettings", "TextBetweenNames", "-");

            if (File.Exists(filenamexml))
            {
                File.Delete(filenamexml);
            } 
        }

        public static void WriteXmlConfig(string inPath)
        {
            string filename = inPath + "\\ConfigFiles\\Settings\\GeneralSettings.xmc";
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

            m_profile.SetValue("GeneralSettings", "Currencysign", mCurrencysign);
            m_profile.SetValue("GeneralSettings", "Currencysignafteramount", mCurrencysignafteramount);

            m_profile.SetValue("GeneralSettings", "TimeBeforeDoorCloses", mTimebeforethedoorcloses);

            m_profile.SetValue("GeneralSettings", "Timebeforettimerstart", mTimebeforettimerstart);

            m_profile.SetValue("GeneralSettings", "TextBetweenNames", mTextbetweenNames);

            // now encrypt...
            SettingsEncrypt.Encrypt(filenamexml, filename);
            // and delete
            File.Delete(filenamexml);
        }
    }
}
