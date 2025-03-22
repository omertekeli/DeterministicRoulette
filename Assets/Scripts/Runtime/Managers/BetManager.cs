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
        private int _turnTotalBetAmount;
        private NewPlayerData _playerData;
        private Dictionary<BetTypes, byte> _betPayoutData;
        private Dictionary<string, int[]> _betData;
        private Dictionary<string, BetEntry> _playerBets = new Dictionary<string, BetEntry>(); 

        #endregion

        #endregion

        private void Awake()
        {
            LoadBetPayoutData();
            _betAmount = 0;
            _turnTotalBetAmount = 0;
        }

        private void Start()
        {
            _betData = BetDataHandler.BetData;
            _playerData = PlayerDataHandler.PlayerData;
            _chipAmount = _playerData.Chips;
        }

        private void OnEnable() => SubscribeEvents();
        private void OnDisable() => UnSubscribeEvents();
        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onPlaceBet += OnPlaceBet;
            CoreGameSignals.Instance.onSpinResult += OnSpinResult;
            UISignals.Instance.onChooseChip += OnChooseChip;
            UISignals.Instance.onClearBets += OnClearBets;
        }
        
        private void UnSubscribeEvents()
        {
            CoreGameSignals.Instance.onPlaceBet -= OnPlaceBet;
            UISignals.Instance.onChooseChip -= OnChooseChip;
        }
        private void LoadBetPayoutData() => _betPayoutData = Resources.Load<CD_BetPayout>("Data/CD_BetPayout").ToDictionary();
        
        private void OnSpinResult(int winningNumber)
        {   
            var betEvaluationContext = new BetEvaluationContext(
                winningNumber,
                _playerBets,
                _betData,
                _betPayoutData
                );
            var earnedChipAmount = BetEvaluator.EvaluateBets(betEvaluationContext);
            var isWon = earnedChipAmount > 0 ? true : false;
            var profit = earnedChipAmount - _turnTotalBetAmount;
            Reset(earnedChipAmount);
            CoreGameSignals.Instance.onTurnResult?.Invoke(new TurnResultParams()
            {
                IsWon = isWon,
                Profit = profit,
            });
        }
        
        private void OnPlaceBet(BetParams betParams)
        {
            if (_betAmount < 1) return;
            if (_chipAmount < _betAmount) return;
            _turnTotalBetAmount += _betAmount;
            UpdateChipBalance(-_betAmount);
            AddBet(betParams, _betAmount);
        }

        private void OnChooseChip(ChipParams chipParams)
        {
            _betAmount = chipParams.Amount;
        }
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
        
        private void Reset(int amount)
        {
            UpdateChipBalance(amount);
            ResetBets();
            _turnTotalBetAmount = 0;
        }
        private void UpdateChipBalance(int amount) =>  _chipAmount += amount;
        private void ResetBets() => _playerBets.Clear();
        private void OnClearBets()
        {
            Reset(_turnTotalBetAmount);
        }
    }
}