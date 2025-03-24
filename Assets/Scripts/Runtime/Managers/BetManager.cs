using System.Collections;
using System.Collections.Generic;
using Runtime.Data.ValueObjects;
using Runtime.Enums;
using Runtime.Handlers;
using Runtime.Keys;
using Runtime.Signals;
using Runtime.Utilies;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime.Managers
{ 
    public class BetManager: MonoBehaviour
    {
        #region Self Variables

        #region Serialized Field Variables

        [SerializeField] private float waitSpinResult = 2f;

        #endregion
        
        #region Private Variables
        
        private int _winningNumber;
        private bool _isOnTest;
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
            _isOnTest = false;
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
            CoreGameSignals.Instance.onBallStopped += OnBallStopped;
            UISignals.Instance.onPrepareSpin += OnPrepareSpin;
            UISignals.Instance.onChooseChip += OnChooseChip;
            UISignals.Instance.onClearBets += OnClearBets;
            UISignals.Instance.onToggleTest += OnToggleTest;
        }
        
        private void UnSubscribeEvents()
        {
            CoreGameSignals.Instance.onPlaceBet -= OnPlaceBet;
            CoreGameSignals.Instance.onBallStopped -= OnBallStopped;
            UISignals.Instance.onPrepareSpin -= OnPrepareSpin;
            UISignals.Instance.onChooseChip -= OnChooseChip;
            UISignals.Instance.onClearBets -= OnClearBets;
            UISignals.Instance.onToggleTest += OnToggleTest;
        }

        private void LoadBetPayoutData() => _betPayoutData = Resources.Load<CD_BetPayout>("Data/CD_BetPayout").ToDictionary();
        
        private void OnToggleTest(bool arg0)
        {
            _isOnTest = arg0;
        }
        
        private void OnPrepareSpin()
        {
            _winningNumber = _isOnTest ? Random.Range(0, 37) : BetEvaluator.GetWinningNumber(_playerBets, _betData);
            CoreGameSignals.Instance.onSpin?.Invoke(_winningNumber);
        }

        private void OnBallStopped()
        {
            CoreGameSignals.Instance.onSpinResult?.Invoke(_winningNumber);
            StartCoroutine(EvaluateBets());
        }

        private IEnumerator EvaluateBets()
        {
            var betEvaluationContext = new BetEvaluationContext(
                _winningNumber,
                _playerBets,
                _betData,
                _betPayoutData
            );
            var earnedChipAmount = BetEvaluator.EvaluateBets(betEvaluationContext);
            var isWon = earnedChipAmount > 0 ? true : false;
            var profit = earnedChipAmount - _turnTotalBetAmount;
            Reset(earnedChipAmount);
            
            yield return new WaitForSeconds(waitSpinResult);
            
            CoreGameSignals.Instance.onTurnResult?.Invoke(new TurnResultParams()
            {
                IsWon = isWon,
                EarnedChipAmount = earnedChipAmount,
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
        
        private void OnClearBets()
        {
            Reset(_turnTotalBetAmount);
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
    }
}