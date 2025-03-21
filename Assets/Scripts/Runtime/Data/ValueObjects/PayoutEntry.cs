using System;
using Runtime.Enums;

namespace Runtime.Data.ValueObjects
{
    [Serializable]
    public struct PayoutEntry
    {
        public BetTypes betType;
        public byte multiplier;
    }
}