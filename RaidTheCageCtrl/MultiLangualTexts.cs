using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WiseGuys.configs;
using WiseGuysFrameWork2015.Settings;


namespace RaidTheCageCtrl
{
    public static class MultiLangualTexts
    {

        public static string[] EnglishTexts =
            {
                "Question Nr:",                 //0
                "Question:",                    //1
                "Products taken from the cage - Current Question", //2
                "Random", //3
                "High-Low", //4
                "Low-High", //5
                "Logo Projector", //6
                "Extra Info:", //7
                "Name 1:", //8
                "Name 2:", //9
                "Prices", // 10
                "This Question:", //11
                "Total:", //12
                "Show", //13
                "LifeLine 1", //14
                "LifeLine 2", //15
                "Available", //16
                "Open Cage Door", //17
                "Close Cage Door", //18
                "Show MoneyTree On Projector", //19
                };

        public static string[] SecondLanguageTexts;
            

        public static bool mChanged = false;

        public static string mLanguageName;

        public static MainForm mParent;

        private static IProfile m_profile;

        public static string[] mlocalisationTexts = new string[(int)RaidTheCagesModes.MODES+1];

        public static void SetParent(MainForm pParent)
        {
            mParent = pParent;

            SecondLanguageTexts = new string[EnglishTexts.Count()];

            ReadXmlConfig(mParent.ExePath);
            
        }

        //public static BreakAwayModes GameMode
        //{
        //    get { return mParent.gcBreakAwayGame.GameMode; }            
        //}

        public static string GetContinueTextEnglish(RaidTheCagesModes inMode)
        {
            string[] statustxts = new string[(int)RaidTheCagesModes.MODES + 1];

            statustxts[(int)RaidTheCagesModes.INIT] = "Click START to Start Game";
            
            //statustxts[(int)RaidTheCagesModes.SHOWHOSTNAME] = "Show Host Name (Ctrl+click to skip)";
            //statustxts[(int)RaidTheCagesModes.HIDEHOSTNAME] = "Hide Host Name";

            statustxts[(int)RaidTheCagesModes.STARTINTROMUSIC] = "Start Intro Music";
            statustxts[(int)RaidTheCagesModes.SHOWCONTESTANTSNAMES] = "Show Contestants Names (Ctrl+click to skip)";
            statustxts[(int)RaidTheCagesModes.HIDECONTESTANTSNAMES] = "Hide Contestants Names";
         
            statustxts[(int)RaidTheCagesModes.GOTOEXPLAINTHEGAMES] = "Start Explain The Games (Ctrl+click to skip)";
            //statustxts[(int)RaidTheCagesModes.EXPLAIN3SEC] = "Explain 3 Seconds (Ctrl+click to skip)";
            statustxts[(int)RaidTheCagesModes.GETQUESTIONFROMWISEQ] = "Get Question From WiseQ";
            statustxts[(int)RaidTheCagesModes.GETQUESTIONFROMWISEQSWITCH] = "Get Switch Question From WiseQ";
            statustxts[(int)RaidTheCagesModes.SHOWQUESTION] = "Show Question";
            statustxts[(int)RaidTheCagesModes.SHOWANSWER1] = "Show Answer 1";
            statustxts[(int)RaidTheCagesModes.SHOWANSWER2] = "Show Answer 2";
            statustxts[(int)RaidTheCagesModes.SHOWANSWER3] = "Show Answer 3";
            statustxts[(int)RaidTheCagesModes.SHOWANSWER4] = "Show Answer 4";
            statustxts[(int)RaidTheCagesModes.WAITFORANSWER] = "Select Contestants Answer";
            statustxts[(int)RaidTheCagesModes.ENDSWITCHPLAYER] = "Click when players have switched";
            statustxts[(int)RaidTheCagesModes.SWITCHQUESTION] = "Switch Question";
            statustxts[(int)RaidTheCagesModes.LETSSEEIFYOUARERIGHTMUSIC] = "Start Lets see if you are right music";
            statustxts[(int)RaidTheCagesModes.SHOWRIGHTANSWERBEFORESWITCH] = "Show Right Answer before switch";
            statustxts[(int)RaidTheCagesModes.SHOWRIGHTANSWER] = "Show Right Answer";
            statustxts[(int)RaidTheCagesModes.REMOVEQUESTION] = "Remove Question And Answers / Start Cage Time";
            statustxts[(int)RaidTheCagesModes.REMOVEQUESTIONFINALANSWER] = "Remove Question and Play Music";
            statustxts[(int)RaidTheCagesModes.REMOVEQUESTIONFINALANSWER2] = "Stop Music";
            statustxts[(int)RaidTheCagesModes.SHOWCAGETIME] = "Show Cage Time";
            statustxts[(int)RaidTheCagesModes.STARTCAGETIME] = "Start Cage Time";
            statustxts[(int)RaidTheCagesModes.CAGETIMERUNNING] = "Close Doors - Stop Timer";
            statustxts[(int)RaidTheCagesModes.CAGETIMERUNNINGLAST3SEC] = "Last 3 seconds - Door is closing";
            statustxts[(int)RaidTheCagesModes.OUTOFCAGETIME] = "Out of Cage Time!";
            statustxts[(int)RaidTheCagesModes.HIDECLOCK] = "Hide Clock";

            //if (mParent.mCurCountry == eCountries.PARAQUAY)
            statustxts[(int)RaidTheCagesModes.STOPRFIDSCANNING] = "List Complete";
            //else
            //    statustxts[(int)RaidTheCagesModes.STOPRFIDSCANNING] = "Stop RFID Scanning";

            statustxts[(int)RaidTheCagesModes.ANSWERWRONG] = "Answer Wrong - Game Over!";

            statustxts[(int)RaidTheCagesModes.SHOWARTICLELIST] = "Show Article list (Ctrl+click to skip)";
            statustxts[(int)RaidTheCagesModes.SHOWARTICLE] = "Show Article";
            statustxts[(int)RaidTheCagesModes.SHOWINGARTICLES] = "Showing Articles";            
            statustxts[(int)RaidTheCagesModes.HIDEARTICLELIST] = "Hide Article list";
            statustxts[(int)RaidTheCagesModes.SHOWTOTALAMOUNTTHISQUESTION] = "Show Total Amount this question (Ctrl+click to skip)";
            if (mParent != null)
            {
                if (mParent.mCurCountry == eCountries.UK)
                    statustxts[(int) RaidTheCagesModes.SHOWTOTALAMOUNT] =
                        "Show Total Amount all questions (Ctrl+click to skip)";
                else
                    statustxts[(int) RaidTheCagesModes.SHOWTOTALAMOUNT] = "Show Total Amount all questions";
            }
            else
                statustxts[(int)RaidTheCagesModes.SHOWTOTALAMOUNT] = "Show Total Amount all questions";
            statustxts[(int)RaidTheCagesModes.HIDETOTALAMOUNT1] = "Hide Total Amount Current Question";
            statustxts[(int)RaidTheCagesModes.HIDETOTALAMOUNT] = "Hide Total Amount";
            statustxts[(int)RaidTheCagesModes.PLAYORSTOP] = "Show Play Or Stop Window";
            statustxts[(int)RaidTheCagesModes.SHOWMONEYTREE] = "Show Money Tree (Ctrl+click to skip)";
            statustxts[(int)RaidTheCagesModes.HIDEMONEYTREE] = "Hide MoneyTree";
            statustxts[(int)RaidTheCagesModes.NEXTQUESTION] = "Next Question";

            statustxts[(int)RaidTheCagesModes.DONECLEARSCREENS] = "Game Done - Clear Screens";
            statustxts[(int)RaidTheCagesModes.DONE] = "!!!Game DONE!!!";
            statustxts[(int)RaidTheCagesModes.MODES] = "";

            return statustxts[(int)inMode];
        }

        public static string GetContinueTextSecondLanguage(RaidTheCagesModes inMode)
        {
            return MultiLangualTexts.mlocalisationTexts[(int)inMode];            
        }

         public static void ReadXmlConfig(string inPath)
         {
             string filename = inPath + "\\ConfigFiles\\Settings\\LocalisationTextSettings.xmc";
             string filenamexml = inPath + "\\ConfigFiles\\Settings\\" +
                                  Path.GetFileNameWithoutExtension(filename) + ".xml";

             if (File.Exists(filename))
             {
                 // convert to xml file.. later try away again!
                 SettingsEncrypt.Decrypt(filename, filenamexml);
             }
             // read the xml file with all defaults.......
             m_profile = new Xml();
             m_profile.Name = filenamexml;

             //m_profile.Name = inPath + "\\ConfigFiles\\Settings\\LocalisationSettings.xml";

             mLanguageName = m_profile.GetValue("LocalisationTexts", "LanguageName", "Second Language");

             foreach (RaidTheCagesModes val in Enum.GetValues(typeof(RaidTheCagesModes)))
             {
                 mlocalisationTexts[(int)val] = m_profile.GetValue("LocalisationTexts", val.ToString(), GetContinueTextEnglish(val));
             }

             int cnt = 0;
             foreach (string str in EnglishTexts)
             {
                 SecondLanguageTexts[cnt] = m_profile.GetValue("LocalisationTexts", EnglishTexts[cnt], EnglishTexts[cnt]);
                 cnt++;
             }

             if (File.Exists(filenamexml))
             {
                 File.Delete(filenamexml);
             } 
         }

         public static void WriteXmlConfig(string inPath)
         {
             string filename = inPath + "\\ConfigFiles\\Settings\\LocalisationTextSettings.xmc";
             string filenamexml = inPath + "\\ConfigFiles\\Settings\\" +
                                  Path.GetFileNameWithoutExtension(filename) + ".xml";

             if (File.Exists(filename))
             {
                 // convert to xml file.. later try away again!
                 SettingsEncrypt.Decrypt(filename, filenamexml);
             }

             // read the xml file with all defaults.......
             m_profile = new Xml();
             m_profile.Name = filenamexml;

             //m_profile.Name = inPath + "\\ConfigFiles\\Settings\\LocalisationSettings.xml";

             m_profile.SetValue("LocalisationTexts", "LanguageName", mLanguageName);

             foreach (RaidTheCagesModes val in Enum.GetValues(typeof(RaidTheCagesModes)))
             {
                 m_profile.SetValue("LocalisationTexts", val.ToString(), mlocalisationTexts[(int)val]);
             }

             int cnt = 0;
             foreach (string str in EnglishTexts)
             {
                 m_profile.SetValue("LocalisationTexts", EnglishTexts[cnt], SecondLanguageTexts[cnt]);
                 cnt++;
             }
             // now encrypt...
             SettingsEncrypt.Encrypt(filenamexml, filename);
             // and delete
             File.Delete(filenamexml);
         }
    }
}
