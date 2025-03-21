using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.Data.UnityObjects;
using Runtime.Data.ValueObjects;
using Runtime.Enums;
using Runtime.Handlers;
using Runtime.Keys;
using Runtime.Signals;
using Runtime.Utilies;
using UnityEngine;

namespace Runtime.Managers
{ 
    public class BetManager: MonoBehaviour
    {
        #region Self Variables

        #region Private Variables

        private int _betAmount;
        private int _chipAmount;
        private NewPlayerData _playerData;
        private Dictionary<ChipTypes, int> _chipData;
        private Dictionary<BetTypes, byte> _betPayoutData;
        private Dictionary<string, int[]> _betData;
        private Dictionary<string, BetEntry> _playerBets = new Dictionary<string, BetEntry>(); 

        #endregion

        #endregion

        private void Awake()
        {
            //check player is First time user o not
            LoadFirstTimePlayerData();
            LoadChipData();
            LoadBetPayoutData();
            _betAmount = 0;
        }

        private void Start()
        {
            _betData = BetDataHandler.BetData;
        }

        private void OnEnable() => SubscribeEvents();
        private void OnDisable() => UnSubscribeEvents();
        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onPlaceBet += OnPlaceBet;
            CoreGameSignals.Instance.onSpinResult += OnSpinResult;
            UISignals.Instance.onChooseChip += OnChooseChip;
        }
        
        private void UnSubscribeEvents()
        {
            CoreGameSignals.Instance.onPlaceBet -= OnPlaceBet;
            UISignals.Instance.onChooseChip -= OnChooseChip;
        }

        private void LoadBetPayoutData() => _betPayoutData = Resources.Load<CD_BetPayout>("Data/CD_BetPayout").ToDictionary();
        private void LoadChipData() => _chipData = Resources.Load<CD_Chips>("Data/CD_Chips").ToDictionary();
        
        private void LoadFirstTimePlayerData()
        {
            _playerData = Resources.Load<CD_NewPlayer>("Data/CD_NewPlayer").NewPlayerData;
            _chipAmount = _playerData.Chips;
            Debug.Log("Player chip amount: " + _chipAmount);
        }
        
        private void OnSpinResult(int winningNumber)
        {   
            BetEvaluationContext betEvaluationContext = new BetEvaluationContext(
                winningNumber,
                _playerBets,
                _betData,
                _betPayoutData
                );
            var profit = BetEvaluator.EvaluateBets(betEvaluationContext);
            UpdateChipBalance(profit);
            ClearPlayerBets();
        }
        
        private void OnPlaceBet(BetParams betParams)
        {
            if (_betAmount < 0) return;
            if (_chipAmount < _betAmount) return;
            UpdateChipBalance(-_betAmount);
            AddBet(betParams, _betAmount);
        }
        
        private void OnChooseChip(ChipTypes chipType) => _betAmount = _chipData.GetValueOrDefault(chipType, 0);
        private void AddBet(BetParams betParams, int betAmount)
        {
            if(_playerBets.ContainsKey(betParams.BetName))
            {
                _playerBets[betParams.BetName].Amount += betAmount;
                Debug.Log("Player increase bet on " + betParams.BetName + " and new bet amount is " + _playerBets[betParams.BetName].Amount);
            }
            else
            {
                _playerBets.Add(betParams.BetName, new BetEntry(betParams.BetType, betAmount));
                Debug.Log("Player bets on " + betParams.BetName + " by amount " + betAmount);
            }
        }

        private void UpdateChipBalance(int amount) =>  _chipAmount += amount;
        private void ClearPlayerBets() => _playerBets.Clear();
    }
}