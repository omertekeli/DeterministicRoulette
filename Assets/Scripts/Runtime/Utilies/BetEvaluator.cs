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
                Debug.Log("Bet name: " + bet.Key);
                if (!context.BetNumbers.TryGetValue(bet.Key, out var numbers))
                    continue; 
                if (!numbers.Contains(context.WinningNumber))
                    continue;
                Debug.Log("Player bet amount: " + bet.Value.Amount);
                Debug.Log("Profit from bet: " + bet.Value.Amount * context.PayoutRatios[bet.Value.BetType]);
                profit += bet.Value.Amount * context.PayoutRatios[bet.Value.BetType];
            }
            Debug.Log("Total profit: " + profit);
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