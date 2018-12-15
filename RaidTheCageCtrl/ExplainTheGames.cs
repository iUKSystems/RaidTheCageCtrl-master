using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WiseGuys.WGNetWork2011;

namespace RaidTheCageCtrl
{
    public partial class ExplainTheGames : Form
    {
        private MainForm mParent;

        private WGGFXEngine overlay = new WGGFXEngine("OVERLAY");
        private WGGFXEngine projector = new WGGFXEngine("PROJECTOR");

        private bool mMoneytreeonscreen = false;

        private bool mShowLifelines = false;

        private int mQsel = -1;



        public ExplainTheGames(MainForm pParent)
        {
            InitializeComponent();

            mParent = pParent;

            overlay = mParent.mGFXEngines[(int) eEngines.OVERLAY];
            projector = mParent.mGFXEngines[(int)eEngines.PROJECTOR];

            overlay.Start("moneytree.init");
            projector.Start("moneytree.init");

            UpdateDlg();
        }

        private void UpdateDlg()
        {
            btETGCageQ.Enabled = mMoneytreeonscreen && mQsel < 9;
            btETGQ1.Enabled = mMoneytreeonscreen && mQsel < 0;
            btETGQ2.Enabled = mMoneytreeonscreen && mQsel < 1;
            btETGQ3.Enabled = mMoneytreeonscreen && mQsel < 2;
            btETGQ4.Enabled = mMoneytreeonscreen && mQsel < 3;
            btETGQ5.Enabled = mMoneytreeonscreen && mQsel < 4;
            btETGQ6.Enabled = mMoneytreeonscreen && mQsel < 5;
            btETGQ7.Enabled = mMoneytreeonscreen && mQsel < 6;
            btETGQ8.Enabled = mMoneytreeonscreen && mQsel < 7;
            btETGQ9.Enabled = mMoneytreeonscreen && mQsel < 8;

            btETGCageQ.BackColor = mQsel == 9 ? Color.YellowGreen : SystemColors.Control;
            btETGQ9.BackColor = mQsel == 8 ? Color.YellowGreen : SystemColors.Control;
            btETGQ8.BackColor = mQsel == 7 ? Color.YellowGreen : SystemColors.Control;
            btETGQ7.BackColor = mQsel == 6 ? Color.YellowGreen : SystemColors.Control;
            btETGQ6.BackColor = mQsel == 5 ? Color.YellowGreen : SystemColors.Control;
            btETGQ5.BackColor = mQsel == 4 ? Color.YellowGreen : SystemColors.Control;
            btETGQ4.BackColor = mQsel == 3 ? Color.YellowGreen : SystemColors.Control;
            btETGQ3.BackColor = mQsel == 2 ? Color.YellowGreen : SystemColors.Control;
            btETGQ2.BackColor = mQsel == 1 ? Color.YellowGreen : SystemColors.Control;
            btETGQ1.BackColor = mQsel == 0 ? Color.YellowGreen : SystemColors.Control;

            btETGCageQ.UseVisualStyleBackColor = mQsel != 9;
            btETGQ9.UseVisualStyleBackColor = mQsel != 8;
            btETGQ8.UseVisualStyleBackColor = mQsel != 7;
            btETGQ7.UseVisualStyleBackColor = mQsel != 6;
            btETGQ6.UseVisualStyleBackColor = mQsel != 5;
            btETGQ5.UseVisualStyleBackColor = mQsel != 4;
            btETGQ4.UseVisualStyleBackColor = mQsel != 3;
            btETGQ3.UseVisualStyleBackColor = mQsel != 2;
            btETGQ2.UseVisualStyleBackColor = mQsel != 1;
            btETGQ1.UseVisualStyleBackColor = mQsel != 0;

            btETGShowMoneyTree.Text = mMoneytreeonscreen ? "Hide Money Tree" : "Show Money Tree";
            btShowLifelines.Text = mShowLifelines ? "Hide LifeLines" : "Show LifeLines";
            btShowLifelines.BackColor = mShowLifelines ? Color.YellowGreen : SystemColors.Control;
            btShowLifelines.UseVisualStyleBackColor = !mShowLifelines;

            btPingSwitchQuestion.Enabled = mShowLifelines;
            btPingSwitchPlayer.Enabled = mShowLifelines;

        }

        private void btETGShowMoneyTree_Click(object sender, EventArgs e)
        {
            if (!mMoneytreeonscreen)
            {
                overlay.Start("moneytree.textbaron");
                projector.Start("moneytree.textbaron");
                overlay.Start("moneytree.show");
                projector.Start("moneytree.show");
                mMoneytreeonscreen = true;
            }
            else
            {
                overlay.Start("moneytree.hide");
                projector.Start("moneytree.hide");
                mMoneytreeonscreen = false;
                if (mShowLifelines)
                {
                    overlay.Start("moneytree.hidelifelines");
                    projector.Start("moneytree.hidelifelines");                    
                }
                Close();
            }
            UpdateDlg();
        }

        private void ClickQuestion(int p)
        {
            mQsel = p;

            string id = string.Format("moneytree.show{0}", p + 1);
            overlay.Start(id);
            projector.Start(id);
            UpdateDlg();
        }

        private void btETGCageQ_Click(object sender, EventArgs e)
        {
            ClickQuestion(9);
        }
        
        private void btETGQ9_Click(object sender, EventArgs e)
        {
            ClickQuestion(8);
        }

        private void btETGQ8_Click(object sender, EventArgs e)
        {
            ClickQuestion(7);
        }

        private void btETGQ7_Click(object sender, EventArgs e)
        {
            ClickQuestion(6);
        }

        private void btETGQ6_Click(object sender, EventArgs e)
        {
            ClickQuestion(5);
        }

        private void btETGQ5_Click(object sender, EventArgs e)
        {
            ClickQuestion(4);
        }

        private void btETGQ4_Click(object sender, EventArgs e)
        {
            ClickQuestion(3);
        }

        private void btETGQ3_Click(object sender, EventArgs e)
        {
            ClickQuestion(2);
        }

        private void btETGQ2_Click(object sender, EventArgs e)
        {
            ClickQuestion(1);
        }

        private void btETGQ1_Click(object sender, EventArgs e)
        {
            ClickQuestion(0);
        }

        private void ExplainTheGames_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (mMoneytreeonscreen)
            {
                overlay.Start("moneytree.hide");
                projector.Start("moneytree.hide");
                mMoneytreeonscreen = false;
                if (mShowLifelines)
                {
                    overlay.Start("moneytree.hidelifelines");
                    projector.Start("moneytree.hidelifelines");
                }                
            }
        }

        private void btShowLifelines_Click(object sender, EventArgs e)
        {
            if (!mShowLifelines)
            {
                overlay.Start("moneytree.showlifelines");
                projector.Start("moneytree.showlifelines");
                mShowLifelines = true;
            }
            else
            {
                overlay.Start("moneytree.hidelifelines");
                projector.Start("moneytree.hidelifelines");
                mShowLifelines = false;
            }
            UpdateDlg();
        }

        private void btPingSwitchQuestion_Click(object sender, EventArgs e)
        {
            projector.Start("moneytree.questionswitchlifelineping");
            overlay.Start("moneytree.questionswitchlifelineping");
        }

        private void btPingSwitchPlayer_Click(object sender, EventArgs e)
        {
            projector.Start("moneytree.peopleswitchlifelineping");
            overlay.Start("moneytree.peopleswitchlifelineping");
        }
    }
}
