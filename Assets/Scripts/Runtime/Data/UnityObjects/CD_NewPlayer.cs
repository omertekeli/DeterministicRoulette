using Runtime.Data.ValueObjects;
using UnityEngine;

namespace Runtime.Data.UnityObjects
{
    [CreateAssetMenu(fileName = "CD_NewPlayer", menuName = "DeterministicRoulette/CD_NewPlayer", order = 0)]
    public class CD_NewPlayer : ScriptableObject
    {
        public NewPlayerData NewPlayerData;
    }
}