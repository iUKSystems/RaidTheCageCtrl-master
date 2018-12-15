using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using RaidTheCageCtrl;
using WiseGuys.configs;
using WiseGuysFrameWork2015.Settings;

namespace WiseGuys.Settings
{
    public partial class ucConnectionSettings : SettingsBase
    {
        private MainForm mParent;       
       
        private bool IsUpdating = false;

        public ucConnectionSettings(MainForm pParent, SettingsEditor pSetParent, int InNumber)
            : base(pParent, pSetParent, InNumber)
        {
            InitializeComponent();   
            
            mSetParent.OnFormClosingMsg += OnFormClosingMsg;
            
            mParent = pParent;
                                

            UpdateDlg();
        }

        private void Changed()
        {
            connectionsettings.mChanged = true;
            mChanged = true;
        }

        private void OnFormClosingMsg(object sender)
        {
            if (connectionsettings.mChanged)
            {
                // save...
                //mSetParent.mChanged[(int)ESettingsForms.Connections] = true;
                connectionsettings.WriteXmlConfig(mParent.ExePath);
                connectionsettings.mChanged = false;
            }
        }

        private void AddSpecialEngineChanged(eSpecialEngines inEngine)
        {
            bool consistitem = mParent.mSettingsspecialengineschanged.Contains((int)inEngine);
            if (!consistitem)
                mParent.mSettingsspecialengineschanged.Add((int)inEngine);
        }

        private void AddEngineChanged(eEngines inEngine)
        {
            bool consistitem = mParent.mSettingsengineschanged.Contains((int)inEngine);
            if (!consistitem)
                mParent.mSettingsengineschanged.Add((int)inEngine);
        }
        
        private void UpdateDlg()
        {
            IsUpdating = true;

            // ipaddresses
            tbIpnumberOverlay.Text = connectionsettings.mIpAddresses[(int)eEngines.OVERLAY];
            tbIpnumberProjector.Text = connectionsettings.mIpAddresses[(int)eEngines.PROJECTOR];                        
            tbIpnumberWISEQ.Text = connectionsettings.mIpAddresses[(int)eEngines.WISEQ];            
            tbIpnumberHOST.Text = connectionsettings.mIpAddresses[(int)eEngines.HOST];
            tbIpnumberWISEAUDIO.Text = connectionsettings.mIpAddresses[(int)eEngines.WISEAUDIO];
            tbIpnumberCage.Text = connectionsettings.mIpAddresses[(int)eEngines.CAGE];
            tbIpnumberMidi.Text = connectionsettings.mIpAddresses[(int)eEngines.MIDI];
            tbIpnumberExtraQA.Text = connectionsettings.mIpAddresses[(int)eEngines.EXTRAQA];

           
           // ports
            tbportOverlay.Text = connectionsettings.mPorts[(int)eEngines.OVERLAY].ToString();
            tbportProjector.Text = connectionsettings.mPorts[(int)eEngines.PROJECTOR].ToString();                        
            tbportWISEQ.Text = connectionsettings.mPorts[(int)eEngines.WISEQ].ToString();            
            tbportHOST.Text = connectionsettings.mPorts[(int)eEngines.HOST].ToString();            
            tbportWISEAUDIO.Text = connectionsettings.mPorts[(int)eEngines.WISEAUDIO].ToString();
            tbportCage.Text = connectionsettings.mPorts[(int)eEngines.CAGE].ToString();
            tbportMidi.Text = connectionsettings.mPorts[(int)eEngines.MIDI].ToString();
            tbportExtraQA.Text = connectionsettings.mPorts[(int)eEngines.EXTRAQA].ToString();

            tbportHostMessaging.Text = connectionsettings.mPorts[(int)eEngines.HOSTMESSAGING].ToString();

            tbIpnumberHOST2.Text = connectionsettings.mIpAddressesHost2;
            tbportHOST2.Text = connectionsettings.mPortHost2.ToString();

            IsUpdating = false;
        }

        private void tbIpnumberOverlay_TextChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                connectionsettings.mIpAddresses[(int)eEngines.OVERLAY] = tbIpnumberOverlay.Text;
                AddEngineChanged(eEngines.OVERLAY);
                Changed();
            }
        }    

        private void tbIpnumberPROJECTOR_TextChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                connectionsettings.mIpAddresses[(int) eEngines.PROJECTOR] = tbIpnumberProjector.Text;
                AddEngineChanged(eEngines.PROJECTOR);
                Changed();
            }
        }
        

        private void tbIpnumberWISEQ_TextChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                connectionsettings.mIpAddresses[(int)eEngines.WISEQ] = tbIpnumberWISEQ.Text;
                AddEngineChanged(eEngines.WISEQ);
                Changed();
            }
        }
        


        private void tbIpnumberHOST_TextChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                connectionsettings.mIpAddresses[(int)eEngines.HOST] = tbIpnumberHOST.Text;
                AddEngineChanged(eEngines.HOST);
                Changed();
            }
        }

        private void tbIpnumberHOST2_TextChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                connectionsettings.mIpAddressesHost2 = tbIpnumberHOST2.Text;
                AddEngineChanged(eEngines.HOST);
                Changed();
            }
        }


        private void tbIpnumberCage_TextChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                connectionsettings.mIpAddresses[(int)eEngines.CAGE] = tbIpnumberCage.Text;
                AddEngineChanged(eEngines.CAGE);
                Changed();
            }
        }


        private void tbIpnumberMidi_TextChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                connectionsettings.mIpAddresses[(int)eEngines.MIDI] = tbIpnumberMidi.Text;
                AddEngineChanged(eEngines.MIDI);
                Changed();
            }
        }

        private void tbIpnumberExtraQA_TextChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                connectionsettings.mIpAddresses[(int)eEngines.EXTRAQA] = tbIpnumberExtraQA.Text;
                AddEngineChanged(eEngines.EXTRAQA);
                Changed();
            }
        }

        private void tbIpnumberWISEAUDIO_TextChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                connectionsettings.mIpAddresses[(int)eEngines.WISEAUDIO] = tbIpnumberWISEAUDIO.Text;
                AddEngineChanged(eEngines.WISEAUDIO);
                Changed();
            }

        }


        private void tbportOverlay_TextChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                int.TryParse(tbportOverlay.Text, out connectionsettings.mPorts[(int)eEngines.OVERLAY]);
                AddEngineChanged(eEngines.OVERLAY);
                Changed();
            }
        }


        private void tbportPROJECTOR_TextChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                int.TryParse(tbportProjector.Text, out connectionsettings.mPorts[(int) eEngines.PROJECTOR]);
                AddEngineChanged(eEngines.PROJECTOR);
                Changed();
            }
        }

        private void tbportWISEQ_TextChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                int.TryParse(tbportWISEQ.Text, out connectionsettings.mPorts[(int)eEngines.WISEQ]);
                AddEngineChanged(eEngines.WISEQ);
                Changed();
            }
        }

        

        


        private void tbportHOST_TextChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                int.TryParse(tbportHOST.Text, out connectionsettings.mPorts[(int)eEngines.HOST]);
                AddEngineChanged(eEngines.HOST);
                Changed();
            }
        }

         private void tbportHOST2_TextChanged(object sender, EventArgs e)
        {
             if (!IsUpdating)
            {
                int.TryParse(tbportHOST2.Text, out connectionsettings.mPortHost2);
                AddEngineChanged(eEngines.HOST);
                Changed();
            }            
        }

        private void tbportWISEAUDIO_TextChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                int.TryParse(tbportWISEAUDIO.Text, out connectionsettings.mPorts[(int)eEngines.WISEAUDIO]);
                AddEngineChanged(eEngines.WISEAUDIO);
                Changed();
            }
        }

        private void tbportCage_TextChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                int.TryParse(tbportCage.Text, out connectionsettings.mPorts[(int)eEngines.CAGE]);
                AddEngineChanged(eEngines.CAGE);
                Changed();
            }
        }

        private void tbportMidi_TextChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                int.TryParse(tbportMidi.Text, out connectionsettings.mPorts[(int)eEngines.MIDI]);
                AddEngineChanged(eEngines.MIDI);
                Changed();
            }
        }

        private void tbportExtraQA_TextChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                int.TryParse(tbportExtraQA.Text, out connectionsettings.mPorts[(int)eEngines.EXTRAQA]);
                AddEngineChanged(eEngines.EXTRAQA);
                Changed();
            }
        }

        private void tbportHostMessaging_TextChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                int.TryParse(tbportHostMessaging.Text, out connectionsettings.mPorts[(int)eEngines.HOSTMESSAGING]);
                AddSpecialEngineChanged(eSpecialEngines.HOSTMESSAGING);
                Changed();
            }
        }

       



                    
    }

    public static class connectionsettings
    {
        public static bool mChanged = false;

        private static IProfile m_profile;

        public static string[] mIpAddresses = new string[(int)eEngines.MAX];
        public static int[] mPorts = new int[(int)eEngines.MAX];

        public static string mIpAddressesHost2;
        public static int mPortHost2;

        public static MainForm mParent;

        public static void SetParent(MainForm pParent)
        {
            mParent = pParent;

            ReadXmlConfig(mParent.ExePath);

        }

        public static void ReadXmlConfig(string inPath)
        {

            string filename = inPath + "\\ConfigFiles\\Settings\\ConnectionSettings.xmc";
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

            //m_profile.Name = inPath + "\\ConfigFiles\\ConnectionSettings.xml";


            // get all values
            mIpAddresses[(int)eEngines.OVERLAY] = m_profile.GetValue("IPAddresses", "OVERLAY", "127.0.0.1");
            mIpAddresses[(int)eEngines.PROJECTOR] = m_profile.GetValue("IPAddresses", "LEDFLOOR", "127.0.0.1");                        
            mIpAddresses[(int)eEngines.WISEQ] = m_profile.GetValue("IPAddresses", "WISEQ", "127.0.0.1");            
            mIpAddresses[(int)eEngines.HOST] = m_profile.GetValue("IPAddresses", "HOST", "127.0.0.1");
            mIpAddresses[(int)eEngines.WISEAUDIO] = m_profile.GetValue("IPAddresses", "WISEAUDIO", "127.0.0.1");
            mIpAddresses[(int)eEngines.CAGE] = m_profile.GetValue("IPAddresses", "CAGE", "127.0.0.1");
            mIpAddresses[(int)eEngines.MIDI] = m_profile.GetValue("IPAddresses", "MIDI", "127.0.0.1");
            mIpAddresses[(int)eEngines.EXTRAQA] = m_profile.GetValue("IPAddresses", "EXTRAQA", "127.0.0.1");

            mIpAddressesHost2 = m_profile.GetValue("IPAddresses", "HOST2", "127.0.0.1");


            // and the ports

            int.TryParse(m_profile.GetValue("Ports", "OVERLAY", "8020"), out mPorts[(int)eEngines.OVERLAY]);
            int.TryParse(m_profile.GetValue("Ports", "LEDFLOOR", "8021"), out mPorts[(int)eEngines.PROJECTOR]);                        
            int.TryParse(m_profile.GetValue("Ports", "WISEQ", "8022"), out mPorts[(int)eEngines.WISEQ]);
            int.TryParse(m_profile.GetValue("Ports", "WISEAUDIO", "8023"), out mPorts[(int)eEngines.WISEAUDIO]);            
            int.TryParse(m_profile.GetValue("Ports", "HOST", "8024"), out mPorts[(int)eEngines.HOST]);
            int.TryParse(m_profile.GetValue("Ports", "CAGE", "8025"), out mPorts[(int)eEngines.CAGE]);
            int.TryParse(m_profile.GetValue("Ports", "HOSTMESSAGING", "8026"), out mPorts[(int)eEngines.HOSTMESSAGING]);
            int.TryParse(m_profile.GetValue("Ports", "MIDI", "8300"), out mPorts[(int)eEngines.MIDI]);
            int.TryParse(m_profile.GetValue("Ports", "EXTRAQA", "8310"), out mPorts[(int)eEngines.EXTRAQA]);

            int.TryParse(m_profile.GetValue("Ports", "HOST2", "8024"), out mPortHost2);

            if (File.Exists(filenamexml))
            {
                File.Delete(filenamexml);
            } 
        }

        public static void WriteXmlConfig(string inPath)
        {

            string filename = inPath + "\\ConfigFiles\\Settings\\ConnectionSettings.xmc";
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

            m_profile.SetValue("IPAddresses", "OVERLAY", mIpAddresses[(int)eEngines.OVERLAY]);
            m_profile.SetValue("IPAddresses", "LEDFLOOR", mIpAddresses[(int)eEngines.PROJECTOR]);                        
            m_profile.SetValue("IPAddresses", "WISEQ", mIpAddresses[(int)eEngines.WISEQ]);
            m_profile.SetValue("IPAddresses", "WISEAUDIO", mIpAddresses[(int)eEngines.WISEAUDIO]);            
            m_profile.SetValue("IPAddresses", "HOST", mIpAddresses[(int)eEngines.HOST]);
            m_profile.SetValue("IPAddresses", "CAGE", mIpAddresses[(int)eEngines.CAGE]);
            m_profile.SetValue("IPAddresses", "MIDI", mIpAddresses[(int)eEngines.MIDI]);
            m_profile.SetValue("IPAddresses", "EXTRAQA", mIpAddresses[(int)eEngines.EXTRAQA]);

            m_profile.SetValue("IPAddresses", "HOST2", mIpAddressesHost2);



            m_profile.SetValue("Ports", "OVERLAY", mPorts[(int)eEngines.OVERLAY]);
            m_profile.SetValue("Ports", "LEDFLOOR", mPorts[(int)eEngines.PROJECTOR]);                        
            m_profile.SetValue("Ports", "WISEQ", mPorts[(int)eEngines.WISEQ]);
            m_profile.SetValue("Ports", "WISEAUDIO", mPorts[(int)eEngines.WISEAUDIO]);            
            m_profile.SetValue("Ports", "HOST", mPorts[(int)eEngines.HOST]);
            m_profile.SetValue("Ports", "CAGE", mPorts[(int)eEngines.CAGE]);
            m_profile.SetValue("Ports", "HOSTMESSAGING", mPorts[(int)eEngines.HOSTMESSAGING]);
            m_profile.SetValue("Ports", "MIDI", mPorts[(int)eEngines.MIDI]);
            m_profile.SetValue("Ports", "EXTRAQA", mPorts[(int)eEngines.EXTRAQA]);

            m_profile.SetValue("Ports", "HOST2", mPortHost2);

            // now encrypt...
            SettingsEncrypt.Encrypt(filenamexml, filename);
            // and delete
            File.Delete(filenamexml);
            
        }
    }
}




