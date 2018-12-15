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
    public partial class ucLocalisationSettings : SettingsBase
    {
        private MainForm mParent;        
        
        private bool IsUpdating = false;



        public ucLocalisationSettings(MainForm pParent, SettingsEditor pSetParent, int InNumber)
            : base(pParent, pSetParent, InNumber)
        {
            mSetParent.OnFormClosingMsg += OnFormClosingMsg;
            
            mParent = pParent;
            InitializeComponent();                       


            UpdateDlg();
        }

        private void Changed()
        {
            localisationsettings.mChanged = true;
            mChanged = true;
        }

        private void OnFormClosingMsg(object sender)
        {
            if (localisationsettings.mChanged)
            {
                // save...
                //mSetParent.mChanged[(int)ESettingsForms.Connections] = true;
                localisationsettings.WriteXmlConfig(mParent.ExePath);
                localisationsettings.mChanged = false;
            }
        }

       

        private void UpdateDlg()
        {
            IsUpdating = true;

            tbLocSettingsMonTreeStep1.Text = localisationsettings.mmoneyTreeSettings[0];
            tbLocSettingsMonTreeStep2.Text = localisationsettings.mmoneyTreeSettings[1];
            tbLocSettingsMonTreeStep3.Text = localisationsettings.mmoneyTreeSettings[2];
            tbLocSettingsMonTreeStep4.Text = localisationsettings.mmoneyTreeSettings[3];
            tbLocSettingsMonTreeStep5.Text = localisationsettings.mmoneyTreeSettings[4];
            tbLocSettingsMonTreeStep6.Text = localisationsettings.mmoneyTreeSettings[5];
            tbLocSettingsMonTreeStep7.Text = localisationsettings.mmoneyTreeSettings[6];
            tbLocSettingsMonTreeStep8.Text = localisationsettings.mmoneyTreeSettings[7];
            tbLocSettingsMonTreeStep9.Text = localisationsettings.mmoneyTreeSettings[8];
                    
            IsUpdating = false;
        }

        private void tbLocSettingsMonTreeStep1_TextChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                localisationsettings.mmoneyTreeSettings[0] = tbLocSettingsMonTreeStep1.Text;
                Changed();
            }
        }

        private void tbLocSettingsMonTreeStep2_TextChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                localisationsettings.mmoneyTreeSettings[1] = tbLocSettingsMonTreeStep2.Text;
                Changed();
            }
        }

        private void tbLocSettingsMonTreeStep3_TextChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                localisationsettings.mmoneyTreeSettings[2] = tbLocSettingsMonTreeStep3.Text;
                Changed();
            }
        }

        private void tbLocSettingsMonTreeStep4_TextChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                localisationsettings.mmoneyTreeSettings[3] = tbLocSettingsMonTreeStep4.Text;
                Changed();
            }
        }

        private void tbLocSettingsMonTreeStep5_TextChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                localisationsettings.mmoneyTreeSettings[4] = tbLocSettingsMonTreeStep5.Text;
                Changed();
            }
        }

        private void tbLocSettingsMonTreeStep6_TextChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                localisationsettings.mmoneyTreeSettings[5] = tbLocSettingsMonTreeStep6.Text;
                Changed();
            }
        }

        private void tbLocSettingsMonTreeStep7_TextChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                localisationsettings.mmoneyTreeSettings[6] = tbLocSettingsMonTreeStep7.Text;
                Changed();
            }
        }

        private void tbLocSettingsMonTreeStep8_TextChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                localisationsettings.mmoneyTreeSettings[7] = tbLocSettingsMonTreeStep8.Text;
                Changed();
            }
        }

        private void tbLocSettingsMonTreeStep9_TextChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                localisationsettings.mmoneyTreeSettings[8] = tbLocSettingsMonTreeStep9.Text;
                Changed();
            }
        }


       

       

                               
    }

    public static class localisationsettings
    {
        private static IProfile m_profile;


        public static bool mChanged = false;

        public static string[] mmoneyTreeSettings = new string[10];

        public static MainForm mParent;

        public static void SetParent(MainForm pParent)
        {
            mParent = pParent;

            ReadXmlConfig(mParent.ExePath);

        }

        public static void ReadXmlConfig(string inPath)
        {
            string filename = inPath + "\\ConfigFiles\\Settings\\LocalisationSettings.xmc";
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
           


            mmoneyTreeSettings[0] = m_profile.GetValue("LocalisationSettings", "MoneyTreepStep1", "1");
            mmoneyTreeSettings[1] = m_profile.GetValue("LocalisationSettings", "MoneyTreepStep2", "2");
            mmoneyTreeSettings[2] = m_profile.GetValue("LocalisationSettings", "MoneyTreepStep3", "3");
            mmoneyTreeSettings[3] = m_profile.GetValue("LocalisationSettings", "MoneyTreepStep4", "4");
            mmoneyTreeSettings[4] = m_profile.GetValue("LocalisationSettings", "MoneyTreepStep5", "5");
            mmoneyTreeSettings[5] = m_profile.GetValue("LocalisationSettings", "MoneyTreepStep6", "6");
            mmoneyTreeSettings[6] = m_profile.GetValue("LocalisationSettings", "MoneyTreepStep7", "7");
            mmoneyTreeSettings[7] = m_profile.GetValue("LocalisationSettings", "MoneyTreepStep8", "8");
            mmoneyTreeSettings[8] = m_profile.GetValue("LocalisationSettings", "MoneyTreepStep9", "9");
            mmoneyTreeSettings[9] = m_profile.GetValue("LocalisationSettings", "MoneyTreepStep10", "10");

            if (File.Exists(filenamexml))
            {
                File.Delete(filenamexml);
            } 

        }

        public static void WriteXmlConfig(string inPath)
        {
            string filename = inPath + "\\ConfigFiles\\Settings\\LocalisationSettings.xmc";
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
            //m_profile.Name = inPath + "\\ConfigFiles\\LocalisationSettings.xml";



            m_profile.SetValue("LocalisationSettings", "MoneyTreepStep1", mmoneyTreeSettings[0]);
            m_profile.SetValue("LocalisationSettings", "MoneyTreepStep2", mmoneyTreeSettings[1]);
            m_profile.SetValue("LocalisationSettings", "MoneyTreepStep3", mmoneyTreeSettings[2]);
            m_profile.SetValue("LocalisationSettings", "MoneyTreepStep4", mmoneyTreeSettings[3]);
            m_profile.SetValue("LocalisationSettings", "MoneyTreepStep5", mmoneyTreeSettings[4]);
            m_profile.SetValue("LocalisationSettings", "MoneyTreepStep6", mmoneyTreeSettings[5]);
            m_profile.SetValue("LocalisationSettings", "MoneyTreepStep7", mmoneyTreeSettings[6]);
            m_profile.SetValue("LocalisationSettings", "MoneyTreepStep8", mmoneyTreeSettings[7]);
            m_profile.SetValue("LocalisationSettings", "MoneyTreepStep9", mmoneyTreeSettings[8]);
            m_profile.SetValue("LocalisationSettings", "MoneyTreepStep10", mmoneyTreeSettings[9]);

            // now encrypt...
            SettingsEncrypt.Encrypt(filenamexml, filename);
            // and delete
            File.Delete(filenamexml);
        }
    }
}
