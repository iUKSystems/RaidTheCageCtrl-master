using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using com.pakhee.common;

namespace RaidTheCageCtrl
{
    public class SonySoftwareProtectionLicense
    {
        public string mClientstring;
        public string mProjectstring;
        public int mClientID;
        public int mProjectID;
        public DateTime mStartDate;
        public DateTime mEndDate;
        public string mHardwareCode;
        public string mChecksum = "";
        public bool mValid = false;

        [Obfuscation(Exclude = true)]
        public String iv = "c4_mcJPD4nBqXAV3"; //CryptLib.GenerateRandomIV(16); //16 bytes = 128 bits
        [Obfuscation(Exclude = true)]
        public string key = CryptLib.getHashSha256("KayAshDan$2010~+\\2012Patrick!", 32); //32 bytes = 256 bits

        public CryptLib _crypt = new CryptLib();

        private MainForm mParent;

        public SonySoftwareProtectionLicense(MainForm pParent)
        {
            mParent = pParent;
        }

        public bool ReadLicense(string inFile)
        {
            bool error = false;
            
            string tempstring;
            

            if (System.IO.File.Exists(inFile))
            {
                XmlTextReader theReader;
                theReader = new XmlTextReader(inFile);
                if (theReader != null)
                {
                    try
                    {
                        while (theReader.Read())
                        {
                            if (theReader.Name == "License")
                            {
                                // nothing                                
                            }
                            else if (theReader.Name == "clientcode")
                            {
                                theReader.Read();
                                if (theReader.NodeType == XmlNodeType.Text)
                                {
                                    tempstring = theReader.Value;
                                    int.TryParse(tempstring, out mClientID);

                                }
                            }
                            else if (theReader.Name == "productcode")
                            {
                                theReader.Read();
                                if (theReader.NodeType == XmlNodeType.Text)
                                {
                                    tempstring = theReader.Value;
                                    int.TryParse(tempstring, out mProjectID);
                                }
                            }
                            else if (theReader.Name == "hardwarecode")
                            {

                                theReader.Read();
                                if (theReader.NodeType == XmlNodeType.Text)
                                {

                                    mHardwareCode = theReader.Value;

                                }
                            }
                            else if (theReader.Name == "StartDate")
                            {

                                theReader.Read();
                                if (theReader.NodeType == XmlNodeType.Text)
                                {
                                    mStartDate = DateTime.ParseExact(theReader.Value, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);                                    
                                }
                            }
                            else if (theReader.Name == "ExpireDate")
                            {
                                theReader.Read();
                                if (theReader.NodeType == XmlNodeType.Text)
                                {
                                    mEndDate = DateTime.ParseExact(theReader.Value, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);                                    

                                }
                            }
                            else if (theReader.Name == "Checksum")
                            {
                                theReader.Read();
                                if (theReader.NodeType == XmlNodeType.Text)
                                {
                                    mChecksum = theReader.Value;
                                }
                            }                            
                        }
                        theReader.Close();
                        error = false;
                    }
                    catch (XmlException exp)
                    {
                        Console.WriteLine(exp.Message);
                    }
                }
            }
            else
                error = true;

            if (error)
                return false;
            else
                return true;
        }

        public bool CheckLicense()
        {
            bool ok = true;

            string checksum = CalcChecksum();
            // first the checksum...
            if (checksum != mChecksum)
            {
                ok = false;
            }
            else
            {
                // ok cool.. now the enddate...
                DateTime nu = DateTime.UtcNow;
                var date = nu.Date;
                //date = date.AddDays(1);
                //var enddate = mEndDate.AddDays(1);

                if (date > mEndDate)
                {
                    // expired!
                    ok = false;
                }
            }
            return ok;
        }

        private string CalcChecksum()
        {

            // calc and display!
            string tempstring = string.Format("{0}|{1}|{2}|{3}|{4}", mClientID, mProjectID,
                                              mStartDate.Date.ToString("MM/dd/yyyy"),
                                              mEndDate.Date.ToString("MM/dd/yyyy"), mHardwareCode);
            return _crypt.encrypt(tempstring, key, iv);
            
        }


        internal bool CheckNewLicense(string inLicCode)
        {
            bool ok = true;
            int test;

            License thenewlicense = new License();

            uint theCRC = 0;
            if (inLicCode.Length == 24)
            {
                try
                {
                    thenewlicense.mClient = Convert.ToUInt16((inLicCode[2] - 65) * 26 * 26 + (inLicCode[3] - 65) * 26 + (inLicCode[5] - 65));
                    thenewlicense.mProduct = Convert.ToUInt16((inLicCode[6] - 65) * 26 * 26 + (inLicCode[7] - 65) * 26 + (inLicCode[8] - 65));
                    uint temp = Convert.ToUInt16((inLicCode[13] - 65) * 26 * 26 + (inLicCode[15] - 65) * 26 + (inLicCode[16] - 65));
                    thenewlicense.mCRC = Convert.ToUInt32((((inLicCode[10] - 65) * 26 * 26 + (inLicCode[11] - 65) * 26 + (inLicCode[12] - 65)) << 10) + (temp >> 4));
                    thenewlicense.mKey = temp & 15;
                    thenewlicense.mStart = Convert.ToUInt32((inLicCode[17] - 65) * 26 * 26 + (inLicCode[18] - 65) * 26 + (inLicCode[20] - 65));
                    thenewlicense.mEnd = Convert.ToUInt32((inLicCode[21] - 65) * 26 * 26 + (inLicCode[22] - 65) * 26 + (inLicCode[23] - 65));

                    string totalstring = thenewlicense.mClient.ToString() + thenewlicense.mProduct.ToString() + thenewlicense.mKey.ToString() + thenewlicense.mStart.ToString() + thenewlicense.mEnd.ToString();
                    Crc32 newCrc = new Crc32();
                    uint newcrc = newCrc.ComputeChecksum(StrToByteArray(totalstring));
                    newcrc = newcrc & 0x00ffffff;
                    thenewlicense.mValid = newcrc == thenewlicense.mCRC;
                    if (!thenewlicense.mValid)
                    {
                        MessageBox.Show("This License is not valid!");
                        return false;
                    }
                    int returnint = AddLicense(thenewlicense);
                    if (returnint == 0)
                        ok = true;
                    else
                    {
                        string messageboxstring = "";
                        switch (returnint)
                        {
                            case 1:
                                messageboxstring = "This license is not for this client!";
                                break;
                            case 2:
                                messageboxstring = "This license is not for this product!";
                                break;
                            // JPB This is a test change, removing redundant code...
                            //case 3:
                            //    messageboxstring = "This license end date is shorter then the current end date!";
                            //    break;
                            case 4:
                                messageboxstring = "This license end date is already expired!";
                                break;
                            case 5:
                                messageboxstring = "Unable to save new license!";
                                break;
                        }
                        MessageBox.Show(messageboxstring);
                        ok = false;
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("This license is not valid.", "Problem", MessageBoxButtons.OK);
                    ok = false;
                }

                //else
                //    ok = false;
                /*
		        mStart = (inLicCode[17]-'A')*26*26 + (inLicCode[18]-'A')*26 + (inLicCode[20]-'A');
		        mEnd = (inLicCode[21]-'A')*26*26 + (inLicCode[22]-'A')*26 + (inLicCode[23]-'A');
                */
                //SetCRC ();
                //mValid = (theCRC == mCRC);
            }
            else
            {
                MessageBox.Show("This license is not valid.", "Problem", MessageBoxButtons.OK);
                ok = false;
            }







            //newCrc.ComputeChecksum(
            return ok;
        }

        private int AddLicense(License thenewlicense)
        {
            int returnint = 0; // everything ok

            // more like an update...
            // ok, check first if it's for this client.. so check the client and project id.
            if (mClientID != thenewlicense.mClient || mProjectID != thenewlicense.mProduct)
            {
                if (mClientID != thenewlicense.mClient)
                    return 1;
                if (mProjectID != thenewlicense.mProduct)
                    return 2;

            }

            // cool... so far so good.. now check if the expire date later is then the current license...
            DateTime newexpire = SystemTimeFromDaysSince2000(Convert.ToInt32(thenewlicense.mEnd));

            //if (mEndDate > newexpire)
            //    return 3;

            if (DateTime.UtcNow > newexpire)
                return 4;   // already expired...

            // ok.. so far so good... we have a new license...
            // write to the file... set the new startdate and enddate...
            mEndDate = newexpire;
            mStartDate = SystemTimeFromDaysSince2000(Convert.ToInt32(thenewlicense.mStart));

            // calculate new checksum
            mChecksum = CalcChecksum();

            if (!SaveLicense(mParent.ExePath + @"\rtc.lic"))
                return 5;

            return 0;
        }

        public bool SaveLicense(string inFilename)
        {
            bool theRetVal = true;
            XmlTextWriter theWriter = new XmlTextWriter(inFilename, System.Text.Encoding.UTF8);
            theWriter.Formatting = Formatting.Indented;


            try
            {
                theWriter.WriteStartDocument();
                theWriter.WriteStartElement("License");
                theWriter.WriteElementString("clientcode", mClientID.ToString());
                theWriter.WriteElementString("productcode", mProjectID.ToString());
                theWriter.WriteElementString("hardwarecode", mHardwareCode);
                theWriter.WriteElementString("StartDate", mStartDate.ToString("MM/dd/yyyy"));
                theWriter.WriteElementString("ExpireDate", mEndDate.ToString("MM/dd/yyyy"));
                theWriter.WriteElementString("Checksum", mChecksum);
                theWriter.WriteEndElement();
                theWriter.WriteEndDocument();
                theWriter.Close();
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
                theRetVal = false;
            }
            return theRetVal;
        }

        public string GetSerialHardisk()
        {
            ManagementObject dsk = new ManagementObject(@"win32_logicaldisk.deviceid=""c:""");
            dsk.Get();
            return dsk["VolumeSerialNumber"].ToString();            
        }

        public List<string> GetSerialHardisk2()
        {
            List<string> harddiskserials = new List<string>();

            var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");

            foreach (ManagementObject wmi_HD in searcher.Get())
            {
                try
                {
                    string SerialNo = wmi_HD.GetPropertyValue("SerialNumber").ToString();//get the serialNumber of diskdrive

                    //string group = wmi_HD.GetPropertyValue("Name").ToString();

                    harddiskserials.Add(SerialNo);
                }
                catch (Exception)
                {


                }


                /*
                if (group == "\\\\.\\PHYSICALDRIVE0")
                {
                    harddiskserial = SerialNo;
                    //return SerialNo;
                }
                 */
            }

            return harddiskserials;
        }

        public static byte[] StrToByteArray(string str)
        {
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            return encoding.GetBytes(str);
        }

        static readonly int[] sMonth = new int[] { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
        static readonly int[] sMonth2Day = new int[] { 0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334 };
        static readonly int[] sMonth2DayS = new int[] { 0, 31, 60, 91, 121, 152, 182, 213, 244, 274, 305, 335 };
        
        public DateTime SystemTimeFromDaysSince2000(int inTime)
        {
            int year, month, day;

            year = 0;
            month = 0;
            day = 0;



            year = 4 * (inTime / (4 * 365 + 1));
            inTime -= (year / 4) * (4 * 365 + 1);
            if (inTime >= 366)
            {
                year++;
                inTime -= 366;
                if (inTime >= 365)
                {
                    year++;
                    inTime -= 365;
                    if (inTime >= 365)
                    {
                        year++;
                        inTime -= 365;
                        if (inTime >= 365)
                        {
                            year++;
                            inTime -= 365;
                        }
                    }
                }
            }
            year += 2000;
            for (int i = 11; i >= 0; i--)
            {
                if (inTime >= (((year & 3) != 0) ? sMonth2Day[i] : sMonth2DayS[i]))
                {
                    month = i + 1;
                    inTime -= (((year & 3) != 0) ? sMonth2Day[i] : sMonth2DayS[i]);
                    break;
                }
            }
            day = inTime + 1;
            DateTime returndate = new DateTime(year, month, day);
            return returndate;

        }
    }
}
