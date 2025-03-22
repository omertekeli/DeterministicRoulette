using Runtime.Data.UnityObjects;
using Runtime.Data.ValueObjects;
using UnityEngine;

namespace Runtime.Handlers
{
    public static class PlayerDataHandler
    {
        private static NewPlayerData _playerData;
        private static bool _isLoaded = false;
        public static NewPlayerData PlayerData
        {
            get
            {
                if (_isLoaded) return _playerData;
                _isLoaded = true;
                LoadFirstTimePlayerData();
                return _playerData;
            }
        }
        
        private static void LoadFirstTimePlayerData()
        {
            _playerData = Resources.Load<CD_NewPlayer>("Data/CD_NewPlayer").NewPlayerData;
        }
    }
}