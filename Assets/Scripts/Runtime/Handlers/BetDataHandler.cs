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
            if (betDict == null)
            {
                Debug.LogError("JSON deserialization başarısız!");
            }
            else if (betDict.bets == null)
            {
                Debug.LogError("BetDictionary.bets içi boş!");
            }
            else
            {
                _betData = betDict.ToDictionary();
                Debug.Log("BetData başarıyla yüklendi!");
            }
            if (jsonFile == null) return;
            Debug.Log("BetData JSON içeriği: " + jsonFile.text);
            _betData = JsonUtility.FromJson<BetDictionary>(jsonFile.text).ToDictionary();
            Debug.Log("Bet data loaded");
            foreach (var bet in _betData)
            {
                Debug.Log($"Bahis Tipi: {bet.Key}, Sayılar: {string.Join(", ", bet.Value)}");
            }
                
        }
    }
}