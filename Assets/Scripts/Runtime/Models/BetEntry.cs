using Runtime.Enums;

namespace Runtime.Keys
{
    public class BetEntry
    {
        public BetTypes BetType;
        public int Amount;
        public int TotalBetTimes;

        public BetEntry(BetTypes betType, int amount)
        {
            this.BetType = betType;
            this.Amount = amount;
            this.TotalBetTimes = 1;
        }
    }
}