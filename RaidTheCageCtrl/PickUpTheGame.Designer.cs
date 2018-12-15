namespace RaidTheCageCtrl
{
    partial class PickUpTheGame
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PickUpTheGame));
            this.label1 = new System.Windows.Forms.Label();
            this.cbQnr = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbTotalAmount = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cbPeopleSwitch = new System.Windows.Forms.CheckBox();
            this.cbQuestionSwitch = new System.Windows.Forms.CheckBox();
            this.btOK = new System.Windows.Forms.Button();
            this.lbcontinuestop = new System.Windows.Forms.Label();
            this.cbGoToContueStop = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Question Number:";
            // 
            // cbQnr
            // 
            this.cbQnr.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbQnr.FormattingEnabled = true;
            this.cbQnr.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.cbQnr.Location = new System.Drawing.Point(111, 10);
            this.cbQnr.Name = "cbQnr";
            this.cbQnr.Size = new System.Drawing.Size(61, 21);
            this.cbQnr.TabIndex = 1;
            this.cbQnr.SelectedIndexChanged += new System.EventHandler(this.cbQnr_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Total Amount:";
            // 
            // tbTotalAmount
            // 
            this.tbTotalAmount.Location = new System.Drawing.Point(95, 42);
            this.tbTotalAmount.Name = "tbTotalAmount";
            this.tbTotalAmount.Size = new System.Drawing.Size(121, 20);
            this.tbTotalAmount.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(28, 105);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(111, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "People SwitchLifeline:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 77);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(120, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Question SwitchLifeline:";
            // 
            // cbPeopleSwitch
            // 
            this.cbPeopleSwitch.AutoSize = true;
            this.cbPeopleSwitch.Checked = true;
            this.cbPeopleSwitch.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbPeopleSwitch.Location = new System.Drawing.Point(160, 104);
            this.cbPeopleSwitch.Name = "cbPeopleSwitch";
            this.cbPeopleSwitch.Size = new System.Drawing.Size(47, 17);
            this.cbPeopleSwitch.TabIndex = 5;
            this.cbPeopleSwitch.Text = "YES";
            this.cbPeopleSwitch.UseVisualStyleBackColor = true;
            // 
            // cbQuestionSwitch
            // 
            this.cbQuestionSwitch.AutoSize = true;
            this.cbQuestionSwitch.Checked = true;
            this.cbQuestionSwitch.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbQuestionSwitch.Location = new System.Drawing.Point(160, 77);
            this.cbQuestionSwitch.Name = "cbQuestionSwitch";
            this.cbQuestionSwitch.Size = new System.Drawing.Size(47, 17);
            this.cbQuestionSwitch.TabIndex = 5;
            this.cbQuestionSwitch.Text = "YES";
            this.cbQuestionSwitch.UseVisualStyleBackColor = true;
            // 
            // btOK
            // 
            this.btOK.Location = new System.Drawing.Point(74, 196);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(98, 32);
            this.btOK.TabIndex = 6;
            this.btOK.Text = "OK";
            this.btOK.UseVisualStyleBackColor = true;
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // lbcontinuestop
            // 
            this.lbcontinuestop.AutoSize = true;
            this.lbcontinuestop.Location = new System.Drawing.Point(28, 133);
            this.lbcontinuestop.Name = "lbcontinuestop";
            this.lbcontinuestop.Size = new System.Drawing.Size(198, 13);
            this.lbcontinuestop.TabIndex = 4;
            this.lbcontinuestop.Text = "Go to Continue Stop before this question";
            this.lbcontinuestop.Visible = false;
            // 
            // cbGoToContueStop
            // 
            this.cbGoToContueStop.AutoSize = true;
            this.cbGoToContueStop.Location = new System.Drawing.Point(98, 157);
            this.cbGoToContueStop.Name = "cbGoToContueStop";
            this.cbGoToContueStop.Size = new System.Drawing.Size(47, 17);
            this.cbGoToContueStop.TabIndex = 5;
            this.cbGoToContueStop.Text = "YES";
            this.cbGoToContueStop.UseVisualStyleBackColor = true;
            this.cbGoToContueStop.Visible = false;
            // 
            // PickUpTheGame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(245, 240);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.cbQuestionSwitch);
            this.Controls.Add(this.cbGoToContueStop);
            this.Controls.Add(this.cbPeopleSwitch);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lbcontinuestop);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbTotalAmount);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbQnr);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PickUpTheGame";
            this.Text = "PickUpTheGame";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbQnr;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbTotalAmount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox cbPeopleSwitch;
        private System.Windows.Forms.CheckBox cbQuestionSwitch;
        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.Label lbcontinuestop;
        private System.Windows.Forms.CheckBox cbGoToContueStop;
    }
}