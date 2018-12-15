using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RaidTheCageCtrl.GameParts;
using WiseGuys.Ctrls;
using WiseGuysFrameWork2015;
using WiseGuysFrameWork2015DIV;

namespace RaidTheCageCtrl
{
    public partial class MainForm : WGForm
    {       

        // gameparts
        public ucRaidTheCage gcRaidTheCage;

        private TabPage tpRaidTheCage;

        // ctrls below
        // ctrls below
        private WiseGuys.Ctrls.ucGameCtrl gcGameCtrl;

        // tab below
        private TabPage tpGameCtrl;

        private void CreateTabs()
        {            

            //MessageBox.Show("AddMainGame");
            // addMainGame
            gcRaidTheCage = new ucRaidTheCage(this);
            tpRaidTheCage = new TabPage();
            tpRaidTheCage.Text = "Raid The Cage";
            tpRaidTheCage.Controls.Add(gcRaidTheCage);
            tcGameParts.TabPages.Add(tpRaidTheCage);
            //lGamectrls.Add(gcRaidTheCage);
            ActiveGameCtrl = gcRaidTheCage;

            //MessageBox.Show("AddCtrlsBelow");
            // ctrls below
            // addgame controll
            gcGameCtrl = new ucGameCtrl(this);
            tpGameCtrl = new TabPage();

            tpGameCtrl.Text = "Game Control";
            tpGameCtrl.Controls.Add(gcGameCtrl);
            tcGameCtrl.TabPages.Add(tpGameCtrl);
            lGamectrls.Add(gcGameCtrl);
            ActiveCtrl = gcGameCtrl;
        }

       

       

    }
}
