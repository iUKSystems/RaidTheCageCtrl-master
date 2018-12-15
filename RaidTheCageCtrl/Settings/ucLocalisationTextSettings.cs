using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;
using WiseGuys.Settings;
using WiseGuysFrameWork2015.Settings;

namespace RaidTheCageCtrl.Settings
{
    public partial class ucLocalisationTextSettings : SettingsBase
    {
        private MainForm mParent;
        private SettingsEditor mSetParent;

        private bool mIsUpdating = false;

        public ucLocalisationTextSettings(MainForm pParent, SettingsEditor pSetParent, int InNumber)
            : base(pParent, pSetParent, InNumber)
        {
            InitializeComponent();

            if (pSetParent != null)
            {
                mSetParent = pSetParent;
                mSetParent.OnFormClosingMsg += OnFormClosingMsg;
            }
            mParent = pParent;

            

            mIsUpdating = true;
            CreateControls();

            UpdateDlg();

            mIsUpdating = false;
        }

        private void UpdateDlg()
        {
            tbSecondLanguageName.Text = MultiLangualTexts.mLanguageName;
        }

        private void Changed()
        {
            MultiLangualTexts.mChanged = true;
            mChanged = true;
        }

        private void OnFormClosingMsg(object sender)
        {
            if (MultiLangualTexts.mChanged)
            {
                // save...
                //mSetParent.mChanged[(int)ESettingsForms.Localisation] = true;
                MultiLangualTexts.WriteXmlConfig(mParent.ExePath);
                MultiLangualTexts.mChanged = false;
            }
        }

        private void CreateControls()
        {
            int cnt = 0;
            foreach (RaidTheCagesModes val in Enum.GetValues(typeof(RaidTheCagesModes)))
            {
                if (MultiLangualTexts.GetContinueTextEnglish(val) != "")
                {
                    Label lb = new Label();
                    lb.Text = MultiLangualTexts.GetContinueTextEnglish(val);
                    lb.Location = new Point(0, cnt * 22);
                    lb.Size = new Size(280, 20);
                    lb.TextAlign = ContentAlignment.MiddleRight;
                    this.panel1.Controls.Add(lb);

                    wgtextbox tb = new wgtextbox();
                    tb.rtcmode = val;
                    tb.Text = MultiLangualTexts.mlocalisationTexts[(int)val];
                    tb.Size = new Size(280, 20);
                    tb.TextChanged += tb_TextChanged;
                    tb.Location = new Point(290, cnt++ * 22);
                    this.panel1.Controls.Add(tb);
                }
            }

            cnt = 0;
            foreach (string str in MultiLangualTexts.EnglishTexts)
            {

                Label lb = new Label();
                lb.Text = MultiLangualTexts.EnglishTexts[cnt];
                lb.Location = new Point(0, cnt * 22);
                lb.Size = new Size(280, 20);
                lb.TextAlign = ContentAlignment.MiddleRight;
                this.panel2.Controls.Add(lb);

                wgtextbox2 tb = new wgtextbox2();
                tb.mTextnr = cnt;
                tb.Text = MultiLangualTexts.SecondLanguageTexts[cnt];
                tb.Size = new Size(280, 20);
                tb.TextChanged += tb_TextChanged2;
                tb.Location = new Point(290, cnt++ * 22);
                this.panel2.Controls.Add(tb);

            }


        }

        void tb_TextChanged(object sender, EventArgs e)
        {
            if (mIsUpdating)
                return;

            wgtextbox tb = (wgtextbox)sender;

            // ok.. we need to update the class... first set the mchanged to true, so everything will be saved
            Changed();

            MultiLangualTexts.mlocalisationTexts[(int)tb.rtcmode] = tb.Text;
        }

        void tb_TextChanged2(object sender, EventArgs e)
        {
            if (mIsUpdating)
                return;

            wgtextbox2 tb = (wgtextbox2)sender;

            // ok.. we need to update the class... first set the mchanged to true, so everything will be saved
            Changed();

            MultiLangualTexts.SecondLanguageTexts[tb.mTextnr] = tb.Text;
        }

        private void tbSecondLanguageName_TextChanged(object sender, EventArgs e)
        {
            if (!mIsUpdating)
            {
                MultiLangualTexts.mChanged = true;
                MultiLangualTexts.mLanguageName = tbSecondLanguageName.Text;
            }
        }
    }
}
