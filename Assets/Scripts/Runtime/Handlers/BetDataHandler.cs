using UnityEngine;
using System.Collections.Generic;
using Runtime.Data.ValueObjects;

namespace Runtime.Handlers
{
    public static class BetDataHandler
    {
        private static Dictionary<string, int[]> _betData;

        public static Dictionary<string, int[]> BetData
        {
            get
            {
                if (_betData == null)
                {
                    LoadBetData();
                }
                return _betData;
            }
        }

        private static void LoadBetData()
        {
            TextAsset jsonFile = Resources.Load<TextAsset>("Data/Bets");
            BetDictionary betDict = JsonUtility.FromJson<BetDictionary>(jsonFile.text);
            _betData = betDict.ToDictionary();
            if (jsonFile == null) return;
            _betData = JsonUtility.FromJson<BetDictionary>(jsonFile.text).ToDictionary();
        }
    }
}