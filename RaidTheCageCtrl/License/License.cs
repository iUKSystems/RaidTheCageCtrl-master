using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace RaidTheCageCtrl
{
    [Serializable]
    [StructLayout(LayoutKind.Explicit, Size = 12, Pack=1)]
    public struct MESSAGE_LOG_HEADER_STRUCT2
    {
        [FieldOffset(0)]
        public ushort mClient;
        [FieldOffset(2)]
        public ushort mProduct;
        [FieldOffset(4)]
        public uint mCRC;
        [FieldOffset(7)]
        public byte mKey;
        [FieldOffset(8)]
        public ushort mStart;
        [FieldOffset(10)]
        public ushort mEnd;
    }

    public class License
    {
        public License()
        {
            mClient=0;
            mProduct = 0;
            mCRC = 0;
            mKey = 0;
            mStart = 0;
            mEnd = 0;
            mClientName = "";
            mValid = false;
        }
        
        public uint mClient;
        public uint mProduct;
        public uint mCRC;
        public uint mKey;
        public uint mStart;
        public uint mEnd;

        public string mClientName;

        public bool mValid;

        

        public bool CheckLicenseHasp()
        {
            bool ok = true;

            return ok;
        }

        
    }
}
