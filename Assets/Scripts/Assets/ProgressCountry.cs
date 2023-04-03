using System;
using System.Collections.Generic;

namespace Assets
{
    [Serializable]
    public class ProgressCountry
    {
        public byte Id;
        public byte IsPlayerCountry;
        public List<byte> OwnRegionsId;
    }
}