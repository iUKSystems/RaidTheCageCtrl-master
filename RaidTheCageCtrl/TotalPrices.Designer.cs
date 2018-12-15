namespace RaidTheCageCtrl
{
    partial class TotalPrices
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TotalPrices));
            this.label12 = new System.Windows.Forms.Label();
            this.dataGridViewOutTheCage1 = new System.Windows.Forms.DataGridView();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Article = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewOutTheCage1)).BeginInit();
            this.SuspendLayout();
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(88, 6);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(180, 13);
            this.label12.TabIndex = 54;
            this.label12.Text = "Products taken from the cage - Total";
            // 
            // dataGridViewOutTheCage1
            // 
            this.dataGridViewOutTheCage1.AllowDrop = true;
            this.dataGridViewOutTheCage1.AllowUserToAddRows = false;
            this.dataGridViewOutTheCage1.AllowUserToDeleteRows = false;
            this.dataGridViewOutTheCage1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.dataGridViewOutTheCage1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewOutTheCage1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.Article,
            this.Price});
            this.dataGridViewOutTheCage1.Location = new System.Drawing.Point(14, 29);
            this.dataGridViewOutTheCage1.Name = "dataGridViewOutTheCage1";
            this.dataGridViewOutTheCage1.ReadOnly = true;
            this.dataGridViewOutTheCage1.RowHeadersVisible = false;
            this.dataGridViewOutTheCage1.RowTemplate.Height = 18;
            this.dataGridViewOutTheCage1.RowTemplate.ReadOnly = true;
            this.dataGridViewOutTheCage1.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewOutTheCage1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewOutTheCage1.Size = new System.Drawing.Size(384, 503);
            this.dataGridViewOutTheCage1.TabIndex = 53;
            this.dataGridViewOutTheCage1.Tag = "XC";
            // 
            // ID
            // 
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.Visible = false;
            // 
            // Article
            // 
            this.Article.HeaderText = "Article";
            this.Article.Name = "Article";
            this.Article.ReadOnly = true;
            this.Article.Width = 280;
            // 
            // Price
            // 
            this.Price.HeaderText = "Price";
            this.Price.Name = "Price";
            this.Price.ReadOnly = true;
            // 
            // TotalPrices
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(409, 540);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.dataGridViewOutTheCage1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TotalPrices";
            this.Text = "TotalPrices";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TotalPrices_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewOutTheCage1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.DataGridView dataGridViewOutTheCage1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Article;
        private System.Windows.Forms.DataGridViewTextBoxColumn Price;
    }
}