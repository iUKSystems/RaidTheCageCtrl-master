namespace RaidTheCageCtrl
{
    partial class NewLicenseCode
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
            this.label1 = new System.Windows.Forms.Label();
            this.tbNewLicenseCode = new System.Windows.Forms.TextBox();
            this.btNewLicenseOK = new System.Windows.Forms.Button();
            this.btNewLicenseCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(138, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "New License Code:";
            // 
            // tbNewLicenseCode
            // 
            this.tbNewLicenseCode.Location = new System.Drawing.Point(30, 25);
            this.tbNewLicenseCode.Name = "tbNewLicenseCode";
            this.tbNewLicenseCode.Size = new System.Drawing.Size(326, 20);
            this.tbNewLicenseCode.TabIndex = 1;
            // 
            // btNewLicenseOK
            // 
            this.btNewLicenseOK.Location = new System.Drawing.Point(281, 59);
            this.btNewLicenseOK.Name = "btNewLicenseOK";
            this.btNewLicenseOK.Size = new System.Drawing.Size(75, 23);
            this.btNewLicenseOK.TabIndex = 2;
            this.btNewLicenseOK.Text = "OK";
            this.btNewLicenseOK.UseVisualStyleBackColor = true;
            this.btNewLicenseOK.Click += new System.EventHandler(this.btNewLicenseOK_Click);
            // 
            // btNewLicenseCancel
            // 
            this.btNewLicenseCancel.Location = new System.Drawing.Point(30, 59);
            this.btNewLicenseCancel.Name = "btNewLicenseCancel";
            this.btNewLicenseCancel.Size = new System.Drawing.Size(75, 23);
            this.btNewLicenseCancel.TabIndex = 2;
            this.btNewLicenseCancel.Text = "Cancel";
            this.btNewLicenseCancel.UseVisualStyleBackColor = true;
            // 
            // NewLicenseCode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 94);
            this.Controls.Add(this.btNewLicenseCancel);
            this.Controls.Add(this.btNewLicenseOK);
            this.Controls.Add(this.tbNewLicenseCode);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewLicenseCode";
            this.ShowInTaskbar = false;
            this.Text = "NewLicenseCode";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbNewLicenseCode;
        private System.Windows.Forms.Button btNewLicenseOK;
        private System.Windows.Forms.Button btNewLicenseCancel;
    }
}