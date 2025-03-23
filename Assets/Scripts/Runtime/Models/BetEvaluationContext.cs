using System.Collections.Generic;
using Runtime.Enums;
using Runtime.Keys;

namespace Runtime.Data.ValueObjects
{
    public class BetEvaluationContext
    {
        public int WinningNumber { get; private set; }
        public Dictionary<string, BetEntry> PlayerBets { get; private set; }
        public Dictionary<string, int[]> BetNumbers { get; private set; }
        public Dictionary<BetTypes, byte> PayoutRatios { get; private set; }

        public BetEvaluationContext(
            int winningNumber,
            Dictionary<string, BetEntry> playerBets,
            Dictionary<string, int[]> betNumbers, 
            Dictionary<BetTypes, byte> payoutRatios)
        {
            PlayerBets = playerBets;
            WinningNumber = winningNumber;
            BetNumbers = betNumbers;
            PayoutRatios = payoutRatios;
        }
    }
}