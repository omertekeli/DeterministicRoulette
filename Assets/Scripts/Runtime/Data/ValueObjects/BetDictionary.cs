using System;
using System.Collections.Generic;
using Runtime.Keys;

namespace Runtime.Data.ValueObjects
{
    [Serializable]
    class BetDictionary
    {
        public List<BetJsonData> bets;

        public Dictionary<string, int[]> ToDictionary()
        {
            Dictionary<string, int[]> dict = new Dictionary<string, int[]>();
            if (bets == null) return dict; 
            foreach (var bet in bets)
            {
                dict[bet.key] = bet.values;
            }
            return dict;
        }
    }
}