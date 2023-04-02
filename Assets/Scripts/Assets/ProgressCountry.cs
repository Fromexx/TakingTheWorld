using System;
using System.Collections.Generic;

namespace Assets
{
    [Serializable]
    public class ProgressCountry
    {
        public byte Id;
        public List<byte> OwnRegionsId;
    }
}