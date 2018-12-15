using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace RaidTheCageCtrl
{
    public partial class LicenseSplash2 : Form
    {
        public bool restart;
        public bool visible;
        //private bool ok;
        //private AxQTOControlLib.AxQTControl axQTControl1;
        private int timercnt;
        private SonySoftwareProtectionLicense mLic;        
        public LicenseSplash2(SonySoftwareProtectionLicense inLic)
        {
            restart = false;            
            mLic = inLic;
            InitializeComponent();
            visible = false;
            timercnt=10;
            string errMsg = "";
            string pathtomovie = "";
            btOK.Text="OK (10)";
            if (inLic.mValid)
            {
                tbNewLic.Visible = false;
                tbSetLicense.Visible = false;
                btCancel.Visible = false;
                // set date...                
                lbLicValidDate.Text = inLic.mEndDate.ToString("dd/MM/yyyy");
                timer1.Start();
            }
            else
            {
                // ok button gone!
                btOK.Visible = false;
                btUpdateLicense.Visible = false;
            }
            try
            {
                visible = true;
                pictureBox1.Visible = true;
            }
            catch (COMException ex)
            {
                //MessageBox.Show("Exeption1");
                // catch COM exceptions from QT control
                errMsg = "Error Code: " + ex.ErrorCode.ToString("X") + "\n";
                //QTUtils qtu = new QTUtils();
                //errMsg += "QT Error code : " + qtu.QTErrorFromErrorCode(ex.ErrorCode).ToString();
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Exeption2");
                // catch any exception
                errMsg = ex.ToString();
            }

            if (errMsg != "")
            {
                string msg = "Unable to open movie:\n\n";
                msg += pathtomovie + "\n\n";
                msg += errMsg;
                //MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                axQTControl1.Dispose();
                //MessageBox.Show("before close");
                //this.Close();

            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (--timercnt <= 0)
            {
                timer1.Stop();
                btOK.PerformClick();
            }
            else
            {
                btOK.Text=string.Format("OK ({0})",timercnt);
            }
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            
            DialogResult = DialogResult.Cancel;
            
            Close();
        }

        private void btUpdateLicense_Click(object sender, EventArgs e)
        {
            btUpdateLicense.Visible = false;
            tbNewLic.Visible = true;
            tbSetLicense.Visible = true;
            btCancel.Visible = true;
            timer1.Stop();
            btOK.Text = "OK";
        }

        private void tbSetLicense_Click(object sender, EventArgs e)
        {
            if (mLic.CheckNewLicense(tbNewLic.Text))
            {
                restart = true;
                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
