﻿//#define USINGDEMO
#define USEENCRYPT
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using com.pakhee.common;
//using Infragistics.Win;
//using Infragistics.Win.UltraWinProgressBar;
//using Infragistics.Win.UltraWinStatusBar;
using DejaVu;
using DejaVu.Collections.Generic;
using Infralution.Licensing;
using LogFile;
using RaidTheCageCtrl.GameParts;
using RaidTheCageCtrl.Settings;
using SocketTools;
using WiseGuys.Settings;
using WiseGuys.WGNetWork2011;
using WiseGuys.configs;
using WiseGuysFrameWork;
using WiseGuysFrameWork.Audio;
using WiseGuysFrameWork2015;
using WiseGuysFrameWork2015.Audio;
using WiseGuysFrameWork2015.Settings;
using WiseGuysFrameWork2015DIV;
using Ping = System.Net.NetworkInformation.Ping;

//using ProgressBarStyle = System.Windows.Forms.ProgressBarStyle;

enum eSettings
{
    GeneralSettings = 0,
    ConnectionSettings = 1,
    LocalisationSettings = 2,
    MidiSettings = 3,
    LocalisationTextSettings = 4,            
}



namespace RaidTheCageCtrl
{
    public partial class MainForm : WGForm
    {
        [Flags]
        private enum KeyStates
        {
            None = 0,
            Down = 1,
            Toggled = 2
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern short GetKeyState(int keyCode);

        public WGAudio mAudio;

        public Form frm;

        private bool mLogoProjectorHongarije = true;

        //private WGMultiCast mMultiCast;

        public MidiDebug wgmidi = new MidiDebug();        

        public bool mSwitchAnswersWithQuestion;

        HaspDongle theDongle;
        DialogResult returncode;
        LicenseSplash newsplashlicense;
        License lic;

        private bool mHostMessagingconnected = false;

        private bool mUseOverlayAsPreview;

        private SettingsEditor setdlg;

        private bool mLicenseOk = false;

        public int mArticlelengthbeforebreaking;

        public bool mAlwaysskiparticlelist;

        private string mBaseFormText;

        /// <summary>
        /// engines 
        /// </summary>
        public WGGFXEngine[] mGFXEngines = new WGGFXEngine[(int)eEngines.MAX];
        private WGConnector[] mGFXEnginesConnectors = new WGConnector[(int)eEngines.MAX];
        private WGConnector mGFXEnginesConnectorHost2;

        public bool[] mGFXEnginesConnected = new bool[(int)eEngines.MAX];
        List<PictureBox> lEnginePictureBoxes = new List<PictureBox>();

        //public List<datagriditemdata> litemscurquestion = new List<datagriditemdata>();
        public List<datagriditemdata> litemscurquestionOrder;

        // bitmaps
        public System.Drawing.Bitmap bmDisConnected;
        public System.Drawing.Bitmap bmConnected;
        public System.Drawing.Bitmap bmDisabled;
        public System.Drawing.Bitmap bmConnect;
        public System.Drawing.Bitmap bmConnect1;        
        

        private string mReceivedfromQDB = "";

        //public List<QuestionData> lQuestions = new List<QuestionData>();
        private List<QuestionData> lQuestions1 = new List<QuestionData>();
        private List<QuestionData> lQuestions2 = new List<QuestionData>();
        public List<List<QuestionData>> lAllQuestions = new List<List<QuestionData>>();

        public long mScoreThisQuestion;

        private eSortItems mCurSortItems = eSortItems.LOWTOHIGH;

        public bool mLogoProjector = false;

        public eCountries mCurCountry;

        // midi
        System.Timers.Timer miditmr = new System.Timers.Timer();
        private bool mMidiConnected = false;
        private bool mMidiConnecting = false;
        private int ClientID;

        

        private bool mUseHostTabletAndMessaging;

        private System.Windows.Forms.Timer mLicenseCheck = new System.Windows.Forms.Timer();
        private SonySoftwareProtectionLicense softlic;// = new SonySoftwareProtectionLicense(this);
        private bool mUsingsoftwareProtection;


         //////////////////////////////////////////////////////////////
        // for undo... ///////////////////////////////////////////////
        //////////////////////////////////////////////////////////////
        public UndoRedo<int> mQnr = new UndoRedo<int>(0);

        public UndoRedo<int> mSelectedAnswer = new UndoRedo<int>(-1);

        public UndoRedo<bool> mFirstQuestion = new UndoRedo<bool>(true);

        public UndoRedo<eStackTypes> MCurStackType = new UndoRedo<eStackTypes>(eStackTypes.Main);

        //public UndoRedoList<datagriditemdata> litemscurquestionUndo = new UndoRedoList<datagriditemdata>();
        public List<datagriditemdata> litemscurquestionUndo = new List<datagriditemdata>();
        

        //private int ClientID = 0;

        private ucGeneralSettings genset;
        private ucConnectionSettings conset;
        private ucLocalisationSettings loctestset;        
        private ucMidiTextSettings midiset;
        private ucLocalisationTextSettings gclocalisation;

        public SocketWrench ipdaemonHostMessaging = new SocketWrench();
        private int ipdaemonHostMessagingConnecthandle = -1;



        public MainForm()
        {
            



            //MessageBox.Show("Start APP1");
            Stopwatch st1 = new Stopwatch();
            st1.Start();
            InitializeComponent();
            //MessageBox.Show("Start APP2");
            long mil1 = st1.ElapsedMilliseconds;
            Console.WriteLine(mil1);
            LoadDefaults(true);
            //MessageBox.Show("Start APP3");
            long mil2 = st1.ElapsedMilliseconds;
            Console.WriteLine(mil2);
            //visualStyler1.License = SkinSoft.VisualStyler.Licensing.VisualStylerLicense.FromResource(visualStyler1,
            //                                                                          "RaidTheCageCtrl.WiseGuys.VisualStyler.License.resources");

            //visualStyler1.LoadVisualStyle(ExePath + @"\OSX (Brushed).vssf");
            //visualStyler1.Refresh();

            //ipportMidi.RuntimeLicense =
            //    "31504E3841414E585246394A4132303030300000000000000000000000000000000000000000000032594E374744545300005357365636584B4A463530500000";
            //ipdaemonHostMessaging.RuntimeLicense =
            //    "31504E3841414E585246394A4132303030300000000000000000000000000000000000000000000032594E374744545300005357365636584B4A463530500000";            

            if (!ipdaemonHostMessaging.Initialize("ENGNPGIPIRLFTJGMIGSTHVHLCWBLVL"))
            {
                MessageBox.Show("Unable to initialize Socket server");
                Application.Exit();
                Close();
                return;
            }
            ipdaemonHostMessaging.KeepAlive = true;

            Softgroup.NetResize.License.LicenseName = "Patrick van der Lee";
            Softgroup.NetResize.License.LicenseUser = "Pat@wiseguys.tv";
            Softgroup.NetResize.License.LicenseKey = "GNA6S7ECTWAX6IBBOMHMAEXDC";

            mLicenseCheck.Interval = 5 * 60 * 1000; // every 5 minutes
            mLicenseCheck.Tick += new EventHandler(mLicenseCheck_Tick);

            softlic = new SonySoftwareProtectionLicense(this);

            //Softgroup.NetResize.License.LicenseName = "Patrick van der Lee";
            //Softgroup.NetResize.License.LicenseUser = "Patrick@WiseGuys.tv";
            //Softgroup.NetResize.License.LicenseKey = "LHAGCE1ASSIU2QBWOEDG2K2UF";

            //if (!CheckWhichVersionOfNet45Installed())
            //    MessageBox.Show(
            //        ".NET 4.5 is not installed.. please install for proper working of this software package!");
            //mMultiCast = new WGMultiCast(this, ProjectID, "01", statusStrip1, toolStripStatusLabel1);
            //mMultiCast.OnLicenseChange += OnLicenseChange;
            //mMultiCast.OnReceivedUDP += OnReceivedUDP;
           
#if USINGDEMO
            EvaluationMonitor evaluationMonitor = new RegistryEvaluationMonitor("WebMon0003");
            DateTime currentdate = DateTime.Now;

            /*
            System.Reflection.Assembly ass = System.Reflection.Assembly.GetExecutingAssembly();
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(ass.Location);
             */
            Process[] process = Process.GetProcesses();  // Process.GetProcesses(); if you dont have.
            Process foundproc = null;

            foreach (Process proc in process)
            {
                try
                {
                    if (proc.MainModule.FileName.IndexOf("RaidTheCageCtrl") != -1)
                    {
                        foundproc = proc;
                    }
                }
                catch (Exception)
                {


                }
            }
            string fullPath = "";
            DateTime creationdate = DateTime.Now;
            if (foundproc != null)
            {
                fullPath = foundproc.MainModule.FileName;
                creationdate = File.GetLastWriteTime(fullPath);
            }
            
            TimeSpan span = currentdate.Subtract(creationdate);

            if (evaluationMonitor.UsageCount == 1)//evaluationMonitor.LastUseDate == evaluationMonitor.FirstUseDate)
            {
                // first time we start the program! // check if we are allowed to run....
                

                //if (currentdate)   Application.
                //FileInfo fileinfo = new FileInfo(string.Format("{0}\\{1}",ExePath,mApplicationFileName));
                //DateTime Creationtime = fileinfo.LastWriteTime;

               
                if (span.Days >= 2)
                {
                    // error with license...
                    MessageBox.Show("Error Nr: 0x01");
                    evaluationMonitor.Reset(true);
                    Environment.Exit(2);
                    return;
                }
            }
            else if (DateTime.Now < evaluationMonitor.LastUseDate)
            {
                MessageBox.Show("Error Nr: 0x02");
                Environment.Exit(2);
                return;
            }
            else if (span.Days>10)
            {

                MessageBox.Show("Error Nr: 0x03");
                Environment.Exit(2);
                return;
            }
            // cool.. we can run normal now... this exe not runs longer then a week!
#endif

           
#if (!DEBUG)
            //spl.Hide();


            if (!CheckLicense(true))
            {
                Application.Exit();
                Close();
                return;
            }
            mLicenseCheck.Start();
#endif

            //CheckDirs();

           
/*
#if !DEBUG
           
            // first check the encrypted clientID.. and use it for the dongle check.
            if (!File.Exists(ExePath + "\\Client.id"))
            {
                MessageBox.Show("Cannot find Client.id, exiting!");
                System.Environment.Exit(0);   
            }

            try
            {
                StreamReader thereader = new StreamReader(ExePath + "\\Client.id");
                string encodedstring = thereader.ReadLine();
                thereader.Close();
                string decoded = EncDec.Decrypt(encodedstring, mPassword);

                
                int.TryParse(decoded, out ClientID);
            }
            catch (Exception)
            {

                MessageBox.Show("Error reading Client.id, exiting!");
                System.Environment.Exit(0);   
            }

            if (ClientID != 1111)            
            {
                do
                {
                    if (theDongle != null)
                        theDongle = null;

                    theDongle = new HaspDongle(2001, (ushort) ClientID);


                    lic = new License();

                    lic = theDongle.VerifyLicense(theDongle.mProductcode, (short) theDongle.mClient);
                    if (newsplashlicense != null)
                        newsplashlicense = null;
                    newsplashlicense = new LicenseSplash(theDongle, lic);
                    returncode = newsplashlicense.ShowDialog();
                } while (newsplashlicense.restart);
                if (returncode != DialogResult.OK)
                {
                    theDongle = null;
                    newsplashlicense.Dispose();
                    newsplashlicense = null;
                    lic = null;
                    System.Environment.Exit(0);
                }
            }
#endif
*/

            Midi.StartMidi(mPassword);         
            // also check a new xml file called wwtbam.xml where the right show will be loaded...
            IProfile m_profile;
            // read the xml file with all defaults.......
            m_profile = new Xml { Name = ExePath + "\\RaidTheCageStartUp.xml" };

            if (m_profile.HasSection("StartUpSettings"))
            {
                try
                {
                    mCurCountry = (eCountries)Enum.Parse(typeof(eCountries), m_profile.GetValue("StartUpSettings", "Country", "UK"), true);
                }
                catch (Exception)
                {
                    MessageBox.Show("Unable to get current country. Using UK now...valid options are: UK, TURKEY, RUSSIA, ROMANIA, ARGENTINA, URUGUAY, PERU, MEXICO , COLOMBIA, VIETNAM, PARAQUAY, PORTUGAL, HUNGARY, BRAZIL, URUGUAY2018");
                    mCurCountry = eCountries.UK;                    
                }
                
            }
            else
            {
                // create xml file... start using default UK...
                m_profile.SetValue("StartUpSettings", "Country", "UK");
                mCurCountry = eCountries.UK;    
            }

            if (!m_profile.HasSection("MainSettings"))
            {
                m_profile.SetValue("MainSettings", "Language", "ENGLISH");
                mCurLanguage = eLanguage.ENGLISH;
            }
            else
            {
                mCurLanguage = (eLanguage)Enum.Parse(typeof(eLanguage), m_profile.GetValue("MainSettings", "Language", "ENGLISH"), true);
            }

            if (!m_profile.HasSection("SpecialSettings"))
            {
                m_profile.SetValue("SpecialSettings", "SwitchAnswerWithQuestions", "False");
                mSwitchAnswersWithQuestion = false;
            }
            else
            {
                mSwitchAnswersWithQuestion = m_profile.GetValue("SpecialSettings", "SwitchAnswerWithQuestions", false);
            }

            if (!m_profile.HasSection("SpecialSettings2"))
            {
                m_profile.SetValue("SpecialSettings2", "articlelengthbeforebreaking2lines", 12);
                mArticlelengthbeforebreaking = 12;
            }
            else
            {
                mArticlelengthbeforebreaking = m_profile.GetValue("SpecialSettings2", "articlelengthbeforebreaking2lines", 12);
            }

            if (!m_profile.HasSection("SpecialSettings3"))
            {
                m_profile.SetValue("SpecialSettings3", "usehosttabletandmessaging", true);
                mUseHostTabletAndMessaging = true;
            }
            else
            {
                mUseHostTabletAndMessaging = m_profile.GetValue("SpecialSettings3", "usehosttabletandmessaging",true);
            }

            if (!m_profile.HasSection("SpecialSettings4"))
            {
                m_profile.SetValue("SpecialSettings4", "alwaysskiparticlelist", false);
                mAlwaysskiparticlelist = false;
            }
            else
            {
                mAlwaysskiparticlelist = m_profile.GetValue("SpecialSettings4", "alwaysskiparticlelist", false);
            }


            if (!m_profile.HasSection("SpecialSettings5"))
            {
                m_profile.SetValue("SpecialSettings5", "useOverlayAsPreview", false);
                mUseOverlayAsPreview = false;
            }
            else
            {
                mUseOverlayAsPreview = m_profile.GetValue("SpecialSettings5", "useOverlayAsPreview", false);
            }

            if (mCurCountry == eCountries.PORTUGAL)
                label10.Visible = false;

            if (mSwitchAnswersWithQuestion)
                btSwitchQuestion.Visible = false;

            for (int i = 0; i < 10; i++)
            {
                lQuestions1.Add(new QuestionData());
                lQuestions2.Add(new QuestionData());
                
            }

            lAllQuestions.Add(lQuestions1);
            lAllQuestions.Add(lQuestions2);

          

            //MessageBox.Show("ReadSettings");
            ReadSettings();

            /*
            ipportMidi.Config("TcpNoDelay=True");
            ipportMidi.AcceptData = false;
            ipportMidi.RemoteHost = connectionsettings.mIpAddresses[(int)eEngines.MIDI];
            ipportMidi.RemotePort = connectionsettings.mPorts[(int)eEngines.MIDI];
            ipportMidi.OnConnected += new Ipport.OnConnectedHandler(ipportMidi_OnConnected);
            ipportMidi.OnDisconnected += new Ipport.OnDisconnectedHandler(ipportMidi_OnDisconnected);
            // receive not needed.. send only!
            //ipportMidi.OnDataIn += new Ipport.OnDataInHandler(ipportMidi_OnDataIned);                
            ipportMidi.EOL = "\r\n";                      // default for standard communication
            ipportMidi.InvokeThrough = this;            
            miditmr.Interval = 1000;
            miditmr.Elapsed += new System.Timers.ElapsedEventHandler(miditmr_Elapsed);
            */


            //ipportMidi.Linger = false;
            //if (PingHost(connectionsettings.mIpAddresses[(int)eEngines.MIDI]))
            //ipportMidi.Connected = true;

            if (mUseHostTabletAndMessaging)
            {
                EnableListenHostMessagingServerPort();
            }
            else
            {
                pbHostMessaging.Visible = false;
                label9.Visible = false;
            }


            
            InitEngines();
            
            
            
            
            if (mUseHostTabletAndMessaging)
            {
                ConnectEngine(eEngines.HOST,true);
            }
            else
            {
                pbHost.Visible = false;
                label4.Visible = false;
            }

            ConnectEngine(eEngines.EXTRAQA, true);
            
            //MessageBox.Show("InitAudio");
            mAudio = new WGAudio(this,mGFXEngines[(int)eEngines.WISEAUDIO],300);
            //MessageBox.Show("CreateGameTabs");

            mAudio.SendAudio(SoundsRaidTheCage.EXPLAINTHEGAMES, SoundCommands.PLAY,false,false);

            using (UndoRedoManager.StartInvisible("createTabs"))
            {
                CreateTabs();
                UndoRedoManager.Commit();
            }

           

            MultiLangualTexts.SetParent(this);
            MidiSettings.SetParent(this);
            UpdateLanguageMenu();
            SetCtrlText(mCurLanguage);


            if (mCurLanguage == eLanguage.ENGLISH)
            {
                eNglishToolStripMenuItem.Checked = true;
                SetCtrlText(eLanguage.ENGLISH);
            }
            else if (mCurLanguage == eLanguage.SECONDLANGUAGE)
            {
                secondLanguageToolStripMenuItem.Checked = true;
                SetCtrlText(eLanguage.SECONDLANGUAGE);
            }
            else if (mCurLanguage == eLanguage.SECONDLANGUAGEENGLISH)
            {
                secondLanguageEnglishToolStripMenuItem.Checked = true;
                SetCtrlText(eLanguage.SECONDLANGUAGEENGLISH);
            }

            ActiveCtrl.UpdateDlg();

            //MessageBox.Show("SetDialogText");
            SetDialogText();
            //MessageBox.Show("UpdatePriceCageInfo");
            UpdatePriceCageInfo();
            //InitializeStatusBar();

            st1.Stop();
            long mil3 = st1.ElapsedMilliseconds;
            Console.WriteLine(mil3);

            if (mCurCountry == eCountries.PARAQUAY)
                btLogoProjector.Text = "LOGO 3D";

            ConnectEngines(true);
        }

        public static DateTime GetNistTime(out bool error)
        {
            error = true;
            try
            {
                var myHttpWebRequest = (HttpWebRequest)WebRequest.Create("http://www.microsoft.com");
                var response = myHttpWebRequest.GetResponse();
                string todaysDates = response.Headers["date"];
                DateTime dateTime = DateTime.ParseExact(todaysDates, "ddd, dd MMM yyyy HH:mm:ss 'GMT'", CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.AssumeUniversal);
                response = null;
                error = false;
                return dateTime;
            }
            catch (Exception)
            {
                try
                {
                    // unable to get the time through internet.. possible no internet connection, but try google.com.. otherwise error = true and return;
                    var myHttpWebRequest = (HttpWebRequest)WebRequest.Create("http://www.google.com");
                    var response = myHttpWebRequest.GetResponse();
                    string todaysDates = response.Headers["date"];
                    DateTime dateTime = DateTime.ParseExact(todaysDates, "ddd, dd MMM yyyy HH:mm:ss 'GMT'", CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.AssumeUniversal);
                    response = null;
                    error = false;
                    return dateTime;
                }
                catch (Exception)
                {
                    error = true;
                    return DateTime.Now;
                }
            }
        }

        /*
        private void OnReceivedUDP(object sender, string inString)
        {
            Debug.WriteLine(inString);
        }

        private void OnLicenseChange(object sender, bool active, string licvaliduntil)
        {
           

            if (licvaliduntil != "")
            {
                mLicenseOk = true;
                ConnectEngines(active);
                if (InvokeRequired)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        this.Text = mBaseFormText + " Licensed until: " + licvaliduntil;
                    });
                }
                else
                {
                    this.Text = mBaseFormText + " Licensed until: " + licvaliduntil;
                }
            }
            else
            {
                mLicenseOk = false;
                ConnectEngines(active);
                if (InvokeRequired)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        this.Text = mBaseFormText;
                    });
                }
                else
                {
                    this.Text = mBaseFormText;
                }
            }
        }
        */
        /*
         * ConnectEngine(eEngines.OVERLAY);
            ConnectEngine(eEngines.PROJECTOR);
            ConnectEngine(eEngines.WISEQ);
            ConnectEngine(eEngines.CAGE);
            ConnectEngine(eEngines.WISEAUDIO);
            ConnectEngine(eEngines.MIDI);
         */

        void mLicenseCheck_Tick(object sender, EventArgs e)
        {
            if (!CheckLicense(false))
            {
                Application.Exit();
                Close();             
            }            
        }

        private bool CheckLicense(bool firsttimecheck)
        {
            bool error = false;

            if (File.Exists(ExePath + @"\rtc.lic"))
            {
                mUsingsoftwareProtection = true;
                if (!softlic.ReadLicense(ExePath + @"\rtc.lic"))
                {
                    MessageBox.Show(
                        "Error reading software license file: rtc.lic, request a new one from Sony! Application will now exit..",
                        "Error");
                    return false;
                }
                else
                {
                    // so far so good.. now we need to check...
                    EvaluationMonitor evaluationMonitor = new RegistryEvaluationMonitor("FtpMon0040");
                    DateTime currentdate = DateTime.Now;

                    //evaluationMonitor.LastUseDate == evaluationMonitor.FirstUseDate)
                    if (evaluationMonitor.UsageCount == 1)                    
                    {
                        if (IsKeyDown(Keys.LShiftKey) && IsKeyDown(Keys.LControlKey))
                        {
                            // ok.. we have special situation.. just use the current time as new time and continue from here...
                            // this is an override to do if we are in the studio without internet connection..only for us to know! Never tell this to a client or they
                            // can re-install the software on any valid date!
                        }
                        else
                        {
                            // first time we start the program! // check if we are allowed to run....
                            DateTime internetdate = GetNistTime(out error);

                            if (error)
                            {                                
                                // we have an error.. no internet connection...
                                MessageBox.Show(
                                    "Reinstallation of software detected... please connect to the internet and restart this application to re-validate!");
                                evaluationMonitor.Reset(true);
                                return false;
                            }
                            DateTime nu = DateTime.Now;

                            TimeSpan difference;

                            if (internetdate >= nu)
                            {
                                difference = internetdate - nu;
                            }
                            else
                            {
                                difference = nu - internetdate;
                            }

                            double daysdifference = difference.TotalDays;
                            //if (currentdate)   Application.
                            //FileInfo fileinfo = new FileInfo(string.Format("{0}\\{1}",ExePath,mApplicationFileName));
                            //DateTime Creationtime = fileinfo.LastWriteTime;
                            // must be a new installation.. so we need to check the current time and date on this PC....
                            //using (WebResponse response = WebRequest.Create("http://www.microsoft.com").GetResponse())
                            //{
                            //    return DateTime.ParseExact(response.Headers["date"], "ddd, dd MMM yyyy HH:mm:ss 'GMT'", CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.AssumeUniversal);
                            //}

                            if (daysdifference > 2)
                            {                                
                                MessageBox.Show(
                                    "This computer date and time is not on the current date and Time, Please adjust to the current date and Time and run this application again to validate");
                                evaluationMonitor.Reset(true);
                                return false;
                            }
                            else
                            {
                                // all ok.. send email..
                                //string content = SendBackLicenceInfo(inLicense);
                                //emails.AddEmailForSending("Patrick@wiseguys.tv", "RTC reinstallation!", content);
                            }
                        }
                        /*
                        Process[] process = Process.GetProcesses(); // Process.GetProcesses(); if you dont have.
                        Process foundproc = null;

                        foreach (Process proc in process)
                        {
                            try
                            {
                                if (proc.MainModule.FileName.IndexOf("RaidTheCageCtrl-V") != -1)
                                {
                                    foundproc = proc;
                                }
                            }
                            catch (Exception)
                            {


                            }
                        }
                        string fullPath = "";
                        DateTime creationdate = DateTime.Now;
                        if (foundproc != null)
                        {
                            fullPath = foundproc.MainModule.FileName;
                            creationdate = File.GetLastWriteTime(fullPath);
                        }
                        else
                        {
                            MessageBox.Show("Unable to find the executable name, did you rename it? Exiting now!");
                            Environment.Exit(2);
                        }

                        TimeSpan span = currentdate.Subtract(creationdate);


                        if (span.Days >= 3)
                        {
                            // error with license...                            
                            MessageBox.Show("Error Nr: 0x01");
                            evaluationMonitor.Reset(true);
                            Environment.Exit(2);
                            return;
                        }
                         */
                    }
                    else if (DateTime.UtcNow < evaluationMonitor.LastUseDate)
                    {
                        MessageBox.Show("Tempering with Date/Time Detected! Program will now exit! Put the computer date/time on the current date/time and restart this application!");
                        return false;
                    }

                    try
                    {
                        StreamReader thereader = new StreamReader(ExePath + "\\Client.id");
                        string encodedstring = thereader.ReadLine();
                        thereader.Close();
                        string decoded = EncDec.Decrypt(encodedstring, mPassword);


                        int.TryParse(decoded, out ClientID);
                    }
                    catch (Exception)
                    {

                        MessageBox.Show("Error reading Client.id, exiting!");
                        return false;
                    }

                    // check Client.id from license with this one...
                    if (softlic.mClientID != ClientID)
                    {
                        // this license is not for this client...
                        MessageBox.Show("This license file is not for this client.... aborting!");
                        return false;
                    }

                    try
                    {
                        StreamReader thereader = new StreamReader(ExePath + "\\Project.id");
                        string encodedstring = thereader.ReadLine();
                        thereader.Close();
                        string decoded = EncDec.Decrypt(encodedstring, mPassword);


                        int.TryParse(decoded, out ProjectID);
                    }
                    catch (Exception)
                    {

                        MessageBox.Show("Error reading Project.id, exiting!");
                        return false;
                    }

                    // check Client.id from license with this one...
                    if (softlic.mProjectID != ProjectID)
                    {
                        // this license is not for this client...
                        MessageBox.Show("This license file is not for this project.... aborting!");
                        return false;
                    }



                    // ok.. so far so good... time has not been turned back.. or the program is reinstalled...
                    // now check the license if we are allowed to run.... and check the checksum!
                    // now also check the hardware code...
                    // get the hardware code..
                    //if (softlic.mHardwareCode != softlic.GetSerialHardisk())
                    if (!softlic.GetSerialHardisk2().Contains(softlic.mHardwareCode))                    
                    {
                        // not for this harddisk..                        
                        MessageBox.Show("Computer Hardware not Matching License, Ask for a new License from Sony after installation on another PC!");                        
                        return false;
                    }


                    bool done = false;



                    if (firsttimecheck)
                    {
                        //LicenseSplash2 spl2;
                        do
                        {

                            do
                            {
                                if (softlic.CheckLicense())
                                {
                                    softlic.mValid = true;
                                    done = true;
                                }
                                newsplashlicense = new LicenseSplash(softlic);
                                returncode = newsplashlicense.ShowDialog();
                            } while (newsplashlicense.restart);


                            if (returncode != DialogResult.OK)
                            {
                                newsplashlicense.Dispose();
                                newsplashlicense = null;
                                softlic = null;
                                return false;
                            }
                            else
                            {

                            }

                            newsplashlicense.Dispose();
                            newsplashlicense = null;
                        } while (!done);
                    }

                    /*
                    else
                        {
                            MessageBox.Show("Error with License! Quiting now!");
                            softlic = null;
                            System.Environment.Exit(0);
                            return;
                        }
                    } while (!done); 
                     */
                }
            }
            else
            {
                mUsingsoftwareProtection = false;
                // first check the encrypted clientID.. and use it for the dongle check.
                if (!File.Exists(ExePath + "\\Client.id"))
                {
                    MessageBox.Show("Cannot find Client.id, exiting!");
                    return false;
                }

                try
                {
                    StreamReader thereader = new StreamReader(ExePath + "\\Client.id");
                    string encodedstring = thereader.ReadLine();
                    thereader.Close();
                    string decoded = EncDec.Decrypt(encodedstring, mPassword);


                    int.TryParse(decoded, out ClientID);
                }
                catch (Exception)
                {

                    MessageBox.Show("Error reading Client.id, exiting!");
                    return false;
                }




                if (theDongle != null)
                    theDongle = null;
               theDongle = new HaspDongle(2001, (ushort)ClientID);

                lic = new License();

                lic = theDongle.VerifyLicense(theDongle.mProductcode, (short)theDongle.mClient);

                if (lic == null)
                {
                    // no valid licnese.. stop!
                    MessageBox.Show("Error with License! Quiting now!");
                    theDongle = null;
                    lic = null;
                    return false;
                }
            }
            return true;
        }

        public static bool IsKeyDown(Keys key)
        {
            return KeyStates.Down == (GetKeyState(key) & KeyStates.Down);
        }

        private static KeyStates GetKeyState(Keys key)
        {
            KeyStates state = KeyStates.None;

            short retVal = GetKeyState((int)key);

            //If the high-order bit is 1, the key is down
            //otherwise, it is up.
            if ((retVal & 0x8000) == 0x8000)
                state |= KeyStates.Down;

            //If the low-order bit is 1, the key is toggled.
            if ((retVal & 1) == 1)
                state |= KeyStates.Toggled;

            return state;
        }

        private void ConnectEngines(bool connect)
        {
            if (pbOverlay.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    ConnectEngine(eEngines.OVERLAY, connect);
                    ConnectEngine(eEngines.PROJECTOR, connect);
                    ConnectEngine(eEngines.WISEQ, connect);
                    ConnectEngine(eEngines.CAGE, connect);                                        
                    ConnectEngine(eEngines.WISEAUDIO, connect);
                    ConnectEngine(eEngines.MIDI, connect);
                    
                });
            }
            else
            {
                ConnectEngine(eEngines.OVERLAY, connect);
                ConnectEngine(eEngines.PROJECTOR, connect);
                ConnectEngine(eEngines.WISEQ, connect);
                ConnectEngine(eEngines.CAGE, connect);
                ConnectEngine(eEngines.WISEAUDIO, connect);
                ConnectEngine(eEngines.MIDI, connect);
            }
        }

        public bool CheckWhichVersionOfNet45Installed()
        {
            object ob = Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full", "Release", 0);

            int version = 0;

            if (int.TryParse(ob + "", out version))
            {
                if (version == 378389 || version == 378758 || version == 379839 || version > 378758)
                    return true;
                /*
                if (version == 378389)
                    return ".NET Framework 4.5 is installed.";
                else if (version == 378758)
                    return ".NET Framework 4.5.1 is installed.";
                else if (version > 378758)
                    return "newer than .NET Framework 4.5.1 is installed.";
                 */
            }

            //return ".NET Framework 4.5 is not installed.";
            return false;
        }

        public static bool PingHost(string nameOrAddress)
        {
            bool pingable = false;
            Ping pinger = new Ping();

            try
            {
                PingReply reply = pinger.Send(nameOrAddress);

                pingable = reply.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                // Discard PingExceptions and return false;
            }

            return pingable;
        }

        private void UpdateLanguageMenu()
        {
            secondLanguageToolStripMenuItem.Text = MultiLangualTexts.mLanguageName;
            secondLanguageEnglishToolStripMenuItem.Text = MultiLangualTexts.mLanguageName + "-English";
        }

        private void SetDialogText()
        {
            mBaseFormText = "RaidTheCage V1.92";
            this.Text = mBaseFormText;
            //this.Text = "RaidTheCage V0.99";
#if !DEBUG 
            if (!mUsingsoftwareProtection)
                this.Text += string.Format(" Licensed until: {0} - {1}", theDongle.SystemTimeFromDaysSince2000(Convert.ToInt32(lic.mEnd)).ToString("dd-MM-yyyy"), Path.GetFileName(Application.ExecutablePath));
            else
                this.Text += string.Format(" Licensed until: {0} - {1}", softlic.mEndDate.ToString("dd-MM-yyyy"), Path.GetFileName(Application.ExecutablePath));
#endif
        }

        private void EnableListenHostMessagingServerPort()
        {
            //ipdaemonHostMessaging.OnConnect += ipdaemonHostMessaging_OnConnect;
            ipdaemonHostMessaging.ThreadModel = SocketWrench.ThreadingModel.modelFreeThread;
            ipdaemonHostMessaging.OnAccept += ipdaemonHostMessaging_OnAccept;
            ipdaemonHostMessaging.OnDisconnect += ipdaemonHostMessaging_OnDisconnect;
            ipdaemonHostMessaging.OnError += ipdaemonHostMessaging_OnError;
            ipdaemonHostMessaging.OnRead += ipdaemonHostMessaging_OnRead;
            ipdaemonHostMessaging.KeepAlive = true;
            ipdaemonHostMessaging.NoDelay = true;
            ipdaemonHostMessaging.Blocking = false;

            if (!ipdaemonHostMessaging.Listen(connectionsettings.mPorts[(int)eEngines.HOSTMESSAGING]))
            {
                MessageBox.Show(string.Format("Unable to listen on Port:{0}, program will now exit", connectionsettings.mPorts[(int)eEngines.HOSTMESSAGING]), "error");
                Application.Exit();
                Close();
                return;
            }

            /*
            ipdaemonHostMessaging.DefaultEOL = "\r\n";
            ipdaemonHostMessaging.LocalPort = connectionsettings.mPorts[(int)eEngines.HOSTMESSAGING];
            // setting the events.....
            ipdaemonHostMessaging.OnConnected += new Ipdaemon.OnConnectedHandler(ipdaemonHostMessaging_OnConnected);
            ipdaemonHostMessaging.OnConnectionRequest += new Ipdaemon.OnConnectionRequestHandler(ipdaemonHostMessaging_OnConnectionRequest);
            ipdaemonHostMessaging.OnDisconnected += new Ipdaemon.OnDisconnectedHandler(ipdaemonHostMessaging_OnDisconnected);
            bool check = ipdaemonHostMessaging.Listening = true;

            if (!check)
            {
                // unable to listen.. messagebox...
                MessageBox.Show(
                    "Unable to open socket for Messaging Host system, please try another port and restart the program!");
            }
             */
        }

        void ipdaemonHostMessaging_OnAccept(object sender, SocketWrench.AcceptEventArgs e)
        {
            

            if (ipdaemonHostMessagingConnecthandle == -1)
            {
                mHostMessagingconnected = true;
                // ok.. we have a connection.. no need to check how may.. just turn the led green...
                pbHostMessaging.Image = Properties.Resources.Led_Green;
                ipdaemonHostMessaging.Accept(e.Handle);
                ipdaemonHostMessagingConnecthandle = e.Handle;
            }
            else
            {
                ipdaemonHostMessaging.Reject();
            }
        }

        void ipdaemonHostMessaging_OnRead(object sender, EventArgs e)
        {
            /*
            string strBuffer = null;

            ipdaemonHostMessaging.ReadLine(ref strBuffer);

            if (strBuffer == "")
                return;
             */
        }

        void ipdaemonHostMessaging_OnError(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        void ipdaemonHostMessaging_OnDisconnect(object sender, EventArgs e)
        {
            pbHostMessaging.Image = Properties.Resources.Led_Red;
            ipdaemonHostMessagingConnecthandle = -1;
            ipdaemonHostMessaging.Disconnect();
            ipdaemonHostMessaging.Listen(connectionsettings.mPorts[(int)eEngines.HOSTMESSAGING]);            
            mHostMessagingconnected = false;
            
            

            //}
        }

        

        /*
        void ipdaemonHostMessaging_OnDisconnected(object sender, IpdaemonDisconnectedEventArgs e)
        {
            // ok, we have a disconnection... we need to check if we still have any available connection....
            //if (ipdaemonHostMessaging.Connections.Count==1)
            //{
            mHostMessagingconnected = false;
            pbHostMessaging.Image = Properties.Resources.Led_Red;


            //}
        }
        

        void ipdaemonHostMessaging_OnConnectionRequest(object sender, IpdaemonConnectionRequestEventArgs e)
        {
            // we only want 1 host messaging connected to the system!!
            if (mHostMessagingconnected)
                e.Accept = false;
        }
         */
        /*
        void ipdaemonHostMessaging_OnConnected(object sender, IpdaemonConnectedEventArgs e)
        {
            mHostMessagingconnected = true;
            // ok.. we have a connection.. no need to check how may.. just turn the led green...
            pbHostMessaging.Image = Properties.Resources.Led_Green;
            // test send msg back...
            //ipdaemonHostMessaging.SendLine(e.ConnectionId,"I got your connection.... good work!");
            ipdaemonHostMessaging.Connections[e.ConnectionId].AcceptData = true;
            ipdaemonHostMessaging.Connections[e.ConnectionId].EOL = "\r\n";
            
        }
         */

        /*
private void InitializeStatusBar()
{

	// Note: Under windows XP if the 'SupportThemes' property
	// is left is True (its default setting) then some of
	// the explicit appearance, border style and button
	// style properties are ignored.
	//this.ultraStatusBar1.SupportThemes = false;
	
	// Set the border style for the status bar control
	//this.ultraStatusBar1.BorderStyle = UIElementBorderStyle.Rounded1;
	
	// Set the default border style for panels
	//this.ultraStatusBar1.BorderStylePanel = UIElementBorderStyle.Rounded1;
	
	// Set the style for button type panels
	//this.ultraStatusBar1.ButtonStyle = UIElementButtonStyle.PopupSoftBorderless;
	
	// Set the # of pixels between panels 
	this.ultraStatusBar1.InterPanelSpacing = 3;
	
	// Specify the margins inside the status bar control.			
	this.ultraStatusBar1.Padding = new UIElementMargins(2, 1,1,2);

	// Set some apperance setting for the control
            this.ultraStatusBar1.Appearance.BackGradientStyle = GradientStyle.VerticalWithGlassRight50;
	this.ultraStatusBar1.Appearance.BackColor = Color.Blue;
	this.ultraStatusBar1.Appearance.BackColor2 = Color.Aqua;

	// Set the default appearance for panels
	this.ultraStatusBar1.PanelAppearance.BackColor = Color.Transparent;

	// Set some additional properties on the control
	this.ultraStatusBar1.PanelsVisible = true;
	this.ultraStatusBar1.ResizeStyle = ResizeStyle.Immediate;
	this.ultraStatusBar1.ScaledImageSize = new Size(8,8);
	this.ultraStatusBar1.ScaleImages = ScaleImage.OnlyWhenNeeded;
	this.ultraStatusBar1.ShowToolTips = true;
	this.ultraStatusBar1.SizeGripVisible = DefaultableBoolean.True;
	this.ultraStatusBar1.UseMnemonic = true;
	this.ultraStatusBar1.WrapText = true;

	// Add some panels to the collection
	UltraStatusPanel panel;

	

	



	// Add a date style panel 
	panel = this.ultraStatusBar1.Panels.Add("P8", PanelStyle.Date );
	panel.DateTimeFormat = "MMM-dd-yyyy";

	// Add a time style panel 
	panel = this.ultraStatusBar1.Panels.Add("P9", PanelStyle.Time );
	panel.DateTimeFormat = "hh:mm:ss tt";

	

}
*/
       

        private void InitEngines()
        {
            lEnginePictureBoxes.Add(pbOverlay);
            lEnginePictureBoxes.Add(pbProjector);            
            lEnginePictureBoxes.Add(pbWiseQ);
            lEnginePictureBoxes.Add(pbWiseAudio);
            lEnginePictureBoxes.Add(pbHost);
            lEnginePictureBoxes.Add(pbCage);            
            lEnginePictureBoxes.Add(pbHostMessaging);
            lEnginePictureBoxes.Add(pbMidi);
            lEnginePictureBoxes.Add(pbExtraQA);

            mGFXEngines[(int)eEngines.OVERLAY] = new WGGFXEngine("OVERLAY");
            mGFXEngines[(int)eEngines.OVERLAY].OnDisconnect += OnDisconnect;
            mGFXEngines[(int)eEngines.OVERLAY].OnConnect += OnConnect;
            mGFXEnginesConnected[(int)eEngines.OVERLAY] = false;

            mGFXEngines[(int)eEngines.PROJECTOR] = new WGGFXEngine("PROJECTOR");
            mGFXEngines[(int)eEngines.PROJECTOR].OnDisconnect += OnDisconnect;
            mGFXEngines[(int)eEngines.PROJECTOR].OnConnect += OnConnect;
            mGFXEnginesConnected[(int)eEngines.PROJECTOR] = false;

            mGFXEngines[(int)eEngines.WISEQ] = new WGGFXEngine("WISEQ");
            mGFXEngines[(int)eEngines.WISEQ].OnDisconnect += OnDisconnect;
            mGFXEngines[(int)eEngines.WISEQ].OnConnect += OnConnect;
            mGFXEngines[(int)eEngines.WISEQ].OnDataIn += OnDataIn;
            mGFXEnginesConnected[(int)eEngines.WISEQ] = false;

            mGFXEngines[(int)eEngines.WISEAUDIO] = new WGGFXEngine("WISEAUDIO");
            mGFXEngines[(int)eEngines.WISEAUDIO].OnDisconnect += OnDisconnect;
            mGFXEngines[(int)eEngines.WISEAUDIO].OnConnect += OnConnect;
            mGFXEnginesConnected[(int)eEngines.WISEAUDIO] = false;

            mGFXEngines[(int)eEngines.HOST] = new WGGFXEngine("HOST");
            mGFXEngines[(int)eEngines.HOST].OnDisconnect += OnDisconnect;
            mGFXEngines[(int)eEngines.HOST].OnConnect += OnConnect;
            mGFXEnginesConnected[(int)eEngines.HOST] = false;


            mGFXEngines[(int)eEngines.CAGE] = new WGGFXEngine("CAGE");
            mGFXEngines[(int)eEngines.CAGE].OnDisconnect += OnDisconnect;
            mGFXEngines[(int)eEngines.CAGE].OnConnect += OnConnect;
            mGFXEngines[(int)eEngines.CAGE].OnDataIn += OnDataIn;
            mGFXEnginesConnected[(int)eEngines.CAGE] = false;

            mGFXEngines[(int)eEngines.EXTRAQA] = new WGGFXEngine("EXTRAQA");
            mGFXEngines[(int)eEngines.EXTRAQA].OnDisconnect += OnDisconnect;
            mGFXEngines[(int)eEngines.EXTRAQA].OnConnect += OnConnect;
            mGFXEnginesConnected[(int)eEngines.EXTRAQA] = false;

            mGFXEngines[(int)eEngines.MIDI] = new WGGFXEngine("MIDI");
            mGFXEngines[(int)eEngines.MIDI].OnDisconnect += OnDisconnect;
            mGFXEngines[(int)eEngines.MIDI].OnConnect += OnConnect;
            mGFXEnginesConnected[(int)eEngines.MIDI] = false;  

            bmDisConnected = Properties.Resources.Led_Red;
            bmConnected = Properties.Resources.Led_Green;
            bmDisabled = Properties.Resources.Led_Gray;
            bmConnect = Properties.Resources.Led_Yellow;
            bmConnect1 = Properties.Resources.Led_Blue;
        }

        private void ReConnectSpecialEngines()
        {
/*
#if !DEBUG
            if (!mLicenseOk)
                return;
#endif
 */
            foreach (eSpecialEngines eng in mSettingsspecialengineschanged)
            {
                switch (eng)
                {
                    case eSpecialEngines.HOSTMESSAGING:
                        // stop listening...           
                        ipdaemonHostMessaging.Disconnect();
                        //ipdaemonHostMessaging.LocalPort = connectionsettings.mPorts[(int)eEngines.HOSTMESSAGING];

                        if (!ipdaemonHostMessaging.Listen(connectionsettings.mPorts[(int)eEngines.HOSTMESSAGING]))
                        {
                            MessageBox.Show(string.Format("Unable to listen on Port:{0}, connection will not work!", connectionsettings.mPorts[(int)eEngines.HOSTMESSAGING]), "error");                            
                        }
                        /*
                        try
                        {
                            bool check = ipdaemonHostMessaging.Listening = true;
                        }
                        catch (Exception)
                        {
                            MessageBox.Show(
                                string.Format(
                                    "Unable to open socket for Messaging Host system Port:{0}, please try another port!",
                                    ipdaemonHostMessaging.LocalPort));

                        }
                         */
                        break;                    
                }
            }
        }

        private void ReConnectEngines()
        {
/*
#if !DEBUG
            if (!mLicenseOk)
                return;
#endif
 */
            foreach (eEngines eng in mSettingsengineschanged)
            {

                mGFXEngines[(int)eng].Autoconnect = false;
                if (mGFXEnginesConnected[(int)eng])
                {
                    mGFXEngines[(int)eng].Disconnect();
                    mGFXEnginesConnected[(int)eng] = false;
                }
                mGFXEngines[(int)eng].Autoconnect = true;
                ConnectEngine(eng,true);

            }
        }

        private void ConnectEngine(eEngines inEngine, bool connect)
        {
            //if (!mLicenseOk)
            //    return;
            //bool connect = false;
            if (!mGFXEnginesConnected[(int)inEngine]&&connect)
            {
                // cool.. we need to connect
                lEnginePictureBoxes[(int)inEngine].Image = bmConnect;
                lEnginePictureBoxes[(int)inEngine].Refresh();

                if (mGFXEngines[(int)inEngine].Connectors.Count > 0)
                {
                    mGFXEngines[(int)inEngine].Delete(mGFXEnginesConnectors[(int)inEngine]);
                    mGFXEnginesConnectors[(int)inEngine] = null;
                }

                
                if (inEngine == eEngines.HOST)
                {
                    mGFXEnginesConnectors[(int)inEngine] = new WGConnector(this, inEngine.ToString(),
                                                                            connectionsettings.mIpAddresses[
                                                                                (int)inEngine],
                                                                            connectionsettings.mPorts[(int)inEngine],
                                                                            false,
                                                                            eEnginetypes.SYNCODE);
                    mGFXEnginesConnectorHost2 = new WGConnector(this, "HOST2",
                                                                           connectionsettings.mIpAddressesHost2,
                                                                           connectionsettings.mPortHost2,
                                                                           false,
                                                                           eEnginetypes.SYNCODE);
                }
                else if (inEngine == eEngines.MIDI || inEngine == eEngines.WISEQ || inEngine == eEngines.WISEAUDIO || inEngine == eEngines.CAGE)
                {
                    mGFXEnginesConnectors[(int)inEngine] = new WGConnector(this, inEngine.ToString(),
                                                                            connectionsettings.mIpAddresses[
                                                                                (int)inEngine],
                                                                            connectionsettings.mPorts[(int)inEngine],
                                                                            false,
                                                                            eEnginetypes.SYNCODE);
                }
                else
                {
                    mGFXEnginesConnectors[(int)inEngine] = new WGConnector(this, inEngine.ToString(),
                                                                            connectionsettings.mIpAddresses[
                                                                                (int)inEngine],
                                                                            connectionsettings.mPorts[(int)inEngine],
                                                                            true,
                                                                            eEnginetypes.SYNCODE);
                }

                //else
                //{
                //    mGFXEnginesConnectors[(int)inEngine] = new Connector(inEngine.ToString(), mIpAddresses[(int)inEngine], mPorts[(int)inEngine]);
                //}

                if (inEngine == eEngines.WISEQ || inEngine == eEngines.CAGE || inEngine == eEngines.MIDI)
                {
                    mGFXEnginesConnectors[(int)inEngine].Infotypeback = returntypes.RETURN_PLAIN;
                    //mGFXEnginesConnectors[(int)inEngine].Endoflinechars = "\r\n";      // special for wiseQ                    
                }



                //if (inEngine == eEngines.DATABASESERVER)
                //    mGFXEnginesConnectors[(int)inEngine].Endoflinechars = "¶";

                if (inEngine == eEngines.HOST)
                {
                    mGFXEngines[(int)inEngine].Add(mGFXEnginesConnectors[(int)inEngine]);
                    mGFXEngines[(int)inEngine].Add(mGFXEnginesConnectorHost2);
                }
                else 
                    mGFXEngines[(int)inEngine].Add(mGFXEnginesConnectors[(int)inEngine]);


                // now approach.. because of autoconnect... just set to true..
                mGFXEngines[(int)inEngine].Connect();
            }
            else
            {
                mGFXEngines[(int) inEngine].Autoconnect = false;
                mGFXEngines[(int)inEngine].Disconnect();
                lEnginePictureBoxes[(int)inEngine].Image = bmDisConnected;
                mGFXEnginesConnected[(int)inEngine] = false;
            }

        }

        private void OnDisconnect(WGNetWork inEngine)
        {
            if (inEngine.Name == "OVERLAY")
            {
                mGFXEnginesConnected[(int)eEngines.OVERLAY] = false;
                pbOverlay.Image = bmDisConnected;                
            }
            else if (inEngine.Name == "PROJECTOR")
            {
                mGFXEnginesConnected[(int)eEngines.PROJECTOR] = false;
                pbProjector.Image = bmDisConnected;                
            }            
            else if (inEngine.Name == "WISEQ")
            {
                // gfx1
                mGFXEnginesConnected[(int)eEngines.WISEQ] = false;
                pbWiseQ.Image = bmDisConnected;                
            }
            else if (inEngine.Name == "WISEAUDIO")
            {
                // gfx1
                mGFXEnginesConnected[(int)eEngines.WISEAUDIO] = false;
                pbWiseAudio.Image = bmDisConnected;                
            }
            else if (inEngine.Name == "MIDI")
            {
                // midi
                mGFXEnginesConnected[(int)eEngines.MIDI] = false;
                pbMidi.Image = bmDisConnected;                
            }
            else if (inEngine.Name == "HOST")
            {
                if (inEngine.HowManyConnected() == 1)
                {
                    // led blue...
                    lEnginePictureBoxes[(int)eEngines.HOST].Image = Properties.Resources.Led_Blue;
                }
                else if (inEngine.HowManyConnected() == 0)
                {
                    // gfx1
                    mGFXEnginesConnected[(int)eEngines.HOST] = false;
                    pbHost.Image = bmDisConnected;
                    lEnginePictureBoxes[(int)eEngines.HOST].Image = Properties.Resources.Led_Gray;
                    
                }
                
            }
            else if (inEngine.Name == "CAGE")
            {
                // gfx1
                mGFXEnginesConnected[(int)eEngines.CAGE] = false;
                pbCage.Image = bmDisConnected;                
            }
            else if (inEngine.Name == "EXTRAQA")
            {
                // gfx1
                mGFXEnginesConnected[(int)eEngines.EXTRAQA] = false;
                pbExtraQA.Image = bmDisConnected;                
            }
           
        }

        private void OnConnect(WGNetWork inEngine)
        {
            bool isallconnected = inEngine.IsAllConnected();

            switch (inEngine.Name)
            {
                case "OVERLAY":
                    if (inEngine.IsAllConnected())
                    {
                        // led green...
                        lEnginePictureBoxes[(int)eEngines.OVERLAY].Image = Properties.Resources.Led_Green;
                        mGFXEnginesConnected[(int)eEngines.OVERLAY] = true;
                    }
                    if (OnEngineConnect != null)
                    {
                        OnEngineConnect(this, (int)eEngines.OVERLAY);
                    }
                    break;
                case "PROJECTOR":
                    if (inEngine.IsAllConnected())
                    {
                        // led green...
                        lEnginePictureBoxes[(int)eEngines.PROJECTOR].Image = Properties.Resources.Led_Green;
                        mGFXEnginesConnected[(int)eEngines.PROJECTOR] = true;
                        mLogoProjector = false;
                        UpdateDlg();
                    }
                    if (OnEngineConnect != null)
                    {
                        OnEngineConnect(this, (int)eEngines.PROJECTOR);
                    }
                    break;
                case "MIDI":
                    if (inEngine.IsAllConnected())
                    {
                        // led green...
                        lEnginePictureBoxes[(int)eEngines.MIDI].Image = Properties.Resources.Led_Green;
                        mGFXEnginesConnected[(int)eEngines.MIDI] = true;                       
                        UpdateDlg();
                    }
                    if (OnEngineConnect != null)
                    {
                        OnEngineConnect(this, (int)eEngines.MIDI);
                    }
                    break;     
                case "WISEQ":
                    if (inEngine.IsAllConnected())
                    {
                        // led green...
                        lEnginePictureBoxes[(int)eEngines.WISEQ].Image = Properties.Resources.Led_Green;
                        mGFXEnginesConnected[(int)eEngines.WISEQ] = true;
                    }
                    if (OnEngineConnect != null)
                    {
                        OnEngineConnect(this, (int)eEngines.WISEQ);
                    }
                    break;
                case "WISEAUDIO":
                    if (inEngine.IsAllConnected())
                    {
                        // led green...
                        lEnginePictureBoxes[(int)eEngines.WISEAUDIO].Image = Properties.Resources.Led_Green;
                        mGFXEnginesConnected[(int)eEngines.WISEAUDIO] = true;
                    }
                    if (OnEngineConnect != null)
                    {
                        OnEngineConnect(this, (int)eEngines.WISEAUDIO);
                    }
                    break;
                case "HOST":
                    if (inEngine.HowManyConnected() == 1)
                    {
                        // led blue...
                        lEnginePictureBoxes[(int)eEngines.HOST].Image = Properties.Resources.Led_Blue;
                    }
                    else if (inEngine.HowManyConnected() == 2)
                    {
                        // led green...
                        lEnginePictureBoxes[(int)eEngines.HOST].Image = Properties.Resources.Led_Green;
                        mGFXEnginesConnected[(int)eEngines.HOST] = true;
                    }
                    
                    if (OnEngineConnect != null)
                    {
                        OnEngineConnect(this, (int)eEngines.HOST);
                    }
                    break;
                case "CAGE":
                    if (inEngine.IsAllConnected())
                    {
                        // led green...
                        lEnginePictureBoxes[(int)eEngines.CAGE].Image = Properties.Resources.Led_Green;
                        mGFXEnginesConnected[(int)eEngines.CAGE] = true;
                    }
                    if (OnEngineConnect != null)
                    {
                        OnEngineConnect(this, (int)eEngines.CAGE);
                    }
                    break;
                case "EXTRAQA":
                    if (inEngine.IsAllConnected())
                    {
                        // led green...
                        lEnginePictureBoxes[(int)eEngines.EXTRAQA].Image = Properties.Resources.Led_Green;
                        mGFXEnginesConnected[(int)eEngines.EXTRAQA] = true;
                    }
                    if (OnEngineConnect != null)
                    {
                        OnEngineConnect(this, (int)eEngines.EXTRAQA);
                    }
                    break;      
            }
        }

        private void OnDataIn(WGNetWork inEngine, byte[] inData)
        {
            if (inData == null)
                return;
            switch (inEngine.Name)
            {
                case "WISEQ":
                    // ok cool...we get all the information until we find the END¶ sign!
                    char[] chars = new char[inData.Length + 1];
                    System.Text.Decoder d = System.Text.Encoding.UTF8.GetDecoder();
                    int charLen = d.GetChars(inData, 0, inData.Length, chars, 0);

                    string inData2 = new string(chars);
                    inData2 = inData2.Trim(new char[] { '\0' });

#if USEENCRYPT
                    inData2 = Decrypt(inData2);
#endif

                    //string message = Encoding.UTF8.GetString(inData, 0, inData.Length);

                    //string test2 = Encoding.UTF8.GetString(inData);

                    if (inData2.IndexOf("END¶") != -1)
                    {
                        // cool.. the end..
                        ProcessDBString(mReceivedfromQDB);
                        // ready for a new string!
                        mReceivedfromQDB = "";
                    }
                    else
                    {
                        // just add to 1 major big string!
                        mReceivedfromQDB += inData2;
                    }
                    break;
                case "CAGE":
                    
                    // ok cool...we get all the information until we find the END¶ sign!
                    chars = new char[inData.Length + 1];
                    d = System.Text.Encoding.UTF8.GetDecoder();
                    charLen = d.GetChars(inData, 0, inData.Length, chars, 0);

                    inData2 = new string(chars);
                    inData2 = inData2.Trim(new char[] { '\0' });

#if USEENCRYPT
                    inData2 = Decrypt(inData2);
#endif
                    
                    CageReceiveProcess(inData2);
                    break;
            }
        }

        public void OrderCageArticles()
        {
            if (mCurSortItems == eSortItems.LOWTOHIGH)
                litemscurquestionOrder = litemscurquestionUndo.OrderBy(x => x.price).ToList();
            else if (mCurSortItems == eSortItems.HIGHTOLOW)
                litemscurquestionOrder = litemscurquestionUndo.OrderByDescending(x => x.price).ToList();
            else
            {
                // random :( ayoko random :)
                Random rnd1 = new Random(DateTime.Now.Millisecond);
                Random rnd2 = new Random(rnd1.Next(0, 34567));

                bool[] mitemsfound = new bool[litemscurquestionUndo.Count];
                litemscurquestionOrder = new List<datagriditemdata>();
                int id = -1;
                do
                {
                    do
                    {
                        id = rnd2.Next(0, litemscurquestionUndo.Count);
                    } while (mitemsfound[id]);


                    litemscurquestionOrder.Add(litemscurquestionUndo[id]);
                    mitemsfound[id] = true;

                } while (litemscurquestionOrder.Count != litemscurquestionUndo.Count);
            }

            /*
            bool started = false;
            if (!UndoRedoManager.IsCommandStarted)
            {
                UndoRedoManager.StartInvisible("Update Undo Article List");
                started = true;
            }
            litemscurquestionOrderUndo.Clear();
            foreach (datagriditemdata item in litemscurquestionOrder)
            {
                litemscurquestionOrderUndo.Add(item);
            }
            if (started)
            {
                UndoRedoManager.Commit();
            }
             */
            UpdateDatagridarticlecurqquestion();
        }

        private void CageReceiveProcess(string inData2)
        {
            string[] lines = Regex.Split(inData2, "¶");


            if (lines.Count()==2)
            {

                // action
                if (lines[0] == "LISTCOMPLETE")
                {
                    //using (UndoRedoManager.StartInvisible("articlereceived"))
                    //{
                    if (gcRaidTheCage != null)
                    {
                        if (gcRaidTheCage.GameMode == RaidTheCagesModes.STOPRFIDSCANNING)
                        {
                            gcRaidTheCage.OnContinue();
                            ActiveCtrl.UpdateDlg();
                            gcRaidTheCage.UpdateDlg(); // for host
                        }
                    }
                    //    UndoRedoManager.Commit();
                    //}
                }
                else if (lines[0] == "CLEARLISTCURQUESTION")
                {
                    //using (UndoRedoManager.StartInvisible("articlereceived"))
                    //{
                        litemscurquestionUndo.Clear();
                    //    UndoRedoManager.Commit();
                    //}
                    ClearDatagridcurrentQuestion();
                }
            }
            else if (lines.Count() == 3)
            {
                if (lines[0] == "DOORSTATUS")
                {
                    // send msg doorstatus...
                    if (OnDoorStatusMsg != null)
                    {
                        OnDoorStatusMsg(this, lines[1]);
                    }
                }
            }
            else if (lines.Count() == 4)
            {
                // cool.. add to datagrid... product info...
                using (UndoRedoManager.StartInvisible("articlereceived"))
                {
                    litemscurquestionUndo.Add(new datagriditemdata(lines[1], lines[2]));
                    OrderCageArticles();
                    gcRaidTheCage.mAmountThisQuestion.Value = CalculatTotalAmountThisQuestion();
                    gcRaidTheCage.UpdateDlg();
                    UndoRedoManager.Commit();
                }
            }            
        }

        public void UpdateDatagridarticlecurqquestion()
        {
            dataGridViewOutTheCage1.Rows.Clear();

            foreach (datagriditemdata data in litemscurquestionOrder)
            {
                dataGridViewOutTheCage1.Rows.Add(data.article, data.price);    
            }            
            UpdatePriceCageInfo();    
        }

        private void ClearDatagridcurrentQuestion()
        {
            dataGridViewOutTheCage1.Rows.Clear();
            UpdatePriceCageInfo();
        }

        private void UpdatePriceCageInfo()
        {
            lbnrProducts.Text = dataGridViewOutTheCage1.Rows.Count.ToString();

            long totalprice = CalculatTotalAmountThisQuestion();

            lbTotalPrice.Text = totalprice.ToString();
            mScoreThisQuestion = totalprice;
        }

        public long CalculatTotalAmountThisQuestion()
        {
            long totalprice = 0;
            long tempint;

            foreach (DataGridViewRow row in dataGridViewOutTheCage1.Rows)
            {
                long.TryParse(row.Cells[1].Value.ToString(), out tempint);
                totalprice += tempint;
            }
            return totalprice;
        }

        private void ProcessDBString(string inString)
        {
            bool error = false;

            int pos1 = 0;
            int pos2 = 0;
            int pos3 = 0;

            //MainQuestion tmpq = new MainQuestion();

            int count = inString.Split('¶').Length - 1;

            // normal question
                QuestionData tmpq = new QuestionData();
                int round = 0;
                count = inString.Split('¶').Length - 1;

                for (int i = 0; i < count; i++)
                {
                    pos2 = inString.IndexOf('|', pos1);
                    pos3 = inString.IndexOf('¶', pos2);
                    if (pos2 != -1 && pos3 != 0)
                    {
                        // cool... ... get....
                        string command = inString.Substring(pos1, pos2 - pos1);
                        string value = inString.Substring(pos2 + 1, pos3 - pos2 - 1);
                        pos1 = pos2 = pos3 + 1;

                        switch (command)
                        {
                            case "ROUND":
                                int.TryParse(value, out round);
                                //mcurQuestion.Question = value;
                                //tbMainQuestion.Text = value;
                                //Debug.WriteLine(value);
                                break;
                            case "QUESTION":
                                tmpq.mQuestion = value;
                                break;
                            case "QUESTION2":
                                tmpq.mQuestion2 = value;
                                //mcurQuestion.Question = value;
                                //tbMainQuestion.Text = value;
                                //Debug.WriteLine(value);
                                break;
                            case "RIGHTANSWER":
                                tmpq.mRightAnswer = value;
                                break;
                            case "RIGHTANSWER2":
                                tmpq.mRightAnswer2 = value;
                                break;
                            case "REF":
                                tmpq.mRef = value;
                                //mcurQuestion.Question = value;
                                //tbMainQuestion.Text = value;
                                //Debug.WriteLine(value);
                                break;
                            case "QUESTID":
                                uint ID;
                                uint.TryParse(value, out ID);
                                tmpq.mID = ID;
                                break;
                            case "ANSWER1":
                                tmpq.mAnswers[0] = value;
                                break;
                            case "ANSWER2":
                                tmpq.mAnswers[1] = value;
                                break;
                            case "ANSWER3":
                                tmpq.mAnswers[2] = value;
                                break;
                            case "ANSWER4":
                                tmpq.mAnswers[3] = value;
                                break;
                            case "EXPLANATION":
                                string[] lines = Regex.Split(value, "==!==");
                                if (lines.Length<2)
                                    tmpq.mExplanation = value;
                                else
                                {
                                    tmpq.mExplanation = lines[0];
                                    tmpq.mExplanation2 = lines[1];
                                }
                                break;
                            case "PRONOUNCIATION":
                                tmpq.mPronounciation = value;
                                break;
                            case "LEVEL":
                                int level;
                                int.TryParse(value, out level);
                                tmpq.mLevel = level;
                                break;
                            case "CATEGORY":
                                tmpq.mCat = value;
                                break;
                        }
                    }
                    else
                    {
                        error = true;
                        MessageBox.Show("error with receiving question from qdbs!, not all data received");
                    }
                }
            if (!error)
                    {

                        lAllQuestions[(int)MCurStackType.Value][mQnr.Value].mRef = tmpq.mRef;
                        lAllQuestions[(int)MCurStackType.Value][mQnr.Value].mQuestion = tmpq.mQuestion;
                        lAllQuestions[(int)MCurStackType.Value][mQnr.Value].mQuestion2 = tmpq.mQuestion2;
                        lAllQuestions[(int)MCurStackType.Value][mQnr.Value].mAnswers[0] = tmpq.mAnswers[0];
                        lAllQuestions[(int)MCurStackType.Value][mQnr.Value].mAnswers[1] = tmpq.mAnswers[1];
                        lAllQuestions[(int)MCurStackType.Value][mQnr.Value].mAnswers[2] = tmpq.mAnswers[2];
                        lAllQuestions[(int)MCurStackType.Value][mQnr.Value].mAnswers[3] = tmpq.mAnswers[3];

                        lAllQuestions[(int)MCurStackType.Value][mQnr.Value].mRightAnswer = tmpq.mRightAnswer;
                        lAllQuestions[(int)MCurStackType.Value][mQnr.Value].mRightAnswer2 = tmpq.mRightAnswer2;

                        lAllQuestions[(int)MCurStackType.Value][mQnr.Value].mLevel = tmpq.mLevel;
                        lAllQuestions[(int)MCurStackType.Value][mQnr.Value].mCat = tmpq.mCat;

                        lAllQuestions[(int)MCurStackType.Value][mQnr.Value].mExplanation = tmpq.mExplanation;
                        lAllQuestions[(int)MCurStackType.Value][mQnr.Value].mExplanation2 = tmpq.mExplanation2;
                        lAllQuestions[(int)MCurStackType.Value][mQnr.Value].mPronounciation = tmpq.mPronounciation;
                        lAllQuestions[(int)MCurStackType.Value][mQnr.Value].mID = tmpq.mID;

                        if (OnQuestionReceived != null)
                        {
                            OnQuestionReceived(this);
                        }
                    }                
        }

        private void ReadSettings()
        {
            generalsettings.SetParent(this);
            connectionsettings.SetParent(this);
            localisationsettings.SetParent(this);
            MidiSettings.SetParent(this);
            MultiLangualTexts.SetParent(this);          

            
            RaidTheCageData.ReadXmlConfig(ExePath);
        }

       

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // cool.. psw protected....
            bool pswok = false;
#if FALSE   // don't use... yet :)
            
            Encoding theEnc = Encoding.UTF8;

            SettingsPwd pdfdlg = new SettingsPwd();
            if (pdfdlg.ShowDialog() == DialogResult.OK)
            {
                // check if psw correct.. otherwise not show all settings...
                if (CalculateMD5Hash(theEnc.GetBytes(pdfdlg.psw)) == "5BA4189D4F26E078BB36C6E522862E13")
                {
                    // correct
                    pswok = true;
                }
                else
                {
                    // wrong
                    pswok =false;
                }
            }
#else
            pswok = true;
#endif
            mSettingsengineschanged.Clear();
            mSettingsspecialengineschanged.Clear();

            setdlg = new SettingsEditor(this, true);

            genset = new ucGeneralSettings(this, setdlg, 0);
            conset = new ucConnectionSettings(this, setdlg, 1);
            loctestset = new ucLocalisationSettings(this, setdlg, 2);
            midiset = new ucMidiTextSettings(this,setdlg,3);
            gclocalisation = new ucLocalisationTextSettings(this,setdlg,4);

            SettingsTab tab = new SettingsTab();
            tab.AddBase(genset, "General Settings");
            setdlg.AddTreeNode(tab);

            tab = new SettingsTab();
            tab.AddBase(conset, "Connection Settings");
            setdlg.AddTreeNode(tab);

            tab = new SettingsTab();
            tab.AddBase(loctestset, "Localisation Settings");
            setdlg.AddTreeNode(tab);

            tab = new SettingsTab();
            tab.AddBase(midiset, "Midi Settings");
            setdlg.AddTreeNode(tab);

            tab = new SettingsTab();
            tab.AddBase(gclocalisation, "Localisation String Settings");
            setdlg.AddTreeNode(tab);

            

            if (setdlg.ShowDialog() == DialogResult.OK)
            {
                if (mSettingsengineschanged.Count > 0)
                    ReConnectEngines();
                if (mSettingsspecialengineschanged.Count > 0)
                    ReConnectSpecialEngines();
                // check if we have any setting changed....
                // check if we have any setting changed....
                bool changed = false;

                foreach (SettingsTab tab2 in setdlg.lSettingtabs)
                {
                    if (tab2.msetbase.mChanged)
                    {                        
                        changed = true;                        
                    }
                    if ((tab2.msetbase.mNumber == (int)eSettings.LocalisationTextSettings) && tab2.msetbase.mChanged)
                    {
                        // get the continue text again and set the menu again...
                        ActiveCtrl.UpdateDlg();
                        UpdateLanguageMenu();
                        SetCtrlText(mCurLanguage);
                    }
                }
                if (changed)
                {
                    if (OnSettingsChange != null)
                    {
                        OnSettingsChange(this);
                    }
                } 

                /*
                if (theDialog.SettingChanged())
                {
                    if (theDialog.mChanged[(int)ESettingsForms.Localisation])
                    {
                        // get the continue text again and set the menu again...
                        ActiveCtrl.UpdateDlg();
                        UpdateLanguageMenu();
                        SetCtrlText(mCurLanguage);
                    }
                    if (OnSettingsChange != null)
                    {
                        OnSettingsChange(this);
                    }
                }                   
                */
            }
            setdlg.Dispose();  
        }

        internal Size GetSizetcCtrls()
        {
            Size Newsize = new Size();
            Newsize = tcGameCtrl.Size;

            // new method... add 25%            
            Newsize.Width += (45 * Newsize.Width) / 100;

            return Newsize;
        }

        internal Size GetSizetcrounds()
        {
            Size Newsize = new Size();
            Newsize = tcGameParts.Size;

            // new method... add 25%            
            Newsize.Height += (2 * Newsize.Width) / 100;

            return Newsize;
        }

        private void newShowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InitAllGraphics();
            if (gcRaidTheCage != null)
            {
                gcRaidTheCage.CheckMidiNote(RaidTheCagesModes.INIT,"Starting New Show");
                gcRaidTheCage.StartNewShow();

                if (!mLogoProjector)
                    mGFXEngines[(int) eEngines.PROJECTOR].Start("showlogodirect");
                else
                    mGFXEngines[(int)eEngines.PROJECTOR].Start("showlogo3ddirect");

                mGFXEngines[(int) eEngines.PROJECTOR].SetOpacity("logo", 255);
                mGFXEngines[(int)eEngines.PROJECTOR].SetOpacity("logo3d", 255);

                gcRaidTheCage.SendToRfid("DISABLELISTCOMPLETE");
            }
        }

        private void pickUpGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // init all gfx
            InitAllGraphics();
            if (gcRaidTheCage != null)
            {
                gcRaidTheCage.PickUpTheGame();
                gcRaidTheCage.SendToRfid("DISABLELISTCOMPLETE");
            }
        }

        public void InitAllGraphics()
        {
            mGFXEngines[(int) eEngines.OVERLAY].Start("initall");
            mGFXEngines[(int)eEngines.PROJECTOR].Start("initall");

            mGFXEngines[(int) eEngines.PROJECTOR].LoadScreen("mainscreen");
            mGFXEngines[(int)eEngines.PROJECTOR].Start("logoloop.hidelogo");
            if (mCurCountry != eCountries.PARAQUAY)
                mLogoProjector = false;
            UpdateDlg();
        }

        public void GetQfromWiseQ()
        {
            if (mGFXEnginesConnected[(int)eEngines.WISEQ])
            {
                string sendstring;
                
                   sendstring = string.Format("GETQ|{0}|{1}|||", (int)MCurStackType.Value, mQnr.Value);

#if USEENCRYPT
                   sendstring = Encrypt(sendstring);
#endif
                sendstring += "\r\n";
                mGFXEngines[(int)eEngines.WISEQ].SendTextPlain(sendstring);
            }
        }

        public void MakeDBQuestionUsed(uint inID)
        {
            if (mGFXEnginesConnected[(int)eEngines.WISEQ])
            {
                string sendstring = string.Format("USED|{0}|||", inID);
#if USEENCRYPT
                sendstring = Encrypt(sendstring);
#endif
                sendstring += "\r\n";
                mGFXEngines[(int)eEngines.WISEQ].SendTextPlain(sendstring);
            }

        }

       public void UpdateDlg()
       {
           lbQnr.Text = (mQnr.Value + 1).ToString();

           if (mCurCountry == eCountries.HUNGARY || mCurCountry == eCountries.BRAZIL || mCurCountry == eCountries.URUGUAY2018)
           {
               btLogoProjector.Visible = false;
               btLogoProjector.BackColor = mLogoProjectorHongarije ? Color.YellowGreen : SystemColors.Control;
               btLogoProjector.UseVisualStyleBackColor = !mLogoProjectorHongarije;               
           }
           else
           {
               btLogoProjector.BackColor = mLogoProjector ? Color.YellowGreen : SystemColors.Control;
               btLogoProjector.UseVisualStyleBackColor = !mLogoProjector;               
           }
           

       }

       protected override bool ProcessDialogKey(Keys keyData)
       {
           //keystrokeProcessed = true;
           switch (keyData)
           {
               case Keys.Decimal:
                   if (ActiveCtrl != null)
                       ActiveCtrl.OnContinue();
                   return true;
           }
           return base.ProcessDialogKey(keyData);
       }

       public void UpdateQuestionDataCtrl()      // make public for undo.. but check if we are playing a bonus question!
       {
           if (mFirstQuestion.Value)
           {
               tbQuestion.Text = lAllQuestions[(int)MCurStackType.Value][mQnr.Value].mQuestion;
               tbExtraInfo.Text = lAllQuestions[(int)MCurStackType.Value][mQnr.Value].mExplanation;
           }
           else
           {
               tbQuestion.Text = lAllQuestions[(int) MCurStackType.Value][mQnr.Value].mQuestion2;
               tbExtraInfo.Text = lAllQuestions[(int)MCurStackType.Value][mQnr.Value].mExplanation2;
           }
           btMainAnswer1.Text = lAllQuestions[(int)MCurStackType.Value][mQnr.Value].mAnswers[0];
           btMainAnswer2.Text = lAllQuestions[(int)MCurStackType.Value][mQnr.Value].mAnswers[1];
           btMainAnswer3.Text = lAllQuestions[(int)MCurStackType.Value][mQnr.Value].mAnswers[2];
           btMainAnswer4.Text = lAllQuestions[(int)MCurStackType.Value][mQnr.Value].mAnswers[3];
           

           UpdateQuestonDataCtrlColors();
       }

        public void UpdateQuestonDataCtrlColors()
        {
            if (mFirstQuestion.Value)
            {
                btMainAnswer1.BackColor = lAllQuestions[(int)MCurStackType.Value][mQnr.Value].mRightAnswer == "1"
                                              ? Color.YellowGreen
                                              : SystemColors.Control;
                btMainAnswer2.BackColor = lAllQuestions[(int)MCurStackType.Value][mQnr.Value].mRightAnswer == "2"
                                              ? Color.YellowGreen
                                              : SystemColors.Control;
                btMainAnswer3.BackColor = lAllQuestions[(int)MCurStackType.Value][mQnr.Value].mRightAnswer == "3"
                                              ? Color.YellowGreen
                                              : SystemColors.Control;
                btMainAnswer4.BackColor = lAllQuestions[(int)MCurStackType.Value][mQnr.Value].mRightAnswer == "4"
                                              ? Color.YellowGreen
                                              : SystemColors.Control;
            }
            else
            {
                btMainAnswer1.BackColor = lAllQuestions[(int)MCurStackType.Value][mQnr.Value].mRightAnswer2 == "1"
                                              ? Color.YellowGreen
                                              : SystemColors.Control;
                btMainAnswer2.BackColor = lAllQuestions[(int)MCurStackType.Value][mQnr.Value].mRightAnswer2 == "2"
                                              ? Color.YellowGreen
                                              : SystemColors.Control;
                btMainAnswer3.BackColor = lAllQuestions[(int)MCurStackType.Value][mQnr.Value].mRightAnswer2 == "3"
                                              ? Color.YellowGreen
                                              : SystemColors.Control;
                btMainAnswer4.BackColor = lAllQuestions[(int)MCurStackType.Value][mQnr.Value].mRightAnswer2 == "4"
                                              ? Color.YellowGreen
                                              : SystemColors.Control;
            }

            if (mSelectedAnswer.Value == 0)
                btMainAnswer1.BackColor = Color.Yellow;
            else if (mSelectedAnswer.Value == 1)
                btMainAnswer2.BackColor = Color.Yellow;
            if (mSelectedAnswer.Value == 2)
                btMainAnswer3.BackColor = Color.Yellow;
            if (mSelectedAnswer.Value == 3)
                btMainAnswer4.BackColor = Color.Yellow;

            if (mFirstQuestion.Value)
            {
                btMainAnswer1.UseVisualStyleBackColor = lAllQuestions[(int)MCurStackType.Value][mQnr.Value].mRightAnswer != "1" && mSelectedAnswer.Value != 0;
                btMainAnswer2.UseVisualStyleBackColor = lAllQuestions[(int)MCurStackType.Value][mQnr.Value].mRightAnswer != "2" && mSelectedAnswer.Value != 1;
                btMainAnswer3.UseVisualStyleBackColor = lAllQuestions[(int)MCurStackType.Value][mQnr.Value].mRightAnswer != "3" && mSelectedAnswer.Value != 2;
                btMainAnswer4.UseVisualStyleBackColor = lAllQuestions[(int)MCurStackType.Value][mQnr.Value].mRightAnswer != "4" && mSelectedAnswer.Value != 3;
            }
            else
            {
                btMainAnswer1.UseVisualStyleBackColor = lAllQuestions[(int)MCurStackType.Value][mQnr.Value].mRightAnswer2 != "1" && mSelectedAnswer.Value != 0;
                btMainAnswer2.UseVisualStyleBackColor = lAllQuestions[(int)MCurStackType.Value][mQnr.Value].mRightAnswer2 != "2" && mSelectedAnswer.Value != 1;
                btMainAnswer3.UseVisualStyleBackColor = lAllQuestions[(int)MCurStackType.Value][mQnr.Value].mRightAnswer2 != "3" && mSelectedAnswer.Value != 2;
                btMainAnswer4.UseVisualStyleBackColor = lAllQuestions[(int)MCurStackType.Value][mQnr.Value].mRightAnswer2 != "4" && mSelectedAnswer.Value != 3;
            }
        }

        private void WriteXml()
        {
            IProfile m_profile;
            // read the xml file with all defaults.......
            m_profile = new Xml { Name = ExePath + "\\RaidTheCageStartUp.xml" };

            if (mCurLanguage == eLanguage.ENGLISH)
                m_profile.SetValue("MainSettings", "Language", "ENGLISH");
            else if (mCurLanguage == eLanguage.SECONDLANGUAGE)
                m_profile.SetValue("MainSettings", "Language", "SECONDLANGUAGE");
            else if (mCurLanguage == eLanguage.SECONDLANGUAGEENGLISH)
                m_profile.SetValue("MainSettings", "Language", "SECONDLANGUAGEENGLISH");
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            KillAllAudio(false);
            if (OnFormClosingMsg != null)
            {
                OnFormClosingMsg(this);
            }

            if (mLanguageChanged)
                WriteXml();

            ipdaemonHostMessaging.Disconnect();
            ipdaemonHostMessaging.Dispose();

            Thread.Sleep(1000);

            wgmidi.Close();
            wgmidi.Dispose();

            /// stop the audio thread task....
            if (mAudio != null)
                mAudio.StopAudioThread();
            

            //ipdaemonHostMessaging.Shutdown();
            if (mGFXEnginesConnected[(int)eEngines.MIDI])
                mGFXEngines[(int)eEngines.MIDI].Disconnect();
            //ipportMidi.Dispose();
            //ipportMidi.Connected = false;
            //ipportMidi.Linger = true;
            //ipportMidi = null;
            miditmr.Stop();
            
            // close all the ports...
            mGFXEngines[(int)eEngines.OVERLAY].Disconnect();
            mGFXEngines[(int)eEngines.PROJECTOR].Disconnect();            
            mGFXEngines[(int)eEngines.WISEQ].Disconnect();
            mGFXEngines[(int)eEngines.WISEAUDIO].Disconnect();
            mGFXEngines[(int)eEngines.HOST].Disconnect();
            mGFXEngines[(int)eEngines.CAGE].Disconnect();                        
            mGFXEngines[(int)eEngines.PROJECTOR].Disconnect();
            
            
            
        }



        public void SwitchQuestionCtrl()
        {
            if (mFirstQuestion.Value)
            {
                // go to question 2....
                // invisible....
                bool start = false;
                if (!UndoRedoManager.IsCommandStarted)
                {
                    UndoRedoManager.StartInvisible("switch question");
                    start = true;
                }
                if (!mSwitchAnswersWithQuestion)
                    mFirstQuestion.Value = false;
                if (start)
                    UndoRedoManager.Commit();
                
                btSwitchQuestion.Text = "Go To Question 1";
                UpdateQuestionDataCtrl();
            }
            else
            {
                bool start = false;
                if (!UndoRedoManager.IsCommandStarted)
                {
                    UndoRedoManager.StartInvisible("switch question");
                    start = true;
                }

                mFirstQuestion.Value = true;
                if (start)
                    UndoRedoManager.Commit();
                btSwitchQuestion.Text = "Go To Question 2";
                UpdateQuestionDataCtrl();
            }
        }

        private void btSwitchQuestion_Click(object sender, EventArgs e)
        {
            SwitchQuestionCtrl();
        }

        private void btMainAnswer1_Click(object sender, EventArgs e)
        {
            gcRaidTheCage.SelectAnswer(0);
        }

        private void btMainAnswer2_Click(object sender, EventArgs e)
        {
            gcRaidTheCage.SelectAnswer(1);
        }

        private void btMainAnswer3_Click(object sender, EventArgs e)
        {
            gcRaidTheCage.SelectAnswer(2);
        }

        private void btMainAnswer4_Click(object sender, EventArgs e)
        {
            gcRaidTheCage.SelectAnswer(3);
        }

        private void showTotalPriceListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gcRaidTheCage.ShowTotalPriceList();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            frm = Form.ActiveForm;
            //GetScreenshot();
        }

        private void btOrderArticlesRandom_Click(object sender, EventArgs e)
        {
            mCurSortItems = eSortItems.RANDOM;
            OrderCageArticles();   
        }

        private void btOrderArticlesHighToLow_Click(object sender, EventArgs e)
        {
            mCurSortItems = eSortItems.HIGHTOLOW;
            OrderCageArticles();   
        }

        private void btOrderArticlesLowToHigh_Click(object sender, EventArgs e)
        {
            mCurSortItems = eSortItems.LOWTOHIGH;
            OrderCageArticles();   
        }

        private void stopAllAudioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KillAllAudio(false);
        }

        public void KillAllAudio(bool Undo)
        {
            string sendstring2; ;
            DateTime currenttime = DateTime.Now;

            sendstring2 = string.Format("@{0:d4}{1:d2}{2:d2}-{3:d2}{4:d2}{5:d2},CMD,STOPALL", currenttime.Year, currenttime.Month, currenttime.Day, currenttime.Hour, currenttime.Minute, currenttime.Second);
            //else
            //   sendstring.Format(L"@%04d%02d%02d-%02d%02d%02d,CMD,%s|%03d,L",t.wYear,t.wMonth,t.wDay,t.wHour,t.wMinute,t.wSecond,play?L"PLAY":L"STOP",inAudio);
            //sendstring2 += '\r';
            //sendstring2 += '\n';

            //if (mUseWiseGuysAudioPlayer)
            //{
            //Encoding theEnc = Encoding.UTF8;
            // cool... we are going tot add the md5!
            //string sendtext = sendstring2 + CalculateMD5Hash(theEnc.GetBytes(sendstring2 + EncodestringAudioPlayer));
            //sendtext += '\r';
            //sendtext += '\n';
            //mGFXEngines[(int)eEngines.AUDIOBUDDY].SendTextPlain(sendtext);
            //}

            string sendstring = EncDec.Encrypt(string.Format("{0}#{1}#", sendstring2, DateTime.Now.Ticks), mPassword);
            sendstring += '\r';
            sendstring += '\n';

            mGFXEngines[(int)eEngines.WISEAUDIO].SendTextPlain(sendstring);

            //mAudio.KillAllAudio(Undo);

            /*
            string sendstring2; ;
            DateTime currenttime = DateTime.Now;

            sendstring2 = string.Format("@{0:d4}{1:d2}{2:d2}-{3:d2}{4:d2}{5:d2},CMD,STOPALL", currenttime.Year, currenttime.Month, currenttime.Day, currenttime.Hour, currenttime.Minute, currenttime.Second);
            

            sendstring2 += '\r';
            sendstring2 += '\n';
            mGFXEngines[(int)eEngines.WISEAUDIO].SendTextPlain(sendstring2);            
             */
        }

        private void btLogoProjector_Click(object sender, EventArgs e)
        {
            if (mCurCountry == eCountries.HUNGARY || mCurCountry == eCountries.BRAZIL || mCurCountry == eCountries.URUGUAY2018)
            {
                if (!mLogoProjectorHongarije)
                {
                    mGFXEngines[(int) eEngines.PROJECTOR].Start("showlogohongarije");
                    mLogoProjectorHongarije = true;
                }
                else
                {
                    mGFXEngines[(int)eEngines.PROJECTOR].Start("hidelogohongarije");
                    mLogoProjectorHongarije = false;
                }
            }
            else if (mCurCountry == eCountries.PARAQUAY)
            {
                if (!mLogoProjector)
                {
                    // switch to 3D
                    mGFXEngines[(int) eEngines.PROJECTOR].Start("showlogo3d");
                    mLogoProjector = true;
                }
                else
                {
                    // switch to 2D
                    mGFXEngines[(int)eEngines.PROJECTOR].Start("showlogo");
                    mLogoProjector = false;
                }
                UpdateDlg();
            }
            else
            {
                if (!mLogoProjector)
                {
                    // logo on projector
                    // start animation..
                    mGFXEngines[(int) eEngines.PROJECTOR].Start("logoloop.showlogo");
                    mGFXEngines[(int) eEngines.PROJECTOR].LoadScreen("logoscreen");

                    mLogoProjector = true;

                }
                else
                {
                    mGFXEngines[(int) eEngines.PROJECTOR].LoadScreen("mainscreen");
                    mGFXEngines[(int) eEngines.PROJECTOR].Start("logoloop.hidelogo");

                    mLogoProjector = false;
                }
            }
            UpdateDlg();
        }

        private void pbWiseAudio_Click(object sender, EventArgs e)
        {
            if (mGFXEnginesConnected[(int)eEngines.WISEAUDIO])
            {
                // disconnect... not autoconnect anymore...
                mGFXEngines[(int)eEngines.WISEAUDIO].Autoconnect = false;
                mGFXEngines[(int)eEngines.WISEAUDIO].Disconnect();
                pbWiseAudio.Image = bmDisConnected;
                mGFXEnginesConnected[(int)eEngines.WISEAUDIO] = false;
            }
            else
            {
                mGFXEngines[(int)eEngines.WISEAUDIO].Autoconnect = true;
                mGFXEngines[(int)eEngines.WISEAUDIO].Connect();
            }  
        }

        private void SetCtrlText(eLanguage eLanguage)
        {
            switch (eLanguage)
            {
                case eLanguage.ENGLISH:
                case eLanguage.SECONDLANGUAGEENGLISH:
                    lbquestionnrtext.Text = MultiLangualTexts.EnglishTexts[0];
                    lbquestiontext.Text = MultiLangualTexts.EnglishTexts[1];
                    lbproductsfromthecage.Text = MultiLangualTexts.EnglishTexts[2];
                    btOrderArticlesRandom.Text = MultiLangualTexts.EnglishTexts[3];
                    btOrderArticlesHighToLow.Text = MultiLangualTexts.EnglishTexts[4];
                    btOrderArticlesLowToHigh.Text = MultiLangualTexts.EnglishTexts[5];
                    btLogoProjector.Text = MultiLangualTexts.EnglishTexts[6];
                    lbExtraInfo.Text = MultiLangualTexts.EnglishTexts[7];
                    break;
                case eLanguage.SECONDLANGUAGE:
                    lbquestionnrtext.Text = MultiLangualTexts.SecondLanguageTexts[0];
                    lbquestiontext.Text = MultiLangualTexts.SecondLanguageTexts[1];
                    lbproductsfromthecage.Text = MultiLangualTexts.SecondLanguageTexts[2];
                    btOrderArticlesRandom.Text = MultiLangualTexts.SecondLanguageTexts[3];
                    btOrderArticlesHighToLow.Text = MultiLangualTexts.SecondLanguageTexts[4];
                    btOrderArticlesLowToHigh.Text = MultiLangualTexts.SecondLanguageTexts[5];
                    btLogoProjector.Text = MultiLangualTexts.SecondLanguageTexts[6];
                    lbExtraInfo.Text = MultiLangualTexts.SecondLanguageTexts[7]; 
                    break;
            }
            if (OnLanguageChangeMsg != null)
            {
                OnLanguageChangeMsg(this, eLanguage);
            }

        }

        private void eNglishToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mCurLanguage = eLanguage.ENGLISH;
            eNglishToolStripMenuItem.Checked = true;
            secondLanguageToolStripMenuItem.Checked = false;
            secondLanguageEnglishToolStripMenuItem.Checked = false;
            ActiveCtrl.UpdateDlg();
            mLanguageChanged = true;
            SetCtrlText(eLanguage.ENGLISH);
        }

        private void secondLanguageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mCurLanguage = eLanguage.SECONDLANGUAGE;
            eNglishToolStripMenuItem.Checked = false;
            secondLanguageToolStripMenuItem.Checked = true;
            secondLanguageEnglishToolStripMenuItem.Checked = false;
            ActiveCtrl.UpdateDlg();
            mLanguageChanged = true;
            SetCtrlText(eLanguage.SECONDLANGUAGE);
        }

        private void secondLanguageEnglishToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mCurLanguage = eLanguage.SECONDLANGUAGEENGLISH;
            eNglishToolStripMenuItem.Checked = false;
            secondLanguageToolStripMenuItem.Checked = false;
            secondLanguageEnglishToolStripMenuItem.Checked = true;
            ActiveCtrl.UpdateDlg();
            mLanguageChanged = true;
            SetCtrlText(eLanguage.SECONDLANGUAGEENGLISH);
        }

        internal DataGridView GetDataGridview()
        {
            return dataGridViewOutTheCage1;
        }

        internal void FillInQuestionNr(int p)
        {
            lAllQuestions[(int)MCurStackType.Value][mQnr.Value].mRef = string.Format("Ref:{0}", p+1);
            lAllQuestions[(int)MCurStackType.Value][mQnr.Value].mQuestion = string.Format("Question {0}", p + 1); 
            lAllQuestions[(int)MCurStackType.Value][mQnr.Value].mQuestion2 = string.Format("Question2 {0}", p + 1); 
            lAllQuestions[(int)MCurStackType.Value][mQnr.Value].mAnswers[0] = string.Format("Q{0} Answer1", p + 1);
            lAllQuestions[(int)MCurStackType.Value][mQnr.Value].mAnswers[1] = string.Format("Q{0} Answer2", p + 1);
            lAllQuestions[(int)MCurStackType.Value][mQnr.Value].mAnswers[2] = string.Format("Q{0} Answer3", p + 1);
            lAllQuestions[(int)MCurStackType.Value][mQnr.Value].mAnswers[3] = string.Format("Q{0} Answer4", p + 1);

            if (p==9)
            {
                lAllQuestions[(int)MCurStackType.Value][mQnr.Value].mRightAnswer = "1";
                lAllQuestions[(int)MCurStackType.Value][mQnr.Value].mRightAnswer2 = "2";
            }
            else
            {                            
                lAllQuestions[(int) MCurStackType.Value][mQnr.Value].mRightAnswer = (p%4+1).ToString();
                lAllQuestions[(int)MCurStackType.Value][mQnr.Value].mRightAnswer2 = (p%4+1).ToString();
            }

            lAllQuestions[(int)MCurStackType.Value][mQnr.Value].mLevel = 1;
            lAllQuestions[(int)MCurStackType.Value][mQnr.Value].mCat = "Test question cat";

            lAllQuestions[(int)MCurStackType.Value][mQnr.Value].mExplanation = "Test question explanation 1";
            lAllQuestions[(int)MCurStackType.Value][mQnr.Value].mExplanation2 = "Test question explanation 2";
            lAllQuestions[(int)MCurStackType.Value][mQnr.Value].mPronounciation = "Test question Pronounciacion 1";
            lAllQuestions[(int)MCurStackType.Value][mQnr.Value].mID = 0;

            if (OnQuestionReceived != null)
            {
                OnQuestionReceived(this);
            }
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void showMidiDebugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            wgmidi.Show();
            //GetScreenshot();
        }

        private void pauzeAudioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gcRaidTheCage != null)
            {
                gcRaidTheCage.PauzeSounds();
                pauzeAudioToolStripMenuItem.Enabled = false;
                resumeAudioToolStripMenuItem.Enabled = true;
            }
        }

        private void resumeAudioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gcRaidTheCage != null)
            {
                gcRaidTheCage.ResumeSounds();
                pauzeAudioToolStripMenuItem.Enabled = true;
                resumeAudioToolStripMenuItem.Enabled = false;
            }
        }

        private void pbMidi_Click(object sender, EventArgs e)
        {
            if (mGFXEnginesConnected[(int)eEngines.MIDI])
            {
                // disconnect... not autoconnect anymore...
                mGFXEngines[(int)eEngines.MIDI].Autoconnect = false;
                mGFXEngines[(int)eEngines.MIDI].Disconnect();
                pbMidi.Image = bmDisConnected;
                mGFXEnginesConnected[(int)eEngines.MIDI] = false;
            }
            else
            {
                mGFXEngines[(int)eEngines.MIDI].Autoconnect = true;
                mGFXEngines[(int)eEngines.MIDI].Connect();
            }  
        }

        private void pbCage_Click(object sender, EventArgs e)
        {
            if (mGFXEnginesConnected[(int)eEngines.CAGE])
            {
                // disconnect... not autoconnect anymore...
                mGFXEngines[(int)eEngines.CAGE].Autoconnect = false;
                mGFXEngines[(int)eEngines.CAGE].Disconnect();
                pbCage.Image = bmDisConnected;
                mGFXEnginesConnected[(int)eEngines.CAGE] = false;
            }
            else
            {
                mGFXEngines[(int)eEngines.CAGE].Autoconnect = true;
                mGFXEngines[(int)eEngines.CAGE].Connect();
            }  
        }

        public void SendScreen()
        {
            //var bm = GetScreenshot();
            //var bmBytes = GetBitmapBytes(bm, ImageFormat.Png);
            //var sendBuffer = server.CreateBuffer();
            //sendBuffer.Write(bmBytes);
            //server.SendToAll(sendBuffer, NetChannel.ReliableInOrder1);
        }

        public void GetScreenshot()
        {
            var frm = Form.ActiveForm;
            using (var bmp = new Bitmap(frm.Width, frm.Height))
            {
                frm.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
                bmp.Save(@"d:\test.png");
            }

            /*
                 var bitmap = new Bitmap(frm.Width, frm.Height);

                 using (var bmp = new Bitmap(frm.Width, frm.Height))
                 {
                     frm.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
                 }            

                 bitmap.Save(@"d:\test.jpg", ImageFormat.Jpeg);
             */
                /*
            var bounds = Screen.GetBounds(Point.Empty);

            //0,0,bmp.Width,bmp.Height

            var bitmap = new Bitmap(bounds.Width, bounds.Height);

            using (var g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
            }
                 */
            //return bitmap;
        }       

        static public byte[] GetBitmapBytes(Bitmap bitmap, ImageFormat imageFormat)
        {
            var streamScreenshot = new MemoryStream();
            bitmap.Save(streamScreenshot, imageFormat);
            return streamScreenshot.ToArray();
        }
        static public Bitmap GetBitmapFromBytes(byte[] buffer)
        {
            var streamScreenshot = new MemoryStream(buffer);
            return new Bitmap(streamScreenshot);
        }

        private void pbWiseQ_Click(object sender, EventArgs e)
        {
            if (mGFXEnginesConnected[(int)eEngines.WISEQ])
            {
                // disconnect... not autoconnect anymore...
                mGFXEngines[(int)eEngines.WISEQ].Autoconnect = false;
                mGFXEngines[(int)eEngines.WISEQ].Disconnect();
                pbWiseQ.Image = bmDisConnected;
                mGFXEnginesConnected[(int)eEngines.WISEQ] = false;
            }
            else
            {
                mGFXEngines[(int)eEngines.WISEQ].Autoconnect = true;
                mGFXEngines[(int)eEngines.WISEQ].Connect();
            }  
        }

        private void pbOverlay_Click(object sender, EventArgs e)
        {
            if (mGFXEnginesConnected[(int)eEngines.OVERLAY])
            {
                // disconnect... not autoconnect anymore...
                mGFXEngines[(int)eEngines.OVERLAY].Autoconnect = false;
                mGFXEngines[(int)eEngines.OVERLAY].Disconnect();
                pbOverlay.Image = bmDisConnected;
                mGFXEnginesConnected[(int)eEngines.OVERLAY] = false;
            }
            else
            {
                mGFXEngines[(int)eEngines.OVERLAY].Autoconnect = true;
                mGFXEngines[(int)eEngines.OVERLAY].Connect();
            }  
        }

        private void pbProjector_Click(object sender, EventArgs e)
        {
            if (mGFXEnginesConnected[(int)eEngines.OVERLAY])
            {
                // disconnect... not autoconnect anymore...
                mGFXEngines[(int)eEngines.PROJECTOR].Autoconnect = false;
                mGFXEngines[(int)eEngines.PROJECTOR].Disconnect();
                pbProjector.Image = bmDisConnected;
                mGFXEnginesConnected[(int)eEngines.PROJECTOR] = false;
            }
            else
            {
                mGFXEngines[(int)eEngines.PROJECTOR].Autoconnect = true;
                mGFXEngines[(int)eEngines.PROJECTOR].Connect();
            }  
        }

        private void pbHostMessaging_Click(object sender, EventArgs e)
        {

        }

        private void pbExtraQA_Click(object sender, EventArgs e)
        {
            if (mGFXEnginesConnected[(int)eEngines.EXTRAQA])
            {
                // disconnect... not autoconnect anymore...
                mGFXEngines[(int)eEngines.EXTRAQA].Autoconnect = false;
                mGFXEngines[(int)eEngines.EXTRAQA].Disconnect();
                pbExtraQA.Image = bmDisConnected;
                mGFXEnginesConnected[(int)eEngines.EXTRAQA] = false;
            }
            else
            {
                mGFXEngines[(int)eEngines.EXTRAQA].Autoconnect = true;
                mGFXEngines[(int)eEngines.EXTRAQA].Connect();
            }  
        }

        private void btMidiNote5_Click(object sender, EventArgs e)
        {
            if (gcRaidTheCage != null)
                gcRaidTheCage.CheckExtraMidiNote("Answer Wrong");
        }

               
    }

    public class datagriditemdata
    {
        public string article;
        public int price;

        public datagriditemdata(string inarticle, string inPrice)
        {
            article = inarticle;
            int.TryParse(inPrice, out price);

        }
    }

    


}
