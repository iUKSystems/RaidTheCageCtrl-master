using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaidTheCageCtrl
{
    public class CategoryData
    {
        public string mCategory;
    }

    public class QuestionData
    {
        public uint mID;
        public bool mUsed;
        public int mStackID;
        public string mRef="";
        public int mLevel;
        public string mCat="";
        public string mQuestion="";
        public string mQuestion2 = "";
        public string[] mAnswers = new string[4];        
        public bool mApproved;
        public string mPronounciation="";
        public string mExplanation = "";
        public string mExplanation2= "";
        public string mSource="";
        public string mRightAnswer = "";
        public string mRightAnswer2 = "";

        // english...
        public QuestionData()
        {
            mAnswers[0] = "";
            mAnswers[1] = "";
            mAnswers[2] = "";
            mAnswers[3] = "";
        }

        /*
        public QuestionData(uint inID, string inRef, string inCat, string inQuestion, string inAnswer)
        {
            mID = inID;           
            mRef = inRef;           
            mCat = inCat;
            mQuestion = inQuestion;
            mAnswer = inAnswer;                       
        }
         */
    }
}
