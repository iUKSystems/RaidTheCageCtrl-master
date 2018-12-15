namespace RaidTheCageCtrl.GameParts
{
    partial class ucRaidTheCage
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
            this.tbTimeLeft = new System.Windows.Forms.TextBox();
            this.btStopCageTimer = new System.Windows.Forms.Button();
            this.lbName1 = new System.Windows.Forms.Label();
            this.tbName1 = new System.Windows.Forms.TextBox();
            this.tbName2 = new System.Windows.Forms.TextBox();
            this.lbthisquestion = new System.Windows.Forms.Label();
            this.tbPriceThisQuestion = new System.Windows.Forms.TextBox();
            this.lbTotal = new System.Windows.Forms.Label();
            this.tbPriceTotal = new System.Windows.Forms.TextBox();
            this.gbPrices = new System.Windows.Forms.GroupBox();
            this.btShowTotalAmount = new System.Windows.Forms.Button();
            this.cbLifeLineUsed2 = new System.Windows.Forms.CheckBox();
            this.cbLifeLineUsed1 = new System.Windows.Forms.CheckBox();
            this.btUseLifeLine2 = new XCtrls.BitmapButton();
            this.btUseLifeLine1 = new XCtrls.BitmapButton();
            this.lblifeline2 = new System.Windows.Forms.Label();
            this.lblifeline1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btShowMoneyTreeOnProjector = new System.Windows.Forms.Button();
            this.btWWTBAMLifeLineRiminderStrap = new System.Windows.Forms.Button();
            this.btOpenCageDoor = new System.Windows.Forms.Button();
            this.btCloseCageDoor = new System.Windows.Forms.Button();
            this.lbName2 = new System.Windows.Forms.Label();
            this.btLightReset = new System.Windows.Forms.Button();
            this.bmbLogging = new XCtrls.BitmapButton();
            this.lblogging2 = new System.Windows.Forms.Label();
            this.lblogging1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbHost = new System.Windows.Forms.TextBox();
            this.tbCoHost = new System.Windows.Forms.TextBox();
            this.btShowTitle1 = new System.Windows.Forms.Button();
            this.btShowTitle2 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.tbPlace = new System.Windows.Forms.TextBox();
            this.btStartExplain3sec = new System.Windows.Forms.Button();
            this.gbPrices.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Time Left:";
            // 
            // tbTimeLeft
            // 
            this.tbTimeLeft.Location = new System.Drawing.Point(13, 32);
            this.tbTimeLeft.Name = "tbTimeLeft";
            this.tbTimeLeft.Size = new System.Drawing.Size(100, 20);
            this.tbTimeLeft.TabIndex = 1;
            // 
            // btStopCageTimer
            // 
            this.btStopCageTimer.Location = new System.Drawing.Point(25, 58);
            this.btStopCageTimer.Name = "btStopCageTimer";
            this.btStopCageTimer.Size = new System.Drawing.Size(75, 23);
            this.btStopCageTimer.TabIndex = 2;
            this.btStopCageTimer.Text = "STOP";
            this.btStopCageTimer.UseVisualStyleBackColor = true;
            this.btStopCageTimer.Visible = false;
            this.btStopCageTimer.Click += new System.EventHandler(this.btStopCageTimer_Click);
            // 
            // lbName1
            // 
            this.lbName1.Location = new System.Drawing.Point(14, 14);
            this.lbName1.Name = "lbName1";
            this.lbName1.Size = new System.Drawing.Size(55, 13);
            this.lbName1.TabIndex = 0;
            this.lbName1.Text = "Name 1:";
            this.lbName1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbName1
            // 
            this.tbName1.Location = new System.Drawing.Point(75, 11);
            this.tbName1.Name = "tbName1";
            this.tbName1.Size = new System.Drawing.Size(169, 20);
            this.tbName1.TabIndex = 3;
            this.tbName1.TextChanged += new System.EventHandler(this.tbName1_TextChanged);
            // 
            // tbName2
            // 
            this.tbName2.Location = new System.Drawing.Point(75, 37);
            this.tbName2.Name = "tbName2";
            this.tbName2.Size = new System.Drawing.Size(169, 20);
            this.tbName2.TabIndex = 3;
            this.tbName2.TextChanged += new System.EventHandler(this.tbName2_TextChanged);
            // 
            // lbthisquestion
            // 
            this.lbthisquestion.Location = new System.Drawing.Point(4, 34);
            this.lbthisquestion.Name = "lbthisquestion";
            this.lbthisquestion.Size = new System.Drawing.Size(75, 13);
            this.lbthisquestion.TabIndex = 0;
            this.lbthisquestion.Text = "This Question:";
            this.lbthisquestion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lbthisquestion.Click += new System.EventHandler(this.label4_Click);
            // 
            // tbPriceThisQuestion
            // 
            this.tbPriceThisQuestion.Location = new System.Drawing.Point(85, 31);
            this.tbPriceThisQuestion.Name = "tbPriceThisQuestion";
            this.tbPriceThisQuestion.Size = new System.Drawing.Size(169, 20);
            this.tbPriceThisQuestion.TabIndex = 3;
            this.tbPriceThisQuestion.TextChanged += new System.EventHandler(this.tbPriceThisQuestion_TextChanged);
            // 
            // lbTotal
            // 
            this.lbTotal.Location = new System.Drawing.Point(0, 60);
            this.lbTotal.Name = "lbTotal";
            this.lbTotal.Size = new System.Drawing.Size(79, 13);
            this.lbTotal.TabIndex = 0;
            this.lbTotal.Text = "Total:";
            this.lbTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lbTotal.Click += new System.EventHandler(this.label4_Click);
            // 
            // tbPriceTotal
            // 
            this.tbPriceTotal.Location = new System.Drawing.Point(85, 57);
            this.tbPriceTotal.Name = "tbPriceTotal";
            this.tbPriceTotal.Size = new System.Drawing.Size(169, 20);
            this.tbPriceTotal.TabIndex = 3;
            this.tbPriceTotal.TextChanged += new System.EventHandler(this.tbPriceTotal_TextChanged);
            // 
            // gbPrices
            // 
            this.gbPrices.Controls.Add(this.btShowTotalAmount);
            this.gbPrices.Controls.Add(this.tbPriceThisQuestion);
            this.gbPrices.Controls.Add(this.tbPriceTotal);
            this.gbPrices.Controls.Add(this.lbthisquestion);
            this.gbPrices.Controls.Add(this.lbTotal);
            this.gbPrices.Location = new System.Drawing.Point(14, 57);
            this.gbPrices.Name = "gbPrices";
            this.gbPrices.Size = new System.Drawing.Size(345, 100);
            this.gbPrices.TabIndex = 4;
            this.gbPrices.TabStop = false;
            this.gbPrices.Text = "Prices";
            // 
            // btShowTotalAmount
            // 
            this.btShowTotalAmount.Location = new System.Drawing.Point(264, 55);
            this.btShowTotalAmount.Name = "btShowTotalAmount";
            this.btShowTotalAmount.Size = new System.Drawing.Size(75, 23);
            this.btShowTotalAmount.TabIndex = 4;
            this.btShowTotalAmount.Tag = "XC";
            this.btShowTotalAmount.Text = "Show";
            this.btShowTotalAmount.UseVisualStyleBackColor = true;
            this.btShowTotalAmount.Click += new System.EventHandler(this.btShowTotalAmount_Click);
            // 
            // cbLifeLineUsed2
            // 
            this.cbLifeLineUsed2.AutoSize = true;
            this.cbLifeLineUsed2.Location = new System.Drawing.Point(651, 113);
            this.cbLifeLineUsed2.Name = "cbLifeLineUsed2";
            this.cbLifeLineUsed2.Size = new System.Drawing.Size(69, 17);
            this.cbLifeLineUsed2.TabIndex = 9;
            this.cbLifeLineUsed2.Text = "Available";
            this.cbLifeLineUsed2.UseVisualStyleBackColor = true;
            this.cbLifeLineUsed2.CheckedChanged += new System.EventHandler(this.cbLifeLineUsed2_CheckedChanged);
            // 
            // cbLifeLineUsed1
            // 
            this.cbLifeLineUsed1.AutoSize = true;
            this.cbLifeLineUsed1.Location = new System.Drawing.Point(534, 113);
            this.cbLifeLineUsed1.Name = "cbLifeLineUsed1";
            this.cbLifeLineUsed1.Size = new System.Drawing.Size(69, 17);
            this.cbLifeLineUsed1.TabIndex = 10;
            this.cbLifeLineUsed1.Text = "Available";
            this.cbLifeLineUsed1.UseVisualStyleBackColor = true;
            this.cbLifeLineUsed1.CheckedChanged += new System.EventHandler(this.cbLifeLineUsed1_CheckedChanged);
            // 
            // btUseLifeLine2
            // 
            this.btUseLifeLine2.BorderColor = System.Drawing.Color.DarkBlue;
            this.btUseLifeLine2.FocusRectangleEnabled = true;
            this.btUseLifeLine2.Image = null;
            this.btUseLifeLine2.ImageBorderColor = System.Drawing.Color.Chocolate;
            this.btUseLifeLine2.ImageBorderEnabled = false;
            this.btUseLifeLine2.ImageDropShadow = false;
            this.btUseLifeLine2.ImageFocused = null;
            this.btUseLifeLine2.ImageInactive = null;
            this.btUseLifeLine2.ImageMouseOver = null;
            this.btUseLifeLine2.ImageNormal = global::RaidTheCageCtrl.Properties.Resources.SwitchPeopleLifeline;
            this.btUseLifeLine2.ImagePressed = null;
            this.btUseLifeLine2.InnerBorderColor = System.Drawing.Color.LightGray;
            this.btUseLifeLine2.InnerBorderColor_Focus = System.Drawing.Color.LightBlue;
            this.btUseLifeLine2.InnerBorderColor_MouseOver = System.Drawing.Color.Gold;
            this.btUseLifeLine2.Location = new System.Drawing.Point(634, 19);
            this.btUseLifeLine2.Name = "btUseLifeLine2";
            this.btUseLifeLine2.OffsetPressedContent = true;
            this.btUseLifeLine2.Size = new System.Drawing.Size(98, 88);
            this.btUseLifeLine2.StretchImage = true;
            this.btUseLifeLine2.TabIndex = 8;
            this.btUseLifeLine2.Tag = "XC";
            this.btUseLifeLine2.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btUseLifeLine2.TextDropShadow = false;
            this.btUseLifeLine2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btUseLifeLine2.UseVisualStyleBackColor = true;
            this.btUseLifeLine2.Click += new System.EventHandler(this.btUseLifeLine2_Click);
            // 
            // btUseLifeLine1
            // 
            this.btUseLifeLine1.BorderColor = System.Drawing.Color.DarkBlue;
            this.btUseLifeLine1.FocusRectangleEnabled = true;
            this.btUseLifeLine1.Image = null;
            this.btUseLifeLine1.ImageBorderColor = System.Drawing.Color.Chocolate;
            this.btUseLifeLine1.ImageBorderEnabled = false;
            this.btUseLifeLine1.ImageDropShadow = false;
            this.btUseLifeLine1.ImageFocused = null;
            this.btUseLifeLine1.ImageInactive = null;
            this.btUseLifeLine1.ImageMouseOver = null;
            this.btUseLifeLine1.ImageNormal = global::RaidTheCageCtrl.Properties.Resources.SwitchQuestionLifeline;
            this.btUseLifeLine1.ImagePressed = null;
            this.btUseLifeLine1.InnerBorderColor = System.Drawing.Color.LightGray;
            this.btUseLifeLine1.InnerBorderColor_Focus = System.Drawing.Color.LightBlue;
            this.btUseLifeLine1.InnerBorderColor_MouseOver = System.Drawing.Color.Gold;
            this.btUseLifeLine1.Location = new System.Drawing.Point(516, 19);
            this.btUseLifeLine1.Name = "btUseLifeLine1";
            this.btUseLifeLine1.OffsetPressedContent = true;
            this.btUseLifeLine1.Size = new System.Drawing.Size(98, 88);
            this.btUseLifeLine1.StretchImage = true;
            this.btUseLifeLine1.TabIndex = 7;
            this.btUseLifeLine1.Tag = "XC";
            this.btUseLifeLine1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btUseLifeLine1.TextDropShadow = false;
            this.btUseLifeLine1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btUseLifeLine1.UseVisualStyleBackColor = true;
            this.btUseLifeLine1.Click += new System.EventHandler(this.btUseLifeLine1_Click);
            // 
            // lblifeline2
            // 
            this.lblifeline2.Location = new System.Drawing.Point(631, 3);
            this.lblifeline2.Name = "lblifeline2";
            this.lblifeline2.Size = new System.Drawing.Size(101, 13);
            this.lblifeline2.TabIndex = 5;
            this.lblifeline2.Text = "LifeLine 2";
            this.lblifeline2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblifeline1
            // 
            this.lblifeline1.Location = new System.Drawing.Point(513, 3);
            this.lblifeline1.Name = "lblifeline1";
            this.lblifeline1.Size = new System.Drawing.Size(101, 13);
            this.lblifeline1.TabIndex = 6;
            this.lblifeline1.Text = "LifeLine 1";
            this.lblifeline1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.tbTimeLeft);
            this.groupBox2.Controls.Add(this.btStopCageTimer);
            this.groupBox2.Location = new System.Drawing.Point(373, 57);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(124, 100);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Clock";
            // 
            // btShowMoneyTreeOnProjector
            // 
            this.btShowMoneyTreeOnProjector.Location = new System.Drawing.Point(572, 134);
            this.btShowMoneyTreeOnProjector.Name = "btShowMoneyTreeOnProjector";
            this.btShowMoneyTreeOnProjector.Size = new System.Drawing.Size(148, 37);
            this.btShowMoneyTreeOnProjector.TabIndex = 12;
            this.btShowMoneyTreeOnProjector.Text = "Show MoneyTree On Projector";
            this.btShowMoneyTreeOnProjector.UseVisualStyleBackColor = true;
            this.btShowMoneyTreeOnProjector.Visible = false;
            this.btShowMoneyTreeOnProjector.Click += new System.EventHandler(this.btShowMoneyTreeOnProjector_Click);
            // 
            // btWWTBAMLifeLineRiminderStrap
            // 
            this.btWWTBAMLifeLineRiminderStrap.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btWWTBAMLifeLineRiminderStrap.Location = new System.Drawing.Point(0, 185);
            this.btWWTBAMLifeLineRiminderStrap.Name = "btWWTBAMLifeLineRiminderStrap";
            this.btWWTBAMLifeLineRiminderStrap.Size = new System.Drawing.Size(527, 72);
            this.btWWTBAMLifeLineRiminderStrap.TabIndex = 13;
            this.btWWTBAMLifeLineRiminderStrap.Tag = "xc";
            this.btWWTBAMLifeLineRiminderStrap.Text = "Show Lifeline Reminder Strap";
            this.btWWTBAMLifeLineRiminderStrap.UseVisualStyleBackColor = true;
            this.btWWTBAMLifeLineRiminderStrap.Click += new System.EventHandler(this.btWWTBAMLifeLineRiminderStrap_Click);
            // 
            // btOpenCageDoor
            // 
            this.btOpenCageDoor.Location = new System.Drawing.Point(543, 172);
            this.btOpenCageDoor.Name = "btOpenCageDoor";
            this.btOpenCageDoor.Size = new System.Drawing.Size(102, 34);
            this.btOpenCageDoor.TabIndex = 14;
            this.btOpenCageDoor.Tag = "XC";
            this.btOpenCageDoor.Text = "Open Cage Door";
            this.btOpenCageDoor.UseVisualStyleBackColor = true;
            this.btOpenCageDoor.Click += new System.EventHandler(this.btOpenCageDoor_Click);
            // 
            // btCloseCageDoor
            // 
            this.btCloseCageDoor.Location = new System.Drawing.Point(663, 172);
            this.btCloseCageDoor.Name = "btCloseCageDoor";
            this.btCloseCageDoor.Size = new System.Drawing.Size(102, 34);
            this.btCloseCageDoor.TabIndex = 14;
            this.btCloseCageDoor.Tag = "XC";
            this.btCloseCageDoor.Text = "Close Cage Door";
            this.btCloseCageDoor.UseVisualStyleBackColor = true;
            this.btCloseCageDoor.Click += new System.EventHandler(this.btCloseCageDoor_Click);
            // 
            // lbName2
            // 
            this.lbName2.Location = new System.Drawing.Point(18, 40);
            this.lbName2.Name = "lbName2";
            this.lbName2.Size = new System.Drawing.Size(51, 13);
            this.lbName2.TabIndex = 0;
            this.lbName2.Text = "Name 2:";
            this.lbName2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btLightReset
            // 
            this.btLightReset.Location = new System.Drawing.Point(793, 176);
            this.btLightReset.Name = "btLightReset";
            this.btLightReset.Size = new System.Drawing.Size(102, 34);
            this.btLightReset.TabIndex = 15;
            this.btLightReset.Text = "LIGHTS Reset";
            this.btLightReset.UseVisualStyleBackColor = true;
            this.btLightReset.Click += new System.EventHandler(this.btLightReset_Click);
            // 
            // bmbLogging
            // 
            this.bmbLogging.BorderColor = System.Drawing.Color.DarkBlue;
            this.bmbLogging.FocusRectangleEnabled = true;
            this.bmbLogging.Image = null;
            this.bmbLogging.ImageBorderColor = System.Drawing.Color.Chocolate;
            this.bmbLogging.ImageBorderEnabled = true;
            this.bmbLogging.ImageDropShadow = true;
            this.bmbLogging.ImageFocused = null;
            this.bmbLogging.ImageInactive = null;
            this.bmbLogging.ImageMouseOver = null;
            this.bmbLogging.ImageNormal = global::RaidTheCageCtrl.Properties.Resources.LoggingDisabled;
            this.bmbLogging.ImagePressed = null;
            this.bmbLogging.InnerBorderColor = System.Drawing.Color.LightGray;
            this.bmbLogging.InnerBorderColor_Focus = System.Drawing.Color.LightBlue;
            this.bmbLogging.InnerBorderColor_MouseOver = System.Drawing.Color.Gold;
            this.bmbLogging.Location = new System.Drawing.Point(793, 22);
            this.bmbLogging.Name = "bmbLogging";
            this.bmbLogging.OffsetPressedContent = true;
            this.bmbLogging.Size = new System.Drawing.Size(71, 64);
            this.bmbLogging.StretchImage = true;
            this.bmbLogging.TabIndex = 18;
            this.bmbLogging.Tag = "XC";
            this.bmbLogging.TextDropShadow = true;
            this.bmbLogging.UseVisualStyleBackColor = true;
            this.bmbLogging.Click += new System.EventHandler(this.bmbLogging_Click);
            // 
            // lblogging2
            // 
            this.lblogging2.Location = new System.Drawing.Point(790, 90);
            this.lblogging2.Name = "lblogging2";
            this.lblogging2.Size = new System.Drawing.Size(80, 23);
            this.lblogging2.TabIndex = 16;
            this.lblogging2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblogging1
            // 
            this.lblogging1.Location = new System.Drawing.Point(786, 113);
            this.lblogging1.Name = "lblogging1";
            this.lblogging1.Size = new System.Drawing.Size(85, 13);
            this.lblogging1.TabIndex = 17;
            this.lblogging1.Text = "Logging is off";
            this.lblogging1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(553, 217);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Host:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(537, 242);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 19;
            this.label3.Text = "Co-Host:";
            // 
            // tbHost
            // 
            this.tbHost.Location = new System.Drawing.Point(591, 214);
            this.tbHost.Name = "tbHost";
            this.tbHost.Size = new System.Drawing.Size(208, 20);
            this.tbHost.TabIndex = 20;
            this.tbHost.TextChanged += new System.EventHandler(this.tbHost_TextChanged);
            // 
            // tbCoHost
            // 
            this.tbCoHost.Location = new System.Drawing.Point(591, 239);
            this.tbCoHost.Name = "tbCoHost";
            this.tbCoHost.Size = new System.Drawing.Size(208, 20);
            this.tbCoHost.TabIndex = 20;
            this.tbCoHost.TextChanged += new System.EventHandler(this.tbCoHost_TextChanged);
            // 
            // btShowTitle1
            // 
            this.btShowTitle1.Location = new System.Drawing.Point(805, 212);
            this.btShowTitle1.Name = "btShowTitle1";
            this.btShowTitle1.Size = new System.Drawing.Size(75, 23);
            this.btShowTitle1.TabIndex = 21;
            this.btShowTitle1.Text = "Show";
            this.btShowTitle1.UseVisualStyleBackColor = true;
            this.btShowTitle1.Click += new System.EventHandler(this.btShowTitle1_Click);
            // 
            // btShowTitle2
            // 
            this.btShowTitle2.Location = new System.Drawing.Point(805, 237);
            this.btShowTitle2.Name = "btShowTitle2";
            this.btShowTitle2.Size = new System.Drawing.Size(75, 23);
            this.btShowTitle2.TabIndex = 21;
            this.btShowTitle2.Text = "Show";
            this.btShowTitle2.UseVisualStyleBackColor = true;
            this.btShowTitle2.Click += new System.EventHandler(this.btShowTitle2_Click);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(250, 14);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Place:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbPlace
            // 
            this.tbPlace.Location = new System.Drawing.Point(311, 11);
            this.tbPlace.Name = "tbPlace";
            this.tbPlace.Size = new System.Drawing.Size(186, 20);
            this.tbPlace.TabIndex = 22;
            this.tbPlace.TextChanged += new System.EventHandler(this.tbPlace_TextChanged);
            // 
            // btStartExplain3sec
            // 
            this.btStartExplain3sec.Location = new System.Drawing.Point(793, 141);
            this.btStartExplain3sec.Name = "btStartExplain3sec";
            this.btStartExplain3sec.Size = new System.Drawing.Size(102, 23);
            this.btStartExplain3sec.TabIndex = 23;
            this.btStartExplain3sec.Text = "Start Explain 3 Sec";
            this.btStartExplain3sec.UseVisualStyleBackColor = true;
            this.btStartExplain3sec.Click += new System.EventHandler(this.btStartExplain3sec_Click);
            // 
            // ucRaidTheCage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.btStartExplain3sec);
            this.Controls.Add(this.tbPlace);
            this.Controls.Add(this.btShowTitle2);
            this.Controls.Add(this.btShowTitle1);
            this.Controls.Add(this.tbCoHost);
            this.Controls.Add(this.tbHost);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.bmbLogging);
            this.Controls.Add(this.lblogging2);
            this.Controls.Add(this.lblogging1);
            this.Controls.Add(this.btLightReset);
            this.Controls.Add(this.btCloseCageDoor);
            this.Controls.Add(this.btOpenCageDoor);
            this.Controls.Add(this.btWWTBAMLifeLineRiminderStrap);
            this.Controls.Add(this.btShowMoneyTreeOnProjector);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.cbLifeLineUsed2);
            this.Controls.Add(this.cbLifeLineUsed1);
            this.Controls.Add(this.btUseLifeLine2);
            this.Controls.Add(this.btUseLifeLine1);
            this.Controls.Add(this.lblifeline2);
            this.Controls.Add(this.lblifeline1);
            this.Controls.Add(this.gbPrices);
            this.Controls.Add(this.tbName2);
            this.Controls.Add(this.tbName1);
            this.Controls.Add(this.lbName2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lbName1);
            this.Name = "ucRaidTheCage";
            this.Size = new System.Drawing.Size(898, 390);
            this.Load += new System.EventHandler(this.ucRaidTheCage_Load);
            this.Resize += new System.EventHandler(this.ucRaidTheCage_Resize);
            this.gbPrices.ResumeLayout(false);
            this.gbPrices.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbTimeLeft;
        private System.Windows.Forms.Button btStopCageTimer;
        private System.Windows.Forms.Label lbName1;
        private System.Windows.Forms.TextBox tbName1;
        private System.Windows.Forms.TextBox tbName2;
        private System.Windows.Forms.Label lbthisquestion;
        private System.Windows.Forms.TextBox tbPriceThisQuestion;
        private System.Windows.Forms.Label lbTotal;
        private System.Windows.Forms.TextBox tbPriceTotal;
        private System.Windows.Forms.GroupBox gbPrices;
        private System.Windows.Forms.CheckBox cbLifeLineUsed2;
        private System.Windows.Forms.CheckBox cbLifeLineUsed1;
        private XCtrls.BitmapButton btUseLifeLine2;
        private XCtrls.BitmapButton btUseLifeLine1;
        private System.Windows.Forms.Label lblifeline2;
        private System.Windows.Forms.Label lblifeline1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btShowMoneyTreeOnProjector;
        private System.Windows.Forms.Button btWWTBAMLifeLineRiminderStrap;
        private System.Windows.Forms.Button btShowTotalAmount;
        private System.Windows.Forms.Button btOpenCageDoor;
        private System.Windows.Forms.Button btCloseCageDoor;
        private System.Windows.Forms.Label lbName2;
        private System.Windows.Forms.Button btLightReset;
        private XCtrls.BitmapButton bmbLogging;
        private System.Windows.Forms.Label lblogging2;
        private System.Windows.Forms.Label lblogging1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbHost;
        private System.Windows.Forms.TextBox tbCoHost;
        private System.Windows.Forms.Button btShowTitle1;
        private System.Windows.Forms.Button btShowTitle2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbPlace;
        private System.Windows.Forms.Button btStartExplain3sec;
    }
}
