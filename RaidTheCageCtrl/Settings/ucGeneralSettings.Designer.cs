namespace WiseGuys.Settings
{
    partial class ucGeneralSettings
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
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbgensettingscurrencysign = new System.Windows.Forms.TextBox();
            this.cbCurSignBehindAmount = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbTimebeforethedoorcloses = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tbTimebeforethetimerstart = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tbBetweenNames = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "General settings:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(58, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Currencty Sign:";
            // 
            // tbgensettingscurrencysign
            // 
            this.tbgensettingscurrencysign.Location = new System.Drawing.Point(143, 45);
            this.tbgensettingscurrencysign.Name = "tbgensettingscurrencysign";
            this.tbgensettingscurrencysign.Size = new System.Drawing.Size(35, 20);
            this.tbgensettingscurrencysign.TabIndex = 1;
            this.tbgensettingscurrencysign.TextChanged += new System.EventHandler(this.tbgensettingscurrencysign_TextChanged);
            // 
            // cbCurSignBehindAmount
            // 
            this.cbCurSignBehindAmount.AutoSize = true;
            this.cbCurSignBehindAmount.Location = new System.Drawing.Point(199, 48);
            this.cbCurSignBehindAmount.Name = "cbCurSignBehindAmount";
            this.cbCurSignBehindAmount.Size = new System.Drawing.Size(191, 17);
            this.cbCurSignBehindAmount.TabIndex = 2;
            this.cbCurSignBehindAmount.Text = "Use Currency Sign AFTER Amount";
            this.cbCurSignBehindAmount.UseVisualStyleBackColor = true;
            this.cbCurSignBehindAmount.CheckedChanged += new System.EventHandler(this.cbCurSignBehindAmount_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(58, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(176, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Time when the door needs to close:";
            // 
            // tbTimebeforethedoorcloses
            // 
            this.tbTimebeforethedoorcloses.Location = new System.Drawing.Point(240, 94);
            this.tbTimebeforethedoorcloses.Name = "tbTimebeforethedoorcloses";
            this.tbTimebeforethedoorcloses.Size = new System.Drawing.Size(83, 20);
            this.tbTimebeforethedoorcloses.TabIndex = 3;
            this.tbTimebeforethedoorcloses.TextChanged += new System.EventHandler(this.tbTimebeforethedoorcloses_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(58, 121);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(359, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "(in ms before end of the time, so 3000 is 3 seconds before the timer is on 0)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 167);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(214, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Time before the timer starts after door opens";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(125, 196);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(228, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "(in ms if the timer should start right away fill in 0)";
            // 
            // tbTimebeforethetimerstart
            // 
            this.tbTimebeforethetimerstart.Location = new System.Drawing.Point(240, 164);
            this.tbTimebeforethetimerstart.Name = "tbTimebeforethetimerstart";
            this.tbTimebeforethetimerstart.Size = new System.Drawing.Size(83, 20);
            this.tbTimebeforethetimerstart.TabIndex = 3;
            this.tbTimebeforethetimerstart.TextChanged += new System.EventHandler(this.tbTimebeforethetimerstart_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 232);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(243, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Text between the 2 names of the contestants title:";
            // 
            // tbBetweenNames
            // 
            this.tbBetweenNames.Location = new System.Drawing.Point(264, 229);
            this.tbBetweenNames.Name = "tbBetweenNames";
            this.tbBetweenNames.Size = new System.Drawing.Size(126, 20);
            this.tbBetweenNames.TabIndex = 5;
            this.tbBetweenNames.TextChanged += new System.EventHandler(this.tbBetweenNames_TextChanged);
            // 
            // ucGeneralSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.tbBetweenNames);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tbTimebeforethetimerstart);
            this.Controls.Add(this.tbTimebeforethedoorcloses);
            this.Controls.Add(this.cbCurSignBehindAmount);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tbgensettingscurrencysign);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Name = "ucGeneralSettings";
            this.Size = new System.Drawing.Size(452, 608);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbgensettingscurrencysign;
        private System.Windows.Forms.CheckBox cbCurSignBehindAmount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbTimebeforethedoorcloses;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbTimebeforethetimerstart;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbBetweenNames;
    }
}
