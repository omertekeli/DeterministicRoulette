using UnityEngine;
using System.Collections.Generic;
using Runtime.Data.ValueObjects;
using Runtime.Enums;

[CreateAssetMenu(fileName = "CD_BetPayout", menuName = "DeterministicRoulette/CD_BetPayout")]
public class CD_BetPayout : ScriptableObject
{
    public PayoutEntry[] payouts;
    public Dictionary<BetTypes, byte> ToDictionary()
    {
        Dictionary<BetTypes, byte> dict = new Dictionary<BetTypes, byte>();
        foreach (var entry in payouts)
        {
            dict[entry.betType] = entry.multiplier;
        }
        return dict;
    }
}