namespace WiseGuys.Settings
{
    partial class SettingsEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsEditor));
            this.btSettingsCancel = new System.Windows.Forms.Button();
            this.btSettingsOK = new System.Windows.Forms.Button();
            this.tvQEGameTypeSelect = new System.Windows.Forms.TreeView();
            this.flPanelSettings = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // btSettingsCancel
            // 
            this.btSettingsCancel.Location = new System.Drawing.Point(298, 574);
            this.btSettingsCancel.Name = "btSettingsCancel";
            this.btSettingsCancel.Size = new System.Drawing.Size(75, 23);
            this.btSettingsCancel.TabIndex = 24;
            this.btSettingsCancel.Text = "Cancel";
            this.btSettingsCancel.UseVisualStyleBackColor = true;
            this.btSettingsCancel.Visible = false;
            this.btSettingsCancel.Click += new System.EventHandler(this.btSettingsCancel_Click);
            // 
            // btSettingsOK
            // 
            this.btSettingsOK.Location = new System.Drawing.Point(577, 574);
            this.btSettingsOK.Name = "btSettingsOK";
            this.btSettingsOK.Size = new System.Drawing.Size(75, 23);
            this.btSettingsOK.TabIndex = 23;
            this.btSettingsOK.Text = "OK";
            this.btSettingsOK.UseVisualStyleBackColor = true;
            this.btSettingsOK.Click += new System.EventHandler(this.btSettingsOK_Click);
            // 
            // tvQEGameTypeSelect
            // 
            this.tvQEGameTypeSelect.BackColor = System.Drawing.SystemColors.Control;
            this.tvQEGameTypeSelect.Location = new System.Drawing.Point(12, 12);
            this.tvQEGameTypeSelect.Name = "tvQEGameTypeSelect";
            this.tvQEGameTypeSelect.Size = new System.Drawing.Size(247, 556);
            this.tvQEGameTypeSelect.TabIndex = 22;
            this.tvQEGameTypeSelect.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvQEGameTypeSelect_AfterSelect);
            // 
            // flPanelSettings
            // 
            this.flPanelSettings.BackColor = System.Drawing.Color.Transparent;
            this.flPanelSettings.Location = new System.Drawing.Point(265, 12);
            this.flPanelSettings.Name = "flPanelSettings";
            this.flPanelSettings.Size = new System.Drawing.Size(717, 556);
            this.flPanelSettings.TabIndex = 21;
            // 
            // SettingsEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(994, 609);
            this.Controls.Add(this.btSettingsCancel);
            this.Controls.Add(this.btSettingsOK);
            this.Controls.Add(this.tvQEGameTypeSelect);
            this.Controls.Add(this.flPanelSettings);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsEditor";
            this.ShowInTaskbar = false;
            this.Text = "SettingsEditor";            
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingsEditor_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btSettingsCancel;
        private System.Windows.Forms.Button btSettingsOK;
        private System.Windows.Forms.TreeView tvQEGameTypeSelect;
        private System.Windows.Forms.FlowLayoutPanel flPanelSettings;
    }
}