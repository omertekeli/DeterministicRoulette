using System;
using Runtime.Enums;

namespace Runtime.Data.ValueObjects
{
    [Serializable]
    public struct ChipData
    {
        public ChipTypes chipType;
        public int value;
    }
}