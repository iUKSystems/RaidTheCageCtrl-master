using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RaidTheCageCtrl
{
    public partial class PickUpTheGame : Form
    {
        public int mQnr = 0;
        public int mTotal = 0;

        public bool mQuestionSwitch;
        public bool mPeopleSwitch;

        public bool mGoToStopPopup = false;

        public PickUpTheGame()
        {
            InitializeComponent();
            cbQnr.SelectedIndex = 0;
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;

            mQnr = cbQnr.SelectedIndex;

            int.TryParse(tbTotalAmount.Text, out mTotal);

            mQuestionSwitch = cbQuestionSwitch.Checked;
            mPeopleSwitch = cbPeopleSwitch.Checked;
            mGoToStopPopup = cbGoToContueStop.Checked;

            Close();
        }

        private void cbQnr_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbQnr.SelectedIndex > 4)
            {
                lbcontinuestop.Visible = true;
                cbGoToContueStop.Visible = true;
            }
            else
            {
                lbcontinuestop.Visible = false;
                cbGoToContueStop.Visible = false;
            }
        }
    }
}
