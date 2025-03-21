using System.Collections.Generic;
using Runtime.Data.ValueObjects;
using Runtime.Enums;
using UnityEngine;

namespace Runtime.Data.UnityObjects
{
    [CreateAssetMenu(fileName = "CD_Chips", menuName = "DeterministicRoulette/CD_Chips", order = 1)]
    public class CD_Chips : ScriptableObject
    {
        public List<ChipData> Chips = new List<ChipData>();
        public Dictionary<ChipTypes, int> ToDictionary()
        {
            Dictionary<ChipTypes, int> dict = new Dictionary<ChipTypes, int>();
            foreach (ChipData chip in Chips)
            {
                dict[chip.chipType] = chip.value;
            }
            return dict;
        }
    }
}