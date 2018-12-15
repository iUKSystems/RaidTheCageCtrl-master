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
    public partial class LicenseSplash : Form
    {
        public bool restart;
        public bool visible;
        //private bool ok;
        //private AxQTOControlLib.AxQTControl axQTControl1;
        private int timercnt;
        private SonySoftwareProtectionLicense mLic;  
        private HaspDongle mTheDongle;


        public LicenseSplash(SonySoftwareProtectionLicense inLic)
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
                //DateTime untildate = mTheDongle.SystemTimeFromDaysSince2000(Convert.ToInt32(inLic.mEnd));
                //lbLicValidDate.Text = untildate.Date.ToString("dd/MM/yyyy");
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
//                axQTControl1 = new AxQTOControlLib.AxQTControl();
                System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LicenseSplash));
/*                ((System.ComponentModel.ISupportInitialize)(this.axQTControl1)).BeginInit();

                axQTControl1.AccessibleRole = System.Windows.Forms.AccessibleRole.None;

                axQTControl1.AllowDrop = false;
                axQTControl1.Enabled = true;
                axQTControl1.Location = new System.Drawing.Point(24, 0);
                axQTControl1.Name = "axQTControl1";
                axQTControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axQTControl1.OcxState")));
                axQTControl1.Size = new System.Drawing.Size(400, 300);
                axQTControl1.TabIndex = 0;

                //axQTControl1.MouseDownEvent += new AxQTOControlLib._IQTControlEvents_MouseDownEventHandler(this.axQTControl1_MouseDownEvent);
                //axQTControl1.MouseUpEvent += new AxQTOControlLib._IQTControlEvents_MouseUpEventHandler(this.axQTControl1_MouseUpEvent);

                Controls.Add(this.axQTControl1);
*/
                //MessageBox.Show("Before Movie Path");
//                ((System.ComponentModel.ISupportInitialize)(this.axQTControl1)).EndInit();

//                pathtomovie = string.Format("{0}\\Assets\\Splash.mov", Application.StartupPath);


//                axQTControl1.MovieControllerVisible = false;
                //axQTControl1.FileName = pathtomovie;

/*
                axQTControl1.Width = 340;
                axQTControl1.Height = 106;

                //axQTControl1.SetScale(1);
                axQTControl1.URL = "";
                axQTControl1.URL = pathtomovie;
                axQTControl1.Movie.Loop = true;
                axQTControl1.Movie.Rewind();
                axQTControl1.Movie.Play(1);
 */
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

            /*
            if (mTheDongle.CheckNewLicense(tbNewLic.Text, mTheDongle.mProductcode))
            {
                restart = true;
                DialogResult = DialogResult.OK;
                Close();
            }
             */
        }
    }
}
