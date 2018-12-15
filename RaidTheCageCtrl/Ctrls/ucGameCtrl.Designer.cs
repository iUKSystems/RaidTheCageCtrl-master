namespace WiseGuys.Ctrls
{
    partial class ucGameCtrl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btGameCtrlContinue = new System.Windows.Forms.Button();
            this.btGameCtrlUndo = new System.Windows.Forms.Button();
            this.btGameCtrlRedo = new System.Windows.Forms.Button();
            this.btfocus = new System.Windows.Forms.Button();
            this.btGameCtrlTimeOut = new System.Windows.Forms.Button();
            this.btCorrect = new System.Windows.Forms.Button();
            this.btContestantStepsForward = new System.Windows.Forms.Button();
            this.btStuckInCase = new System.Windows.Forms.Button();
            this.bttestbutton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btGameCtrlContinue
            // 
            this.btGameCtrlContinue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btGameCtrlContinue.Location = new System.Drawing.Point(8, 12);
            this.btGameCtrlContinue.Name = "btGameCtrlContinue";
            this.btGameCtrlContinue.Size = new System.Drawing.Size(506, 65);
            this.btGameCtrlContinue.TabIndex = 0;
            this.btGameCtrlContinue.Tag = "XC";
            this.btGameCtrlContinue.Text = "CONTINUE";
            this.btGameCtrlContinue.UseVisualStyleBackColor = true;
            this.btGameCtrlContinue.Click += new System.EventHandler(this.btGameCtrlContinue_Click);
            // 
            // btGameCtrlUndo
            // 
            this.btGameCtrlUndo.Location = new System.Drawing.Point(358, 86);
            this.btGameCtrlUndo.Name = "btGameCtrlUndo";
            this.btGameCtrlUndo.Size = new System.Drawing.Size(75, 23);
            this.btGameCtrlUndo.TabIndex = 2;
            this.btGameCtrlUndo.Text = "Undo";
            this.btGameCtrlUndo.UseVisualStyleBackColor = true;
            this.btGameCtrlUndo.Click += new System.EventHandler(this.btGameCtrlUndo_Click);
            // 
            // btGameCtrlRedo
            // 
            this.btGameCtrlRedo.Location = new System.Drawing.Point(439, 86);
            this.btGameCtrlRedo.Name = "btGameCtrlRedo";
            this.btGameCtrlRedo.Size = new System.Drawing.Size(75, 23);
            this.btGameCtrlRedo.TabIndex = 2;
            this.btGameCtrlRedo.Text = "Redo";
            this.btGameCtrlRedo.UseVisualStyleBackColor = true;
            this.btGameCtrlRedo.Click += new System.EventHandler(this.btGameCtrlRedo_Click);
            // 
            // btfocus
            // 
            this.btfocus.Location = new System.Drawing.Point(538, 58);
            this.btfocus.Name = "btfocus";
            this.btfocus.Size = new System.Drawing.Size(42, 21);
            this.btfocus.TabIndex = 3;
            this.btfocus.Text = "F";
            this.btfocus.UseVisualStyleBackColor = true;
            // 
            // btGameCtrlTimeOut
            // 
            this.btGameCtrlTimeOut.Location = new System.Drawing.Point(525, 8);
            this.btGameCtrlTimeOut.Name = "btGameCtrlTimeOut";
            this.btGameCtrlTimeOut.Size = new System.Drawing.Size(75, 23);
            this.btGameCtrlTimeOut.TabIndex = 1;
            this.btGameCtrlTimeOut.Text = "TimeOut";
            this.btGameCtrlTimeOut.UseVisualStyleBackColor = true;
            this.btGameCtrlTimeOut.Visible = false;
            this.btGameCtrlTimeOut.Click += new System.EventHandler(this.btGameCtrlTimeOut_Click);
            // 
            // btCorrect
            // 
            this.btCorrect.Location = new System.Drawing.Point(528, 86);
            this.btCorrect.Name = "btCorrect";
            this.btCorrect.Size = new System.Drawing.Size(75, 23);
            this.btCorrect.TabIndex = 2;
            this.btCorrect.Text = "Correct";
            this.btCorrect.UseVisualStyleBackColor = true;
            this.btCorrect.Visible = false;
            this.btCorrect.Click += new System.EventHandler(this.btCorrect_Click);
            // 
            // btContestantStepsForward
            // 
            this.btContestantStepsForward.Location = new System.Drawing.Point(167, 86);
            this.btContestantStepsForward.Name = "btContestantStepsForward";
            this.btContestantStepsForward.Size = new System.Drawing.Size(171, 23);
            this.btContestantStepsForward.TabIndex = 2;
            this.btContestantStepsForward.Text = "!Player Out of Time!";
            this.btContestantStepsForward.UseVisualStyleBackColor = true;
            this.btContestantStepsForward.Click += new System.EventHandler(this.btContestantStepsForward_Click);
            // 
            // btStuckInCase
            // 
            this.btStuckInCase.Location = new System.Drawing.Point(8, 86);
            this.btStuckInCase.Name = "btStuckInCase";
            this.btStuckInCase.Size = new System.Drawing.Size(136, 23);
            this.btStuckInCase.TabIndex = 2;
            this.btStuckInCase.Text = "Stuck in cage";
            this.btStuckInCase.UseVisualStyleBackColor = true;
            this.btStuckInCase.Click += new System.EventHandler(this.btStuckInCase_Click);
            // 
            // bttestbutton
            // 
            this.bttestbutton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bttestbutton.Location = new System.Drawing.Point(625, 14);
            this.bttestbutton.Name = "bttestbutton";
            this.bttestbutton.Size = new System.Drawing.Size(506, 24);
            this.bttestbutton.TabIndex = 4;
            this.bttestbutton.Tag = "XC";
            this.bttestbutton.Text = "CONTINUE";
            this.bttestbutton.UseVisualStyleBackColor = true;
            // 
            // ucGameCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.bttestbutton);
            this.Controls.Add(this.btfocus);
            this.Controls.Add(this.btGameCtrlRedo);
            this.Controls.Add(this.btStuckInCase);
            this.Controls.Add(this.btContestantStepsForward);
            this.Controls.Add(this.btCorrect);
            this.Controls.Add(this.btGameCtrlUndo);
            this.Controls.Add(this.btGameCtrlTimeOut);
            this.Controls.Add(this.btGameCtrlContinue);
            this.Name = "ucGameCtrl";
            this.Size = new System.Drawing.Size(1176, 141);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ucGameCtrl_KeyPress);
            this.Resize += new System.EventHandler(this.ucGameCtrl_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btGameCtrlContinue;
        private System.Windows.Forms.Button btGameCtrlUndo;
        private System.Windows.Forms.Button btGameCtrlRedo;
        private System.Windows.Forms.Button btfocus;
        private System.Windows.Forms.Button btGameCtrlTimeOut;
        private System.Windows.Forms.Button btCorrect;
        private System.Windows.Forms.Button btContestantStepsForward;
        private System.Windows.Forms.Button btStuckInCase;
        private System.Windows.Forms.Button bttestbutton;

    }
}
