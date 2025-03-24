using System.Collections.Generic;
using System.Linq;
using Runtime.Data.ValueObjects;
using Runtime.Keys;
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

        public static int GetWinningNumber(Dictionary<string, BetEntry> playerBets, Dictionary<string, int[]> betData)
        {
            foreach (var bet in playerBets)
            {
                if (!betData.TryGetValue(bet.Key, out var numbers))
                    continue; 
                return numbers[Random.Range(0, numbers.Length)];
            }
            return Random.Range(0, 37);
        }
    }
}