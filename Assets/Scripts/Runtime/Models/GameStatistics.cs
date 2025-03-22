namespace Runtime.Models
{
    public class GameStatistics
    {
        public int TotalRounds { get; private set; }
        public int ProfitLoss { get; private set; }
        public int WonRounds { get; private set; }
        
        public GameStatistics(int totalRounds, int profitLoss, int wonRounds)
        {
            TotalRounds = totalRounds;
            ProfitLoss = profitLoss;
            WonRounds = wonRounds;
        }

        public void AddRound(int roundProfitLoss, bool wonRound)
        {
            TotalRounds++;
            ProfitLoss += roundProfitLoss;
            if (wonRound)
                WonRounds++;
        }
    }
}

