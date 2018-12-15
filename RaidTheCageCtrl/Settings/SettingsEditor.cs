using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using RaidTheCageCtrl;
using RaidTheCageCtrl.Settings;
using WiseGuys;


enum ESettingsForms : int
{
    Connections = 0,
    ClockSettings,
    Localisation,
    Midi,
    MAX,
}

namespace WiseGuys.Settings
{
    public partial class SettingsEditor : Form
    {
        private MainForm mParent;

        // settings
        private ucConnectionSettings gcconnectionsettings;
        private ucGeneralSettings gcgeneralsettings;
        private ucLocalisationSettings gclocalisationsettings;
        private ucLocalisationTextSettings gclocalisation;
        private ucMidiTextSettings gcmidi;


        private bool mPswok = false;
        

        public bool[] mChanged = new bool[(int)ESettingsForms.MAX];

        public SettingsEditor(MainForm pParent, bool pswok)
        {
            mParent = pParent;
            InitializeComponent();
            mPswok = pswok;
            CreateTree();
            tvQEGameTypeSelect.ExpandAll();
            tvQEGameTypeSelect.Focus();


        }
        
        private void CreateTree()
        {
            tvQEGameTypeSelect.Nodes.Clear();
            TreeNode Settings = tvQEGameTypeSelect.Nodes.Add("Settings");
            TreeNode newnode;

            gcgeneralsettings = new ucGeneralSettings(mParent,this);
            newnode = new TreeNode("General Settings");
            newnode.Name = "QEGeneral";
            Settings.Nodes.Add(newnode);

            // select the general one..
            tvQEGameTypeSelect.SelectedNode = newnode;

            gclocalisationsettings = new ucLocalisationSettings(mParent,this);
            newnode = new TreeNode("Localisation Settings");
            newnode.Name = "QELocalisationsettings";
            Settings.Nodes.Add(newnode);


            gcconnectionsettings = new ucConnectionSettings(mParent, this);
            newnode = new TreeNode("Connection Settings");
            newnode.Name = "QEConnections";
            Settings.Nodes.Add(newnode);

            gclocalisation = new ucLocalisationTextSettings(mParent, this);
            newnode = new TreeNode("Localisation String Settings");
            newnode.Name = "QELocalisation";
            Settings.Nodes.Add(newnode);

            gcmidi = new ucMidiTextSettings(mParent,this);
            newnode = new TreeNode("Midi Settings");
            newnode.Name = "QEMidi";
            Settings.Nodes.Add(newnode);
                       
        }

        private void btSettingsCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btSettingsOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void tvQEGameTypeSelect_AfterSelect(object sender, TreeViewEventArgs e)
        {
             TreeNode nodeclicked;
            nodeclicked = e.Node;

            
            // clear
            flPanelSettings.Controls.Clear();


            switch (nodeclicked.Name)
            {
                case "QEGeneral":
                    // add them
                    flPanelSettings.Controls.Add(gcgeneralsettings);
                    break; 
                case "QEConnections":
                    // add them
                    flPanelSettings.Controls.Add(gcconnectionsettings);
                    break; 
                case "QELocalisationsettings":
                    // add them
                    flPanelSettings.Controls.Add(gclocalisationsettings);
                    break;
                case "QELocalisation":
                    // add them
                    flPanelSettings.Controls.Add(gclocalisation);
                    break; 
                case "QEMidi":
                    // add them
                    flPanelSettings.Controls.Add(gcmidi);
                    break; 
            }
        }

        /// <summary>
        /// This event is fired as soon as the form is closing
        /// </summary>
        public FormClosingMsg OnFormClosingMsg;

        /// <summary>
        /// Delegated for FormClosingMsg
        /// </summary>
        public delegate void FormClosingMsg(object sender);

        private void SettingsEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            // send msgs to all the round that the form is closing for saving purposes
            if (OnFormClosingMsg != null)
            {
                OnFormClosingMsg(this);
            }

            // ok.. everything should be saved now... so check the directory for xml files...
            // if found... encrypt and save encrypted and delete xml file...
            /*
            string[] filePaths = Directory.GetFiles(mParent.ExePath + "\\ConfigFiles", "*.xml");

            for (int i=0;i<filePaths.Length;i++)
            {
                if (CheckFileNameEncrypt(Path.GetFileNameWithoutExtension(filePaths[i])))
                {
                    string inFilename = filePaths[i];
                    string outfilename = mParent.ExePath + "\\ConfigFiles\\" +
                                         Path.GetFileNameWithoutExtension(filePaths[i]) + ".xmc";
                    SettingsEncrypt.Encrypt(inFilename, outfilename);
                    // delete xml file...
                    File.Delete(filePaths[i]);
                }
            }
             */
        }

        private bool CheckFileNameEncrypt(string inFilename)
        {
            // only return if the filename are settings behind the password...
            if (inFilename == "MoneyTreeSettings" || inFilename == "LifeLineSettings" || inFilename == "WWTBAMLevelClockSettings" || inFilename == "WWTBAMLevelClockSettings2")
            {
                return true;
            }

            return false;
        }

        internal bool SettingChanged()
        {
            for (int i=0;i<(int)ESettingsForms.MAX;i++)
            {
                if (mChanged[i])
                    return true;
            }
            return false;
        }
        
    }
}
