using System.Linq;
using Runtime.Data.ValueObjects;
using UnityEngine;

namespace Runtime.Utilies
{
    public static class BetEvaluator
    {
        public static int EvaluateBets(BetEvaluationContext context)
        {
            int profit = 0;
            foreach (var bet in context.PlayerBets)
            {
                if (!context.BetNumbers.TryGetValue(bet.Key, out var numbers))
                    continue; 
                if (!numbers.Contains(context.WinningNumber))
                    continue;
                profit += bet.Value.Amount * context.PayoutRatios[bet.Value.BetType];
                Debug.Log("Winner bet: " + bet.Key);
                Debug.Log("Profit: " + profit);
            }
            Debug.Log("Total Profit: " + profit);
            return profit;
        }
    }
}