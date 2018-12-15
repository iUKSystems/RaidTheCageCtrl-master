using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RaidTheCageCtrl;
using WiseGuys;
using WiseGuysFrameWork2015;


namespace WiseGuys.Ctrls
{
    public partial class ucGameCtrl : ucBaseCtrl
    {
        private ucBaseGame mbasctrl;        // used for message get buttonstate again

        private RaidTheCageCtrl.MainForm mParent;

        public ucGameCtrl(RaidTheCageCtrl.MainForm pParent)
        {
            mParent = pParent;
            //mParent.OnCtrlPressed += OnCtrlPressed;

            mbasctrl = new ucBaseGame();
            mbasctrl.OnGetButtonState += OnGetButtonState;

            InitializeComponent();            

            UpdateDlg();

            this.Size = mParent.GetSizetcCtrls();
        }

        private void OnGetButtonState(object sender)
        {
            UpdateDlg();
        }

        /*
        private void OnCtrlPressed(object sender, bool pressed)
        {
            /*
            if (pressed)
            {
                btGameCtrlStart.Text = "Start/No Sel Question";
                btGameCtrlStart.BackColor = ColorCtrls.mCtrlColorBack;
                btGameCtrlStart.ForeColor = ColorCtrls.mCtrlColorText;
            }
            else
            {
                btGameCtrlStart.Text = "Start";
                btGameCtrlStart.BackColor = ColorCtrls.mBackcolor;
                btGameCtrlStart.ForeColor = ColorCtrls.mTextcolor;
            }
         
        }
        */
          
        private void btGameCtrlStart_Click(object sender, EventArgs e)
        {
            OnStart();            
            btfocus.Focus();
        }

        public override void UpdateDlg()
        {
            if (mParent.ActiveGameCtrl == null)
                return;
                ButtonStates curbstate = mParent.ActiveGameCtrl.GetButtonSate();

            // check callgame or studio game and change buttons!
           

            
            //{
            //if (mParent.ActiveGameCtrl.mGameType == GameType.StudioGame)            

            btGameCtrlContinue.Enabled = (curbstate & ButtonStates.MAYCONTINUE) == ButtonStates.MAYCONTINUE;
            btCorrect.Enabled = (curbstate & ButtonStates.MAYTRUE) == ButtonStates.MAYTRUE;
            btStuckInCase.Enabled = (curbstate & ButtonStates.MAYFALSE) == ButtonStates.MAYFALSE;
            btContestantStepsForward.Enabled = (curbstate & ButtonStates.MAYSTEPFORWARD) == ButtonStates.MAYSTEPFORWARD;
            //btTimeOut.Enabled = (curbstate & ButtonStates.MAYTIMEOUT) == ButtonStates.MAYTIMEOUT; 

            if (btGameCtrlContinue.Enabled)
                btGameCtrlContinue.ForeColor = Color.Black;
            else
                btGameCtrlContinue.ForeColor = Color.White;
            // and get the current text
            btGameCtrlContinue.Text = mParent.ActiveGameCtrl.GetContinueText();

            // and enable or disable the undo/redo buttons
            btGameCtrlUndo.Enabled = mParent.ActiveGameCtrl.CanUndo();
            btGameCtrlRedo.Enabled = mParent.ActiveGameCtrl.CanRedo();

            bttestbutton.Text = mParent.ActiveGameCtrl.GetContinueTextEnglish();

            Font testfont1 = sizeTextToControl(bttestbutton, this.CreateGraphics(), 5);

            bttestbutton.Text = mParent.ActiveGameCtrl.GetContinueTextSecondLanguage();
            Font testfont2 = sizeTextToControl(bttestbutton, this.CreateGraphics(), 5);

            if (mParent.mCurLanguage == eLanguage.SECONDLANGUAGEENGLISH)
            {
                if (testfont1.Size > testfont2.Size)
                {
                    btGameCtrlContinue.Font = testfont2;
                }
                else
                    btGameCtrlContinue.Font = testfont1;
            }
            else if (mParent.mCurLanguage == eLanguage.ENGLISH)
            {
                btGameCtrlContinue.Font = testfont1;
            }
            else
            {
                btGameCtrlContinue.Font = testfont2;
            }

            //btGameCtrlContinue.Font = sizeTextToControl(btGameCtrlContinue, this.CreateGraphics(), 10);

        }

        /// <summary>
        /// Size control text to control size
        /// </summary>
        /// <param name="control">The control to find the font size that should be used.</param>
        /// <param name="graphic">The graphics context.</param>
        /// <param name="padding">The padding around the text.</param>
        /// <returns>The font the control should use.</returns>
        internal static Font sizeTextToControl(Control control, Graphics graphic, int padding)
        {
            // Create a small font
            Font font;
            font = new Font(control.Font.FontFamily, 4.0f, control.Font.Style);
            SizeF textSize = graphic.MeasureString(control.Text, font);

            if (textSize.Width > 0)
            {
                // Loop until it fits perfect
                while ((textSize.Width < (control.Width - padding)) && (textSize.Height < (control.Height - padding)))
                {
                    font = new Font(font.FontFamily, font.Size + 0.5f, font.Style);
                    textSize = graphic.MeasureString(control.Text, font);
                }
                font = new Font(font.FontFamily, font.Size - 0.5f, font.Style);
            }
            return font;
        }

        private void btGameCtrlContinue_Click(object sender, EventArgs e)
        {
            OnContinue();
            btfocus.Focus();
        }


        public override void ContinuebtnEnable(bool Enable)
        {
            if (!Enable)
                btGameCtrlContinue.BackColor = Color.Red;
            else
            {
                btGameCtrlContinue.BackColor = Color.Transparent;
            }
            btGameCtrlContinue.Refresh();
        }
        

        public override void OnContinue()
        {
            if (btGameCtrlContinue.Enabled)
            {
                if (mParent.ActiveGameCtrl != null)
                {
                    mParent.ActiveGameCtrl.OnContinue();
                }
                UpdateDlg();
            }
        }

        public override void OnTimeOut()
        {
            if (btGameCtrlTimeOut.Enabled)
            {
                if (mParent.ActiveGameCtrl != null)
                {
                    mParent.ActiveGameCtrl.OnTimeOut();
                }
                UpdateDlg();
            }
        }

        private void btGameCtrlUndo_Click(object sender, EventArgs e)
        {
            OnUndo();
            btfocus.Focus();
        }

        private void btGameCtrlRedo_Click(object sender, EventArgs e)
        {
            OnRedo();
            btfocus.Focus();
        }

        private void OnUndo()
        {
            if (btGameCtrlUndo.Enabled)
            {
                if (mParent.ActiveGameCtrl != null)
                {
                    mParent.ActiveGameCtrl.OnUndo();
                }
                UpdateDlg();
            }
        }

        private void OnRedo()
        {
            if (btGameCtrlRedo.Enabled)
            {
                if (mParent.ActiveGameCtrl != null)
                {
                    mParent.ActiveGameCtrl.OnRedo();
                }
                UpdateDlg();
            }
        }

        private void ucGameCtrl_KeyPress(object sender, KeyPressEventArgs e)
        {
        }
        

        private void btGameCtrlTimeOut_Click(object sender, EventArgs e)
        {

        }       

        /*
        public void Focus()
        {
            btfocus.Focus();
        }
         */

        private void ucGameCtrl_Resize(object sender, EventArgs e)
        {
            this.Size = mParent.GetSizetcCtrls();
        }

        private void btCorrect_Click(object sender, EventArgs e)
        {
            if (btCorrect.Enabled)
            {
                if (mParent.ActiveGameCtrl != null)
                {
                    mParent.ActiveGameCtrl.OnTrue();
                }
                UpdateDlg();
            }
        }

        /*
        private void btWrong_Click(object sender, EventArgs e)
        {
            if (btWrong.Enabled)
            {
                if (mParent.ActiveGameCtrl != null)
                {
                    mParent.ActiveGameCtrl.OnFalse();
                }
                UpdateDlg();
            }
        }
         */

        private void btContestantStepsForward_Click(object sender, EventArgs e)
        {
            if (btContestantStepsForward.Enabled)
            {
                if (mParent.ActiveGameCtrl != null)
                {
                    mParent.ActiveGameCtrl.OnStepForward();
                }
                UpdateDlg();
            }
        }

        private void btStuckInCase_Click(object sender, EventArgs e)
        {
            if (btStuckInCase.Enabled)
                if (mParent.ActiveGameCtrl != null)
                {
                    mParent.ActiveGameCtrl.OnFalse();
                }
            UpdateDlg();
        }

        public Button getButton()
        {
            return bttestbutton;
        }
        /*
        private void btTimeOut_Click(object sender, EventArgs e)
        {
            if (btTimeOut.Enabled)
            {
                if (mParent.ActiveGameCtrl != null)
                {
                    mParent.ActiveGameCtrl.OnTimeOut();
                }
                UpdateDlg();
            }
        }
         */
    }
}
