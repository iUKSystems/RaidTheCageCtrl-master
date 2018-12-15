#define USEENCRYPT

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DejaVu;
using RaidTheCageCtrl.Settings;
using WiseGuys.Settings;
using WiseGuys.WGNetWork2011;
using WiseGuys.configs;
using WiseGuysFrameWork;
using WiseGuysFrameWork.Audio;
using WiseGuysFrameWork2015;
using Logfile = LogFile.Logfile;

public enum RaidTheCagesModes : int
{
    INIT = 0,
    //SHOWHOSTNAME,
    //HIDEHOSTNAME,
    STARTINTROMUSIC,        // special for Portugal
    SHOWCONTESTANTSNAMES,
    HIDECONTESTANTSNAMES,
    GOTOEXPLAINTHEGAMES,
    //EXPLAIN3SEC,
    GETQUESTIONFROMWISEQ,
    GETQUESTIONFROMWISEQSWITCH,
    SHOWQUESTION,
    SHOWANSWER1,
    SHOWANSWER2,
    SHOWANSWER3,
    SHOWANSWER4,
    ENDSWITCHPLAYER,
    SWITCHQUESTION,
    WAITFORANSWER,
    LETSSEEIFYOUARERIGHTMUSIC,
    SHOWRIGHTANSWERBEFORESWITCH,
    SHOWRIGHTANSWER,
    REMOVEQUESTION,
    REMOVEQUESTIONFINALANSWER,
    REMOVEQUESTIONFINALANSWER2,
    ANSWERWRONG,
    SHOWCAGETIME,
    STARTCAGETIME,
    CAGETIMERUNNING,
    CAGETIMERUNNINGLAST3SEC,
    OUTOFCAGETIME,          // also stop rfid scan... contestant is too late...
    STOPRFIDSCANNING,       // if the cage time is stopped before 0... 
    HIDECLOCK,       // if the cage time is stopped before 0... 
    
    SHOWARTICLELIST,
    SHOWINGARTICLES,
    SHOWARTICLE,
    HIDEARTICLELIST,
    SHOWTOTALAMOUNTTHISQUESTION,
    SHOWTOTALAMOUNT,
    HIDETOTALAMOUNT1,
    HIDETOTALAMOUNT,
    PLAYORSTOP,
    SHOWMONEYTREE,
    HIDEMONEYTREE,
    NEXTQUESTION,
    DONECLEARSCREENS,
    DONE,
    MODES,
}

namespace RaidTheCageCtrl.GameParts
{
    public partial class ucRaidTheCage : ucBaseGame
    {
        //private MainForm mParent;

        private bool doorisclosing = false;
        private bool doorisclosing2 = false;

        private bool mIsUpdating = false;

        private bool startpolling = false;

        private Timer m3secexplain1tmr = new Timer();
        private Timer m3secexplain2tmr = new Timer();

        private Timer m3secexplainbacktmr = new Timer();

        private TotalPrices total;
        public bool totalpricesshowing = false;

        private Timer mAnswerSwitchTimer = new Timer();
        private Timer mStartTimeAfterDoorOpensTimer = new Timer();

        private DataGridView dgv = new DataGridView();

        private string timestring;
        private string timestringctrl;

        private int casetimervalue;
        private DateTime mstarttime;
        private DateTime mcurtime;

        public WGGFXEngine overlay = new WGGFXEngine("OVERLAY");
        public WGGFXEngine projector = new WGGFXEngine("PROJECTOR");
        private WGGFXEngine host = new WGGFXEngine("HOST");
        private WGGFXEngine midi = new WGGFXEngine("MIDI");

        private System.Windows.Forms.Timer continuetimer = new System.Windows.Forms.Timer();
        private System.Windows.Forms.Timer cagetimer = new System.Windows.Forms.Timer();
        private System.Windows.Forms.Timer wiseqretreive = new System.Windows.Forms.Timer();

        private int[] mContinuedelays = new int[(int)RaidTheCagesModes.MODES+1];

        private RaidTheCagesModes mLastGameMode;
        private RaidTheCagesModes mLastGameModeTemp;

        private Logfile mLogFile;

        public bool mLogingOn = false;
        public string mLoggingFileName;

        //////////////////////////////////////////////////////////////
        // for undo... ///////////////////////////////////////////////
        //////////////////////////////////////////////////////////////
        private UndoRedo<RaidTheCagesModes> mGameMode = new UndoRedo<RaidTheCagesModes>(RaidTheCagesModes.INIT);
        private UndoRedo<int> mcurrentarticlelist = new UndoRedo<int>(0);
        private UndoRedo<int> mQuestionGotFromWiseQ = new UndoRedo<int>(0);
        private UndoRedo<bool> mLifeLineAvailable1 = new UndoRedo<bool>(true);
        private UndoRedo<bool> mLifeLineAvailable2 = new UndoRedo<bool>(true);
        public List<UndoRedo<bool>> lLifelinesAvailable = new List<UndoRedo<bool>>();

        public UndoRedo<long> mAmountThisQuestion = new UndoRedo<long>(0);
        private UndoRedo<long> mAmountTotal = new UndoRedo<long>(0);

        private UndoRedo<bool> mShowLifelinereminderstrap = new UndoRedo<bool>(false);

        public UndoRedo<bool> mTotalAmountShown = new UndoRedo<bool>(false); 

        public UndoRedo<bool> mUsedSwitchPlayer  = new UndoRedo<bool>(false);
        public UndoRedo<bool> mUsedSwitchQuestion = new UndoRedo<bool>(false);

        private MainForm mParent;

        public ucRaidTheCage(MainForm pParent)
        {
            //MessageBox.Show("InitializeComponent");
            InitializeComponent();

            mParent = pParent;

            mLogFile = new Logfile(pParent);

            //MessageBox.Show("STARTPointers");
            mParent.OnQuestionReceived += OnQuestionReceived;
            mParent.OnFormClosingMsg += OnFormClosingMsg;
            mParent.OnEngineConnect += OnEngineConnect;
            mParent.OnSettingsChange += OnSettingsChange;
            mParent.OnDoorStatusMsg += OnDoorStatusMsg;
            mParent.OnLanguageChangeMsg += OnLanguageChangeMsg;

            lLifelinesAvailable.Add(mLifeLineAvailable1);
            lLifelinesAvailable.Add(mLifeLineAvailable2);

            //MessageBox.Show("EngineShortcuts");
            overlay = mParent.mGFXEngines[(int) eEngines.OVERLAY];
            projector = mParent.mGFXEngines[(int)eEngines.PROJECTOR];
            host = mParent.mGFXEngines[(int)eEngines.HOST];
            midi = mParent.mGFXEngines[(int)eEngines.MIDI];

            continuetimer.Interval = 200; // 200 ms before enabling button..
            continuetimer.Tick += new EventHandler(continuetimer_Tick);

            cagetimer.Interval = 100;
            cagetimer.Tick += new EventHandler(cagetimer_Tick);

            wiseqretreive.Interval = 2000; // 2 seconds 
            wiseqretreive.Tick += new EventHandler(wiseqretreive_Tick);

            mAnswerSwitchTimer.Interval = 1000;
            mAnswerSwitchTimer.Tick += new EventHandler(mAnswerSwitchTimer_Tick);

            m3secexplainbacktmr.Interval = 3500;
            m3secexplainbacktmr.Tick += m3secexplaintmr_Tick;

            m3secexplain1tmr.Tick += m3secexplain1tmr_Tick;
            m3secexplain2tmr.Tick += m3secexplain2tmr_Tick;

            mStartTimeAfterDoorOpensTimer.Interval = generalsettings.mTimebeforettimerstart == 0
                                                         ? 1
                                                         : generalsettings.mTimebeforettimerstart;
            mStartTimeAfterDoorOpensTimer.Tick += new EventHandler(mStartTimeAfterDoorOpensTimer_Tick);

            //MessageBox.Show("CreateContinueDelays");
            ContinueDelays();
            //MessageBox.Show("UpdateDlg");
            UpdateDlg();
            //MessageBox.Show("Createnewtotalwindow");
            total = new TotalPrices(this);
            //MessageBox.Show("Resizectrl");
            this.Size = mParent.GetSizetcrounds();
            //MessageBox.Show("Done");
        }

        void m3secexplain2tmr_Tick(object sender, EventArgs e)
        {
            m3secexplain2tmr.Stop();
            SendToRfid("CLOSECAGEDOOR");
        }

        void m3secexplain1tmr_Tick(object sender, EventArgs e)
        {
            m3secexplain1tmr.Stop();
            mParent.mAudio.SendAudio(SoundsRaidTheCage.DOORS3SEC, SoundCommands.PLAY, false, false);
            m3secexplainbacktmr.Start();
            CheckExtraMidiNote("Closing Door in 3 Seconds");
            m3secexplainbacktmr.Start();
        }

        void m3secexplaintmr_Tick(object sender, EventArgs e)
        {
             m3secexplainbacktmr.Stop();
            Midi.SendMidi(midi, "Midi Reset", MidiSettings.mMidiNotes[0]);
        }

        void mStartTimeAfterDoorOpensTimer_Tick(object sender, EventArgs e)
        {
            mStartTimeAfterDoorOpensTimer.Stop();
            mstarttime = DateTime.Now;
            cagetimer.Start();

        }

        public override ButtonStates GetButtonSate()
        {
            //uint returnbuttonstate = 0;

            ButtonStates[] buttonstategame;
            buttonstategame = new ButtonStates[(int)RaidTheCagesModes.MODES];

            buttonstategame[(int)RaidTheCagesModes.INIT] = ButtonStates.NONE;


            buttonstategame[(int)RaidTheCagesModes.STARTINTROMUSIC] = ButtonStates.MAYCONTINUE;
            //buttonstategame[(int)RaidTheCagesModes.SHOWHOSTNAME] = ButtonStates.MAYCONTINUE;
            //buttonstategame[(int)RaidTheCagesModes.HIDEHOSTNAME] = ButtonStates.MAYCONTINUE;
            buttonstategame[(int)RaidTheCagesModes.SHOWCONTESTANTSNAMES] = ButtonStates.MAYCONTINUE;
            buttonstategame[(int)RaidTheCagesModes.HIDECONTESTANTSNAMES] = ButtonStates.MAYCONTINUE;
           
            buttonstategame[(int)RaidTheCagesModes.GOTOEXPLAINTHEGAMES] = ButtonStates.MAYCONTINUE;
            //buttonstategame[(int)RaidTheCagesModes.EXPLAIN3SEC] = ButtonStates.MAYCONTINUE;
            
            buttonstategame[(int)RaidTheCagesModes.GETQUESTIONFROMWISEQ] = ButtonStates.MAYCONTINUE;
            buttonstategame[(int)RaidTheCagesModes.GETQUESTIONFROMWISEQSWITCH] = ButtonStates.MAYCONTINUE;
            buttonstategame[(int)RaidTheCagesModes.SHOWQUESTION] = ButtonStates.MAYCONTINUE;
            buttonstategame[(int)RaidTheCagesModes.SHOWANSWER1] = ButtonStates.MAYCONTINUE;
            buttonstategame[(int)RaidTheCagesModes.SHOWANSWER2] = ButtonStates.MAYCONTINUE;
            buttonstategame[(int)RaidTheCagesModes.SHOWANSWER3] = ButtonStates.MAYCONTINUE;
            buttonstategame[(int)RaidTheCagesModes.SHOWANSWER4] = ButtonStates.MAYCONTINUE;
            buttonstategame[(int)RaidTheCagesModes.WAITFORANSWER] = ButtonStates.NONE;
            buttonstategame[(int)RaidTheCagesModes.ENDSWITCHPLAYER] = ButtonStates.MAYCONTINUE;
            buttonstategame[(int)RaidTheCagesModes.SWITCHQUESTION] = ButtonStates.MAYCONTINUE;
            buttonstategame[(int)RaidTheCagesModes.LETSSEEIFYOUARERIGHTMUSIC] = ButtonStates.MAYCONTINUE;
            buttonstategame[(int)RaidTheCagesModes.SHOWRIGHTANSWERBEFORESWITCH] = ButtonStates.MAYCONTINUE;
            buttonstategame[(int)RaidTheCagesModes.SHOWRIGHTANSWER] = ButtonStates.MAYCONTINUE;
            buttonstategame[(int)RaidTheCagesModes.REMOVEQUESTION] = ButtonStates.MAYCONTINUE;
            buttonstategame[(int)RaidTheCagesModes.REMOVEQUESTIONFINALANSWER] = ButtonStates.MAYCONTINUE;
            buttonstategame[(int)RaidTheCagesModes.REMOVEQUESTIONFINALANSWER2] = ButtonStates.MAYCONTINUE;            
            buttonstategame[(int)RaidTheCagesModes.SHOWCAGETIME] = ButtonStates.MAYCONTINUE;
            buttonstategame[(int)RaidTheCagesModes.STARTCAGETIME] = ButtonStates.MAYCONTINUE;
            buttonstategame[(int)RaidTheCagesModes.CAGETIMERUNNING] = ButtonStates.MAYCONTINUE;
            buttonstategame[(int)RaidTheCagesModes.CAGETIMERUNNINGLAST3SEC] = ButtonStates.NONE;            // last 3 seconds.. door is closing allready..
            buttonstategame[(int)RaidTheCagesModes.OUTOFCAGETIME] = ButtonStates.MAYCONTINUE;
            buttonstategame[(int)RaidTheCagesModes.HIDECLOCK] = ButtonStates.MAYCONTINUE | ButtonStates.MAYFALSE;     // may false is player out of the game!;
            buttonstategame[(int) RaidTheCagesModes.STOPRFIDSCANNING] = ButtonStates.MAYCONTINUE;

            buttonstategame[(int)RaidTheCagesModes.ANSWERWRONG] = ButtonStates.MAYCONTINUE;

            buttonstategame[(int)RaidTheCagesModes.SHOWARTICLELIST] = ButtonStates.MAYCONTINUE;
            buttonstategame[(int)RaidTheCagesModes.SHOWINGARTICLES] = ButtonStates.NONE;            
            buttonstategame[(int)RaidTheCagesModes.SHOWARTICLE] = ButtonStates.MAYCONTINUE;
            buttonstategame[(int)RaidTheCagesModes.HIDEARTICLELIST] = ButtonStates.MAYCONTINUE;
            buttonstategame[(int)RaidTheCagesModes.SHOWTOTALAMOUNTTHISQUESTION] = ButtonStates.MAYCONTINUE;
            buttonstategame[(int)RaidTheCagesModes.SHOWTOTALAMOUNT] = ButtonStates.MAYCONTINUE;
            buttonstategame[(int)RaidTheCagesModes.HIDETOTALAMOUNT1] = ButtonStates.MAYCONTINUE;
            buttonstategame[(int)RaidTheCagesModes.HIDETOTALAMOUNT] = ButtonStates.MAYCONTINUE;
            buttonstategame[(int)RaidTheCagesModes.PLAYORSTOP] = ButtonStates.MAYCONTINUE;
            buttonstategame[(int)RaidTheCagesModes.SHOWMONEYTREE] = ButtonStates.MAYCONTINUE;
            buttonstategame[(int)RaidTheCagesModes.HIDEMONEYTREE] = ButtonStates.MAYCONTINUE;
            buttonstategame[(int)RaidTheCagesModes.NEXTQUESTION] = ButtonStates.MAYCONTINUE;

            buttonstategame[(int)RaidTheCagesModes.DONECLEARSCREENS] = ButtonStates.MAYCONTINUE;
            buttonstategame[(int)RaidTheCagesModes.DONE] = ButtonStates.NONE;


            return (buttonstategame[(int)GameMode]);
        }

        public string GetContinueTextEnglishWithGameMode(RaidTheCagesModes inMode)
        {
            return MultiLangualTexts.GetContinueTextEnglish(inMode);
        }

        public override string GetContinueTextEnglish()
        {
            return MultiLangualTexts.GetContinueTextEnglish(GameMode);
        }

        public override string GetContinueTextSecondLanguage()
        {
            return MultiLangualTexts.GetContinueTextSecondLanguage(GameMode);
        }

        private string GetContinueTextMultipleLanguage(eLanguage inLanguage)
        {
            string sendstring = "";
            if (inLanguage == eLanguage.SECONDLANGUAGEENGLISH || inLanguage == eLanguage.SECONDLANGUAGE)
            {
                sendstring = MultiLangualTexts.GetContinueTextSecondLanguage(GameMode);
            }
            if (inLanguage == eLanguage.ENGLISH || inLanguage == eLanguage.SECONDLANGUAGEENGLISH)
            {
                if (inLanguage == eLanguage.SECONDLANGUAGEENGLISH)
                    sendstring += "\r\n";

                sendstring += MultiLangualTexts.GetContinueTextEnglish(GameMode);
            }
            return sendstring;
        }

        public override string GetContinueText()
        {
            return GetContinueTextMultipleLanguage(mParent.mCurLanguage);
        }

        void mAnswerSwitchTimer_Tick(object sender, EventArgs e)
        {
            mAnswerSwitchTimer.Stop();

            projector.SetText("question.answer1", mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[0] + "¬" + mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[1] + "¬" + mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[2] + "¬" + mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[3]);
            projector.SetText("question.answer2", mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[0] + "¬" + mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[1] + "¬" + mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[2] + "¬" + mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[3]);
            projector.SetText("question.answer3", mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[0] + "¬" + mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[1] + "¬" + mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[2] + "¬" + mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[3]);
            projector.SetText("question.answer4", mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[0] + "¬" + mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[1] + "¬" + mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[2] + "¬" + mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[3]);
            
            overlay.SetText("question.answer1", mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[0] + "¬" + mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[1] + "¬" + mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[2] + "¬" + mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[3]);
            overlay.SetText("question.answer2", mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[0] + "¬" + mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[1] + "¬" + mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[2] + "¬" + mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[3]);
            overlay.SetText("question.answer3", mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[0] + "¬" + mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[1] + "¬" + mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[2] + "¬" + mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[3]);
            overlay.SetText("question.answer4", mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[0] + "¬" + mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[1] + "¬" + mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[2] + "¬" + mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[3]);
            
        }

        private void ContinueDelays()
        {
#if !DEBUG
            for (int state = 0; state < (int)RaidTheCagesModes.MODES; state++)
            {
                if (state == (int)RaidTheCagesModes.SHOWQUESTION)
                    mContinuedelays[state] = 2400;
                else if (state == (int)RaidTheCagesModes.SHOWANSWER1 || state == (int)RaidTheCagesModes.SHOWANSWER2 || state == (int)RaidTheCagesModes.SHOWANSWER3 || state == (int)RaidTheCagesModes.SHOWANSWER4)
                    mContinuedelays[state] = 1000;
                else if (state == (int)RaidTheCagesModes.SHOWRIGHTANSWER)
                    mContinuedelays[state] = 750;
                else if (state == (int)RaidTheCagesModes.REMOVEQUESTION)
                    mContinuedelays[state] = 2000;
                else if (state == (int)RaidTheCagesModes.HIDECLOCK)
                    mContinuedelays[state] = 1750;
                else if (state == (int)RaidTheCagesModes.SHOWTOTALAMOUNTTHISQUESTION || state == (int)RaidTheCagesModes.SHOWTOTALAMOUNT)
                    mContinuedelays[state] = 2000;
                else if (state == (int)RaidTheCagesModes.SHOWCONTESTANTSNAMES)
                    mContinuedelays[state] = 1000;
                else
                    mContinuedelays[state] = 200;
               
            }
#else
            for (int state = 0; state < (int)RaidTheCagesModes.MODES; state++)
            {
                mContinuedelays[state] = 200;
            }
#endif
        }

        private void KillAllTimers()
        {
            cagetimer.Stop();
            mStartTimeAfterDoorOpensTimer.Stop();
            m3secexplain1tmr.Stop();
            m3secexplain2tmr.Stop();
            m3secexplainbacktmr.Stop();
        }

        private void OnLanguageChangeMsg(object sender, eLanguage inLanguage)
        {
            switch (inLanguage)
            {
                case eLanguage.ENGLISH:
                case eLanguage.SECONDLANGUAGEENGLISH:
                    lbName1.Text = MultiLangualTexts.EnglishTexts[8];
                    lbName2.Text = MultiLangualTexts.EnglishTexts[9];
                    gbPrices.Text = MultiLangualTexts.EnglishTexts[10];
                    lbthisquestion.Text = MultiLangualTexts.EnglishTexts[11];
                    lbTotal.Text = MultiLangualTexts.EnglishTexts[12];
                    btShowTotalAmount.Text = MultiLangualTexts.EnglishTexts[13];
                    lblifeline1.Text = MultiLangualTexts.EnglishTexts[14];
                    lblifeline2.Text = MultiLangualTexts.EnglishTexts[15];
                    cbLifeLineUsed1.Text = MultiLangualTexts.EnglishTexts[16];
                    cbLifeLineUsed2.Text = MultiLangualTexts.EnglishTexts[16];
                    btOpenCageDoor.Text = MultiLangualTexts.EnglishTexts[17];
                    btCloseCageDoor.Text = MultiLangualTexts.EnglishTexts[18];
                    btShowMoneyTreeOnProjector.Text = MultiLangualTexts.EnglishTexts[19];
                    break;
                case eLanguage.SECONDLANGUAGE:
                    lbName1.Text = MultiLangualTexts.SecondLanguageTexts[8];
                    lbName2.Text = MultiLangualTexts.SecondLanguageTexts[9];
                    gbPrices.Text = MultiLangualTexts.SecondLanguageTexts[10];
                    lbthisquestion.Text = MultiLangualTexts.SecondLanguageTexts[11];
                    lbTotal.Text = MultiLangualTexts.SecondLanguageTexts[12];
                    btShowTotalAmount.Text = MultiLangualTexts.SecondLanguageTexts[13];
                    lblifeline1.Text = MultiLangualTexts.SecondLanguageTexts[14];
                    lblifeline2.Text = MultiLangualTexts.SecondLanguageTexts[15];
                    cbLifeLineUsed1.Text = MultiLangualTexts.SecondLanguageTexts[16];
                    cbLifeLineUsed2.Text = MultiLangualTexts.SecondLanguageTexts[16];
                    btOpenCageDoor.Text = MultiLangualTexts.SecondLanguageTexts[17];
                    btCloseCageDoor.Text = MultiLangualTexts.SecondLanguageTexts[18];
                    btShowMoneyTreeOnProjector.Text = MultiLangualTexts.SecondLanguageTexts[19];
                    break;
            }
        }

        private void OnDoorStatusMsg(object sender, string doorstatus)
        {
            btOpenCageDoor.BackColor = doorstatus == "1" ? Color.YellowGreen:SystemColors.Control;
            btOpenCageDoor.UseVisualStyleBackColor = doorstatus == "0";

            btCloseCageDoor.BackColor = doorstatus == "0" ? Color.YellowGreen : SystemColors.Control;
            btCloseCageDoor.UseVisualStyleBackColor = doorstatus == "1";

        }

        private void OnSettingsChange(object sender)
        {
            SendMoneyTreeSettings(eEngines.OVERLAY);
            SendMoneyTreeSettings(eEngines.PROJECTOR);
        }

        private void OnEngineConnect(object sender, int inEngine)
        {
            switch (inEngine)
            {
                case (int)eEngines.PROJECTOR:
                case (int)eEngines.OVERLAY:
                    // send the moneytreesettings...o
                    eEngines e = (eEngines)inEngine;

                    SendMoneyTreeSettings(e);
                    break;
                case (int)eEngines.HOST:
                    UpdateHostScreen();
                    break;
            }
        }

        private void SendMoneyTreeSettings(eEngines inEngine)
        {
            mParent.mGFXEngines[(int) inEngine].SetText("moneytree.txt1", localisationsettings.mmoneyTreeSettings[0]);
            mParent.mGFXEngines[(int)inEngine].SetText("moneytree.txt2", localisationsettings.mmoneyTreeSettings[1]);
            mParent.mGFXEngines[(int)inEngine].SetText("moneytree.txt3", localisationsettings.mmoneyTreeSettings[2]);
            mParent.mGFXEngines[(int)inEngine].SetText("moneytree.txt4", localisationsettings.mmoneyTreeSettings[3]);
            mParent.mGFXEngines[(int)inEngine].SetText("moneytree.txt5", localisationsettings.mmoneyTreeSettings[4]);
            mParent.mGFXEngines[(int)inEngine].SetText("moneytree.txt6", localisationsettings.mmoneyTreeSettings[5]);
            mParent.mGFXEngines[(int)inEngine].SetText("moneytree.txt7", localisationsettings.mmoneyTreeSettings[6]);
            mParent.mGFXEngines[(int)inEngine].SetText("moneytree.txt8", localisationsettings.mmoneyTreeSettings[7]);
            mParent.mGFXEngines[(int)inEngine].SetText("moneytree.txt9", localisationsettings.mmoneyTreeSettings[8]);
        }

        private void OnFormClosingMsg(object sender)
        {
            if (RaidTheCageData.mChanged)
                RaidTheCageData.WriteXmlConfig(mParent.ExePath);
        }

        private void OnQuestionReceived(object sender)
        {
            if (GameMode == RaidTheCagesModes.GETQUESTIONFROMWISEQ)
            {
                wiseqretreive.Stop();
                using (UndoRedoManager.Start("questionReceived"))
                {
                    overlay.AddUndo();
                    projector.AddUndo();
                    mParent.mFirstQuestion.Value = true;
                    UpdateQuestionData();
                    GameMode = RaidTheCagesModes.SHOWQUESTION;
                    UndoRedoManager.Commit();
                }
                mParent.ActiveCtrl.UpdateDlg();                
                UpdateDlg(); // for host
                UpdateExtraQA();
            }
            else if (GameMode == RaidTheCagesModes.GETQUESTIONFROMWISEQSWITCH)
            {
                wiseqretreive.Stop();
                using (UndoRedoManager.Start("questionReceived"))
                {
                    overlay.AddUndo();
                    projector.AddUndo();
                    mParent.mFirstQuestion.Value = true;
                    UpdateQuestionData();
                    if (mParent.mSwitchAnswersWithQuestion)
                    {
                        if (mParent.mCurCountry == eCountries.PORTUGAL)
                            GameMode = RaidTheCagesModes.SWITCHQUESTION;
                        else
                            GameMode = RaidTheCagesModes.SHOWRIGHTANSWERBEFORESWITCH;
                    }
                    else
                        GameMode = RaidTheCagesModes.SWITCHQUESTION;
                    UndoRedoManager.Commit();
                }
                mParent.ActiveCtrl.UpdateDlg();
                UpdateDlg(); // for host
                UpdateExtraQA();
            }


        }

        private void UpdateQuestionData()
        {
            // display the data of the current question
            mParent.UpdateQuestionDataCtrl();
        }

        void wiseqretreive_Tick(object sender, EventArgs e)
        {
            wiseqretreive.Stop();
            MessageBox.Show("Unable to retreive data from WiseQ!");
        }

        private void cagetimer_Tick(object sender, EventArgs e)
        {
            TimeSpan t = TimeSpan.FromSeconds(casetimervalue) - (DateTime.Now - mstarttime);

            if (t.TotalMilliseconds <= generalsettings.mTimebeforethedoorcloses && !doorisclosing2)
            {
                SendToRfid("CLOSECAGEDOOR");
                SendToRfid("STARTPOLLING");    
                
                
                doorisclosing2 = true;
            }

            /*
            if (casetimervalue > 30)
            {
                if (t.TotalMilliseconds <= 30000 && !startpolling)  // 30 seconds close door
                {
                    SendToRfid("STARTPOLLING");
                    startpolling = true;
                }
            }
             */

            if (t.TotalMilliseconds<=3000 && !doorisclosing)  // 3 seconds close door
            {                
                doorisclosing = true;
                 using (UndoRedoManager.StartInvisible("Last 3 seconds"))
                 {
                     SendToRfid("PORT8HIGH");
                     mParent.mAudio.SendAudio(SoundsRaidTheCage.DOORS3SEC, SoundCommands.PLAY, false, false);
                     //Midi.SendMidi(midi, midinotes.DOORS3SEC);
                     CheckExtraMidiNote("Closing Door in 3 Seconds");
                     overlay.Play("question.clockblinkred",0,1000,25,false,0);
                     projector.Play("question.clockblinkred", 0, 1000, 25, false, 0);
                    GameMode = RaidTheCagesModes.CAGETIMERUNNINGLAST3SEC;
                    UndoRedoManager.Commit();
                }
                 mParent.ActiveCtrl.UpdateDlg();

            }

            if (t.TotalMilliseconds<=0)
            {
                // stop timer....
                cagetimer.Stop();
                SetCageTime(0);

                doorisclosing = false;
                doorisclosing2 = false;

                overlay.Start("question.stoploopclock");
                //projector.Start("question.stoploopclock");
                

                using (UndoRedoManager.Start("StartNewGame"))
                {
                    projector.AddUndo();
                    overlay.AddUndo();

                    SendToRfid("PORT8LOW");

                    //Midi.SendMidi(midi, midinotes.DOORSCLOSED);
                    CheckExtraMidiNote("Door Closed");

                    //AudioCageTime(false);
                    mParent.mAudio.SendAudio(SoundsRaidTheCage.PLAYERTOLATE,SoundCommands.PLAY,false,false);

                    projector.Start("question.clockred");
                    overlay.Start("question.clockred");
                    GameMode = RaidTheCagesModes.HIDECLOCK;                     
                    UndoRedoManager.Commit();
                }
                mParent.ActiveCtrl.UpdateDlg();
                mParent.UpdateDlg();
                UpdateDlg();
            }
            else
                SetCageTime(t.TotalMilliseconds);
        }


            

        private void continuetimer_Tick(object sender, EventArgs e)
        {
            continuetimer.Stop();
            mParent.ActiveCtrl.ContinuebtnEnable(true);
        }       

         public RaidTheCagesModes GameMode
         {
             get { return mGameMode.Value; }
             set { mGameMode.Value = value; }
         }

        public void UpdateDlg()
        {
            mIsUpdating = true;

            tbName1.Text = RaidTheCageData.mNames[0];
            tbName2.Text = RaidTheCageData.mNames[1];

            tbHost.Text = RaidTheCageData.mHosts[0];
            tbCoHost.Text = RaidTheCageData.mHosts[1];

            tbPlace.Text = RaidTheCageData.mPlace;

            cbLifeLineUsed1.Checked = lLifelinesAvailable[0].Value;
            cbLifeLineUsed2.Checked = lLifelinesAvailable[1].Value;

            btUseLifeLine1.Enabled = lLifelinesAvailable[0].Value;
            btUseLifeLine2.Enabled = lLifelinesAvailable[1].Value;

            tbPriceThisQuestion.Text = mAmountThisQuestion.Value.ToString();
            tbPriceTotal.Text = mAmountTotal.Value.ToString();

            btShowMoneyTreeOnProjector.Enabled = GameMode == RaidTheCagesModes.GETQUESTIONFROMWISEQ || GameMode == RaidTheCagesModes.SHOWQUESTION;

            btWWTBAMLifeLineRiminderStrap.BackColor = mShowLifelinereminderstrap.Value
                                                         ? Color.YellowGreen
                                                         : Color.Transparent;
            btWWTBAMLifeLineRiminderStrap.Text = mShowLifelinereminderstrap.Value
                                                     ? "Hide LifeLine Icons"
                                                     : "Show LifeLine Icons";

            btWWTBAMLifeLineRiminderStrap.Enabled = GameMode == RaidTheCagesModes.WAITFORANSWER; //|| (GameMode == RaidTheCagesModes.GETQUESTIONFROMWISEQ && mParent.mQnr.Value == 0);

            btUseLifeLine1.Enabled = GameMode == RaidTheCagesModes.WAITFORANSWER && lLifelinesAvailable[0].Value;
            btUseLifeLine2.Enabled = GameMode == RaidTheCagesModes.WAITFORANSWER && lLifelinesAvailable[1].Value;

            btShowTotalAmount.Text = mTotalAmountShown.Value ? "Hide" : "Show";
            btShowTotalAmount.BackColor = mTotalAmountShown.Value ? Color.YellowGreen : SystemColors.Control;
            btShowTotalAmount.UseVisualStyleBackColor = !mTotalAmountShown.Value;

            btShowTotalAmount.Enabled = GameMode == RaidTheCagesModes.NEXTQUESTION || GameMode == RaidTheCagesModes.PLAYORSTOP || GameMode == RaidTheCagesModes.SHOWQUESTION || GameMode == RaidTheCagesModes.DONE;


            mIsUpdating = false;
        }

        public Button GetButtonShowTotalAmount()
        {
            return btShowTotalAmount;
        }

        internal void PickUpTheGame()
        {

            mParent.KillAllAudio(false);
            // open dialog...
            PickUpTheGame dlg = new PickUpTheGame();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                SendToRfid("STARTLOGFILE");
                mStartTimeAfterDoorOpensTimer.Stop();

                if (mParent.mCurCountry == eCountries.TURKEY)
                {
                    SendToRfid("CLOSECAGEDOOR");
                }

                string tosend = "";

                tosend += '\r';
                tosend += '\n';
                // patrick
                byte[] sendbytes = Encoding.UTF8.GetBytes(tosend);

                
                    try
                    {
                        if (mParent.ipdaemonHostMessaging.IsConnected)
                            mParent.ipdaemonHostMessaging.Write(sendbytes);
                    }
                    catch (Exception)
                    {


                    }

               

                /*
                foreach (Connection c in mParent.ipdaemonHostMessaging.Connections.Values)
                {
                    //c.DataToSend = "Broadcast Data";
                    //MessageBox.Show("Sending GetVotes now!");
                    try
                    {
                        mParent.ipdaemonHostMessaging.SendLine(c.ConnectionId, "");
                    }
                    catch (Exception)
                    {
                        // do nothing...


                    }
                }
                 */

                cagetimer.Stop();
                // ok.. pick up the game..... get all the variables...
                // create undo...
                using (UndoRedoManager.StartInvisible("PickUpGame"))
                {
                    mLifeLineAvailable1.Value = dlg.mQuestionSwitch;
                    mLifeLineAvailable2.Value = dlg.mPeopleSwitch;

                    if (!dlg.mGoToStopPopup)
                        mParent.mQnr.Value = dlg.mQnr;
                    else
                        mParent.mQnr.Value = dlg.mQnr-1;        // 1 question back.. as we still have a next question after this...

                    mQuestionGotFromWiseQ.Value = dlg.mQnr;

                    mUsedSwitchPlayer.Value = false;
                    mUsedSwitchQuestion.Value = false;  

                    mAmountTotal.Value = dlg.mTotal;
                    mAmountThisQuestion.Value = 0;

                    mParent.MCurStackType.Value = eStackTypes.Main;

                    mShowLifelinereminderstrap.Value = false;

                    mParent.mSelectedAnswer.Value = -1;

                    SendToRfid("STOPPOLLING");      // just in case...
                    SendToRfid("PORT8LOW");

                    UpdateExtraTotal();

                    if (!dlg.mGoToStopPopup)
                        GameMode = RaidTheCagesModes.GETQUESTIONFROMWISEQ;
                    else
                        GameMode = RaidTheCagesModes.PLAYORSTOP;

                    UndoRedoManager.Commit();
                }
                mParent.ActiveCtrl.UpdateDlg();
                mParent.UpdateDlg();
                mParent.UpdateQuestionDataCtrl();
                UpdateDlg();
                UpdateHostScreen();
                UpdateExtraQA();
            }
        }

        internal void StartNewShow()
        {
            mParent.KillAllAudio(false);

            SendToRfid("STARTLOGFILE");

            mStartTimeAfterDoorOpensTimer.Stop();

            string tosend = "";

            tosend += '\r';
            tosend += '\n';
            // patrick
            byte[] sendbytes = Encoding.UTF8.GetBytes(tosend);

            
            try
            {
                if (mParent.ipdaemonHostMessaging.IsConnected)
                    mParent.ipdaemonHostMessaging.Write(sendbytes);
            }
            catch (Exception)
            {


            }

           

            if (mParent.mCurCountry == eCountries.TURKEY)
            {
                SendToRfid("CLOSECAGEDOOR");
            }

            using (UndoRedoManager.Start("StartNewGame"))
            {
                projector.AddUndo();
                overlay.AddUndo();

                SendToRfid("CLEARLISTCURRENTQUESTION");
                SendToRfid("CLEARLISTTOTAL");
                SendToRfid("STOPPOLLING");      // just in case...
                SendToRfid("PORT8LOW");

                mUsedSwitchPlayer.Value = false;
                mUsedSwitchQuestion.Value = false;  

                mParent.MCurStackType.Value = eStackTypes.Main;

                mShowLifelinereminderstrap.Value = false;

                mAmountThisQuestion.Value = 0;
                mAmountTotal.Value = 0;

                mQuestionGotFromWiseQ.Value = 0;

                lLifelinesAvailable[0].Value = true;
                lLifelinesAvailable[1].Value = true;

                cagetimer.Stop();

                mParent.mQnr.Value = 0;
                mParent.mSelectedAnswer.Value = -1;
                
                UpdateExtraTotal();

                if (mParent.mCurCountry == eCountries.PORTUGAL)
                {
                    GameMode = RaidTheCagesModes.STARTINTROMUSIC;
                }
                else if (mParent.mCurCountry == eCountries.MEXICO || mParent.mCurCountry == eCountries.COLUMBIA || mParent.mCurCountry == eCountries.URUGUAY || mParent.mCurCountry == eCountries.VIETNAM || mParent.mCurCountry == eCountries.PARAQUAY || mParent.mCurCountry == eCountries.HUNGARY || mParent.mCurCountry == eCountries.BRAZIL || mParent.mCurCountry == eCountries.URUGUAY2018)
                    GameMode = RaidTheCagesModes.SHOWCONTESTANTSNAMES;
                else
                    GameMode = RaidTheCagesModes.GOTOEXPLAINTHEGAMES;

                UndoRedoManager.Commit();
            }
            mParent.ActiveCtrl.UpdateDlg();
            mParent.UpdateDlg();
            mParent.UpdateQuestionDataCtrl();
            UpdateDlg();
            UpdateExtraQA();
        }

        private void SetCageTime(double p)
        {
            TimeSpan t = TimeSpan.FromMilliseconds( p );

            timestring = string.Format("{0:D2}{1:D2}{2:D2}",     			
    			t.Minutes, 
    			t.Seconds, 
    			t.Milliseconds);
            timestringctrl = string.Format("{0:D2}:{1:D2}.{2:D2}",
    t.Minutes,
    t.Seconds,
    t.Milliseconds);

tbTimeLeft.Text = timestringctrl;

            string id,id2;
            for (int pos=0;pos<timestring.Length;pos++)
            {
                id = string.Format("question.clock.digit{0}", pos + 1);
                id2 = string.Format("question.clock.digitred{0}", pos + 1);
                
                switch(timestring.Substring(pos,1))
                {
                    case "0":
                        overlay.Play(id, 0, 0, 25, false, 1);
                        projector.Play(id, 0, 0, 25, false, 1);
                        overlay.Play(id2, 0, 0, 25, false, 1);
                        projector.Play(id2, 0, 0, 25, false, 1);
                        break;
                    case "1":
                        overlay.Play(id, 1, 1, 25, false, 1);
                        projector.Play(id, 1, 1, 25, false, 1);
                        overlay.Play(id2, 1, 1, 25, false, 1);
                        projector.Play(id2, 1, 1, 25, false, 1);
                        break;
                    case "2":
                        overlay.Play(id, 2, 2, 25, false, 1);
                        projector.Play(id, 2, 2, 25, false, 1);
                        overlay.Play(id2, 2, 2, 25, false, 1);
                        projector.Play(id2, 2, 2, 25, false, 1);
                        break;
                    case "3":
                        overlay.Play(id, 3, 3, 25, false, 1);
                        projector.Play(id, 3, 3, 25, false, 1);
                        overlay.Play(id2, 3, 3, 25, false, 1);
                        projector.Play(id2, 3, 3, 25, false, 1);
                        break;
                    case "4":
                        overlay.Play(id, 4, 4, 25, false, 1);
                        projector.Play(id, 4, 4, 25, false, 1);
                        overlay.Play(id2, 4, 4, 25, false, 1);
                        projector.Play(id2, 4, 4, 25, false, 1);
                        break;
                    case "5":
                        overlay.Play(id, 5, 5, 25, false, 1);
                        projector.Play(id, 5, 5, 25, false, 1);
                        overlay.Play(id2, 5, 5, 25, false, 1);
                        projector.Play(id2, 5, 5, 25, false, 1);
                        break;
                    case "6":
                        overlay.Play(id, 6, 6, 25, false, 1);
                        projector.Play(id, 6, 6, 25, false, 1);
                        overlay.Play(id2, 6, 6, 25, false, 1);
                        projector.Play(id2, 6, 6, 25, false, 1);
                        break;
                    case "7":
                        overlay.Play(id, 7, 7, 25, false, 1);
                        projector.Play(id, 7, 7, 25, false, 1);
                        overlay.Play(id2, 7, 7, 25, false, 1);
                        projector.Play(id2, 7, 7, 25, false, 1);
                        break;
                    case "8":
                        overlay.Play(id, 8, 8, 25, false, 1);
                        projector.Play(id, 8, 8, 25, false, 1);
                        overlay.Play(id2, 8, 8, 25, false, 1);
                        projector.Play(id2, 8, 8, 25, false, 1);
                        break;
                    case "9":
                        overlay.Play(id, 9, 9, 25, false, 1);
                        projector.Play(id, 9, 9, 25, false, 1);
                        overlay.Play(id2, 9, 9, 25, false, 1);
                        projector.Play(id2, 9, 9, 25, false, 1);
                        break;
                }
            }        
        }

        

        public void SendToRfid(string p)
        {
            if (mParent.mGFXEnginesConnected[(int)eEngines.CAGE])
            {
                string message = p + "¶";

#if USEENCRYPT
                message = mParent.Encrypt(message);
#endif
                message += "\r\n";

                mParent.mGFXEngines[(int) eEngines.CAGE].SendTextPlainUnicode(message);
            }
        }

        public string HostAmount(long inAmount)
        {
            string stringtouse = "";

            if (!generalsettings.mCurrencysignafteramount)
            {
                stringtouse = generalsettings.mCurrencysign;
                stringtouse += FormatAmount(inAmount);
            }
            else
            {
                stringtouse = FormatAmount(inAmount);
                stringtouse += generalsettings.mCurrencysign;
            }

            return stringtouse;

        }

        public string FormatAmount(double inAmount)
        {
            double roundup = RoundUp(inAmount, 0);

            string returnstring = "";


            NumberFormatInfo nfi;// = new NumberFormatInfo { NumberGroupSeparator = ".", NumberDecimalDigits = 0 };

            if (mParent.mCurCountry == eCountries.HUNGARY || mParent.mCurCountry == eCountries.BRAZIL)
            {
                nfi = new NumberFormatInfo { NumberGroupSeparator = ".", NumberDecimalDigits = 0 };

                //returnstring = roundup.ToString("N0", new CultureInfo("hu-HU"));
                returnstring = inAmount.ToString("n", nfi);
            }
            else
            {
                returnstring = roundup.ToString("N0", new CultureInfo("en-GB"));
            }
        
            //if (mParent.mCurCountry == eCountries.VIETNAM)
            //    return roundup.ToString("N0", new CultureInfo("en-GB"));
            //else
            return returnstring;
        }

        public double RoundUp(double d, int decimals)
        {
            double degree = Math.Pow(10, decimals);
            return Math.Ceiling(d * degree) / degree;
        }

        internal void SetTotalAmount(long inAmount)
        {
            string stringtouse = "";

            if (!generalsettings.mCurrencysignafteramount)
            {
                stringtouse = generalsettings.mCurrencysign;
                stringtouse += FormatAmount(inAmount);
            }
            else
            {                
                stringtouse = FormatAmount(inAmount);
                stringtouse += generalsettings.mCurrencysign;
            }

            if (stringtouse.Length > 8)
                stringtouse = stringtouse.Replace(" ", "");

            string id;
            int number;

            int xposprojector;
            int xposoverlay;
            int totallengthprojector;
            int totallengthoverlay;

            int mSpacebeforeoverlay;
            int mSpaceafteroverlay;

            int mSpacebeforeprojector;
            int mSpaceafterprojector;

            int mExtraSpacingProjector;
            int mExtraSpacingOverlay;  

            if (mParent.mCurCountry == eCountries.HUNGARY || mParent.mCurCountry == eCountries.BRAZIL || mParent.mCurCountry == eCountries.URUGUAY2018)
            {
                xposprojector = 0;
                xposoverlay = 0;
                totallengthprojector = 0;
                totallengthoverlay = 0;

                mSpacebeforeoverlay = 0;
                mSpaceafteroverlay = 0;

                mSpacebeforeprojector = 0;
                mSpaceafterprojector = 0;

                mExtraSpacingProjector = 14;
                mExtraSpacingOverlay = 14;   
            }
            else
            {
                xposprojector = 180;
                xposoverlay = 180;
                totallengthprojector = 0;
                totallengthoverlay = 0;

                mSpacebeforeoverlay = 0;
                mSpaceafteroverlay = 0;

                mSpacebeforeprojector = 0;
                mSpaceafterprojector = 0;

                mExtraSpacingProjector = 4;
                mExtraSpacingOverlay = 14;    
            }
            

            string let;
            for (number = 0; number < stringtouse.Length; number++)
            {
                id = string.Format("totalamount.blinkcash.cashanim.num{0}", number + 1);

                let = stringtouse.Substring(number, 1);

                LoadLetterinIDProjector(id, let, true, out mSpacebeforeprojector, out mSpaceafterprojector);
                LoadLetterinIDOverlay(id, let, true, out mSpacebeforeoverlay, out mSpaceafteroverlay);


                // move the id... projector
                int sizeprojector = 180;
                if (let == ".")
                {
                    sizeprojector = 50;
                }
                else if (let == " ")
                {
                    sizeprojector = 50;
                }
                else if (let == ",")
                {
                    sizeprojector = 65;
                }
                else if (let == "*")
                {
                    sizeprojector = 200;
                }
                else if (let == "R")
                {
                    sizeprojector = 250;
                }
                //if (number == 0)
                //{
                //    screenendtrack.SetPosition(id, x-mSpacebefore, 0);
                //    totallength += size - mSpaceafter - mSpacebefore + mExtraSpacing;
                //    x += size - mSpacebefore - mSpaceafter + mExtraSpacing;
                // }
                //else
                //{

                if (mParent.mCurCountry == eCountries.HUNGARY || mParent.mCurCountry == eCountries.BRAZIL || mParent.mCurCountry == eCountries.URUGUAY2018)
                {
                    if (let == "." || let == ",")
                        projector.SetPosition(id, xposprojector - mSpacebeforeprojector, 1065);
                    else if (let == "R")
                        projector.SetPosition(id, xposprojector - mSpacebeforeprojector, 960);
                    else
                        projector.SetPosition(id, xposprojector - mSpacebeforeprojector, 955);
                }
                else
                    projector.SetPosition(id, xposprojector - mSpacebeforeprojector, 955);

                totallengthprojector += (sizeprojector - mSpacebeforeprojector - mSpaceafterprojector + mExtraSpacingProjector);
                xposprojector += (sizeprojector - mSpacebeforeprojector - mSpaceafterprojector + mExtraSpacingProjector);
                //}

                // opacity on...                
                projector.SetOpacity(id, 255);
                projector.Play(id, 0, 0, 25, false, 1);

                // and the overlay
                // move the id... projector
                int sizeoverlay = 180;
                if (let == ".")
                {
                    sizeoverlay = 50;
                }
                else if (let == " ")
                {
                    sizeoverlay = 50;
                }
                else if (let == ",")
                {
                    sizeoverlay = 65;
                }
                else if (let == "*")
                {
                    sizeoverlay = 200;
                }
                else if (let == "R")
                {
                    sizeoverlay = 250;
                }

                //if (number == 0)
                //{
                //    screenendtrack.SetPosition(id, x-mSpacebefore, 0);
                //    totallength += size - mSpaceafter - mSpacebefore + mExtraSpacing;
                //    x += size - mSpacebefore - mSpaceafter + mExtraSpacing;
                // }
                //else
                //{
                //overlay.SetPosition(id, xposoverlay - mSpacebeforeoverlay, 955);
                if (mParent.mCurCountry == eCountries.HUNGARY || mParent.mCurCountry == eCountries.BRAZIL || mParent.mCurCountry == eCountries.URUGUAY2018)
                {
                    if (let == "." || let == ",")
                        overlay.SetPosition(id, xposoverlay - mSpacebeforeoverlay, 1065);
                    else if (let == "R")
                        overlay.SetPosition(id, xposoverlay - mSpacebeforeoverlay, 958);
                    else
                        overlay.SetPosition(id, xposoverlay - mSpacebeforeoverlay, 955);
                }
                else
                    overlay.SetPosition(id, xposoverlay - mSpacebeforeoverlay, 955);


                totallengthoverlay += (sizeoverlay - mSpacebeforeoverlay - mSpaceafteroverlay + mExtraSpacingOverlay);
                xposoverlay += (sizeoverlay - mSpacebeforeoverlay - mSpaceafteroverlay + mExtraSpacingOverlay);
                //}

               

                // opacity on...                
                overlay.SetOpacity(id, 255);
                overlay.Play(id, 0, 0, 25, false, 1);

            }
            for (; number < 14; number++)
            {
                id = string.Format("totalamount.blinkcash.cashanim.num{0}", number + 1);
                projector.SetOpacity(id, 0);
                overlay.SetOpacity(id, 0);
            }

            int middleprojector;
            int middleoverlay;

            int newstartpointx = 0;

            if (mParent.mCurCountry == eCountries.BRAZIL)
                totallengthprojector += -350;
            else
                totallengthprojector += -300;

            if (mParent.mCurCountry == eCountries.HUNGARY || mParent.mCurCountry == eCountries.BRAZIL || mParent.mCurCountry == eCountries.URUGUAY2018)
            {
                // ok.. the bar is 938 width... so to move it in the middle we have to check if the width is not smaller.. and then add half
                if (totallengthprojector < 820)
                {
                    projector.Transform("totalamount.blinkcash.cashanim", 1, 0, 0, 1);
                    int newhalf = totallengthoverlay / 2;
                    newstartpointx = 960 - newhalf;
                }
                else
                {
                    // we need to transform to make it fit...
                    // middleoverlay = ((1920 - totallengthoverlay) / 2);
                    int perc = (totallengthprojector * 100) / 790;
                    perc = 200 - perc;
                    float percf = (float)perc / 100;

                    // 100 = 1.00 how much is 10
                    projector.Transform("totalamount.blinkcash.cashanim", percf, 0, 0, 1);

                    int newprojectorwidth = (int)(totallengthprojector * percf);
                    int newhalf = newprojectorwidth / 2;

                    newstartpointx = 960 - newhalf;
                }

                if (mParent.mCurCountry == eCountries.BRAZIL)
                    totallengthoverlay += -350;
                else
                    totallengthoverlay += -300;


                if (totallengthoverlay < 820)
                {
                    // normal.. just calculate the middle
                    overlay.Transform("totalamount.blinkcash.cashanim", 1, 0, 0, 1);
                    //middleoverlay = ((1133 - totallengthoverlay) / 2);
                    //middleoverlay += 160;
                    int newhalf = totallengthoverlay / 2;
                    newstartpointx = 960 - newhalf;
                }
                else
                {
                    // we need to transform to make it fit...
                   // middleoverlay = ((1920 - totallengthoverlay) / 2);
                    int perc = (totallengthoverlay * 100) / 790;
                    perc = 200 - perc;
                    float percf = (float)perc / 100;

                    // 100 = 1.00 how much is 10
                    overlay.Transform("totalamount.blinkcash.cashanim", percf, 0, 0, 1);

                    int newoverlaywidth = (int)(totallengthoverlay * percf);
                    int newhalf = newoverlaywidth / 2;

                    newstartpointx = 960 - newhalf;

                    //int test = 100 - perc;


                    //middleoverlay = test * 16;
                }

                //middleoverlay = ((1920 - totallengthoverlay) / 2);

                //middleprojector = ((960 - totallengthprojector) / 2) + 200;
                //middleprojector =  960-(totallengthprojector / 2);
                //middleprojector = 0;
            }
            else
            {
                middleprojector = ((360 - totallengthprojector) / 2) + 30;
                middleoverlay = ((1920 - totallengthoverlay) / 2);

                if (totallengthoverlay > 1300)
                {
                    overlay.Transform("totalamount.blinkcash.cashanim", (float)0.8, 0, 0, 1);
                    overlay.SetPosition("totalamount.blinkcash.cashanim", middleoverlay + 180, 0);
                }
                else
                {
                    overlay.Transform("totalamount.blinkcash.cashanim", 1, 0, 0, 1);
                }
            }
            // set the complete composite on the middle...

            projector.SetPosition("totalamount.blinkcash.cashanim", newstartpointx, 0);

            // set the complete composite on the middle...
            //newstartpointx = 960;

            overlay.SetPosition("totalamount.blinkcash.cashanim", newstartpointx, 0);

            // ok.. check if the number of characters is too much.. then we need to resize and reposition...
            /*
            if (totallengthprojector > 400)
            {
                projector.Transform("totalamount.blinkcash.cashanim", (float)0.9, 0, 0, 1);
                projector.SetPosition("totalamount.blinkcash.cashanim", middleprojector+40, 0);
            }
            else
            {
                projector.Transform("totalamount.blinkcash.cashanim", 1, 0, 0, 1);

            }
             */

           


        }

        private void LoadLetterinIDProjector(string inID, string letter, bool packcash, out int mSpacebefore, out int mSpaceafter)
        {
            mSpacebefore = 0;
            mSpaceafter = 0;
            switch (letter)
            {
                case " ":
                    projector.SetSequence(inID, @"Totalprizemoney\Prize_numbers\sign_dot123.", ".tga", 20, 0, 4, false);
                    mSpacebefore = 0;
                    mSpaceafter = 0;
                    break;
                case ".":
                    projector.SetSequence(inID, @"Totalprizemoney\Prize_numbers\sign_dot.", ".tga", 20, 0, 4, false);
                    mSpacebefore = 14;
                    mSpaceafter = 12;
                    break;
                case ",":
                    projector.SetSequence(inID, @"Totalprizemoney\Prize_numbers\sign_comma.", ".tga", 20, 0, 4, false);
                    mSpacebefore = 20;
                    mSpaceafter = 19;
                    break;
                /*
            case "£":
                if (!highlite)
                    inEngine.SetText(inID, packcash ? @"Money_bars_amounts\Pack_cash\pack_money_3Dnumbers\cashwhite_pound.tga" : @"Money_bars_amounts\Break_cash\break_money_3Dnumbers\cashred_pound.tga");
                else
                    inEngine.SetText(inID, packcash ? @"Money_bars_amounts\Pack_cash\pack_money_glow\glow_cashwhite_pound.tga" : @"Money_bars_amounts\Break_cash\break_money_glow\glow_cashred_pound.tga");

                mSpacebefore = 86;
                mSpaceafter = 74;
                break;
                 */
                case "€":
                    projector.SetSequence(inID, @"Totalprizemoney\Prize_numbers\sign_euro.", ".tga", 20, 0, 4, false);

                    mSpacebefore = 21;
                    mSpaceafter = 34;
                    break;
                case "$":
                    projector.SetSequence(inID, @"Totalprizemoney\Prize_numbers\sign_dollar.", ".tga", 20, 0, 4, false);

                    mSpacebefore = 24;
                    mSpaceafter = 36;
                    break;
                case "F":   // from Ft of the hungarian version
                    projector.SetSequence(inID, @"Totalprizemoney\Prize_numbers\sign_Hungary.", ".tga", 20, 0, 4, false);
                    mSpacebefore = -40;
                    mSpaceafter = 38;
                    break;
                case "R":   // from R$ of the brazilian version
                    projector.SetSequence(inID, @"Totalprizemoney\Prize_numbers\sign_Brazil.", ".tga", 20, 0, 4, false);
                    mSpacebefore = 19;
                    mSpaceafter = 3;
                    break;
                case "*":       // romania LEI
                    projector.SetSequence(inID, @"Totalprizemoney\Prize_numbers\sign_lei.", ".tga", 20, 0, 4, false);

                    mSpacebefore = -12;
                    mSpaceafter = 13;
                    break;
                case "0":

                    projector.SetSequence(inID, @"Totalprizemoney\Prize_numbers\number00.", ".tga", 20, 0, 4, false);

                    mSpacebefore = 35;
                    mSpaceafter = 33;
                    break;
                case "1":
                    projector.SetSequence(inID, @"Totalprizemoney\Prize_numbers\number01.", ".tga", 20, 0, 4, false);

                    mSpacebefore = 53;
                    mSpaceafter = 31;
                    break;
                case "2":

                    projector.SetSequence(inID, @"Totalprizemoney\Prize_numbers\number02.", ".tga", 20, 0, 4, false);

                    mSpacebefore = 39;
                    mSpaceafter = 37;
                    break;
                case "3":

                    projector.SetSequence(inID, @"Totalprizemoney\Prize_numbers\number03.", ".tga", 20, 0, 4, false);

                    mSpacebefore = 37;
                    mSpaceafter = 35;
                    break;
                case "4":

                    projector.SetSequence(inID, @"Totalprizemoney\Prize_numbers\number04.", ".tga", 20, 0, 4, false);

                    mSpacebefore = 33;
                    mSpaceafter = 31;
                    break;
                case "5":

                    projector.SetSequence(inID, @"Totalprizemoney\Prize_numbers\number05.", ".tga", 20, 0, 4, false);

                    mSpacebefore = 38;
                    mSpaceafter = 37;
                    break;
                case "6":

                    projector.SetSequence(inID, @"Totalprizemoney\Prize_numbers\number06.", ".tga", 20, 0, 4, false);

                    mSpacebefore = 38;
                    mSpaceafter = 36;
                    break;
                case "7":

                    projector.SetSequence(inID, @"Totalprizemoney\Prize_numbers\number07.", ".tga", 20, 0, 4, false);

                    mSpacebefore = 40;
                    mSpaceafter = 38;
                    break;
                case "8":

                    projector.SetSequence(inID, @"Totalprizemoney\Prize_numbers\number08.", ".tga", 20, 0, 4, false);

                    mSpacebefore = 37;
                    mSpaceafter = 35;
                    break;
                case "9":

                    projector.SetSequence(inID, @"Totalprizemoney\Prize_numbers\number09.", ".tga", 20, 0, 4, false);

                    mSpacebefore = 38;
                    mSpaceafter = 36;
                    break;
            }
            projector.Play(inID, 19, 19, 25, false, 1);
        }

        private void LoadLetterinIDOverlay(string inID, string letter, bool packcash, out int mSpacebefore, out int mSpaceafter)
        {
            mSpacebefore = 0;
            mSpaceafter = 0;
            switch (letter)
            {
                case " ":
                    overlay.SetSequence(inID, @"Totalprizemoney\Prize_numbers\sign_dot123.", ".tga", 20, 0, 4, false);
                    mSpacebefore = 0;
                    mSpaceafter = 0;
                    break;
                case ".":
                    overlay.SetSequence(inID, @"Totalprizemoney\Prize_numbers\sign_dot.", ".tga", 20, 0, 4, false);
                    mSpacebefore = 14;
                    mSpaceafter = 12;
                    break;
                case ",":
                    overlay.SetSequence(inID, @"Totalprizemoney\Prize_numbers\sign_comma.", ".tga", 20, 0, 4, false);
                    mSpacebefore = 20;
                    mSpaceafter = 19;
                    break;
                /*
            case "£":
                if (!highlite)
                    inEngine.SetText(inID, packcash ? @"Money_bars_amounts\Pack_cash\pack_money_3Dnumbers\cashwhite_pound.tga" : @"Money_bars_amounts\Break_cash\break_money_3Dnumbers\cashred_pound.tga");
                else
                    inEngine.SetText(inID, packcash ? @"Money_bars_amounts\Pack_cash\pack_money_glow\glow_cashwhite_pound.tga" : @"Money_bars_amounts\Break_cash\break_money_glow\glow_cashred_pound.tga");

                mSpacebefore = 86;
                mSpaceafter = 74;
                break;
                 */
                case "€":
                    overlay.SetSequence(inID, @"Totalprizemoney\Prize_numbers\sign_euro.", ".tga", 20, 0, 4, false);

                    mSpacebefore = 21;
                    mSpaceafter = 34;
                    break;
                case "$":
                    overlay.SetSequence(inID, @"Totalprizemoney\Prize_numbers\sign_dollar.", ".tga", 20, 0, 4, false);

                    mSpacebefore = 24;
                    mSpaceafter = 36;
                    break;
                case "F":   // from Ft of the hungarian version
                    overlay.SetSequence(inID, @"Totalprizemoney\Prize_numbers\sign_Hungary.", ".tga", 20, 0, 4, false);
                    mSpacebefore = -40;
                    mSpaceafter = 38;
                    break;
                case "R":   // from R$ of the brazilian version
                    overlay.SetSequence(inID, @"Totalprizemoney\Prize_numbers\sign_Brazil.", ".tga", 20, 0, 4, false);
                    mSpacebefore = 19;
                    mSpaceafter = 3;
                    break;
                case "*":       // romania LEI
                    overlay.SetSequence(inID, @"Totalprizemoney\Prize_numbers\sign_lei.", ".tga", 20, 0, 4, false);

                    mSpacebefore = -12;
                    mSpaceafter = 13;
                    break;
                case "0":

                    overlay.SetSequence(inID, @"Totalprizemoney\Prize_numbers\number00.", ".tga", 20, 0, 4, false);

                    mSpacebefore = 35;
                    mSpaceafter = 33;
                    break;
                case "1":
                    overlay.SetSequence(inID, @"Totalprizemoney\Prize_numbers\number01.", ".tga", 20, 0, 4, false);

                    mSpacebefore = 53;
                    mSpaceafter = 31;
                    break;
                case "2":

                    overlay.SetSequence(inID, @"Totalprizemoney\Prize_numbers\number02.", ".tga", 20, 0, 4, false);

                    mSpacebefore = 39;
                    mSpaceafter = 37;
                    break;
                case "3":

                    overlay.SetSequence(inID, @"Totalprizemoney\Prize_numbers\number03.", ".tga", 20, 0, 4, false);

                    mSpacebefore = 37;
                    mSpaceafter = 35;
                    break;
                case "4":

                    overlay.SetSequence(inID, @"Totalprizemoney\Prize_numbers\number04.", ".tga", 20, 0, 4, false);

                    mSpacebefore = 33;
                    mSpaceafter = 31;
                    break;
                case "5":

                    overlay.SetSequence(inID, @"Totalprizemoney\Prize_numbers\number05.", ".tga", 20, 0, 4, false);

                    mSpacebefore = 38;
                    mSpaceafter = 37;
                    break;
                case "6":

                    overlay.SetSequence(inID, @"Totalprizemoney\Prize_numbers\number06.", ".tga", 20, 0, 4, false);

                    mSpacebefore = 38;
                    mSpaceafter = 36;
                    break;
                case "7":

                    overlay.SetSequence(inID, @"Totalprizemoney\Prize_numbers\number07.", ".tga", 20, 0, 4, false);

                    mSpacebefore = 40;
                    mSpaceafter = 38;
                    break;
                case "8":

                    overlay.SetSequence(inID, @"Totalprizemoney\Prize_numbers\number08.", ".tga", 20, 0, 4, false);

                    mSpacebefore = 37;
                    mSpaceafter = 35;
                    break;
                case "9":

                    overlay.SetSequence(inID, @"Totalprizemoney\Prize_numbers\number09.", ".tga", 20, 0, 4, false);

                    mSpacebefore = 38;
                    mSpaceafter = 36;
                    break;
            }
            overlay.Play(inID, 19, 19, 25, false, 1);
        }

        

        public void ShowArticle(int inArticlenr)
        {            
            //projector.Start(string.Format("pricelist.showtag{0}", 14-inArticlenr));
            if (inArticlenr < 6)
            {
                overlay.Start(string.Format("pricelist.showtag{0}", inArticlenr + 1));
                projector.Start(string.Format("pricelist.showtag{0}", inArticlenr + 1));
            }
            else
            {
                string ID = string.Format("pricelist.move{0}", inArticlenr - 5);
                overlay.Start(ID);
                projector.Start(ID);
            }
        }

        private void SendDataToArticleList()
        {
            int cnt = 0;
            foreach (DataGridViewRow row in dgv.Rows)
            {
                projector.SetText(string.Format("pricelist.tag{0}.txt", 14 - cnt), row.Cells[0].Value.ToString());

                if (mParent.mCurCountry == eCountries.RUSSIA || mParent.mCurCountry == eCountries.ROMANIA || mParent.mCurCountry == eCountries.ARGENTINA || mParent.mCurCountry == eCountries.UK || mParent.mCurCountry == eCountries.URUGUAY || mParent.mCurCountry == eCountries.PERU || mParent.mCurCountry == eCountries.MEXICO || mParent.mCurCountry == eCountries.COLUMBIA || mParent.mCurCountry == eCountries.VIETNAM || mParent.mCurCountry == eCountries.PARAQUAY || mParent.mCurCountry == eCountries.PORTUGAL || mParent.mCurCountry == eCountries.HUNGARY || mParent.mCurCountry == eCountries.BRAZIL || mParent.mCurCountry == eCountries.URUGUAY2018)
                {
                    if (row.Cells[0].Value.ToString().Length >= mParent.mArticlelengthbeforebreaking)
                    {
                        // so its bigger.. now check if it has a space..
                        int spacepos = row.Cells[0].Value.ToString().IndexOf(" ");
                        if (spacepos != -1)
                        {
                            // we have a space... change this space too breaking character
                            StringBuilder tosend = new StringBuilder(row.Cells[0].Value.ToString());
                            tosend[spacepos] = '¬';
                            overlay.SetText(string.Format("pricelist.tag{0}.txt", cnt + 1), tosend.ToString());
                        }
                        else
                            overlay.SetText(string.Format("pricelist.tag{0}.txt", cnt + 1),
                                            row.Cells[0].Value.ToString());
                    }
                    else
                        overlay.SetText(string.Format("pricelist.tag{0}.txt", cnt + 1), row.Cells[0].Value.ToString());
                }
                else
                {
                    overlay.SetText(string.Format("pricelist.tag{0}.txt", cnt + 1), row.Cells[0].Value.ToString());
                }
                cnt++;
            }

            /*
             * Centered.... not used anymore.. from bottom to top... and not move
            int cnt = 0;
            foreach (DataGridViewRow row in dgv.Rows)
            {
                projector.SetText(string.Format("pricelist.tag{0}.txt", 1+cnt),row.Cells[0].Value.ToString());
                overlay.SetText(string.Format("pricelist.tag{0}.txt", 1+cnt++), row.Cells[0].Value.ToString());
            }
            // move the tags down.. so they are centered.....
            // y position with 14 tags = 0... 1 size tag is 40...
            int ypos = ((14 - cnt)*40)/2;
            projector.SetY("pricelist.tagmove", ypos);
            overlay.SetY("pricelist.tagmove", ypos);        // todo: still get the right y size for the overlay!
             */
        }

        private bool checkanswer()
        {
            if (mParent.mFirstQuestion.Value)
            {
                if (mParent.mSelectedAnswer.Value == 0 && mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mRightAnswer == "1" ||
                    mParent.mSelectedAnswer.Value == 1 && mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mRightAnswer == "2" ||
                    mParent.mSelectedAnswer.Value == 2 && mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mRightAnswer == "3" ||
                    mParent.mSelectedAnswer.Value == 3 && mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mRightAnswer == "4")
                    return true;
            }
            else
            {
                if (mParent.mSelectedAnswer.Value == 0 && mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mRightAnswer2 == "1" ||
                    mParent.mSelectedAnswer.Value == 1 && mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mRightAnswer2 == "2" ||
                    mParent.mSelectedAnswer.Value == 2 && mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mRightAnswer2 == "3" ||
                    mParent.mSelectedAnswer.Value == 3 && mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mRightAnswer2 == "4")
                    return true;
            }
            return false;
            
        }

        private void ShowRightAnswerEngine(eEngines inEngine)
        {
            switch (inEngine)
            {
                case eEngines.OVERLAY:
                    // check if the right answer is also the selected answer...
                    //if (mParent.mSelectedAnswer.Value.ToString() == mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mRightAnswer)
                    //{
                    //    overlay.Play(string.Format("question.selectanswer{0}", mParent.mFirstQuestion.Value ? mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mRightAnswer : mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mRightAnswer2), 0, 0, 25, false, 1);
//
  //                  }
                    overlay.Start(string.Format("question.correctanswer{0}", mParent.mFirstQuestion.Value ? mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mRightAnswer : mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mRightAnswer2));
                    break;
                case eEngines.PROJECTOR:
                    // check if the right answer is also the selected answer...
                    /*
                    int sa = mParent.mSelectedAnswer.Value + 1;
                    if (sa.ToString() == (mParent.mFirstQuestion.Value ? mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mRightAnswer : mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mRightAnswer2))
                    {
                        projector.SetOpacity(string.Format("question.select{0}", sa), 0);
                        /*
                        //stop the anim and hide the highlite..
                        //question.selectanswer1
                        projector.Play(
                            string.Format("question.selectanswer{0}",
                                          mParent.lQuestions[mParent.mQnr.Value].mRightAnswer), 0, 0, 25, false, 1);

                         */
                    //}

                    projector.Start(string.Format("question.correctanswer{0}", mParent.mFirstQuestion.Value ? mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mRightAnswer : mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mRightAnswer2));
                    break;
            }
        }

        private void ShowAnswerEngine(int inAnswernr, eEngines inEngines)
        {
            switch (inEngines)
            {
                case eEngines.OVERLAY:
                    if (inAnswernr == 0)
                        overlay.Start("question.showanswer1");
                    else if (inAnswernr == 1)
                        overlay.Start("question.showanswer2");
                    else if (inAnswernr == 2)
                        overlay.Start("question.showanswer3");
                    else if (inAnswernr == 3)
                        overlay.Start("question.showanswer4");
                    break;
                case eEngines.PROJECTOR:
                    if (inAnswernr == 0)
                        projector.Start("question.showanswer1");
                    else if (inAnswernr == 1)
                        projector.Start("question.showanswer2");
                    else if (inAnswernr == 2)
                        projector.Start("question.showanswer3");
                    else if (inAnswernr == 3)
                        projector.Start("question.showanswer4");
                    break;
            }
        }

        private void ShowQuestionEngine(eEngines inEngine)
        {
            switch (inEngine)
            {
                case eEngines.PROJECTOR:
                    projector.Start("question.showquestion");
                    break;
                case eEngines.OVERLAY:
                    overlay.Start("question.showquestion");
                    break;
            }
        }

        private void SendQuestionDataToEngines()
        {
            projector.SetText("question.question", mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mQuestion.Replace("|", "¬"));
            projector.SetText("question.question2", mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mQuestion2.Replace("|", "¬"));
            projector.SetText("question.answer1", mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[0] + "¬" + mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[1] + "¬" + mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[2] + "¬" + mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[3]);
            projector.SetText("question.answer2", mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[0] + "¬" + mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[1] + "¬" + mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[2] + "¬" + mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[3]);
            projector.SetText("question.answer3", mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[0] + "¬" + mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[1] + "¬" + mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[2] + "¬" + mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[3]);
            projector.SetText("question.answer4", mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[0] + "¬" + mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[1] + "¬" + mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[2] + "¬" + mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[3]);

            overlay.SetText("question.question", mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mQuestion.Replace("|", "¬"));
            overlay.SetText("question.question2", mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mQuestion2.Replace("|", "¬"));
            overlay.SetText("question.answer1", mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[0] + "¬" + mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[1] + "¬" + mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[2] + "¬" + mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[3]);
            overlay.SetText("question.answer2", mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[0] + "¬" + mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[1] + "¬" + mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[2] + "¬" + mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[3]);
            overlay.SetText("question.answer3", mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[0] + "¬" + mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[1] + "¬" + mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[2] + "¬" + mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[3]);
            overlay.SetText("question.answer4", mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[0] + "¬" + mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[1] + "¬" + mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[2] + "¬" + mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[3]);
        }

        private void btStopCageTimer_Click(object sender, EventArgs e)
        {
            if (GameMode == RaidTheCagesModes.CAGETIMERUNNING)
            {
                // stop the timer....
                cagetimer.Stop();
            }
        }

        private void tbName1_TextChanged(object sender, EventArgs e)
        {
            if (!mIsUpdating)
            {
                RaidTheCageData.mNames[0] = tbName1.Text;
                RaidTheCageData.mChanged = true;
                UpdateHostScreen();
            }
        }

        private void tbName2_TextChanged(object sender, EventArgs e)
        {
            if (!mIsUpdating)
            {
                RaidTheCageData.mNames[1] = tbName2.Text;
                RaidTheCageData.mChanged = true;
                UpdateHostScreen();
            }
        }

        public override void OnUndo()
        {
            if (continuetimer.Enabled)
                return;

            KillAllTimers();

            if (UndoRedoManager.CanUndo)
            {
                BackupSounds();
                UndoRedoManager.Undo();
                CheckSounds();
                overlay.Undo();
                projector.Undo();
                UpdateDlg();
                mParent.ActiveCtrl.UpdateDlg();
                mParent.UpdateDlg();
                mParent.UpdateQuestonDataCtrlColors();
                UpdateHostScreen();
                mParent.OrderCageArticles();
                mParent.UpdateQuestionDataCtrl();
                UpdateExtraQA();
                UpdateExtraTotal();
                if (GameMode == RaidTheCagesModes.STOPRFIDSCANNING)
                {
                    // start the polling again...
                    SendToRfid("STARTPOLLING");
                    SendToRfid("WAITINGFORLISTCOMPLETE");
                }
                else
                {
                    SendToRfid("DISABLELISTCOMPLETE");
                }
                projector.Start("logo");
                projector.Start("logo3d");
            }
        }

       

        public override void OnRedo()
        {
            if (continuetimer.Enabled)
                return;

            KillAllTimers();

            if (UndoRedoManager.CanRedo)
            {
                BackupSounds();
                UndoRedoManager.Redo();
                CheckSounds();

                overlay.Redo();
                projector.Redo();
                UpdateDlg();
                mParent.ActiveCtrl.UpdateDlg();
                mParent.UpdateDlg();
                mParent.UpdateQuestonDataCtrlColors();
                UpdateHostScreen();
                mParent.OrderCageArticles();
                mParent.UpdateQuestionDataCtrl();
                UpdateExtraQA();
                UpdateExtraTotal();
                if (GameMode == RaidTheCagesModes.SHOWTOTALAMOUNT)
                {
                    // start the polling again...
                    SendToRfid("STOPPOLLING");
                }

                if (GameMode == RaidTheCagesModes.STOPRFIDSCANNING)
                {
                    SendToRfid("WAITINGFORLISTCOMPLETE");
                }
                else
                {
                    SendToRfid("DISABLELISTCOMPLETE");
                }
            }
        }

        private void BackupSounds()
        {
            for (int i = 0; i < 100; i++)
            {
                mParent.mAudio.mSoundPlaysBU[i] = mParent.mAudio.mSoundPlays[i].Value;
            }
        }

        private void CheckSounds()
        {
            // check if we have any looping running sounds.. if not... kill all sounds!
            bool soundplaying = false;
            for (int i = 0; i < 100; i++)
            {
                if (mParent.mAudio.mSoundPlaysBU[i] == true && mParent.mAudio.mSoundPlays[i].Value == true)
                {
                    soundplaying = true;
                }
                if (mParent.mAudio.mSoundPlaysBU[i] != mParent.mAudio.mSoundPlays[i].Value)
                {
                    // we have a change in sound.... just use the undo list..
                    if (mParent.mAudio.mSoundPlays[i].Value)
                    {
                        // play
                        mParent.mAudio.SendAudio(i,
                                                 SoundCommands.PLAY, true, true);
                        soundplaying = true;
                    }
                    else
                    {
                        // stop
                        mParent.mAudio.SendAudio(i,
                                                 SoundCommands.STOP, true, true);
                    }
                }
            }
            if (!soundplaying)
            {
                mParent.KillAllAudio(false);
            }
        }

        internal void SelectAnswer(int p)
        {
            if (GameMode == RaidTheCagesModes.WAITFORANSWER)
            {
                // cool.. get the selection.. and go to show correct answer....

                overlay.Start("question.loopclock");
                projector.Start("question.loopclock");
                using (UndoRedoManager.Start("SelectAnswer"))
                {
                    overlay.AddUndo();
                    projector.AddUndo();

                    mParent.mSelectedAnswer.Value = p;

                    //Midi.SendMidi(midi,midinotes.ANSWERLOCKED);
                    CheckExtraMidiNote("Answer Locked");

                    StartAudioQuestion(false);
                    mParent.mAudio.SendAudio(SoundsRaidTheCage.LOCKINANSWER,SoundCommands.PLAY,false,false);
                    mParent.mAudio.SendAudio(SoundsRaidTheCage.LOCKINANSWERUNDERSCORE, SoundCommands.PLAY, true, false);

                    // huib jan is checking this. for now just fade in the select glow
                    //overlay.Start(string.Format("question.selectanswerloop{0}",p+1));
                    //projector.Start(string.Format("question.selectanswerloop{0}", p + 1));
                    if (mParent.mCurCountry == eCountries.HUNGARY || mParent.mCurCountry == eCountries.BRAZIL || mParent.mCurCountry == eCountries.URUGUAY2018)
                    {
                        projector.Start(string.Format("question.selectanswer{0}", p + 1));
                        overlay.Start(string.Format("question.selectanswer{0}", p + 1));
                    }
                    else
                    {
                        projector.SetOpacity(string.Format("question.select{0}", p + 1), 255);
                        overlay.SetOpacity(string.Format("question.select{0}", p + 1), 255);    
                    }
                    

                    GameMode = RaidTheCagesModes.LETSSEEIFYOUARERIGHTMUSIC;
                    UndoRedoManager.Commit();
                }

                mParent.ActiveCtrl.UpdateDlg();
                mParent.UpdateQuestonDataCtrlColors();
                UpdateHostScreen();
                UpdateExtraQA();
            }
        }

        public override void OnFalse() // use this as stop the cage timer...            
        {
            //Midi.SendMidi(midi, midinotes.STUCKINCAGE);
            CheckExtraMidiNote("Stuck in Cage");
            mParent.mAudio.SendAudio(SoundsRaidTheCage.STUCKINCAGE, SoundCommands.PLAY, false, false);

            using (UndoRedoManager.Start("questionReceived"))
            {
                overlay.AddUndo();
                projector.AddUndo();
                mParent.InitAllGraphics();
                GameMode = RaidTheCagesModes.DONE;
                UndoRedoManager.Commit();
            }
            mParent.ActiveCtrl.UpdateDlg();
            UpdateDlg(); // for host
            SendToRfid("STOPLOGFILE");
        }

        public override void OnStepForward() // use this as stop the cage timer...            
        {
            cagetimer.Stop();

            if (!doorisclosing)
            {
                SendToRfid("CLOSECAGEDOOR");
                doorisclosing = true;
            }
            using (UndoRedoManager.Start("StartNewGame"))
            {
                projector.AddUndo();
                overlay.AddUndo();
                SendToRfid("WAITINGFORLISTCOMPLETE");
                GameMode = RaidTheCagesModes.STOPRFIDSCANNING;
                UndoRedoManager.Commit();
            }
            mParent.ActiveCtrl.UpdateDlg();
            mParent.UpdateDlg();
            UpdateDlg();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        internal void ShowTotalPriceList()
        {
            if (!totalpricesshowing)
            {
                total.Show();
                totalpricesshowing = true;
            }
        }

        private void cbLifeLineUsed1_CheckedChanged(object sender, EventArgs e)
        {
            if (!mIsUpdating)
            {
                using (UndoRedoManager.StartInvisible("ChangeStatusLifeLine"))
                {


                    lLifelinesAvailable[0].Value = cbLifeLineUsed1.Checked;
                    UndoRedoManager.Commit();
                }
                
                UpdateDlg();
            }
        }

        private void cbLifeLineUsed2_CheckedChanged(object sender, EventArgs e)
        {
            if (!mIsUpdating)
            {
                using (UndoRedoManager.StartInvisible("ChangeStatusLifeLine"))
                {


                    lLifelinesAvailable[1].Value = cbLifeLineUsed2.Checked;
                    UndoRedoManager.Commit();
                }

                UpdateDlg();
            }
        }

        private void tbPriceThisQuestion_TextChanged(object sender, EventArgs e)
        {
            if (!mIsUpdating)
            {
                using (UndoRedoManager.StartInvisible("Change Price This Question"))
                {
                    long templong;
                    long.TryParse(tbPriceThisQuestion.Text, out templong);
                    mAmountThisQuestion.Value = templong;
                    UndoRedoManager.Commit();
                }
            }
        }

        private void tbPriceTotal_TextChanged(object sender, EventArgs e)
        {
            if (!mIsUpdating)
            {
                using (UndoRedoManager.StartInvisible("Change Price Total"))
                {
                    long templong;
                    long.TryParse(tbPriceTotal.Text, out templong);
                    mAmountTotal.Value = templong;
                    UndoRedoManager.Commit();
                }
            }
        }

        

        private void AudioCageTime(bool play)
        {
            switch (mParent.mQnr.Value)
            {
                case 0:
                    mParent.mAudio.SendAudio(SoundsRaidTheCage.TIMER10,
                                             play ? SoundCommands.PLAY : SoundCommands.STOP, false, false);
                    break;
                case 1:
                    mParent.mAudio.SendAudio(SoundsRaidTheCage.TIMER20,
                                             play ? SoundCommands.PLAY : SoundCommands.STOP, false, false);
                    break;
                case 2:
                    mParent.mAudio.SendAudio(SoundsRaidTheCage.TIMER30,
                                             play ? SoundCommands.PLAY : SoundCommands.STOP, false, false);
                    break;
                case 3:
                    mParent.mAudio.SendAudio(SoundsRaidTheCage.TIMER40,
                                             play ? SoundCommands.PLAY : SoundCommands.STOP, false, false);
                    break;
                case 4:
                    mParent.mAudio.SendAudio(SoundsRaidTheCage.TIMER50,
                                             play ? SoundCommands.PLAY : SoundCommands.STOP, false, false);
                    break;
                case 5:
                    mParent.mAudio.SendAudio(SoundsRaidTheCage.TIMER60,
                                             play ? SoundCommands.PLAY : SoundCommands.STOP, false, false);
                    break;
                case 6:
                    mParent.mAudio.SendAudio(SoundsRaidTheCage.TIMER70,
                                             play ? SoundCommands.PLAY : SoundCommands.STOP, false, false);
                    break;
                case 7:
                    mParent.mAudio.SendAudio(SoundsRaidTheCage.TIMER80,
                                             play ? SoundCommands.PLAY : SoundCommands.STOP, false, false);
                    break;
                case 8:
                    mParent.mAudio.SendAudio(SoundsRaidTheCage.TIMER90,
                                             play ? SoundCommands.PLAY : SoundCommands.STOP, false, false);
                    break;
                case 9:
                    mParent.mAudio.SendAudio(SoundsRaidTheCage.TIMER100,
                                             play ? SoundCommands.PLAY : SoundCommands.STOP, false, false);
                    break;
            }
        }

        private void AudioShowArticleList(bool play)
        {
            if (!play)
            {
                // stop all...
                mParent.mAudio.SendAudio(SoundsRaidTheCage.SUMMINGUPPRICESQ1,SoundCommands.STOP, true, false);
                mParent.mAudio.SendAudio(SoundsRaidTheCage.SUMMINGUPPRICESQ2Q3, SoundCommands.STOP, true, false);
                mParent.mAudio.SendAudio(SoundsRaidTheCage.SUMMINGUPPRICESQ4Q5, SoundCommands.STOP, true, false);
                mParent.mAudio.SendAudio(SoundsRaidTheCage.SUMMINGUPPRICESQ6Q7, SoundCommands.STOP, true, false);
                mParent.mAudio.SendAudio(SoundsRaidTheCage.SUMMINGUPPRICESQ8Q9, SoundCommands.STOP, true, false);
                mParent.mAudio.SendAudio(SoundsRaidTheCage.SUMMINGUPPRICESQ10, SoundCommands.STOP, true, false);
            }
            else
            {
                switch (mParent.mQnr.Value)
                {
                    case 0:
                        mParent.mAudio.SendAudio(SoundsRaidTheCage.SUMMINGUPPRICESQ1,
                                                 play ? SoundCommands.PLAY : SoundCommands.STOP, true, false);
                        break;
                    case 1:
                    case 2:
                        mParent.mAudio.SendAudio(SoundsRaidTheCage.SUMMINGUPPRICESQ2Q3,
                                                 play ? SoundCommands.PLAY : SoundCommands.STOP, true, false);
                        break;
                    case 3:
                    case 4:
                        mParent.mAudio.SendAudio(SoundsRaidTheCage.SUMMINGUPPRICESQ4Q5,
                                                 play ? SoundCommands.PLAY : SoundCommands.STOP, true, false);
                        break;
                    case 5:
                    case 6:
                        mParent.mAudio.SendAudio(SoundsRaidTheCage.SUMMINGUPPRICESQ6Q7,
                                                 play ? SoundCommands.PLAY : SoundCommands.STOP, true, false);
                        break;
                    case 7:
                    case 8:
                        mParent.mAudio.SendAudio(SoundsRaidTheCage.SUMMINGUPPRICESQ8Q9,
                                                 play ? SoundCommands.PLAY : SoundCommands.STOP, true, false);
                        break;
                    case 9:
                        mParent.mAudio.SendAudio(SoundsRaidTheCage.SUMMINGUPPRICESQ10,
                                                 play ? SoundCommands.PLAY : SoundCommands.STOP, true, false);
                        break;
                }
            }
            
        }

        private void StartLetsSeeIfYouAreRightMusic(bool play)
        {
            Debug.WriteLine(String.Format("Start-Stop: {0}",play));
            bool bLoop = (mParent.mCurCountry != eCountries.PORTUGAL);
            if (mParent.mQnr.Value < 5)
                mParent.mAudio.SendAudio(SoundsRaidTheCage.QUESTIONREVEAL1TO5, play ? SoundCommands.PLAY : SoundCommands.STOP, bLoop, false);
            else
                mParent.mAudio.SendAudio(SoundsRaidTheCage.QUESTIONREVEAL6TO9, play ? SoundCommands.PLAY : SoundCommands.STOP, bLoop, false);
        }

        private void StartAudioQuestion(bool play)
        {
            if (mParent.mCurCountry == eCountries.HUNGARY)
            {
                mParent.mAudio.SendAudio(SoundsRaidTheCage.QUESTIONSHOWWOOSH, play ? SoundCommands.PLAY : SoundCommands.STOP, false, false);
            }
            else
            {
                switch (mParent.mQnr.Value)
                {
                    case 0:
                    case 2:
                        mParent.mAudio.SendAudio(SoundsRaidTheCage.QUESTIONUNDERSCORE1AND3, play ? SoundCommands.PLAY : SoundCommands.STOP, true, false);
                        break;
                    case 1:
                    case 3:
                        mParent.mAudio.SendAudio(SoundsRaidTheCage.QUESTIONUNDERSCORE2AND4, play ? SoundCommands.PLAY : SoundCommands.STOP, true, false);
                        break;
                    case 4:
                        mParent.mAudio.SendAudio(SoundsRaidTheCage.QUESTIONUNDERSCORE5, play ? SoundCommands.PLAY : SoundCommands.STOP, true, false);
                        break;
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                        mParent.mAudio.SendAudio(SoundsRaidTheCage.QUESTIONUNDERSCORE6TO9, play ? SoundCommands.PLAY : SoundCommands.STOP, true, false);
                        break;
                    case 9:
                        mParent.mAudio.SendAudio(SoundsRaidTheCage.QUESTIONUNDERSCORE10, play ? SoundCommands.PLAY : SoundCommands.STOP, true, false);
                        break;

                }
            }
            
        }

        private void btShowMoneyTreeOnProjector_Click(object sender, EventArgs e)
        {
            ShowMoneyTree();
        }

        public void ShowMoneyTree()
        {
            // show moneytree.. small delay and show the current position.... then messagebox to remove money tree and continue with the show
            projector.Start("moneytree.init");
            projector.Start("moneytree.textbaroff");
            projector.Start("moneytree.show");

            if (lLifelinesAvailable[0].Value)
                projector.Start("moneytree.showquestionswitchlifeline");
            else
                projector.Start("moneytree.showquestionswitchlifelineused");

            if (lLifelinesAvailable[1].Value)
                projector.Start("moneytree.showpeopleswitchlifeline");
            else
                projector.Start("moneytree.showpeopleswitchlifelineused");
            // small delay... 
            //if (mParent.mQnr.Value>0)
            projector.Start(string.Format("moneytree.show{0}", mParent.mQnr), 1.0f);
            MessageBox.Show("Click to remove MoneyTree from the Projector and Continue with the show", "Info",
                            MessageBoxButtons.OK);
            if (lLifelinesAvailable[0].Value)
                projector.Start("moneytree.showquestionswitchlifelineback");
            else
                projector.Start("moneytree.showquestionswitchlifelineusedback");

            if (lLifelinesAvailable[1].Value)
                projector.Start("moneytree.showpeopleswitchlifelineback");
            else
                projector.Start("moneytree.showpeopleswitchlifelineusedback");
            projector.Start("moneytree.hide");
        }

        public void ShowMoneyTreeAll(int inLevel)
        {
            // show moneytree.. small delay and show the current position.... then messagebox to remove money tree and continue with the show
            projector.Start("moneytree.init");
            projector.Start("moneytree.textbaroff");        // this turns the time amount bar off.. why should be show it now :)
            projector.Start("moneytree.show");

            overlay.Start("moneytree.init");
            overlay.Start("moneytree.textbaroff");
            overlay.Start("moneytree.show");

            if (lLifelinesAvailable[0].Value)
            {
                projector.Start("moneytree.showquestionswitchlifeline");
                overlay.Start("moneytree.showquestionswitchlifeline");
            }
            else
            {
                overlay.Start("moneytree.showquestionswitchlifelineused");
                projector.Start("moneytree.showquestionswitchlifelineused");
            }

            if (lLifelinesAvailable[1].Value)
            {
                overlay.Start("moneytree.showpeopleswitchlifeline");
                projector.Start("moneytree.showpeopleswitchlifeline");
            }
            else
            {
                projector.Start("moneytree.showpeopleswitchlifelineused");
                overlay.Start("moneytree.showpeopleswitchlifelineused");
            }
            // small delay... 
            //if (mParent.mQnr.Value>0)
            projector.Start(string.Format("moneytree.show{0}", inLevel), 1.0f);
            overlay.Start(string.Format("moneytree.show{0}", inLevel), 1.0f);
        }

        private void ShowLifelinereminderstrap()
        {
            // todo: implement with the overlay
            bool startnewcommand = false;

            if (!UndoRedoManager.IsCommandStarted)
            {
                UndoRedoManager.Start("ShowHideLifelineReminderstrap");
                projector.AddUndo();
                overlay.AddUndo();
                startnewcommand = true;
            }

            //using (UndoRedoManager.Start("ShowHideLifelineReminderstrap"))
            //{
                

                if (!mShowLifelinereminderstrap.Value)
                {
                    // show..
                    if (lLifelinesAvailable[1].Value)
                    {

                        overlay.Start("question.showpeopleswitchlifeline");
                    }
                    else
                    {
                        overlay.Start("question.showpeopleswitchlifelineused");
                    }
                    if (lLifelinesAvailable[0].Value)
                    {
                        overlay.Start("question.showquestionswitchlifeline");
                    }
                    else
                    {
                        overlay.Start("question.showquestionswitchlifelineused");
                    }
                    mShowLifelinereminderstrap.Value = true;
                }
                else
                {
                    // hide
                    // show..
                    if (lLifelinesAvailable[1].Value)
                    {

                        overlay.Start("question.showpeopleswitchlifelineback");
                    }
                    else
                    {
                        overlay.Start("question.showpeopleswitchlifelineusedback");
                    }
                    if (lLifelinesAvailable[0].Value)
                    {
                        overlay.Start("question.showquestionswitchlifelineback");
                    }
                    else
                    {
                        overlay.Start("question.showquestionswitchlifelineusedback");
                    }
                    mShowLifelinereminderstrap.Value = false;
                //}

                  
            }
                if (startnewcommand)
                    UndoRedoManager.Commit();
        }

        private void btWWTBAMLifeLineRiminderStrap_Click(object sender, EventArgs e)
        {
            ShowLifelinereminderstrap();

            UpdateDlg();                        
        }

        private void btUseLifeLine1_Click(object sender, EventArgs e)
        {
             using (UndoRedoManager.Start("StartNewGame"))
            {
                overlay.AddUndo();
                projector.AddUndo();

                

                mParent.mAudio.SendAudio(SoundsRaidTheCage.SELECTQUESTIONSWITCHLIFELINE, SoundCommands.PLAY, false, false);

                projector.Start("question.selectswitchquestion");

                if (!mShowLifelinereminderstrap.Value)
                {
                    // show the lifeline reminder strap and small delay...
                    ShowLifelinereminderstrap();
                    System.Threading.Thread.Sleep(2000);
                }

                lLifelinesAvailable[0].Value = false;

                mUsedSwitchQuestion.Value = true;

                if (mShowLifelinereminderstrap.Value)
                    overlay.Start("question.selectswitchquestion");

                if (mParent.mSwitchAnswersWithQuestion)
                    GameMode = RaidTheCagesModes.GETQUESTIONFROMWISEQSWITCH;
                else
                    GameMode = RaidTheCagesModes.SWITCHQUESTION;
                UndoRedoManager.Commit();
            }
            mParent.ActiveCtrl.UpdateDlg();
            mParent.UpdateDlg();
            UpdateDlg();
            
        }

        private void btUseLifeLine2_Click(object sender, EventArgs e)
        {
            using (UndoRedoManager.Start("StartNewGame"))
            {
                overlay.AddUndo();
                projector.AddUndo();

                

                mParent.mAudio.SendAudio(SoundsRaidTheCage.SELECTPEOPLESWITCHLIFELINE, SoundCommands.PLAY, false, false);

                projector.Start("question.selectswitchplayer");

                if (!mShowLifelinereminderstrap.Value)
                {
                    // show the lifeline reminder strap and small delay...
                    ShowLifelinereminderstrap();
                    System.Threading.Thread.Sleep(2000);
                }

                if (mShowLifelinereminderstrap.Value)
                    overlay.Start("question.selectswitchplayer");

                lLifelinesAvailable[1].Value = false;

                mUsedSwitchPlayer.Value = true;

                GameMode = RaidTheCagesModes.ENDSWITCHPLAYER;
                UndoRedoManager.Commit();
            }
            mParent.ActiveCtrl.UpdateDlg();
            mParent.UpdateDlg();
            UpdateDlg();

            
        }

        private void ucRaidTheCage_Resize(object sender, EventArgs e)
        {
            this.Size = mParent.GetSizetcrounds();
        }
        public override void OnContinue()
        {
            bool ctrlpressed = Control.ModifierKeys == Keys.Control;

            bool canplaymidi = true;

            if (GameMode == RaidTheCagesModes.INIT)
            {
                OnStart(ctrlpressed);
            }
            else
            {
                if (continuetimer.Enabled)
                    return;
                bool commit = true;

                mLastGameModeTemp = GameMode;

                int overridedelay = -1;
                
                using (UndoRedoManager.Start("Continue"))
                {
                    overlay.AddUndo();
                    projector.AddUndo();

                    switch (GameMode)
                    {
                            /*
                        case RaidTheCagesModes.SHOWHOSTNAME:
                            if (ctrlpressed)
                            {
                                GameMode = RaidTheCagesModes.SHOWCONTESTANTSNAMES;
                            }
                            else
                            {
                                // show the host name
                                // send tot he control...
                                overlay.SetText("nametitle.barsmall.txt1", RaidTheCageData.mHosts[0]);
                                overlay.Start("nametitle.show1");
                                GameMode = RaidTheCagesModes.HIDEHOSTNAME;
                            }                            
                            break;
                        case RaidTheCagesModes.HIDEHOSTNAME:
                            overlay.Start("nametitle.hide1");
                            GameMode = RaidTheCagesModes.SHOWCONTESTANTSNAMES;
                            break;
                             */
                        case RaidTheCagesModes.STARTINTROMUSIC:
                            mParent.mAudio.SendAudio(SoundsRaidTheCage.INTRO, SoundCommands.PLAY, true, false);
                            GameMode = RaidTheCagesModes.SHOWCONTESTANTSNAMES;
                            break;
                        case RaidTheCagesModes.SHOWCONTESTANTSNAMES:
                            
                            if (ctrlpressed)
                            {
                                GameMode = RaidTheCagesModes.GOTOEXPLAINTHEGAMES;
                            }
                            else
                            {
                                // show the contestants names
                                overlay.SetText("nametitle.barbig.txt2A", RaidTheCageData.mNames[0] + " " + generalsettings.mTextbetweenNames + " " + RaidTheCageData.mNames[1]);
                                overlay.SetText("nametitle.barbig.txt2B", RaidTheCageData.mPlace);
                                overlay.Start("nametitle.show2");
                                GameMode = RaidTheCagesModes.HIDECONTESTANTSNAMES;
                            }                            
                            break;
                        case RaidTheCagesModes.HIDECONTESTANTSNAMES:
                            overlay.Start("nametitle.hide2");
                            GameMode = RaidTheCagesModes.GOTOEXPLAINTHEGAMES;
                            break;
                            /*
                        case RaidTheCagesModes.EXPLAIN3SEC:
                            if (ctrlpressed)
                            {
                                // nothing
                                canplaymidi = false;
                            }
                            else
                            {
                                // we need to start a timer... 
                                if (generalsettings.mTimebeforethedoorcloses > 3000)
                                {
                                    // we need to close the door first.. then start a timer for the rest...
                                    canplaymidi = false;
                                    // close door....
                                    SendToRfid("CLOSECAGEDOOR");

                                    m3secexplain1tmr.Interval = generalsettings.mTimebeforethedoorcloses - 3000;
                                    m3secexplain1tmr.Start();
                                }
                                else if (generalsettings.mTimebeforethedoorcloses <= 3000)
                                {
                                    // audio and midi first.. then a timer for the door if smaller then 3000
                                    mParent.mAudio.SendAudio(SoundsRaidTheCage.DOORS3SEC, SoundCommands.PLAY, false, false);
                                    m3secexplainbacktmr.Start();
                                    if (generalsettings.mTimebeforethedoorcloses == 3000)
                                    {
                                        SendToRfid("CLOSECAGEDOOR");
                                    }
                                    else
                                    {
                                        // we need to start a timer to close the door....
                                        m3secexplain2tmr.Interval = 3000 - generalsettings.mTimebeforethedoorcloses;
                                        m3secexplain2tmr.Start();
                                    }
                                }                                                                                                                                
                            }
                            GameMode = RaidTheCagesModes.GOTOEXPLAINTHEGAMES;
                            break;
                             */
                        case RaidTheCagesModes.GOTOEXPLAINTHEGAMES:
                            if (ctrlpressed)
                            {
                                canplaymidi = false;
                                overlay.Start("question.init"); // extra for incase we want to show the 2 lifeline icons..                                
                            }
                            else
                            {
                                mParent.mAudio.SendAudio(SoundsRaidTheCage.INTRO, SoundCommands.STOP, true, false);
                                canplaymidi = false;
                                // midi send start explain the games..
                                CheckMidiNote(GameMode);

                                mParent.mAudio.SendAudio(SoundsRaidTheCage.EXPLAINTHEGAMES, SoundCommands.PLAY, true, true);    // make true, because can not undo here anyway
                                ExplainTheGames exp = new ExplainTheGames(mParent);
                                exp.ShowDialog();
                                if (mParent.mCurCountry != eCountries.PORTUGAL)
                                {
                                    if (mParent.mCurCountry == eCountries.HUNGARY || mParent.mCurCountry == eCountries.BRAZIL || mParent.mCurCountry == eCountries.URUGUAY2018)
                                    {
                                        mParent.mAudio.SendAudio(SoundsRaidTheCage.EXPLAINTHEGAMES, SoundCommands.STOP, true,
                                        true,3000);
                                    }
                                    else
                                    {                                                                            
                                        mParent.mAudio.SendAudio(SoundsRaidTheCage.EXPLAINTHEGAMES, SoundCommands.STOP, true,
                                        true);
                                    }
                                }
                                overlay.Start("question.init");     // extra for incase we want to show the 2 lifeline icons..                                
                            }
                            GameMode = RaidTheCagesModes.GETQUESTIONFROMWISEQ;
                            break;
                        
                        case RaidTheCagesModes.GETQUESTIONFROMWISEQ:
                            if (ctrlpressed)
                            {
                                canplaymidi = false;
                                mParent.FillInQuestionNr(mParent.mQnr.Value);
                                mQuestionGotFromWiseQ.Value++;
                                //GameMode = RaidTheCagesModes.SHOWQUESTION;
                            }
                            else
                            {
                                mParent.GetQfromWiseQ();
                                wiseqretreive.Start();
                                mQuestionGotFromWiseQ.Value++;
                            }
                            break;
                        case RaidTheCagesModes.GETQUESTIONFROMWISEQSWITCH:
                            mParent.MCurStackType.Value = eStackTypes.Switch;
                            mParent.GetQfromWiseQ();
                            wiseqretreive.Start();
                            //mQuestionGotFromWiseQ.Value++;
                            break;
                        case RaidTheCagesModes.SHOWQUESTION:
                            
                            if (mParent.mCurCountry == eCountries.PORTUGAL)
                            {
                                mParent.mAudio.SendAudio(SoundsRaidTheCage.EXPLAINTHEGAMES, SoundCommands.STOP, true,
                                    true);
                                mParent.mAudio.SendAudio(SoundsRaidTheCage.QUITORCONTINUE, SoundCommands.STOP, true,
                                    true);
                            }
                            AudioShowArticleList(false);
                            mParent.mAudio.SendAudio(SoundsRaidTheCage.INTRO, SoundCommands.STOP, true, false);
                            mParent.MakeDBQuestionUsed(mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mID);
                            //Midi.SendMidi(midi, midinotes.SHOWQUESTION);
                            StartAudioQuestion(true);
                            projector.Start("question.init");
                            overlay.Start("question.init");
                            SendQuestionDataToEngines();
                            ShowQuestionEngine(eEngines.OVERLAY);
                            ShowQuestionEngine(eEngines.PROJECTOR);
                            // + show clock on projector allready...
                            if (mParent.mQnr.Value < 9)
                            {
                                SetCageTime(((mParent.mQnr.Value + 1)*10)*1000);
                                projector.Start("question.showclock");
                                overlay.Start("question.showclock");
                            }
                            else
                            {
                                overlay.Start("question.showrtclogo");
                                projector.Start("question.showrtclogo");
                            }

                            if (lLifelinesAvailable[1].Value)
                            {
                                projector.Start("question.showpeopleswitchlifeline");
                                //overlay.Start("question.showpeopleswitchlifeline");
                            }
                            else
                            {
                                projector.Start("question.showpeopleswitchlifelineused");
                                //overlay.Start("question.showpeopleswitchlifelineused");
                            }
                            if (lLifelinesAvailable[0].Value)
                            {
                                projector.Start("question.showquestionswitchlifeline");
                                //overlay.Start("question.showquestionswitchlifeline");
                            }
                            else
                            {
                                projector.Start("question.showquestionswitchlifelineused");
                                //overlay.Start("question.showquestionswitchlifelineused");
                            }
                            GameMode = RaidTheCagesModes.SHOWANSWER1;
                            break;
                        case RaidTheCagesModes.SHOWANSWER1:
                            mParent.mAudio.SendAudio(SoundsRaidTheCage.SHOWANSWER1, SoundCommands.PLAY, false, false);
                            ShowAnswerEngine(0, eEngines.OVERLAY);
                            ShowAnswerEngine(0, eEngines.PROJECTOR);
                            GameMode = RaidTheCagesModes.SHOWANSWER2;
                            break;
                        case RaidTheCagesModes.SHOWANSWER2:
                            mParent.mAudio.SendAudio(SoundsRaidTheCage.SHOWANSWER2, SoundCommands.PLAY, false, false);
                            ShowAnswerEngine(1, eEngines.OVERLAY);
                            ShowAnswerEngine(1, eEngines.PROJECTOR);
                            GameMode = RaidTheCagesModes.SHOWANSWER3;
                            break;
                        case RaidTheCagesModes.SHOWANSWER3:
                            mParent.mAudio.SendAudio(SoundsRaidTheCage.SHOWANSWER3, SoundCommands.PLAY, false, false);
                            ShowAnswerEngine(2, eEngines.OVERLAY);
                            ShowAnswerEngine(2, eEngines.PROJECTOR);
                            GameMode = RaidTheCagesModes.SHOWANSWER4;
                            break;
                        case RaidTheCagesModes.SHOWANSWER4:
                            mParent.mAudio.SendAudio(SoundsRaidTheCage.SHOWANSWER4, SoundCommands.PLAY, false, false);
                            ShowAnswerEngine(3, eEngines.OVERLAY);
                            ShowAnswerEngine(3, eEngines.PROJECTOR);
                            GameMode = RaidTheCagesModes.WAITFORANSWER;
                            break;
                        case RaidTheCagesModes.ENDSWITCHPLAYER:
                            projector.Start("question.endswitchplayer");
                            overlay.Start("question.endswitchplayer");
                            GameMode = RaidTheCagesModes.WAITFORANSWER;
                            break;
                        case RaidTheCagesModes.SHOWRIGHTANSWERBEFORESWITCH:

                            // show the right answer highlite.. before we switch the question and answers....
                            overlay.Start(string.Format("question.correctanswer{0}", mParent.lAllQuestions[0][mParent.mQnr.Value].mRightAnswer));
                            projector.Start(string.Format("question.correctanswer{0}", mParent.lAllQuestions[0][mParent.mQnr.Value].mRightAnswer));

                            GameMode = RaidTheCagesModes.SWITCHQUESTION;
                            break;
                        case RaidTheCagesModes.SWITCHQUESTION:
                            mParent.mAudio.SendAudio(SoundsRaidTheCage.SWITCHQUESTION, SoundCommands.PLAY, false, false);
                            overlay.Start("question.correctanswerresetdelay");
                            projector.Start("question.correctanswerresetdelay");
                            if (mParent.mSwitchAnswersWithQuestion)
                            {
                                mParent.MakeDBQuestionUsed(mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mID);
                                // send the new question from the switch stack to the question.
                                projector.SetText("question.question2", mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mQuestion.Replace("|", "¬"));
                                overlay.SetText("question.question2", mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mQuestion.Replace("|", "¬"));

                                // start switch answer timer..
                                mAnswerSwitchTimer.Start();
                            }
                            else
                            {
                                //mParent.mFirstQuestion.Value = false;
                            }

                            projector.Start("question.switchquestion");
                            overlay.Start("question.switchquestion");
                            projector.Start("question.endswitchquestion", 1.5f);
                            overlay.Start("question.endswitchquestion", 1.5f);
                            mParent.SwitchQuestionCtrl();

                            if (mParent.mSwitchAnswersWithQuestion)
                            {
                                GameMode = RaidTheCagesModes.SHOWANSWER1;
                            }
                            else
                                GameMode = RaidTheCagesModes.WAITFORANSWER;
                            break;
                        case RaidTheCagesModes.LETSSEEIFYOUARERIGHTMUSIC:
                            //Midi.SendMidi(midi, midinotes.LETSSEECORRECT);
                            mParent.mAudio.SendAudio(SoundsRaidTheCage.LOCKINANSWERUNDERSCORE, SoundCommands.STOP, true, false);
                            StartLetsSeeIfYouAreRightMusic(true);
                            GameMode = RaidTheCagesModes.SHOWRIGHTANSWER;
                            break;
                        case RaidTheCagesModes.SHOWRIGHTANSWER:
                            StartLetsSeeIfYouAreRightMusic(false);                                                        
                            ShowRightAnswerEngine(eEngines.OVERLAY);
                            ShowRightAnswerEngine(eEngines.PROJECTOR);
                            
                            if (checkanswer())
                            {
                                if (mParent.mQnr.Value == 9)
                                {
                                    if (mLogingOn)
                                    {
                                        // still add to the logfile and write..
                                        mLogFile.FillInLogData((int) mParent.MCurStackType.Value, mParent.mQnr.Value,
                                            mParent.lAllQuestions[(int) mParent.MCurStackType.Value][mParent.mQnr.Value]
                                                .mRef,
                                            mParent.lAllQuestions[(int) mParent.MCurStackType.Value][mParent.mQnr.Value]
                                                .mQuestion,
                                            mParent.lAllQuestions[(int) mParent.MCurStackType.Value][mParent.mQnr.Value]
                                                .mAnswers[0],
                                            mParent.lAllQuestions[(int) mParent.MCurStackType.Value][mParent.mQnr.Value]
                                                .mAnswers[1],
                                            mParent.lAllQuestions[(int) mParent.MCurStackType.Value][mParent.mQnr.Value]
                                                .mAnswers[2],
                                            mParent.lAllQuestions[(int) mParent.MCurStackType.Value][mParent.mQnr.Value]
                                                .mAnswers[3],
                                            mParent.lAllQuestions[(int) mParent.MCurStackType.Value][mParent.mQnr.Value]
                                                .mRightAnswer,
                                            mParent.lAllQuestions[(int) mParent.MCurStackType.Value][mParent.mQnr.Value]
                                                .mPronounciation,
                                            mParent.lAllQuestions[(int) mParent.MCurStackType.Value][mParent.mQnr.Value]
                                                .mExplanation,
                                            mParent.mSelectedAnswer.Value, mUsedSwitchPlayer.Value,
                                            mUsedSwitchQuestion.Value, null, mAmountThisQuestion.Value,
                                            mAmountTotal.Value,
                                            mParent.lAllQuestions[(int) mParent.MCurStackType.Value][mParent.mQnr.Value]
                                                .mQuestion2,
                                            mParent.lAllQuestions[(int) mParent.MCurStackType.Value][mParent.mQnr.Value]
                                                .mRightAnswer2, false);

                                        if (mUsedSwitchQuestion.Value && mParent.mSwitchAnswersWithQuestion)
                                        {
                                            // also save the old question for printing...
                                            mLogFile.FillInLogData(0, mParent.mQnr.Value,
                                                mParent.lAllQuestions[0][mParent.mQnr.Value].mRef,
                                                mParent.lAllQuestions[0][mParent.mQnr.Value].mQuestion,
                                                mParent.lAllQuestions[0][mParent.mQnr.Value].mAnswers[0],
                                                mParent.lAllQuestions[0][mParent.mQnr.Value].mAnswers[1],
                                                mParent.lAllQuestions[0][mParent.mQnr.Value].mAnswers[2],
                                                mParent.lAllQuestions[0][mParent.mQnr.Value].mAnswers[3],
                                                mParent.lAllQuestions[0][mParent.mQnr.Value].mRightAnswer,
                                                mParent.lAllQuestions[0][mParent.mQnr.Value].mPronounciation,
                                                mParent.lAllQuestions[0][mParent.mQnr.Value].mExplanation,
                                                mParent.mSelectedAnswer.Value, mUsedSwitchPlayer.Value,
                                                mUsedSwitchQuestion.Value, null, mAmountThisQuestion.Value,
                                                mAmountTotal.Value,
                                                mParent.lAllQuestions[0][mParent.mQnr.Value].mQuestion2,
                                                mParent.lAllQuestions[0][mParent.mQnr.Value].mRightAnswer2, false);
                                        }

                                        mLogFile.PrintCompleteFile(mParent.mQnr.Value);
                                    }
                                }
                                // correct!
                                //StartLetsSeeIfYouAreRightMusic(false);                                                        
                                mParent.mAudio.SendAudio(SoundsRaidTheCage.RIGHTANSWER, SoundCommands.PLAY, false, false);
                                CheckExtraMidiNote("Answer Correct");
                                //Midi.SendMidi(midi, midinotes.ANSWERCORRECT);
                                if (mParent.mQnr.Value < 9)
                                {
                                    GameMode = RaidTheCagesModes.REMOVEQUESTION;
                                }
                                else
                                {
                                    GameMode = RaidTheCagesModes.REMOVEQUESTIONFINALANSWER;
                                }
                            }
                            else
                            {
                                // wrong
                                //StartLetsSeeIfYouAreRightMusic(false);  
                                mParent.mAudio.SendAudio(SoundsRaidTheCage.WRONGANSWER, SoundCommands.PLAY, false, false);
                                SendToRfid("STOPLOGFILE");

                                if (mLogingOn)
                                {
                                    // still add to the logfile and write..
                                    mLogFile.FillInLogData((int) mParent.MCurStackType.Value, mParent.mQnr.Value,
                                        mParent.lAllQuestions[(int) mParent.MCurStackType.Value][mParent.mQnr.Value]
                                            .mRef,
                                        mParent.lAllQuestions[(int) mParent.MCurStackType.Value][mParent.mQnr.Value]
                                            .mQuestion,
                                        mParent.lAllQuestions[(int) mParent.MCurStackType.Value][mParent.mQnr.Value]
                                            .mAnswers[0],
                                        mParent.lAllQuestions[(int) mParent.MCurStackType.Value][mParent.mQnr.Value]
                                            .mAnswers[1],
                                        mParent.lAllQuestions[(int) mParent.MCurStackType.Value][mParent.mQnr.Value]
                                            .mAnswers[2],
                                        mParent.lAllQuestions[(int) mParent.MCurStackType.Value][mParent.mQnr.Value]
                                            .mAnswers[3],
                                        mParent.lAllQuestions[(int) mParent.MCurStackType.Value][mParent.mQnr.Value]
                                            .mRightAnswer,
                                        mParent.lAllQuestions[(int) mParent.MCurStackType.Value][mParent.mQnr.Value]
                                            .mPronounciation,
                                        mParent.lAllQuestions[(int) mParent.MCurStackType.Value][mParent.mQnr.Value]
                                            .mExplanation,
                                        mParent.mSelectedAnswer.Value, mUsedSwitchPlayer.Value,
                                        mUsedSwitchQuestion.Value, null, mAmountThisQuestion.Value, mAmountTotal.Value,
                                        mParent.lAllQuestions[(int) mParent.MCurStackType.Value][mParent.mQnr.Value]
                                            .mQuestion2,
                                        mParent.lAllQuestions[(int) mParent.MCurStackType.Value][mParent.mQnr.Value]
                                            .mRightAnswer2, false);

                                    if (mUsedSwitchQuestion.Value && mParent.mSwitchAnswersWithQuestion)
                                    {
                                        // also save the old question for printing...
                                        mLogFile.FillInLogData(0, mParent.mQnr.Value,
                                            mParent.lAllQuestions[0][mParent.mQnr.Value].mRef,
                                            mParent.lAllQuestions[0][mParent.mQnr.Value].mQuestion,
                                            mParent.lAllQuestions[0][mParent.mQnr.Value].mAnswers[0],
                                            mParent.lAllQuestions[0][mParent.mQnr.Value].mAnswers[1],
                                            mParent.lAllQuestions[0][mParent.mQnr.Value].mAnswers[2],
                                            mParent.lAllQuestions[0][mParent.mQnr.Value].mAnswers[3],
                                            mParent.lAllQuestions[0][mParent.mQnr.Value].mRightAnswer,
                                            mParent.lAllQuestions[0][mParent.mQnr.Value].mPronounciation,
                                            mParent.lAllQuestions[0][mParent.mQnr.Value].mExplanation,
                                            mParent.mSelectedAnswer.Value, mUsedSwitchPlayer.Value,
                                            mUsedSwitchQuestion.Value, null, mAmountThisQuestion.Value,
                                            mAmountTotal.Value,
                                            mParent.lAllQuestions[0][mParent.mQnr.Value].mQuestion2,
                                            mParent.lAllQuestions[0][mParent.mQnr.Value].mRightAnswer2, false);
                                    }

                                    mLogFile.PrintCompleteFile(mParent.mQnr.Value);
                                }
                                //Midi.SendMidi(midi, midinotes.ANSWERWRONG);
                                CheckExtraMidiNote("Answer Wrong");
                                GameMode = RaidTheCagesModes.DONECLEARSCREENS;
                            }

                            break;                        
                        case RaidTheCagesModes.REMOVEQUESTION:
                            projector.Start("question.hidequestion");                            
                            projector.Start("question.hideclock");
                            overlay.Start("question.hidequestion");
                            if (mParent.mCurCountry == eCountries.HUNGARY || mParent.mCurCountry == eCountries.BRAZIL || mParent.mCurCountry == eCountries.URUGUAY2018)
                            {
                                // extra anim to show logo again.. with animation...
                                projector.Start("question.hidequestionshowlogo");      
                            }
                            else
                            {
                                projector.SetOpacity("logo", 255, 0.4f, 1f);
                                projector.SetOpacity("logo3d", 255, 0.4f, 1f);    
                            }
                            
                            if (lLifelinesAvailable[1].Value)
                            {
                                projector.Start("question.showpeopleswitchlifelineback");                                
                            }
                            else
                            {
                                projector.Start("question.showpeopleswitchlifelineusedback");                                
                            }
                            if (lLifelinesAvailable[0].Value)
                            {
                                projector.Start("question.showquestionswitchlifelineback");                                
                            }
                            else
                            {
                                projector.Start("question.showquestionswitchlifelineusedback");                                
                            }
                            if (mShowLifelinereminderstrap.Value)
                            {                                
                                if (lLifelinesAvailable[1].Value)
                                {
                                    //projector.Start("question.showpeopleswitchlifelineback");
                                    overlay.Start("question.showpeopleswitchlifelineback");
                                }
                                else
                                {
                                    //projector.Start("question.showpeopleswitchlifelineusedback");
                                    overlay.Start("question.showpeopleswitchlifelineusedback");
                                }
                                if (lLifelinesAvailable[0].Value)
                                {
                                    //projector.Start("question.showquestionswitchlifelineback");
                                    overlay.Start("question.showquestionswitchlifelineback");
                                }
                                else
                                {
                                    //projector.Start("question.showquestionswitchlifelineusedback");
                                    overlay.Start("question.showquestionswitchlifelineusedback");
                                }
                                mShowLifelinereminderstrap.Value = false;                                
                            }
                            //GameMode = RaidTheCagesModes.STARTCAGETIME;
                            //break;
                        //case RaidTheCagesModes.SHOWCAGETIME:  // overlay only // question out...  // not use anymore! Allready on screen
                        //    SetCageTime(((mParent.mQnr.Value + 1) * 10) * 1000);
                        //    overlay.Start("question.showclock");

                            
                        //    projector.Start("question.hidequestion");
                        //    overlay.Start("question.hidequestion");
                        //    GameMode = RaidTheCagesModes.STARTCAGETIME;
                        //    break;
                        //case RaidTheCagesModes.STARTCAGETIME:

                            //Midi.SendMidi(midi, midinotes.OPENCAGEDOORS);
                            
                            AudioCageTime(true);
                            SetCageTime(((mParent.mQnr.Value + 1) * 10) * 1000);

                            if (mShowLifelinereminderstrap.Value)
                            {
                                if (lLifelinesAvailable[1].Value)
                                {
                                    //projector.Start("question.showpeopleswitchlifelineback");
                                    overlay.Start("question.showpeopleswitchlifelineback");
                                }
                                else
                                {
                                    //projector.Start("question.showpeopleswitchlifelineusedback");
                                    overlay.Start("question.showpeopleswitchlifelineusedback");
                                }
                                if (lLifelinesAvailable[0].Value)
                                {
                                    //projector.Start("question.showquestionswitchlifelineback");
                                    overlay.Start("question.showquestionswitchlifelineback");
                                }
                                else
                                {
                                    //projector.Start("question.showquestionswitchlifelineusedback");
                                    overlay.Start("question.showquestionswitchlifelineusedback");
                                }
                            }
                            //projector.Start("question.hidequestion");
                            //overlay.Start("question.hidequestion");

                            startpolling = false;
                            doorisclosing = false;
                            doorisclosing2 = false;
                            casetimervalue = ((mParent.mQnr.Value + 1) * 10);
                            mStartTimeAfterDoorOpensTimer.Interval = generalsettings.mTimebeforettimerstart == 0
                                                                         ? 1
                                                                         : generalsettings.mTimebeforettimerstart;
                            mStartTimeAfterDoorOpensTimer.Start();
                           
                            mParent.litemscurquestionUndo.Clear();
                            SendToRfid("CLEARLISTCURRENTQUESTION");                            
                            SendToRfid("OPENCAGEDOOR");                                 
                            GameMode = RaidTheCagesModes.CAGETIMERUNNING;
                            break;
                        case RaidTheCagesModes.REMOVEQUESTIONFINALANSWER:
                            mParent.mAudio.SendAudio(SoundsRaidTheCage.FINALANSWERCORRECT,SoundCommands.PLAY, false,false);
                            overlay.Start("question.hidequestionwronganswer");
                            projector.Start("initall");

                            if (mParent.mQnr.Value == 9)
                            {
                                overlay.Start("question.hidertclogo");
                            }
                            if (mParent.mCurCountry != eCountries.PARAQUAY)
                            {
                                projector.LoadScreen("mainscreen");
                                projector.Start("logoloop.hidelogo");
                                mParent.mLogoProjector = false;
                            }
                            GameMode = RaidTheCagesModes.REMOVEQUESTIONFINALANSWER2;
                            break;
                        case RaidTheCagesModes.REMOVEQUESTIONFINALANSWER2:
                            mParent.mAudio.SendAudio(SoundsRaidTheCage.FINALANSWERCORRECT, SoundCommands.STOP, false, false);
                            // stop music...
                            GameMode = RaidTheCagesModes.DONE;
                            break;
                        case RaidTheCagesModes.CAGETIMERUNNING:
                            // close doors... stop timer..... hide time..
                            mStartTimeAfterDoorOpensTimer.Stop();
                            //Midi.SendMidi(midi, midinotes.CLOSEDOORMANUAL);
                            overlay.Start("question.stoploopclock");
                            //projector.Start("question.stoploopclock");
                            AudioCageTime(false);
                            SendToRfid("STARTPOLLING");
                            SendToRfid("CLOSECAGEDOOR");
                            SendToRfid("PORT8LOW");
                            doorisclosing = true;
                            doorisclosing2 = true;
                            cagetimer.Stop();

                            mParent.mAudio.SendAudio(SoundsRaidTheCage.MANUALSTOPCAGETIME,SoundCommands.PLAY,false,false);

                            string hostsendstring = string.Format("Cage Time Left: {0} ", timestringctrl);

                            string tosend = hostsendstring;

                            tosend += '\r';
                            tosend += '\n';
                            // patrick
                            byte[] sendbytes = Encoding.UTF8.GetBytes(tosend);

                          
                                try
                                {
                                    if (mParent.ipdaemonHostMessaging.IsConnected)
                                        mParent.ipdaemonHostMessaging.Write(sendbytes);                                    
                                }
                                catch (Exception)
                                {


                                }

                           

                            /*
                            foreach (Connection c in mParent.ipdaemonHostMessaging.Connections.Values)
                            {
                                //c.DataToSend = "Broadcast Data";
                                //MessageBox.Show("Sending GetVotes now!");
                                try
                                {
                                    mParent.ipdaemonHostMessaging.SendLine(c.ConnectionId, hostsendstring);
                                }
                                catch (Exception)
                                {
                                    // do nothing...


                                }
                            }
                             */
                            //projector.Start("question.hidelock");
                            //overlay.Start("question.hidelock");
                            GameMode = RaidTheCagesModes.HIDECLOCK;
                            break;
                        case RaidTheCagesModes.HIDECLOCK:
                            AudioShowArticleList(true);
                            overlay.Start("question.hideclock");
                            //projector.SetOpacity("logo", 255, 0.2f, 0.5f);
                            SendToRfid("WAITINGFORLISTCOMPLETE");
                            GameMode = RaidTheCagesModes.STOPRFIDSCANNING;
                            break;
                        case RaidTheCagesModes.STOPRFIDSCANNING:
                           // projector.Start("question.hideclock");
                            //overlay.Start("question.hideclock");
                            SendToRfid("DISABLELISTCOMPLETE");
                            mAmountThisQuestion.Value = mParent.CalculatTotalAmountThisQuestion();

                            SendToRfid("STOPPOLLING");

                            //
                            //UpdateDlg();

                            dgv = mParent.GetDataGridview();

                            // this might be a good time to safe the logfile.. as we go to the next question anyway.. so we have all the info we need...
                            if (mLogingOn)
                            {
                                string[,] articles = new string[20,2];

                                for (int i = 0; i < 20;i++ )
                                {
                                    if (dgv.Rows.Count > i)
                                    {
                                        articles[i, 0] = dgv.Rows[i].Cells[0].Value.ToString();
                                        articles[i, 1] = dgv.Rows[i].Cells[1].Value.ToString();
                                    }
                                    else
                                    {
                                        articles[i, 0] = "";
                                        articles[i, 1] = "";
                                    }
                                }



                                mLogFile.FillInLogData((int)mParent.MCurStackType.Value,mParent.mQnr.Value, mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mRef,
                                        mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mQuestion,
                                        mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[0],
                                        mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[1],
                                        mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[2],
                                        mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[3],
                                        mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mRightAnswer,
                                        mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mPronounciation,
                                        mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mExplanation,
                                        mParent.mSelectedAnswer.Value, mUsedSwitchPlayer.Value, mUsedSwitchQuestion.Value, articles,mAmountThisQuestion.Value,mAmountTotal.Value,
                                        mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mQuestion2,
                                        mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mRightAnswer2,false);

                                if (mUsedSwitchQuestion.Value && mParent.mSwitchAnswersWithQuestion)
                                {
                                    // also save the old question for printing...
                                    mLogFile.FillInLogData(0,mParent.mQnr.Value, mParent.lAllQuestions[0][mParent.mQnr.Value].mRef,
                                      mParent.lAllQuestions[0][mParent.mQnr.Value].mQuestion,
                                      mParent.lAllQuestions[0][mParent.mQnr.Value].mAnswers[0],
                                      mParent.lAllQuestions[0][mParent.mQnr.Value].mAnswers[1],
                                      mParent.lAllQuestions[0][mParent.mQnr.Value].mAnswers[2],
                                      mParent.lAllQuestions[0][mParent.mQnr.Value].mAnswers[3],
                                      mParent.lAllQuestions[0][mParent.mQnr.Value].mRightAnswer,
                                      mParent.lAllQuestions[0][mParent.mQnr.Value].mPronounciation,
                                      mParent.lAllQuestions[0][mParent.mQnr.Value].mExplanation,
                                      mParent.mSelectedAnswer.Value, mUsedSwitchPlayer.Value, mUsedSwitchQuestion.Value, articles, mAmountThisQuestion.Value, mAmountTotal.Value,
                                      mParent.lAllQuestions[0][mParent.mQnr.Value].mQuestion2,
                                      mParent.lAllQuestions[0][mParent.mQnr.Value].mRightAnswer2,false);
                                }

                                mLogFile.PrintCompleteFile(mParent.mQnr.Value);
                            }
                            mAmountTotal.Value += mAmountThisQuestion.Value;

                            if (dgv.Rows.Count > 0 && !mParent.mAlwaysskiparticlelist)
                                GameMode = RaidTheCagesModes.SHOWARTICLELIST;
                            else if (dgv.Rows.Count > 0 && mParent.mQnr.Value == 0)
                                GameMode = RaidTheCagesModes.SHOWTOTALAMOUNT; // no items to display
                            else
                            {
                                if (mParent.mCurCountry == eCountries.PORTUGAL && mParent.mQnr.Value == 0)
                                    GameMode = RaidTheCagesModes.SHOWTOTALAMOUNTTHISQUESTION; // no items to display
                                else if (mParent.mCurCountry == eCountries.PORTUGAL)
                                    GameMode = RaidTheCagesModes.SHOWTOTALAMOUNT; // no items to display
                                else
                                    GameMode = RaidTheCagesModes.SHOWTOTALAMOUNTTHISQUESTION; // no items to display
                            }
                            break;
                        case RaidTheCagesModes.SHOWARTICLELIST:                                               
                            if (ctrlpressed)
                            {
                                canplaymidi = false;
                                GameMode = RaidTheCagesModes.SHOWTOTALAMOUNTTHISQUESTION;
                            }
                            else
                            {
                                SendDataToArticleList();
                                projector.Start("pricelist.init");
                                overlay.Start("pricelist.init");
                                projector.Start("pricelist.show");
                                overlay.Start("pricelist.show");
                                mcurrentarticlelist.Value = 0;
                                if (mParent.mCurCountry == eCountries.ARGENTINA || mParent.mCurCountry == eCountries.UK || mParent.mCurCountry == eCountries.URUGUAY || mParent.mCurCountry == eCountries.PERU || mParent.mCurCountry == eCountries.MEXICO || mParent.mCurCountry == eCountries.COLUMBIA || mParent.mCurCountry == eCountries.VIETNAM || mParent.mCurCountry == eCountries.PARAQUAY || mParent.mCurCountry == eCountries.PORTUGAL || mParent.mCurCountry == eCountries.HUNGARY || mParent.mCurCountry == eCountries.BRAZIL || mParent.mCurCountry == eCountries.URUGUAY2018)
                                {                                               
                                   GameMode = RaidTheCagesModes.SHOWINGARTICLES;
                                }
                                else
                                    GameMode = RaidTheCagesModes.SHOWARTICLE;
                                
                            }                           
                            break;
                        case RaidTheCagesModes.SHOWARTICLE:
                            mParent.mAudio.SendAudio(SoundsRaidTheCage.SHOWITEM, SoundCommands.PLAY, false, false);
                            ShowArticle(mcurrentarticlelist.Value);
                            mcurrentarticlelist.Value++;
                            if (mcurrentarticlelist.Value >= dgv.Rows.Count)
                            {
                                GameMode = RaidTheCagesModes.HIDEARTICLELIST;
                            }
                            break;
                        case RaidTheCagesModes.HIDEARTICLELIST:
                           
                            projector.Start("pricelist.hide");
                            overlay.Start("pricelist.hide");
                            // set total amount
                            if (mParent.mCurCountry == eCountries.PORTUGAL)
                                GameMode = RaidTheCagesModes.SHOWTOTALAMOUNT;
                            else
                                GameMode = RaidTheCagesModes.SHOWTOTALAMOUNTTHISQUESTION;
                            break;
                        case RaidTheCagesModes.SHOWTOTALAMOUNTTHISQUESTION:
                            if (ctrlpressed)
                            {
                                canplaymidi = false;
                                mAmountThisQuestion.Value = mParent.CalculatTotalAmountThisQuestion();  // in case we did an undo and then continue with continue                               
                                 if (mParent.mQnr.Value > 0)
                                     GameMode = RaidTheCagesModes.SHOWTOTALAMOUNT;
                                 else
                                 {
                                     //mAmountTotal.Value += mAmountThisQuestion.Value;
                                     if (mParent.mQnr.Value < 9)
                                     {
                                         if (mParent.mQnr.Value > 3)
                                         {
                                             if (mParent.mQnr.Value == 4)
                                                 GameMode = RaidTheCagesModes.SHOWMONEYTREE;
                                             else
                                                 GameMode = RaidTheCagesModes.PLAYORSTOP;
                                         }
                                         else
                                             GameMode = RaidTheCagesModes.NEXTQUESTION;
                                     }
                                 }
                            }
                            else
                            {
                                //mAmountThisQuestion.Value = mParent.CalculatTotalAmountThisQuestion();  // in case we did an undo and then continue with continue
                                SetTotalAmount(mAmountThisQuestion.Value);
                                mParent.mAudio.SendAudio(SoundsRaidTheCage.SHOWTOTALAMOUNTTHISQUESTION, SoundCommands.PLAY, false, false);
                                if ((mParent.mCurCountry == eCountries.HUNGARY || mParent.mCurCountry == eCountries.BRAZIL) && mParent.mQnr.Value == 0)
                                {
                                    projector.Start("totalamount.showamounttotal");
                                    overlay.Start("totalamount.showamounttotal"); 
                                }
                                else
                                {
                                    projector.Start("totalamount.showamountcurquestiontotal");
                                    overlay.Start("totalamount.showamountcurquestiontotal");    
                                }
                                
                                if (mParent.mQnr.Value > 0)
                                {
                                    if (mParent.mCurCountry == eCountries.ARGENTINA || mParent.mCurCountry == eCountries.UK || mParent.mCurCountry == eCountries.URUGUAY || mParent.mCurCountry == eCountries.PERU || mParent.mCurCountry == eCountries.MEXICO || mParent.mCurCountry == eCountries.COLUMBIA || mParent.mCurCountry == eCountries.VIETNAM || mParent.mCurCountry == eCountries.PARAQUAY || mParent.mCurCountry == eCountries.PORTUGAL || mParent.mCurCountry == eCountries.HUNGARY || mParent.mCurCountry == eCountries.BRAZIL || mParent.mCurCountry == eCountries.URUGUAY2018)
                                        GameMode = RaidTheCagesModes.HIDETOTALAMOUNT1;
                                    else
                                        GameMode = RaidTheCagesModes.SHOWTOTALAMOUNT;
                                }
                                else
                                {
                                    GameMode = RaidTheCagesModes.HIDETOTALAMOUNT;
                                    //mAmountTotal.Value += mAmountThisQuestion.Value;
                                }
                            }                           
                            break;
                        case RaidTheCagesModes.SHOWTOTALAMOUNT:                            
                            if (ctrlpressed && mParent.mCurCountry == eCountries.UK)
                            {
                                canplaymidi = false;
                                //mAmountTotal.Value += mAmountThisQuestion.Value;
                                if (mParent.mQnr.Value < 9)
                                {
                                    if (mParent.mQnr.Value > 3)
                                    {
                                        if (mParent.mQnr.Value == 4)
                                            GameMode = RaidTheCagesModes.SHOWMONEYTREE;
                                        else
                                            GameMode = RaidTheCagesModes.PLAYORSTOP;
                                    }
                                    else
                                        GameMode = RaidTheCagesModes.NEXTQUESTION;
                                }
                            }
                            else                            
                            {
                                mParent.mAudio.SendAudio(SoundsRaidTheCage.SHOWTOTAL, SoundCommands.PLAY, false, false);
                                // first init the totalamount and then send the new amount anim and start.. should be quick enough :)
                                projector.Start("totalamount.init");
                                overlay.Start("totalamount.init");
                                //mAmountTotal.Value += mAmountThisQuestion.Value;
                                SetTotalAmount(mAmountTotal.Value);
                                projector.Start("totalamount.showamounttotal");
                                overlay.Start("totalamount.showamounttotal");
                                GameMode = RaidTheCagesModes.HIDETOTALAMOUNT;
                            }
                            break;
                        case RaidTheCagesModes.HIDETOTALAMOUNT1:
                           //AudioShowArticleList(false);
                            projector.Start("totalamount.hide");
                            overlay.Start("totalamount.hide");
                           
                            GameMode = RaidTheCagesModes.SHOWTOTALAMOUNT;
                            break;
                        case RaidTheCagesModes.HIDETOTALAMOUNT:
                            if (mParent.mCurCountry != eCountries.PORTUGAL)
                                AudioShowArticleList(false);
                            projector.Start("totalamount.hide");
                            overlay.Start("totalamount.hide");
                            if (mParent.mQnr.Value < 9)
                            {
                                if (mParent.mQnr.Value > 3)
                                {
                                    //if (mParent.mQnr.Value == 4)
                                    //    GameMode = RaidTheCagesModes.SHOWMONEYTREE;
                                    //else
                                    GameMode = RaidTheCagesModes.PLAYORSTOP;
                                }
                                else
                                    GameMode = RaidTheCagesModes.NEXTQUESTION;
                            }
                            break;
                        case RaidTheCagesModes.SHOWMONEYTREE:
                            if (ctrlpressed)
                            {
                                canplaymidi = false;                    
                                GameMode = RaidTheCagesModes.PLAYORSTOP;
                            }
                            else
                            {
                                // ok.. init first...
                                overlay.Start("moneytree.init");
                                overlay.Start("moneytree.show");

                                if (mLifeLineAvailable1.Value)
                                {
                                    overlay.Play("moneytree.switchquestionanim", 0, 99, 25, false, 1);
                                }
                                else
                                {
                                    overlay.Play("moneytree.switchquestionanimused", 0, 99, 25, false, 1);
                                }

                                if (mLifeLineAvailable2.Value)
                                {
                                    overlay.Play("moneytree.switchpeopleanim", 0, 99, 25, false, 1);
                                }
                                else
                                {
                                    overlay.Play("moneytree.switchpeopleanimused", 0, 99, 25, false, 1);
                                }
                                System.Threading.Thread.Sleep(750);
                                
                                overlay.Start("moneytree.show6");
                                GameMode = RaidTheCagesModes.HIDEMONEYTREE;
                            }
                            break;
                        case RaidTheCagesModes.HIDEMONEYTREE:
                            if (mLifeLineAvailable1.Value)
                            {
                                overlay.Play("moneytree.switchquestionanim", 0, 99, -25, false, 1);
                            }
                            else
                            {
                                overlay.Play("moneytree.switchquestionanimused", 0, 99, -25, false, 1);
                            }

                            if (mLifeLineAvailable2.Value)
                            {
                                overlay.Play("moneytree.switchpeopleanim", 0, 99, -25, false, 1);
                            }
                            else
                            {
                                overlay.Play("moneytree.switchpeopleanimused", 0, 99, -25, false, 1);
                            }
                            overlay.Start("moneytree.hide");

                            GameMode = RaidTheCagesModes.PLAYORSTOP;
                            break;
                        case RaidTheCagesModes.PLAYORSTOP:
                            AudioShowArticleList(false);
                            mParent.mAudio.SendAudio(SoundsRaidTheCage.QUITORCONTINUE,SoundCommands.PLAY,true,true);
                            
                            ContinueOrStop dlg = new ContinueOrStop(mParent,this);

                            canplaymidi = false;
                                // midi send start explain the games..
                                CheckMidiNote(GameMode);

                            if (dlg.ShowDialog() == DialogResult.Yes)
                            {
                                if (mTotalAmountShown.Value)
                                    ShowTotalAmount(false);      // hide it...
                                GameMode = RaidTheCagesModes.NEXTQUESTION;
                            }
                            else
                            {
                                SendToRfid("STOPLOGFILE");



                                // write to log.. players stopped...
                                if (mLogingOn)
                                    mLogFile.AppendToLogPlayerStopped();
                                //mParent.mAudio.SendAudio(SoundsRaidTheCage.QUITORCONTINUE, SoundCommands.STOP, true, true);
                                GameMode = RaidTheCagesModes.DONE;
                            }
                            if (mParent.mCurCountry != eCountries.PORTUGAL)
                            {
                                if (mParent.mCurCountry == eCountries.HUNGARY || mParent.mCurCountry == eCountries.BRAZIL || mParent.mCurCountry == eCountries.URUGUAY2018)
                                {
                                    mParent.mAudio.SendAudio(SoundsRaidTheCage.QUITORCONTINUE, SoundCommands.STOP, true,
                                        true,3000);
                                }
                                else
                                {


                                    mParent.mAudio.SendAudio(SoundsRaidTheCage.QUITORCONTINUE, SoundCommands.STOP, true,
                                        true);
                                }
                            }

                            dlg.Dispose();
                            break;
                        case RaidTheCagesModes.NEXTQUESTION:
                           
                            mUsedSwitchPlayer.Value = false;
                            mUsedSwitchQuestion.Value = false;  

                            mParent.MCurStackType.Value = eStackTypes.Main;
                            string tosendhost = "";

                            tosendhost += '\r';
                            tosendhost += '\n';
                            // patrick
                            byte[] sendbyteshost = Encoding.UTF8.GetBytes(tosendhost);

                           
                            try
                            {
                                if (mParent.ipdaemonHostMessaging.IsConnected)
                                    mParent.ipdaemonHostMessaging.Write(sendbyteshost);
                            }
                            catch (Exception)
                            {


                            }

                           

                            SendToRfid("CLEARLISTCURRENTQUESTION");
                            mParent.mQnr.Value++;
                            mParent.mSelectedAnswer.Value = -1;
                            mAmountThisQuestion.Value = 0;
                            if (!mParent.mFirstQuestion.Value)
                            {
                                mParent.SwitchQuestionCtrl();
                            }
                            //UpdateDlg();
                            GameMode = RaidTheCagesModes.GETQUESTIONFROMWISEQ;
                            break;
                        case RaidTheCagesModes.DONECLEARSCREENS:
                            if (!checkanswer())
                            {
                                // answer wrong...
                                if (mParent.mCurCountry == eCountries.ARGENTINA || mParent.mCurCountry == eCountries.UK || mParent.mCurCountry == eCountries.URUGUAY || mParent.mCurCountry == eCountries.PERU || mParent.mCurCountry == eCountries.MEXICO || mParent.mCurCountry == eCountries.COLUMBIA || mParent.mCurCountry == eCountries.VIETNAM || mParent.mCurCountry == eCountries.PARAQUAY || mParent.mCurCountry == eCountries.PORTUGAL || mParent.mCurCountry == eCountries.HUNGARY || mParent.mCurCountry == eCountries.BRAZIL || mParent.mCurCountry == eCountries.URUGUAY2018)
                                {
                                    //overlay.Start("initall");
                                    overlay.Start("question.hidequestionwronganswer");
                                    projector.Start("initall");

                                    if (mParent.mQnr.Value < 9)
                                    {
                                        overlay.Start("question.hideclock");
                                        //projector.Start("question.hideclock");
                                    }
                                    else
                                    {
                                        overlay.Start("question.hidertclogo");
                                        //projector.Start("question.hidertclogo");
                                        
                                    }

                                    if (mParent.mCurCountry != eCountries.PARAQUAY)
                                    {
                                        projector.LoadScreen("mainscreen");
                                        projector.Start("logoloop.hidelogo");
                                        mParent.mLogoProjector = false;
                                    }
                                }
                                else
                                {
                                    mParent.InitAllGraphics();
                                }
                            }
                            else
                                mParent.InitAllGraphics();
                            GameMode = RaidTheCagesModes.DONE;
                            break;
                        default:
                            // redo the engines...                            
                            overlay.Undo();
                            projector.Undo();
                            commit = false;
                            break;

                    }
                    if (commit)
                    {                        
                        UndoRedoManager.Commit();

                        // ok we have commit an action.. so make the space bar disabled for a small time..
                        mParent.ActiveCtrl.ContinuebtnEnable(false);
                        continuetimer.Stop();
                        mLastGameMode = mLastGameModeTemp;
                        if (overridedelay == -1)
                            continuetimer.Interval = mContinuedelays[(int)mLastGameMode];
                        else
                            continuetimer.Interval = overridedelay;

                        continuetimer.Start();

                    }
                    else
                        UndoRedoManager.Cancel();

                    if (canplaymidi)
                        CheckMidiNote(mLastGameModeTemp);

                    UpdateDlg();
                    mParent.UpdateDlg();
                    UpdateHostScreen();
                    UpdateExtraQA();
                    if (GameMode != RaidTheCagesModes.STOPRFIDSCANNING)
                        UpdateExtraTotal();
                }

                if (GameMode == RaidTheCagesModes.SHOWINGARTICLES)
                {
                    // popup the article list.. send the whole list to the class and there sort and put the names on the buttons...
                    ShowArticleList list = new ShowArticleList(mParent, dgv.Rows);
                    list.ShowDialog();
                    // hide the list..
                    projector.Start("pricelist.hide");
                    overlay.Start("pricelist.hide");

                    using (UndoRedoManager.Start("questionReceived"))
                    {
                        overlay.AddUndo();
                        projector.AddUndo();
                        if (mParent.mCurCountry == eCountries.PORTUGAL && mParent.mQnr.Value == 0)
                            GameMode = RaidTheCagesModes.SHOWTOTALAMOUNTTHISQUESTION;
                        else if (mParent.mCurCountry == eCountries.PORTUGAL)
                            GameMode = RaidTheCagesModes.SHOWTOTALAMOUNT;
                        else
                            GameMode = RaidTheCagesModes.SHOWTOTALAMOUNTTHISQUESTION;
                        UndoRedoManager.Commit();
                        UpdateDlg();
                        mParent.UpdateDlg();
                        UpdateHostScreen();
                        UpdateExtraQA();
                        UpdateExtraTotal();
                    }
                }
            }
        }

        public void CheckExtraMidiNote(string invalue)
        {            
            int strNumber=0;
            int strIndex = 0;
            bool found = false;
            for (strNumber = 0; strNumber < MidiSettings.ExtraMidiNotes.Length; strNumber++)
            {
                strIndex =  MidiSettings.ExtraMidiNotes[strNumber].IndexOf(invalue);
                if (strIndex >= 0)
                {
                    found = true;
                    break;
                }                
            }

            if (found)
            {
                mParent.wgmidi.CheckMidiNote(MidiSettings.ExtraMidiNotes[strNumber],
                                             MidiSettings.mExtraMidiNotes[strNumber]);

                if (MidiSettings.mExtraMidiNotes[strNumber] != -1)
                {
                    // we need to send a note...
                    // send continue action + note to send....
                    Midi.SendMidi(midi, MidiSettings.ExtraMidiNotes[strNumber], MidiSettings.mExtraMidiNotes[strNumber]);
                }
            }
        }

        public void CheckMidiNote(RaidTheCagesModes inGameMode)
        {
            mParent.wgmidi.CheckMidiNote(GetContinueTextEnglishWithGameMode(inGameMode),
                MidiSettings.mMidiNotes[(int)inGameMode]);
            if (MidiSettings.mMidiNotes[(int)inGameMode] != -1)
            {
                // we need to send a note...
                // send continue action + note to send....
                Midi.SendMidi(midi, GetContinueTextEnglishWithGameMode(inGameMode), MidiSettings.mMidiNotes[(int)inGameMode]);
            }
        }

        public void CheckMidiNote(RaidTheCagesModes inGameMode, string incontinuetext)
        {
            mParent.wgmidi.CheckMidiNote(incontinuetext,
                MidiSettings.mMidiNotes[(int)inGameMode]);
            if (MidiSettings.mMidiNotes[(int)inGameMode] != -1)
            {
                // we need to send a note...
                // send continue action + note to send....
                Midi.SendMidi(midi, incontinuetext, MidiSettings.mMidiNotes[(int)inGameMode]);
            }
        }

        public void UpdateHostScreen()
        {
            // send the 2 names...
            host.SetText("name1", RaidTheCageData.mNames[0]);
            host.SetText("name2", RaidTheCageData.mNames[1]);

            //scores
            host.SetText("total", HostAmount(mAmountTotal.Value));
            host.SetText("now", HostAmount(mAmountThisQuestion.Value));

            host.SetOpacity("Layer 2", lLifelinesAvailable[0].Value?0:255);
            host.SetOpacity("Layer 3", lLifelinesAvailable[1].Value ? 0 : 255);

            string articlelist = "";
            int cnt = 0;
            foreach (DataGridViewRow row in dgv.Rows)
            {
                articlelist += row.Cells[0].Value.ToString() + ";";
                cnt++;
            }
            host.SetText("list", articlelist);
            host.SetText("count", cnt + "-" + mParent.CalculatTotalAmountThisQuestion());

            if (GameMode > RaidTheCagesModes.SHOWQUESTION && GameMode <= RaidTheCagesModes.STARTCAGETIME)
            {
                host.SetText("question", mParent.mFirstQuestion.Value ? mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mQuestion : mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mQuestion2);                
            }
            else
            {
                host.SetText("question", "");               
            }

            if (GameMode > RaidTheCagesModes.WAITFORANSWER && GameMode <= RaidTheCagesModes.STARTCAGETIME)
            {
                if (mParent.mFirstQuestion.Value)
                    host.SetText("extrainfo", mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mExplanation);
                else
                    host.SetText("extrainfo", mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mExplanation2);
            }
            else
            {
                host.SetText("extrainfo", "");
            }

            if (GameMode > RaidTheCagesModes.SHOWANSWER1 && GameMode <= RaidTheCagesModes.STARTCAGETIME)
            {
                host.SetText("answera", mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[0]);
            }
            else
            {
                host.SetText("answera", "");
            }

            if (GameMode > RaidTheCagesModes.SHOWANSWER2 && GameMode <= RaidTheCagesModes.STARTCAGETIME)
            {
                host.SetText("answerc", mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[1]);
            }
            else
            {
                host.SetText("answerc", "");
            }

            if (GameMode > RaidTheCagesModes.SHOWANSWER3 && GameMode <= RaidTheCagesModes.STARTCAGETIME)
            {
                host.SetText("answerb", mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[2]);
            }
            else
            {
                host.SetText("answerb", "");
            }

            if (GameMode > RaidTheCagesModes.SHOWANSWER4 && GameMode <= RaidTheCagesModes.STARTCAGETIME)
            {
                host.SetText("answerd", mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[3]);
            }
            else
            {
                host.SetText("answerd", "");
            }

            if (GameMode > RaidTheCagesModes.WAITFORANSWER && GameMode <= RaidTheCagesModes.STARTCAGETIME)
            {
                host.SetOpacity("highlite1", mParent.mSelectedAnswer.Value == 0 ? 255 : 0);
            }
            else
                host.SetOpacity("highlite1", 0);

            if (GameMode > RaidTheCagesModes.WAITFORANSWER && GameMode <= RaidTheCagesModes.STARTCAGETIME)
            {
                host.SetOpacity("highlite2", mParent.mSelectedAnswer.Value == 1 ? 255 : 0);
            }
            else
                host.SetOpacity("highlite2", 0);

            if (GameMode > RaidTheCagesModes.WAITFORANSWER && GameMode <= RaidTheCagesModes.STARTCAGETIME)
            {
                host.SetOpacity("highlite3", mParent.mSelectedAnswer.Value == 2 ? 255 : 0);
            }
            else
                host.SetOpacity("highlite3", 0);

            if (GameMode > RaidTheCagesModes.WAITFORANSWER && GameMode <= RaidTheCagesModes.STARTCAGETIME)
            {
                host.SetOpacity("highlite4", mParent.mSelectedAnswer.Value == 3 ? 255 : 0);
            }
            else
                host.SetOpacity("highlite4", 0);

            if (GameMode > RaidTheCagesModes.SHOWRIGHTANSWER && GameMode <= RaidTheCagesModes.STARTCAGETIME)
            {
                host.SetOpacity("correct1", mParent.mFirstQuestion.Value ? (mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mRightAnswer == "1" ? 255 : 0) : (mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mRightAnswer2 == "1" ? 255 : 0));
            }
            else
                host.SetOpacity("correct1", 0);

            if (GameMode > RaidTheCagesModes.SHOWRIGHTANSWER && GameMode <= RaidTheCagesModes.STARTCAGETIME)
            {
                host.SetOpacity("correct2", mParent.mFirstQuestion.Value ? (mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mRightAnswer == "2" ? 255 : 0) : (mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mRightAnswer2 == "2" ? 255 : 0));
            }
            else
                host.SetOpacity("correct2", 0);

            if (GameMode > RaidTheCagesModes.SHOWRIGHTANSWER && GameMode <= RaidTheCagesModes.STARTCAGETIME)
            {
                host.SetOpacity("correct3", mParent.mFirstQuestion.Value ? (mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mRightAnswer == "3" ? 255 : 0) : (mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mRightAnswer2 == "3" ? 255 : 0));
            }
            else
                host.SetOpacity("correct3", 0);

            if (GameMode > RaidTheCagesModes.SHOWRIGHTANSWER && GameMode <= RaidTheCagesModes.STARTCAGETIME)
            {
                host.SetOpacity("correct4", mParent.mFirstQuestion.Value ? (mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mRightAnswer == "4" ? 255 : 0) : (mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mRightAnswer2 == "4" ? 255 : 0));
            }
            else
                host.SetOpacity("correct4", 0);

            


        }

        public void ShowTotalAmount(bool manual)
        {
            bool started = false;

            if (!UndoRedoManager.IsCommandStarted)
            {
                UndoRedoManager.Start("StartNewGame");
                projector.AddUndo();
                overlay.AddUndo();
                started = true;
            }

            
                            

                if (!mTotalAmountShown.Value)
                {
                    mParent.mAudio.SendAudio(SoundsRaidTheCage.SHOWTOTAL, SoundCommands.PLAY, false, false);
                    // first init the totalamount and then send the new amount anim and start.. should be quick enough :)
                    projector.Start("totalamount.init");
                    overlay.Start("totalamount.init");
                    SetTotalAmount(mAmountTotal.Value);
                    projector.Start("totalamount.show");
                    overlay.Start("totalamount.show");
                    mTotalAmountShown.Value = true;
                }
                else
                {
                    projector.Start("totalamount.init");
                    overlay.Start("totalamount.init");
                    mTotalAmountShown.Value = false;
                    if (manual)
                    {
                        // show logo again
                        projector.SetOpacity("logo", 255, 0.4f);
                        projector.SetOpacity("logo3d", 255, 0.4f);
                    }
                }

                if (started)
                    UndoRedoManager.Commit();
            



            UpdateDlg();
        }

        private void btShowTotalAmount_Click(object sender, EventArgs e)
        {
            ShowTotalAmount(true);
        }

        private void btOpenCageDoor_Click(object sender, EventArgs e)
        {
            SendToRfid("OPENCAGEDOOR");
        }

        private void btCloseCageDoor_Click(object sender, EventArgs e)
        {
            SendToRfid("CLOSECAGEDOOR");
        }

        /*
        private void btStuckInCage_Click(object sender, EventArgs e)
        {
            Midi.SendMidi(midi, midinotes.STUCKINCAGE);
            mParent.mAudio.SendAudio(SoundsRaidTheCage.STUCKINCAGE,SoundCommands.PLAY,false,false);

            using (UndoRedoManager.Start("questionReceived"))
            {
                overlay.AddUndo();
                projector.AddUndo();
                mParent.InitAllGraphics();
                GameMode = RaidTheCagesModes.DONE;                
                UndoRedoManager.Commit();
            }
            mParent.ActiveCtrl.UpdateDlg();
            UpdateDlg(); // for host
        }
         */

        private void ucRaidTheCage_Load(object sender, EventArgs e)
        {

        }

        private void btLightReset_Click(object sender, EventArgs e)
        {
            CheckExtraMidiNote("Lights Reset");
        }

        private void bmbLogging_Click(object sender, EventArgs e)
        {
            if (!mLogingOn)
            {
                // open dialog and ask for filename to write logfile...
                // we use the standard dir logfiles....
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.InitialDirectory = mParent.ExePath + "\\LogFiles";
                sfd.Filter = "Text files (*.txt)|*.txt";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    // cool... save the filename and open log for writing
                    // change picture to green...
                    bmbLogging.ImageNormal = Properties.Resources.LoggingEnabled;

                    lblogging1.Text = "LOGGING IS ON";
                    mLoggingFileName = sfd.FileName;
                    lblogging2.Text = Path.GetFileNameWithoutExtension(sfd.FileName);

                    mLogFile = new Logfile(mParent);
                    mLogFile.mFilename = mLoggingFileName;
                    mLogFile.mContestantName = RaidTheCageData.mNames[0] + " - " + RaidTheCageData.mNames[1];                    
                    mLogFile.CreateLogFileList();
                    mLogFile.PrintLogFileHeader();
                    mLogingOn = true;
                    // also start the logging of the gfx1 and gfx2

                }
            }
            else
            {
                // stop logging...
                bmbLogging.ImageNormal = Properties.Resources.LoggingDisabled;
                lblogging1.Text = "logging is off";
                mLoggingFileName = "";
                lblogging2.Text = "";
                bmbLogging.Refresh();
                mLogingOn = false;
                //mParent.mGFXEngines[(int)eEngines.GFX1].mLoggingon = false;
                //mParent.mGFXEngines[(int)eEngines.GFX2].mLoggingon = false;
            }
        }

        private void UpdateExtraTotal()
        {
            string total="", total2="";
            if (!generalsettings.mCurrencysignafteramount)
            {
                total += generalsettings.mCurrencysign;
                total2 += generalsettings.mCurrencysign;
            }
            
            total += FormatAmount(mAmountThisQuestion.Value);
            total2 += FormatAmount(mAmountTotal.Value);

            if (generalsettings.mCurrencysignafteramount)
            {
                total += generalsettings.mCurrencysign;
                total2 += generalsettings.mCurrencysign;
            }

            mParent.mGFXEngines[(int) eEngines.EXTRAQA].SetText("total1", total);
            mParent.mGFXEngines[(int)eEngines.EXTRAQA].SetText("total2", total2);
        }

        private void UpdateExtraQA()
        {
            if (mParent.mCurCountry == eCountries.VIETNAM)
            {
                if (GameMode >= RaidTheCagesModes.SHOWQUESTION && GameMode <= RaidTheCagesModes.NEXTQUESTION)
                {
                    // show the question.. and the answers.. otherwise empty..
                    // remove 2 signs from the question
                    string question = "";

                    if (!mUsedSwitchQuestion.Value)
                    {
                        question = mParent.lAllQuestions[(int)mParent.MCurStackType.Value][
                            mParent.mQnr.Value].mQuestion;
                    }
                    else
                    {
                        question = mParent.lAllQuestions[(int)mParent.MCurStackType.Value][
                            mParent.mQnr.Value].mQuestion2;
                    }
                    question.Replace("|", " ");

                    mParent.mGFXEngines[(int)eEngines.EXTRAQA].SetText("question", question);
                    mParent.mGFXEngines[(int)eEngines.EXTRAQA].SetText("ans1", mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[0]);
                    mParent.mGFXEngines[(int)eEngines.EXTRAQA].SetText("ans2", mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[1]);
                    mParent.mGFXEngines[(int)eEngines.EXTRAQA].SetText("ans3", mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[2]);
                    mParent.mGFXEngines[(int)eEngines.EXTRAQA].SetText("ans4", mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[3]);

                    // and the colors.. first the green.. then grey if any selection
                    mParent.mGFXEngines[(int)eEngines.EXTRAQA].SetColor("boxanswer1", mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mRightAnswer == "1" ? Color.FromArgb(0, 155, 0) : Color.FromArgb(50,50,50));
                    mParent.mGFXEngines[(int)eEngines.EXTRAQA].SetColor("boxanswer2", mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mRightAnswer == "2" ? Color.FromArgb(0, 155, 0) : Color.FromArgb(50, 50, 50));
                    mParent.mGFXEngines[(int)eEngines.EXTRAQA].SetColor("boxanswer3", mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mRightAnswer == "3" ? Color.FromArgb(0, 155, 0) : Color.FromArgb(50, 50, 50));
                    mParent.mGFXEngines[(int)eEngines.EXTRAQA].SetColor("boxanswer4", mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mRightAnswer == "4" ? Color.FromArgb(0, 155, 0) : Color.FromArgb(50, 50, 50));

                    if (GameMode <= RaidTheCagesModes.SHOWRIGHTANSWER)
                    {
                        // then grey if we have...
                        if (mParent.mSelectedAnswer.Value == 0)
                        {
                            mParent.mGFXEngines[(int) eEngines.EXTRAQA].SetColor("boxanswer1",
                                                                                 Color.FromArgb(255, 255, 0));
                        }
                        else if (mParent.mSelectedAnswer.Value == 1)
                        {
                            mParent.mGFXEngines[(int) eEngines.EXTRAQA].SetColor("boxanswer2",
                                                                                 Color.FromArgb(255, 255, 0));
                        }
                        else if (mParent.mSelectedAnswer.Value == 2)
                        {
                            mParent.mGFXEngines[(int) eEngines.EXTRAQA].SetColor("boxanswer3",
                                                                                 Color.FromArgb(255, 255, 0));
                        }
                        else if (mParent.mSelectedAnswer.Value == 2)
                        {
                            mParent.mGFXEngines[(int) eEngines.EXTRAQA].SetColor("boxanswer4",
                                                                                 Color.FromArgb(255, 255, 0));
                        }
                        else
                        {
                            // remove if its not the correct answer...
                            if (
                                mParent.lAllQuestions[(int) mParent.MCurStackType.Value][mParent.mQnr.Value].
                                    mRightAnswer != "1")
                            {
                                mParent.mGFXEngines[(int) eEngines.EXTRAQA].SetColor("boxanswer1",
                                                                                     Color.FromArgb(50, 50, 50));
                            }
                            // remove if its not the correct answer...
                            if (
                                mParent.lAllQuestions[(int) mParent.MCurStackType.Value][mParent.mQnr.Value].
                                    mRightAnswer != "2")
                            {
                                mParent.mGFXEngines[(int) eEngines.EXTRAQA].SetColor("boxanswer2",
                                                                                     Color.FromArgb(50, 50, 50));
                            }
                            // remove if its not the correct answer...
                            if (
                                mParent.lAllQuestions[(int) mParent.MCurStackType.Value][mParent.mQnr.Value].
                                    mRightAnswer != "3")
                            {
                                mParent.mGFXEngines[(int) eEngines.EXTRAQA].SetColor("boxanswer3",
                                                                                     Color.FromArgb(50, 50, 50));
                            }
                            // remove if its not the correct answer...
                            if (
                                mParent.lAllQuestions[(int) mParent.MCurStackType.Value][mParent.mQnr.Value].
                                    mRightAnswer != "4")
                            {
                                mParent.mGFXEngines[(int) eEngines.EXTRAQA].SetColor("boxanswer4",
                                                                                     Color.FromArgb(50, 50, 50));
                            }
                        }
                    }
                    else
                    {
                        // the selected answer red or green
                        if (mParent.mSelectedAnswer.Value == 0)
                        {
                            if (
                                mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].
                                    mRightAnswer == "1")
                            {
                                mParent.mGFXEngines[(int)eEngines.EXTRAQA].SetColor("boxanswer1",
                                                                                     Color.FromArgb(0, 200, 50));
                            }
                            else
                            {
                                mParent.mGFXEngines[(int)eEngines.EXTRAQA].SetColor("boxanswer1",
                                                                                     Color.FromArgb(200, 0, 0));
                            }
                        }
                        else if (mParent.mSelectedAnswer.Value == 1)
                        {
                            if (
                                mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].
                                    mRightAnswer == "2")
                            {
                                mParent.mGFXEngines[(int)eEngines.EXTRAQA].SetColor("boxanswer2",
                                                                                     Color.FromArgb(0, 200, 50));
                            }
                            else
                            {
                                mParent.mGFXEngines[(int)eEngines.EXTRAQA].SetColor("boxanswer2",
                                                                                     Color.FromArgb(200, 0, 0));
                            }
                        }
                        else if (mParent.mSelectedAnswer.Value == 2)
                        {
                            if (
                                mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].
                                    mRightAnswer == "3")
                            {
                                mParent.mGFXEngines[(int)eEngines.EXTRAQA].SetColor("boxanswer3",
                                                                                     Color.FromArgb(0, 200, 50));
                            }
                            else
                            {
                                mParent.mGFXEngines[(int)eEngines.EXTRAQA].SetColor("boxanswer3",
                                                                                     Color.FromArgb(200, 0, 0));
                            }
                        }

                        else if (mParent.mSelectedAnswer.Value == 2)
                        {
                            if (
                                mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].
                                    mRightAnswer == "3")
                            {
                                mParent.mGFXEngines[(int)eEngines.EXTRAQA].SetColor("boxanswer3",
                                                                                     Color.FromArgb(0, 200, 50));
                            }
                            else
                            {
                                mParent.mGFXEngines[(int)eEngines.EXTRAQA].SetColor("boxanswer3",
                                                                                     Color.FromArgb(200, 0, 0));
                            }
                        }
                        else if (mParent.mSelectedAnswer.Value == 3)
                        {
                            if (
                                mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].
                                    mRightAnswer == "4")
                            {
                                mParent.mGFXEngines[(int)eEngines.EXTRAQA].SetColor("boxanswer4",
                                                                                     Color.FromArgb(0, 200, 50));
                            }
                            else
                            {
                                mParent.mGFXEngines[(int)eEngines.EXTRAQA].SetColor("boxanswer4",
                                                                                     Color.FromArgb(200, 0, 0));
                            }
                        }
                    }
                }
                else
                {
                    mParent.mGFXEngines[(int)eEngines.EXTRAQA].SetText("question", "");
                    mParent.mGFXEngines[(int)eEngines.EXTRAQA].SetText("ans1", "");
                    mParent.mGFXEngines[(int)eEngines.EXTRAQA].SetText("ans2", "");
                    mParent.mGFXEngines[(int)eEngines.EXTRAQA].SetText("ans3", "");
                    mParent.mGFXEngines[(int)eEngines.EXTRAQA].SetText("ans4", "");
                    mParent.mGFXEngines[(int)eEngines.EXTRAQA].SetColor("boxanswer1", Color.FromArgb(50, 50, 50));
                    mParent.mGFXEngines[(int)eEngines.EXTRAQA].SetColor("boxanswer2", Color.FromArgb(50, 50, 50));
                    mParent.mGFXEngines[(int)eEngines.EXTRAQA].SetColor("boxanswer3", Color.FromArgb(50, 50, 50));
                    mParent.mGFXEngines[(int)eEngines.EXTRAQA].SetColor("boxanswer4", Color.FromArgb(50, 50, 50));
                }
            }
            else
            {                           
                if (GameMode >= RaidTheCagesModes.SHOWQUESTION && GameMode <= RaidTheCagesModes.SHOWRIGHTANSWER)
                {
                    // show the question.. and the answers.. otherwise empty..
                    // remove 2 signs from the question
                    string question = "";

                    if (!mUsedSwitchQuestion.Value)
                    {
                        question = mParent.lAllQuestions[(int) mParent.MCurStackType.Value][
                            mParent.mQnr.Value].mQuestion;
                    }
                    else
                    {
                        question = mParent.lAllQuestions[(int)mParent.MCurStackType.Value][
                            mParent.mQnr.Value].mQuestion2;
                    }
                    question.Replace("|", " ");

                    mParent.mGFXEngines[(int)eEngines.EXTRAQA].SetText("question", question);
                    mParent.mGFXEngines[(int)eEngines.EXTRAQA].SetText("ans1", mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[0]);
                    mParent.mGFXEngines[(int)eEngines.EXTRAQA].SetText("ans2", mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[1]);
                    mParent.mGFXEngines[(int)eEngines.EXTRAQA].SetText("ans3", mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[2]);
                    mParent.mGFXEngines[(int)eEngines.EXTRAQA].SetText("ans4", mParent.lAllQuestions[(int)mParent.MCurStackType.Value][mParent.mQnr.Value].mAnswers[3]);
                }
                else
                {               
                    mParent.mGFXEngines[(int) eEngines.EXTRAQA].SetText("question","");
                    mParent.mGFXEngines[(int)eEngines.EXTRAQA].SetText("ans1", "");
                    mParent.mGFXEngines[(int)eEngines.EXTRAQA].SetText("ans2", "");
                    mParent.mGFXEngines[(int)eEngines.EXTRAQA].SetText("ans3", "");
                    mParent.mGFXEngines[(int)eEngines.EXTRAQA].SetText("ans4", "");
                }
            }
        }

        private void tbHost_TextChanged(object sender, EventArgs e)
        {
            if (!mIsUpdating)
            {
                RaidTheCageData.mHosts[0] = tbHost.Text;
                RaidTheCageData.mChanged = true;
                UpdateHostScreen();
            }
        }

        private void tbCoHost_TextChanged(object sender, EventArgs e)
        {
            if (!mIsUpdating)
            {
                RaidTheCageData.mHosts[1] = tbCoHost.Text;
                RaidTheCageData.mChanged = true;
                UpdateHostScreen();
            }
        }

        private void tbPlace_TextChanged(object sender, EventArgs e)
        {
            if (!mIsUpdating)
            {
                RaidTheCageData.mPlace = tbPlace.Text;
                RaidTheCageData.mChanged = true;
                UpdateHostScreen();
            }
        }

        private void btShowTitle1_Click(object sender, EventArgs e)
        {
            overlay.SetText("nametitle.barsmall.txt1", RaidTheCageData.mHosts[0]);
            overlay.Start("nametitle.show1");
            HideNameTitle hnt = new HideNameTitle();
            hnt.ShowDialog();
            overlay.Start("nametitle.hide1");
        }

        private void btShowTitle2_Click(object sender, EventArgs e)
        {
            overlay.SetText("nametitle.barsmall.txt1", RaidTheCageData.mHosts[1]);
            overlay.Start("nametitle.show1");
            HideNameTitle hnt = new HideNameTitle();
            hnt.ShowDialog();
            overlay.Start("nametitle.hide1");
        }

        private void BackupSoundsCommerialBreak()
        {
            for (int i = 0; i < SoundsRaidTheCage.MAX; i++)
            {
                mParent.mAudio.mSoundPlaysBUCommercialBreak[i] = mParent.mAudio.mSoundPlays[i].Value;
            }
        }

        private bool CheckSoundsReverseCommercialBreak()           // for commercialbreakback
        {
            bool mUseUndo = false;
            // check if we have any looping running sounds.. if not... kill all sounds!
            //bool soundplaying = false;
            for (int i = 0; i < SoundsRaidTheCage.MAX; i++)
            {
                if (mParent.mAudio.mSoundPlaysBUCommercialBreak[i] != mParent.mAudio.mSoundPlays[i].Value)
                {
                    // we have a change in sound.... just use the undo list..
                    if (mParent.mAudio.mSoundPlaysBUCommercialBreak[i])
                    {
                        // play
                        mParent.mAudio.SendAudio(i, SoundCommands.PLAY, true, true);
                        mParent.mAudio.mSoundPlays[i].Value = true;
                        mUseUndo = true;
                    }
                    else
                    {
                        // stop
                        mParent.mAudio.SendAudio(i, SoundCommands.STOP, true, true);
                    }
                }
            }
            return mUseUndo;

        }

        internal void PauzeSounds()
        {
            BackupSoundsCommerialBreak();
            mParent.KillAllAudio(false);
            using (UndoRedoManager.StartInvisible("Pauze Sounds"))
            {

                // we have killed all audio so make the list empty.. so we can undo
                for (int i = 0; i < 100; i++)
                    mParent.mAudio.mSoundPlays[i].Value = false;


                UndoRedoManager.Commit();
            }
        }

        internal void ResumeSounds()
        {
            using (UndoRedoManager.StartInvisible("Resume Sounds"))
            {
                if (CheckSoundsReverseCommercialBreak())
                    UndoRedoManager.Commit();
                else
                    UndoRedoManager.Cancel();
            }
        }

        private void btStartExplain3sec_Click(object sender, EventArgs e)
        {
          
            // we need to start a timer... 
            if (generalsettings.mTimebeforethedoorcloses > 3000)
            {
                // we need to close the door first.. then start a timer for the rest...                
                // close door....
                SendToRfid("CLOSECAGEDOOR");

                m3secexplain1tmr.Interval = generalsettings.mTimebeforethedoorcloses - 3000;
                m3secexplain1tmr.Start();
            }
            else if (generalsettings.mTimebeforethedoorcloses <= 3000)
            {
                // audio and midi first.. then a timer for the door if smaller then 3000
                mParent.mAudio.SendAudio(SoundsRaidTheCage.DOORS3SEC, SoundCommands.PLAY, false, false);
                // + midi...                
                CheckExtraMidiNote("Closing Door in 3 Seconds");
                if (generalsettings.mTimebeforethedoorcloses == 3000)
                {
                    SendToRfid("CLOSECAGEDOOR");
                }
                else
                {
                    // we need to start a timer to close the door....
                    m3secexplain2tmr.Interval = 3000 - generalsettings.mTimebeforethedoorcloses;
                    m3secexplain2tmr.Start();
                }
            }
           
        }
    }

    public static class RaidTheCageData
    {
        public static string[] mNames = new string[2];
        public static string[] mHosts = new string[2];

        public static string mPlace;

        private static IProfile m_profile;

        public static bool mChanged = false;        

        public static void ReadXmlConfig(string inPath)
        {

            // read the xml file with all defaults.......
            m_profile = new Xml();

            m_profile.Name = inPath + "\\ConfigFiles\\Settings\\RaidTheCageSettings.xml";

            mNames[0] = m_profile.GetValue("RaidTheCageSettings", "Name1", "Name1");
            mNames[1] = m_profile.GetValue("RaidTheCageSettings", "Name2", "Name2");
            mHosts[0] = m_profile.GetValue("RaidTheCageSettings", "Host", "Host Name");
            mHosts[1] = m_profile.GetValue("RaidTheCageSettings", "CoHost", "Co-Host Name");

            mPlace = m_profile.GetValue("RaidTheCageSettings", "Place", "Place");

        }

        public static void WriteXmlConfig(string inPath)
        {
            // read the xml file with all defaults.......
            m_profile = new Xml();

            m_profile.Name = inPath + "\\ConfigFiles\\Settings\\RaidTheCageSettings.xml";

            m_profile.SetValue("RaidTheCageSettings", "Name1", mNames[0]);
            m_profile.SetValue("RaidTheCageSettings", "Name2", mNames[1]);

            m_profile.SetValue("RaidTheCageSettings", "Host", mHosts[0]);
            m_profile.SetValue("RaidTheCageSettings", "CoHost", mHosts[1]);

            m_profile.SetValue("RaidTheCageSettings", "Place", mPlace);

        }
    }
}
