using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DejaVu;
using RaidTheCageCtrl;

namespace LogFile
{
    class Logfile
    {
        public List<fileData>[] lLogileData;
        public string mFilename;
        public string mContestantName;        

        private MainForm mParent;
        /// <summary>
        /// constructor...
        /// </summary>
        public Logfile(MainForm pParent)
        {
            mParent = pParent;
        }
       
        public void CreateLogFileList()
        {
            lLogileData = new List<fileData>[2];       // 1 main .. 1 switch
            lLogileData[0] = new List<fileData>();
            lLogileData[1] = new List<fileData>();

            for (int i = 0; i < 10; i++)        // 10 questions only
            {
                lLogileData[0].Add(new fileData());
                lLogileData[1].Add(new fileData());                
            }
        }

        public void PrintCompleteFile(int mCurQnr)
        {
            PrintLogFileHeader();
            for (int i = 0; i < mCurQnr + 1;i++ )
            {
                AppendToLog(i);
            }
        }

        public void PrintLogFileHeader()
        {
            DateTime curtime = DateTime.Now;
            StreamWriter strw;
            try
            {
                strw = new StreamWriter(mFilename);
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to create logfilefile!");
                return;
            }
            strw.WriteLine("===========================================================");
            strw.WriteLine(string.Format("LogFile of show:{0} ({1}) ({2})", Path.GetFileNameWithoutExtension(mFilename), curtime.ToString("dd-MM-yyyy"), curtime.ToString("HH:mm:ss")));
            strw.WriteLine(string.Format("Contestants:{0}", mContestantName));
            strw.WriteLine("===========================================================");
            strw.WriteLine();

            strw.Close();
        }

        public void FillInLogData(int stacknr, int inQnr, string inRef, string mQuestion, string mAnswer1, string mAnswer2, string mAnswer3, string mAnswer4, string mRightAnswer, string mPronounciation, string mExplanation, int mPlayerAnswer, bool mswitchplayerused, bool mSwitchquestionused, string[,] marticles, long totalthisq, long totalallq, string mSecondQuestion, string mSecondRightAnswer, bool Walkout)
        {
            lLogileData[stacknr][inQnr].mRef = inRef;
            lLogileData[stacknr][inQnr].mQuestion = mQuestion;
            lLogileData[stacknr][inQnr].mAnswer1 = mAnswer1;
            lLogileData[stacknr][inQnr].mAnswer2 = mAnswer2;
            lLogileData[stacknr][inQnr].mAnswer3 = mAnswer3;
            lLogileData[stacknr][inQnr].mAnswer4 = mAnswer4;
            int.TryParse(mRightAnswer, out lLogileData[stacknr][inQnr].mRightAnswer);
            lLogileData[stacknr][inQnr].mRightAnswer--;
            lLogileData[stacknr][inQnr].mPronounciation = mPronounciation;
            lLogileData[stacknr][inQnr].mExplanation = mExplanation;
            lLogileData[stacknr][inQnr].mPlayerAnswer = mPlayerAnswer;
            lLogileData[stacknr][inQnr].mSwitchplayerused = mswitchplayerused;
            lLogileData[stacknr][inQnr].mSwitchquestionused = mSwitchquestionused;
            lLogileData[stacknr][inQnr].mtotalthisq = totalthisq;
            lLogileData[stacknr][inQnr].mTotalallq = totalallq + totalthisq;        // so this is the final new total, not calcutated yet when we stop the rfid scanning
            lLogileData[stacknr][inQnr].mArticlesfromCage = marticles;
            lLogileData[stacknr][inQnr].mSecondQuestion = mSecondQuestion;
            int.TryParse(mSecondRightAnswer, out lLogileData[stacknr][inQnr].mSecondrightanswer);
            lLogileData[stacknr][inQnr].mSecondrightanswer--;
            lLogileData[stacknr][inQnr].mWalkout = Walkout;
        }

        public void AppendToLogPlayerStopped()
        {
            StreamWriter exportstream;
            exportstream = File.AppendText(mFilename);
            exportstream.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
            exportstream.WriteLine("PLAYERS STOPPED");
            exportstream.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
            exportstream.Close();
        }

        public void AppendToLog(int Qnr)
         {
             if (lLogileData[0][Qnr].mQuestion==null)
                 return;
             StreamWriter exportstream;
             exportstream = File.AppendText(mFilename);
             
                 // if second one exist.. skip the first one...
             if (lLogileData[0][Qnr].mQuestion != null)
             {
                 exportstream.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");

                 exportstream.WriteLine(string.Format("Qnr={0}", Qnr + 1));
                 exportstream.WriteLine(string.Format("Reference={0}", lLogileData[0][Qnr].mRef));
                 exportstream.WriteLine(string.Format("Question={0}", lLogileData[0][Qnr].mQuestion.Replace("|", "")));


                 exportstream.WriteLine(string.Format("Answer A={0}", lLogileData[0][Qnr].mAnswer1));
                 exportstream.WriteLine(string.Format("Answer B={0}", lLogileData[0][Qnr].mAnswer2));
                 exportstream.WriteLine(string.Format("Answer C={0}", lLogileData[0][Qnr].mAnswer3));
                 exportstream.WriteLine(string.Format("Answer D={0}", lLogileData[0][Qnr].mAnswer4));



                 if (lLogileData[0][Qnr].mRightAnswer == 0)
                     exportstream.WriteLine(string.Format("Correct Answer=A-{0}", lLogileData[0][Qnr].mAnswer1));
                 else if (lLogileData[0][Qnr].mRightAnswer == 1)
                     exportstream.WriteLine(string.Format("Correct Answer=B-{0}", lLogileData[0][Qnr].mAnswer2));
                 else if (lLogileData[0][Qnr].mRightAnswer == 2)
                     exportstream.WriteLine(string.Format("Correct Answer=C-{0}", lLogileData[0][Qnr].mAnswer3));
                 else if (lLogileData[0][Qnr].mRightAnswer == 3)
                     exportstream.WriteLine(string.Format("Correct Answer=D-{0}", lLogileData[0][Qnr].mAnswer4));


                 exportstream.WriteLine(string.Format("Pronounciation={0}", lLogileData[0][Qnr].mPronounciation));
                 exportstream.WriteLine(string.Format("Explanation={0}", lLogileData[0][Qnr].mExplanation));


                 if (!lLogileData[0][Qnr].mSwitchquestionused)
                 {
                     if (lLogileData[0][Qnr].mPlayerAnswer == 0)
                         exportstream.WriteLine(string.Format("Player Answered=A-{0} - {1}",
                                                              lLogileData[0][Qnr].mAnswer1,
                                                              lLogileData[0][Qnr].mRightAnswer ==
                                                              lLogileData[0][Qnr].mPlayerAnswer
                                                                  ? "CORRECT"
                                                                  : "WRONG"));
                     else if (lLogileData[0][Qnr].mPlayerAnswer == 1)
                         exportstream.WriteLine(string.Format("Player Answered=B-{0} - {1}",
                                                              lLogileData[0][Qnr].mAnswer2,
                                                              lLogileData[0][Qnr].mRightAnswer ==
                                                              lLogileData[0][Qnr].mPlayerAnswer
                                                                  ? "CORRECT"
                                                                  : "WRONG"));
                     else if (lLogileData[0][Qnr].mPlayerAnswer == 2)
                         exportstream.WriteLine(string.Format("Player Answered=C-{0} - {1}",
                                                              lLogileData[0][Qnr].mAnswer3,
                                                              lLogileData[0][Qnr].mRightAnswer ==
                                                              lLogileData[0][Qnr].mPlayerAnswer
                                                                  ? "CORRECT"
                                                                  : "WRONG"));
                     else if (lLogileData[0][Qnr].mPlayerAnswer == 3)
                         exportstream.WriteLine(string.Format("Player Answered=D-{0} - {1}",
                                                              lLogileData[0][Qnr].mAnswer4,
                                                              lLogileData[0][Qnr].mRightAnswer ==
                                                              lLogileData[0][Qnr].mPlayerAnswer
                                                                  ? "CORRECT"
                                                                  : "WRONG"));
                 }
             }

             if (lLogileData[0][Qnr].mSwitchquestionused && !mParent.mSwitchAnswersWithQuestion)
             {
                 exportstream.WriteLine("LIFELINE Question switched used this question!");
                 exportstream.WriteLine(string.Format("Question2={0}", lLogileData[0][Qnr].mSecondQuestion.Replace("|", "")));

               

                 if (lLogileData[0][Qnr].mSecondrightanswer == 0)
                     exportstream.WriteLine(string.Format("Correct Answer=A-{0}", lLogileData[0][Qnr].mAnswer1));
                 else if (lLogileData[0][Qnr].mSecondrightanswer == 1)
                     exportstream.WriteLine(string.Format("Correct Answer=B-{0}", lLogileData[0][Qnr].mAnswer2));
                 else if (lLogileData[0][Qnr].mSecondrightanswer == 2)
                     exportstream.WriteLine(string.Format("Correct Answer=C-{0}", lLogileData[0][Qnr].mAnswer3));
                 else if (lLogileData[0][Qnr].mSecondrightanswer == 3)
                     exportstream.WriteLine(string.Format("Correct Answer=D-{0}", lLogileData[0][Qnr].mAnswer4));

                 if (lLogileData[0][Qnr].mPlayerAnswer == 0)
                     exportstream.WriteLine(string.Format("Player Answered=A-{0} - {1}",
                                                          lLogileData[0][Qnr].mAnswer1,
                                                          lLogileData[0][Qnr].mSecondrightanswer ==
                                                          lLogileData[0][Qnr].mPlayerAnswer
                                                              ? "CORRECT"
                                                              : "WRONG"));
                 else if (lLogileData[0][Qnr].mPlayerAnswer == 1)
                     exportstream.WriteLine(string.Format("Player Answered=B-{0} - {1}",
                                                          lLogileData[0][Qnr].mAnswer2,
                                                          lLogileData[0][Qnr].mSecondrightanswer ==
                                                          lLogileData[0][Qnr].mPlayerAnswer
                                                              ? "CORRECT"
                                                              : "WRONG"));
                 else if (lLogileData[0][Qnr].mPlayerAnswer == 2)
                     exportstream.WriteLine(string.Format("Player Answered=C-{0} - {1}",
                                                          lLogileData[0][Qnr].mAnswer3,
                                                          lLogileData[0][Qnr].mSecondrightanswer ==
                                                          lLogileData[0][Qnr].mPlayerAnswer
                                                              ? "CORRECT"
                                                              : "WRONG"));
                 else if (lLogileData[0][Qnr].mPlayerAnswer == 3)
                     exportstream.WriteLine(string.Format("Player Answered=D-{0} - {1}",
                                                          lLogileData[0][Qnr].mAnswer4,
                                                          lLogileData[0][Qnr].mSecondrightanswer ==
                                                          lLogileData[0][Qnr].mPlayerAnswer
                                                              ? "CORRECT"
                                                              : "WRONG"));
             }
             else if (lLogileData[0][Qnr].mSwitchquestionused && mParent.mSwitchAnswersWithQuestion)
             {
                 // info from second question... in logfiledata[1]
                 exportstream.WriteLine("LIFELINE Question switched used this question!");
                 exportstream.WriteLine(string.Format("Reference={0}", lLogileData[1][Qnr].mRef));
                 exportstream.WriteLine(string.Format("Question2={0}", lLogileData[1][Qnr].mQuestion.Replace("|", "")));

                 exportstream.WriteLine(string.Format("Answer A={0}", lLogileData[1][Qnr].mAnswer1));
                 exportstream.WriteLine(string.Format("Answer B={0}", lLogileData[1][Qnr].mAnswer2));
                 exportstream.WriteLine(string.Format("Answer C={0}", lLogileData[1][Qnr].mAnswer3));
                 exportstream.WriteLine(string.Format("Answer D={0}", lLogileData[1][Qnr].mAnswer4));

                 if (lLogileData[1][Qnr].mRightAnswer == 0)
                     exportstream.WriteLine(string.Format("Correct Answer=A-{0}", lLogileData[1][Qnr].mAnswer1));
                 else if (lLogileData[1][Qnr].mRightAnswer == 1)
                     exportstream.WriteLine(string.Format("Correct Answer=B-{0}", lLogileData[1][Qnr].mAnswer2));
                 else if (lLogileData[1][Qnr].mRightAnswer == 2)
                     exportstream.WriteLine(string.Format("Correct Answer=C-{0}", lLogileData[1][Qnr].mAnswer3));
                 else if (lLogileData[1][Qnr].mRightAnswer == 3)
                     exportstream.WriteLine(string.Format("Correct Answer=D-{0}", lLogileData[1][Qnr].mAnswer4));

                 if (lLogileData[1][Qnr].mPlayerAnswer == 0)
                     exportstream.WriteLine(string.Format("Player Answered=A-{0} - {1}",
                                                          lLogileData[1][Qnr].mAnswer1,
                                                          lLogileData[1][Qnr].mSecondrightanswer ==
                                                          lLogileData[1][Qnr].mPlayerAnswer
                                                              ? "CORRECT"
                                                              : "WRONG"));
                 else if (lLogileData[1][Qnr].mPlayerAnswer == 1)
                     exportstream.WriteLine(string.Format("Player Answered=B-{0} - {1}",
                                                          lLogileData[1][Qnr].mAnswer2,
                                                          lLogileData[1][Qnr].mSecondrightanswer ==
                                                          lLogileData[1][Qnr].mPlayerAnswer
                                                              ? "CORRECT"
                                                              : "WRONG"));
                 else if (lLogileData[1][Qnr].mPlayerAnswer == 2)
                     exportstream.WriteLine(string.Format("Player Answered=C-{0} - {1}",
                                                          lLogileData[1][Qnr].mAnswer3,
                                                          lLogileData[1][Qnr].mSecondrightanswer ==
                                                          lLogileData[1][Qnr].mPlayerAnswer
                                                              ? "CORRECT"
                                                              : "WRONG"));
                 else if (lLogileData[1][Qnr].mPlayerAnswer == 3)
                     exportstream.WriteLine(string.Format("Player Answered=D-{0} - {1}",
                                                          lLogileData[1][Qnr].mAnswer4,
                                                          lLogileData[1][Qnr].mSecondrightanswer ==
                                                          lLogileData[1][Qnr].mPlayerAnswer
                                                              ? "CORRECT"
                                                              : "WRONG"));
             }

            exportstream.WriteLine(string.Format("Total Amount This Question={0}", lLogileData[0][Qnr].mtotalthisq));
            exportstream.WriteLine(string.Format("Total Amount={0}", lLogileData[0][Qnr].mTotalallq));

            if (lLogileData[0][Qnr].mArticlesfromCage != null)
            {
                exportstream.WriteLine("-------------ARTICLE LIST-------------");

           
                int cnt = 0;
                for (int j = 0; j < 20; j++)
                {
                    if (lLogileData[0][Qnr].mArticlesfromCage[j, 0] != "")
                    {
                        exportstream.WriteLine(string.Format("{0}-{1}",
                            lLogileData[0][Qnr].mArticlesfromCage[j, 0],
                            lLogileData[0][Qnr].mArticlesfromCage[j, 1]));
                        cnt++;
                    }
                }
                exportstream.WriteLine(string.Format("Total Articles={0}", cnt));
            }

            if (lLogileData[0][Qnr].mSwitchplayerused)
                exportstream.WriteLine("LIFELINE Player switched used this question!");

            exportstream.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");                              
                                                     
             exportstream.WriteLine("");
             exportstream.Close();             
         }
    }

    class fileData
    {        
        public string mRef;
        public string mQuestion;
        public string mAnswer1;
        public string mAnswer2;
        public string mAnswer3;
        public string mAnswer4;
        public int mRightAnswer;
        public string mPronounciation;
        public string mExplanation;
        public int mPlayerAnswer;
        public bool mSwitchplayerused;
        public bool mSwitchquestionused;
        public string[,] mArticlesfromCage;  // max 20 articles + prijs
        public long mtotalthisq;
        public long mTotalallq;
        public string mSecondQuestion;
        public int mSecondrightanswer;
        public bool mWalkout;


        // default contructor
        public fileData()
        {
            mArticlesfromCage = new string[20,2];

            for (int i=0;i<20;i++)
            {
                mArticlesfromCage[0, 0] = "";
                mArticlesfromCage[0, 1] = "";
            }
        }
    }
}
