namespace RaidTheCageCtrl
{
    partial class ContinueOrStop
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContinueOrStop));
            this.label1 = new System.Windows.Forms.Label();
            this.btYes = new System.Windows.Forms.Button();
            this.btNo = new System.Windows.Forms.Button();
            this.btShowTotalAmount = new System.Windows.Forms.Button();
            this.btShowMoneyTree = new System.Windows.Forms.Button();
            this.btHideSwitchPlayerLL = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(43, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(250, 31);
            this.label1.TabIndex = 0;
            this.label1.Text = "Do they continue?";
            // 
            // btYes
            // 
            this.btYes.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btYes.Location = new System.Drawing.Point(32, 107);
            this.btYes.Name = "btYes";
            this.btYes.Size = new System.Drawing.Size(118, 48);
            this.btYes.TabIndex = 1;
            this.btYes.Text = "YES";
            this.btYes.UseVisualStyleBackColor = true;
            this.btYes.Click += new System.EventHandler(this.btYes_Click);
            // 
            // btNo
            // 
            this.btNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btNo.Location = new System.Drawing.Point(175, 107);
            this.btNo.Name = "btNo";
            this.btNo.Size = new System.Drawing.Size(118, 48);
            this.btNo.TabIndex = 1;
            this.btNo.Text = "NO";
            this.btNo.UseVisualStyleBackColor = true;
            this.btNo.Click += new System.EventHandler(this.btNo_Click);
            // 
            // btShowTotalAmount
            // 
            this.btShowTotalAmount.Location = new System.Drawing.Point(333, 128);
            this.btShowTotalAmount.Name = "btShowTotalAmount";
            this.btShowTotalAmount.Size = new System.Drawing.Size(110, 48);
            this.btShowTotalAmount.TabIndex = 2;
            this.btShowTotalAmount.Tag = "XC";
            this.btShowTotalAmount.Text = "Show Total Amount";
            this.btShowTotalAmount.UseVisualStyleBackColor = true;
            this.btShowTotalAmount.Click += new System.EventHandler(this.btShowTotalAmount_Click);
            // 
            // btShowMoneyTree
            // 
            this.btShowMoneyTree.Location = new System.Drawing.Point(333, 12);
            this.btShowMoneyTree.Name = "btShowMoneyTree";
            this.btShowMoneyTree.Size = new System.Drawing.Size(110, 44);
            this.btShowMoneyTree.TabIndex = 3;
            this.btShowMoneyTree.Tag = "XC";
            this.btShowMoneyTree.Text = "Show Moneytree";
            this.btShowMoneyTree.UseVisualStyleBackColor = true;
            this.btShowMoneyTree.Click += new System.EventHandler(this.btShowMoneyTree_Click);
            // 
            // btHideSwitchPlayerLL
            // 
            this.btHideSwitchPlayerLL.Location = new System.Drawing.Point(333, 62);
            this.btHideSwitchPlayerLL.Name = "btHideSwitchPlayerLL";
            this.btHideSwitchPlayerLL.Size = new System.Drawing.Size(110, 44);
            this.btHideSwitchPlayerLL.TabIndex = 3;
            this.btHideSwitchPlayerLL.Tag = "XC";
            this.btHideSwitchPlayerLL.Text = "Hide Switch Player LL";
            this.btHideSwitchPlayerLL.UseVisualStyleBackColor = true;
            this.btHideSwitchPlayerLL.Visible = false;
            this.btHideSwitchPlayerLL.Click += new System.EventHandler(this.btHideSwitchPlayerLL_Click);
            // 
            // ContinueOrStop
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(455, 198);
            this.Controls.Add(this.btHideSwitchPlayerLL);
            this.Controls.Add(this.btShowMoneyTree);
            this.Controls.Add(this.btShowTotalAmount);
            this.Controls.Add(this.btNo);
            this.Controls.Add(this.btYes);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ContinueOrStop";
            this.Text = "ContinueOrStop";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ContinueOrStop_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btYes;
        private System.Windows.Forms.Button btNo;
        private System.Windows.Forms.Button btShowTotalAmount;
        private System.Windows.Forms.Button btShowMoneyTree;
        private System.Windows.Forms.Button btHideSwitchPlayerLL;
    }
}