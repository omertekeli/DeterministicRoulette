using System.Collections.Generic;
using Runtime.Data.UnityObjects;
using Runtime.Data.ValueObjects;
using Runtime.Enums;
using UnityEngine;

namespace Runtime.Handlers
{
    public class ChipDataHandler
    {
        private static Dictionary<ChipTypes, int> _chipData;
        private static bool _isLoaded = false;
        public static Dictionary<ChipTypes, int>  ChipData
        {
            get
            {
                if (_isLoaded) return _chipData;
                _isLoaded = true;
                LoadChipData();
                return _chipData;
            }
        }
        
        private static void LoadChipData()
        {
            _chipData = Resources.Load<CD_Chips>("Data/CD_Chips").ToDictionary();
        }
    }
}