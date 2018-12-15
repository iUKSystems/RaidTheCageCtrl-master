namespace RaidTheCageCtrl
{
    partial class LicenseSplash
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LicenseSplash));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbLicValidDate = new System.Windows.Forms.Label();
            this.btUpdateLicense = new System.Windows.Forms.Button();
            this.tbNewLic = new System.Windows.Forms.TextBox();
            this.tbSetLicense = new System.Windows.Forms.Button();
            this.btOK = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::RaidTheCageCtrl.Properties.Resources.WiseGuys_Logo_Final_NoTagLine_Ctrl;
            this.pictureBox1.Location = new System.Drawing.Point(33, 9);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(321, 106);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(29, 118);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(143, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "License Valid Until:";
            // 
            // lbLicValidDate
            // 
            this.lbLicValidDate.AutoSize = true;
            this.lbLicValidDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbLicValidDate.ForeColor = System.Drawing.Color.White;
            this.lbLicValidDate.Location = new System.Drawing.Point(173, 118);
            this.lbLicValidDate.Name = "lbLicValidDate";
            this.lbLicValidDate.Size = new System.Drawing.Size(181, 20);
            this.lbLicValidDate.TabIndex = 3;
            this.lbLicValidDate.Text = "No Valid License Found!";
            // 
            // btUpdateLicense
            // 
            this.btUpdateLicense.BackColor = System.Drawing.SystemColors.Control;
            this.btUpdateLicense.ForeColor = System.Drawing.Color.Black;
            this.btUpdateLicense.Location = new System.Drawing.Point(141, 142);
            this.btUpdateLicense.Name = "btUpdateLicense";
            this.btUpdateLicense.Size = new System.Drawing.Size(99, 23);
            this.btUpdateLicense.TabIndex = 4;
            this.btUpdateLicense.Text = "Update License";
            this.btUpdateLicense.UseVisualStyleBackColor = false;
            this.btUpdateLicense.Click += new System.EventHandler(this.btUpdateLicense_Click);
            // 
            // tbNewLic
            // 
            this.tbNewLic.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbNewLic.Location = new System.Drawing.Point(12, 172);
            this.tbNewLic.Name = "tbNewLic";
            this.tbNewLic.Size = new System.Drawing.Size(364, 31);
            this.tbNewLic.TabIndex = 5;
            this.tbNewLic.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbSetLicense
            // 
            this.tbSetLicense.BackColor = System.Drawing.SystemColors.Control;
            this.tbSetLicense.ForeColor = System.Drawing.Color.Black;
            this.tbSetLicense.Location = new System.Drawing.Point(141, 211);
            this.tbSetLicense.Name = "tbSetLicense";
            this.tbSetLicense.Size = new System.Drawing.Size(99, 23);
            this.tbSetLicense.TabIndex = 4;
            this.tbSetLicense.Text = "Set License";
            this.tbSetLicense.UseVisualStyleBackColor = false;
            this.tbSetLicense.Click += new System.EventHandler(this.tbSetLicense_Click);
            // 
            // btOK
            // 
            this.btOK.BackColor = System.Drawing.SystemColors.Control;
            this.btOK.ForeColor = System.Drawing.Color.Black;
            this.btOK.Location = new System.Drawing.Point(277, 251);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(99, 23);
            this.btOK.TabIndex = 4;
            this.btOK.Text = "OK";
            this.btOK.UseVisualStyleBackColor = false;
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // btCancel
            // 
            this.btCancel.BackColor = System.Drawing.SystemColors.Control;
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.ForeColor = System.Drawing.Color.Black;
            this.btCancel.Location = new System.Drawing.Point(12, 251);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(99, 23);
            this.btCancel.TabIndex = 4;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = false;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // LicenseSplash
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(388, 286);
            this.ControlBox = false;
            this.Controls.Add(this.tbNewLic);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.tbSetLicense);
            this.Controls.Add(this.btUpdateLicense);
            this.Controls.Add(this.lbLicValidDate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.ForeColor = System.Drawing.Color.White;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LicenseSplash";
            this.Text = "LicenseSplash";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbLicValidDate;
        private System.Windows.Forms.Button btUpdateLicense;
        private System.Windows.Forms.TextBox tbNewLic;
        private System.Windows.Forms.Button tbSetLicense;
        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Timer timer1;
    }
}