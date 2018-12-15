using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DejaVu;
using RaidTheCageCtrl.GameParts;
using WiseGuysFrameWork.Audio;

namespace RaidTheCageCtrl
{
    public partial class ContinueOrStop : Form
    {
        private MainForm mParent;
        private ucRaidTheCage tParent;

        private bool mMoneytreeshown = false;

        public ContinueOrStop(MainForm pParent, ucRaidTheCage sParent)
        {
            InitializeComponent();

            mParent = pParent;
            tParent = sParent;

           
        }

        private void btYes_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
            Close();
        }

        private void btNo_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
            Close();
        }

        private void btShowTotalAmount_Click(object sender, EventArgs e)
        {
            tParent.ShowTotalAmount(true);
            UpdateDlg();
        }

        private void UpdateDlg()
        {
            btShowTotalAmount.Text = tParent.mTotalAmountShown.Value ? "Hide Total Amount" : "Show Total Amount";
            btShowTotalAmount.BackColor = tParent.mTotalAmountShown.Value ? Color.YellowGreen : SystemColors.Control;
            btShowTotalAmount.UseVisualStyleBackColor = !tParent.mTotalAmountShown.Value;
            btShowTotalAmount.Enabled = !mMoneytreeshown;

            btShowMoneyTree.Text = mMoneytreeshown ? "Hide MoneyTree" : "Show MoneyTree";
            btShowMoneyTree.BackColor = mMoneytreeshown ? Color.YellowGreen : SystemColors.Control;
            btShowMoneyTree.UseVisualStyleBackColor = !mMoneytreeshown;
            btShowMoneyTree.Enabled = !tParent.mTotalAmountShown.Value;
        }

        private void btShowMoneyTree_Click(object sender, EventArgs e)
        {
            if (!mMoneytreeshown)
            {
                if (mParent.mQnr.Value == 8) // last question (still next question after this...
                {
                    if (tParent.lLifelinesAvailable[1].Value)
                    {
                        btHideSwitchPlayerLL.Visible = true;
                    }
                }

                mParent.gcRaidTheCage.ShowMoneyTreeAll(mParent.mQnr.Value + 1);
                mMoneytreeshown = true;
            }
            else
            {
                // hide moneytree
                btHideSwitchPlayerLL.Visible = false;

                if (tParent.lLifelinesAvailable[1].Value)
                {
                    tParent.projector.Play("moneytree.switchpeopleanim", 0, 50, -50, false, 1);
                    tParent.overlay.Play("moneytree.switchpeopleanim", 0, 50, -50, false, 1);
                }
                else
                {
                    tParent.projector.Play("moneytree.switchpeopleanimused", 0, 50, -50, false, 1);
                    tParent.overlay.Play("moneytree.switchpeopleanimused", 0, 50, -50, false, 1);
                }

                if (tParent.lLifelinesAvailable[0].Value)
                {
                    tParent.projector.Play("moneytree.switchquestionanim", 0, 50, -50, false, 1);
                    tParent.overlay.Play("moneytree.switchquestionanim", 0, 50, -50, false, 1);
                }
                else
                {
                    tParent.projector.Play("moneytree.switchquestionanimused", 0, 50, -50, false, 1);
                    tParent.overlay.Play("moneytree.switchquestionanimused", 0, 50, -50, false, 1);
                }

                //tParent.overlay.Start("moneytree.hidelifelines");
                //tParent.projector.Start("moneytree.hidelifelines");
                tParent.overlay.Start("moneytree.hide");
                tParent.projector.Start("moneytree.hide");
                mMoneytreeshown = false;
            }
            UpdateDlg();
        }

        private void ContinueOrStop_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (mMoneytreeshown)
            {
                // hide moneytree
                btHideSwitchPlayerLL.Visible = false;

                if (tParent.lLifelinesAvailable[1].Value)
                {
                    tParent.projector.Play("moneytree.switchpeopleanim", 0, 50, -50, false, 1);
                    tParent.overlay.Play("moneytree.switchpeopleanim", 0, 50, -50, false, 1);
                }
                else
                {
                    tParent.projector.Play("moneytree.switchpeopleanimused", 0, 50, -50, false, 1);
                    tParent.overlay.Play("moneytree.switchpeopleanimused", 0, 50, -50, false, 1);
                }

                if (tParent.lLifelinesAvailable[0].Value)
                {
                    tParent.projector.Play("moneytree.switchquestionanim", 0, 50, -50, false, 1);
                    tParent.overlay.Play("moneytree.switchquestionanim", 0, 50, -50, false, 1);
                }
                else
                {
                    tParent.projector.Play("moneytree.switchquestionanimused", 0, 50, -50, false, 1);
                    tParent.overlay.Play("moneytree.switchquestionanimused", 0, 50, -50, false, 1);
                }

                //tParent.overlay.Start("moneytree.hidelifelines");
                //tParent.projector.Start("moneytree.hidelifelines");
                tParent.overlay.Start("moneytree.hide");
                tParent.projector.Start("moneytree.hide");
                mMoneytreeshown = false;
            }
        }

        private void btHideSwitchPlayerLL_Click(object sender, EventArgs e)
        {
            mParent.mAudio.SendAudio(SoundsRaidTheCage.HIDESWITCHPLAYERLLSTOPORPLAY, SoundCommands.PLAY, false, false);
            using (UndoRedoManager.StartInvisible("Continue"))
            {
                tParent.lLifelinesAvailable[1].Value = false;
                UndoRedoManager.Commit();
            }
            tParent.projector.Start("moneytree.endswitchplayer");
            tParent.overlay.Start("moneytree.endswitchplayer");
            btHideSwitchPlayerLL.Visible = false;
            // at the end just hide the button...
        }
    }
}
