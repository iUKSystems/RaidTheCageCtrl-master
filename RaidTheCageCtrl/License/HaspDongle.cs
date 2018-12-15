using System;
using System.Collections.Generic;

using System.Text;
using System.Runtime.InteropServices;
using System.Collections;
using System.Windows.Forms;
using Aladdin.HASP;

namespace RaidTheCageCtrl
{
    
    public class HaspDongle
    {
        public int mProductcode;
        // crc shit
        uint[] mCRCTable;

        FeatureOptions options;
        HaspStatus status;
        HaspFeature feature;
        Hasp hasp;
        HaspFile file;
       
        int mKey; // 0 = unvalid, 1 = AZIOK, 2 = YWYZX, 3- QTZED (wiseguys)	    
	    int mHaspHandle;

	    int mSize;
	    int mDate;

	    public ushort mClient;
	    ushort mLicenseCount;

        List<License> mLicenses;
 

        /// <summary>
        /// constructor
        /// </summary>
        public HaspDongle(int inProductcode, ushort inClientcode)
        {
            mProductcode = inProductcode;//1001;// 1050;
            mCRCTable = new uint[256];
            mKey = 0;
            mClient = inClientcode;
            mLicenseCount = 0;

            options = FeatureOptions.NotRemote;   
            feature = HaspFeature.ProgNumDefault;
            feature.SetOptions(options, FeatureOptions.Default);

            hasp = new Hasp(feature);

            mLicenses = new List<License>();

            // first get the right key!
            using (hasp)
            {
                mKey = GetKey();
                if (mKey != 0 && hasp != null)
                {
                    GetGeneralInfo();
                    if (mLicenseCount>0)
                        GetLicenses();
                }
            }
        }

       
        private void GetGeneralInfo()
        {
            // date time first
            // get size of 


            DateTime hasptime = DateTime.Now;
            status = hasp.GetRtc(ref hasptime);
            if (status == HaspStatus.NoTime)
            {
                hasptime = DateTime.Now;
            }
            // create 
            mDate = DaysSince2000FromSystemTime(hasptime);
            /*
            
                if (hasp_hasptime_to_datetime(theTime, &theDay, &theMonth, &theYear, &theHour, &theMinute, &theSecond) == HASP_STATUS_OK)
            {
                SYSTEMTIME theSystemTime;
                theSystemTime.wYear = theYear;
                theSystemTime.wMonth = theMonth;
                theSystemTime.wDay = theDay;
                theSystemTime.wHour = 0;
                theSystemTime.wMinute = 0;
                theSystemTime.wSecond = 0;
                theSystemTime.wMilliseconds = 0;
                mDate = DaysSince2000FromSystemTime(theSystemTime);
            }
            */

            byte[] signaturebytes = null;
            byte[] clientbytes = null;
            byte[] licensesbytes = null;
            signaturebytes=new byte[4];
            clientbytes = new byte[2];
            licensesbytes = new byte[2];
            // read from Memory
            
            file = hasp.GetFile(HaspFiles.Main);

            uint signature;
            file.FilePos = 0;
            file.Read(signaturebytes, 0, 4);
            signature = BitConverter.ToUInt32(signaturebytes, 0);

            if (signature != 1400456047)
            {
                mLicenseCount = 0;
            }
            else
            {
                file.FilePos = 4;
                // check how many licenses we have
                file.Read(clientbytes, 0, 2);
                //mClient = BitConverter.ToUInt16(clientbytes, 0);

                file.FilePos = 6;
                // and the number of licenses
                file.Read(licensesbytes, 0, 2);
                mLicenseCount = BitConverter.ToUInt16(licensesbytes, 0);
            }
        }

        private void GetLicenses()
        {
            byte[] all = null;
            all = new byte[12];

            for (int lic = 0; lic < mLicenseCount; lic++)
            {
                License templicense = new License();

                file.FilePos = 24+(lic*12);
                // client id
                file.Read(all, 0, 12);

                MESSAGE_LOG_HEADER_STRUCT2 temp2;
                temp2.mCRC = 0;
                temp2.mKey = 0;
                //object pat = RawDeserialize(all, 0, Marshal.SizeOf(MESSAGE_LOG_HEADER_STRUCT2));
                temp2 = (MESSAGE_LOG_HEADER_STRUCT2)BytesToStruct(all);
                templicense.mClient = temp2.mClient;
                templicense.mProduct = temp2.mProduct;
                templicense.mKey = temp2.mKey;
                templicense.mStart = temp2.mStart;
                templicense.mEnd = temp2.mEnd;
                temp2.mCRC = temp2.mCRC & 0x00ffffff;
                //temp2.mCRC = temp2.mCRC & 0x00ffffff;
                templicense.mCRC = temp2.mCRC;
                templicense.mValid = true;
                mLicenses.Add(templicense);
                //temp2.mCRC = temp2.mCRC & 0x00ffffff;

            }
        }

        public MESSAGE_LOG_HEADER_STRUCT2 BytesToStruct(byte[] dataIn)
        {
            GCHandle hDataIn = GCHandle.Alloc(dataIn, GCHandleType.Pinned);
            MESSAGE_LOG_HEADER_STRUCT2 ys = (MESSAGE_LOG_HEADER_STRUCT2)Marshal.PtrToStructure(hDataIn.AddrOfPinnedObject(), typeof(MESSAGE_LOG_HEADER_STRUCT2));
            hDataIn.Free();
            return ys;
        }

        public static byte[] RawSerialize(object anything)
        {
            int rawSize = Marshal.SizeOf(anything);
            IntPtr buffer = Marshal.AllocHGlobal(rawSize);
            Marshal.StructureToPtr(anything, buffer, false);
            byte[] rawDatas = new byte[rawSize];
            Marshal.Copy(buffer, rawDatas, 0, rawSize);
            Marshal.FreeHGlobal(buffer);
            return rawDatas;
        }

        private int GetKey()
        {
            

            int key = 0;

            
                status = hasp.Login(VendorCode.Code_AZIOK);
                if (status == HaspStatus.StatusOk || status == HaspStatus.NoTime)
                {
                    key = 1;
                }

                if (key == 0)
                {
                    status = hasp.Login(VendorCode.Code_YWYZX);
                    if (status == HaspStatus.StatusOk || status == HaspStatus.NoTime)
                    {
                        key = 2;
                    }
                }

                if (key == 0)
                {
                    status = hasp.Login(VendorCode.Code_QTZED);
                    if (status == HaspStatus.StatusOk || status == HaspStatus.NoTime)
                    {
                        key = 3;
                    }
                }
            return key;
        }

        public bool AddLicense(License inLicense)
        {
            byte[] signaturebytes = null;
            byte[] clientbytes = null;
            byte[] licensesbytes = null;
            signaturebytes=new byte[4];
            clientbytes = new byte[2];
            licensesbytes = new byte[2];

            /*
            // test quickly add license in dongle..
            // fill the struct first
            MESSAGE_LOG_HEADER_STRUCT2 temp2;
            temp2.mClient = Convert.ToUInt16(inLicense.mClient);
            temp2.mCRC = inLicense.mCRC;
            temp2.mEnd = Convert.ToUInt16(inLicense.mEnd);
            temp2.mKey = Convert.ToByte(inLicense.mKey);
            temp2.mProduct = Convert.ToUInt16(inLicense.mProduct);
            temp2.mStart = Convert.ToUInt16(inLicense.mStart);

            byte[] all = null;
            byte[] licensesbytes = null;
            licensesbytes = new byte[2];
            all = new byte[12];

            all=RawSerialize(temp2);

            file.FilePos = 24 + (mLicenseCount*24);
            // client id
            status = file.Write(all, 0, 12);

            // and put license count on 1
            file.FilePos = 6;
            // and the number of licenses
            licensesbytes = BitConverter.GetBytes((uint)mLicenseCount+1);
            status = file.Write(licensesbytes, 0, 2);
           */


            if (!inLicense.mValid) {
		    MessageBox.Show("This license is not valid.", "Problem", MessageBoxButtons.OK);
		    return false;
	        }
	        if (!(mKey != 0 && mHaspHandle != -1)) {
		        MessageBox.Show("Insert a valid dongle.", "Problem",MessageBoxButtons.OK);
		        return false;
	        }
	        if (mClient == 0 || mLicenseCount == 0) {
		       //uint theSignature = 1400456047;
                signaturebytes = BitConverter.GetBytes(1400456047);
                //signaturebytes[0] = 0x6f;
                //signaturebytes[1] = 0x43;
                //signaturebytes[2] = 0x79;
                //signaturebytes[3] = 0x53;
		       // mClient = inLicense.mClient;
                System.Text.ASCIIEncoding  encoding=new System.Text.ASCIIEncoding();
                //signaturebytes = encoding.GetBytes(theSignature.ToString());
                file.FilePos = 0;
                status = file.Write(signaturebytes, 0, 4);
                clientbytes = BitConverter.GetBytes(inLicense.mClient);
                file.FilePos = 4;
                status = file.Write(clientbytes, 0, 2);
                licensesbytes = BitConverter.GetBytes(1);
                file.FilePos = 6;
                status = file.Write(licensesbytes, 0, 2);

                WriteLicense(inLicense,0,false);
                return true;
	        }
            
	        if (mClient != inLicense.mClient) {
		        MessageBox.Show("This license has been prepared for a different dongle.", "Problem", MessageBoxButtons.OK);
		        return false;
	        }

	        for (int i = 0 ; i < mLicenses.Count ; i++) {
		        if ((mLicenses[i].mProduct == inLicense.mProduct) &&
                    (mLicenses[i].mClient == inLicense.mClient)&&
                    (mLicenses[i].mStart == inLicense.mStart)&&
                    (mLicenses[i].mEnd == inLicense.mEnd))
                {
			        MessageBox.Show("This license already exists on this dongle.", "Problem", MessageBoxButtons.OK);
			        return false;
		        }
	        }

	        for (int i = 0 ; i < mLicenses.Count ; i++) {
		         if ((mLicenses[i].mProduct == inLicense.mProduct) &&
                    (mLicenses[i].mClient == inLicense.mClient))
                 {
                     // same product and client...
                     if (mLicenses[i].mEnd < inLicense.mEnd)
                     {

                         // write over expired license or update license!
                         WriteLicense(inLicense, i, true);
                         return true;
                     }
                     else
                         MessageBox.Show("Newer license already exists on this dongle.", "Problem", MessageBoxButtons.OK);
                     return false;
                    /*
			        mLicenses[i] = inLicense;
			        hasp_write (mHaspHandle, HASP_FILEID_MAIN, 24+i*12, 12, &inLicense);
        //			mLicenseCount++;
			        hasp_write (mHaspHandle, HASP_FILEID_MAIN, 6, 2, &mLicenseCount);
			        Beep (440, 200);
                     */
			        
		        }
	        }

	        //if (mLicenseCount < (mSize - 24)/12) {
		        WriteLicense(inLicense,mLicenseCount,false);
		        return true;
	        //}

            //MessageBox.Show("There is no room in this dongle for the license.", "Problem", MessageBoxButtons.OK);
	        //return;
            
        }

        public void WriteLicense(License inLicense, int inLicNr, bool replace)
        {
            MESSAGE_LOG_HEADER_STRUCT2 temp2;
            temp2.mClient = Convert.ToUInt16(inLicense.mClient);
            temp2.mCRC = inLicense.mCRC;
            temp2.mEnd = Convert.ToUInt16(inLicense.mEnd);
            temp2.mKey = Convert.ToByte(inLicense.mKey);
            temp2.mProduct = Convert.ToUInt16(inLicense.mProduct);
            temp2.mStart = Convert.ToUInt16(inLicense.mStart);

            byte[] all = null;
            byte[] licensesbytes = null;
            licensesbytes = new byte[2];
            all = new byte[12];

            all = RawSerialize(temp2);

            file.FilePos = 24 + (inLicNr * 12);
            // client id
            status = file.Write(all, 0, 12);

            // and put license count on 1
            file.FilePos = 6;
            // and the number of licenses
            licensesbytes = BitConverter.GetBytes((uint)mLicenseCount + (replace?0:1));
            status = file.Write(licensesbytes, 0, 2);
        }

        public void DeleteLicense(int inIndex)
        {

        }
        public License VerifyLicense(int inProduct, short inCLient)
        {
	        for (int i = 0 ; i < mLicenses.Count ; i++) 
            {
                if (mLicenses[i].mProduct == inProduct && mLicenses[i].mClient == inCLient &&
			        mLicenses[i].mStart <= DaysSince2000FromNow (mDate) &&
			        mLicenses[i].mEnd >= DaysSince2000FromNow (mDate)) 
                {
                    return mLicenses[i];
		        }
	        }
	        return null;
        }
        static readonly int[] sMonth = new int[] { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
        static readonly int[] sMonth2Day = new int[] { 0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334 };
        static readonly int[] sMonth2DayS = new int[] {0, 31, 60, 91, 121, 152, 182, 213, 244, 274, 305, 335};    
      

        private int DaysSince2000FromSystemTime(DateTime inTime)
        {
           int theTime =
         (inTime.Year - 2000) * 365 + // jaren
         (inTime.Year - 2000 + 3) / 4 + // schrikkeldagen
         sMonth2Day[inTime.Month - 1] + // maanden
         inTime.Day - 1 + // dagen
         (((inTime.Year % 4) == 0 && inTime.Month > 2) ? 1 : 0); // schrikkeldag
            return theTime;
        }

        private int DaysSince2000FromNow(int inDate)
        {
            if (inDate != 0) return inDate; // time from dongle
            DateTime theTime = DateTime.Now;
            return DaysSince2000FromSystemTime(theTime);
        }


        
        public DateTime SystemTimeFromDaysSince2000 (int inTime)
        {
            int year, month, day;

            year = 0;
            month = 0;
            day = 0;

            

            year = 4 * (inTime / (4 * 365 + 1));
            inTime -= (year / 4) * (4 * 365 + 1);
	        if (inTime >= 366) {
                year++;
		        inTime -= 366;
		        if (inTime >= 365) {
                    year++;
			        inTime -= 365;
			        if (inTime >= 365) {
                        year++;
				        inTime -= 365;
				        if (inTime >= 365) {
                            year++;
					        inTime -= 365;
				        }
			        }
		        }
	        }
            year += 2000;
	        for (int i = 11 ; i >= 0 ; i--) {
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

        public bool CheckNewLicense(string inLicCode,int inProduct)
        {
            bool ok = true;            

            License thenewlicense = new License();

            //uint theCRC = 0;
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
                    //if (thenewlicense.mValid && inProduct == thenewlicense.mProduct)
                    if (AddLicense(thenewlicense))
                        ok = true;
                    else
                        ok = false;
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

        public static byte[] StrToByteArray(string str)
        {
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            return encoding.GetBytes(str);
        }
    }

    public class Crc32
    {
        uint[] table;

        public uint ComputeChecksum(byte[] bytes)
        {
            uint crc = 0xffffffff;
            for (int i = 0; i < bytes.Length; ++i)
            {
                //value = (value >> 8) ^ mCRCTable[(value & 0xFF) ^ *buffer++];
                crc = (uint)((crc >> 8) ^ (table[(crc & 0xFF) ^ bytes[i]]));

                //value = (value >> 8) ^ mCRCTable[(value & 0xFF)];

                //byte index = (byte)(((crc) & 0xff) ^ bytes[i]);
                //crc = (uint)((crc >> 8) ^ (table[index] & 0xFF));
            }
            return ~crc ^ 0xFFFFFFFF; ;
        }

        public Crc32()
        {
            uint poly = 0xedb88320;
            table = new uint[256];
            uint temp = 0;
            for (int i = 0; i < table.Length; ++i)
            {
                temp = (uint)i;
                for (int j = 8; j > 0; --j)
                {
                    if ((temp & 1) == 1)
                    {
                        temp = (uint)((temp >> 1) ^ poly);
                    }
                    else
                    {
                        temp >>= 1;
                    }
                }
                table[i] = temp;
            }
        }
    }

}
