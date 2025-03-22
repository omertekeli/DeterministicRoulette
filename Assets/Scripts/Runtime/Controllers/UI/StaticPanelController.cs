using System;
using Runtime.Keys;
using Runtime.Models;
using Runtime.Signals;
using Runtime.Utilies;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Controllers.UI
{
    public class StaticPanelController: MonoBehaviour
    {
        #region Self Variables

        #region SerializeField Variables

        [SerializeField] private Image background;
        [SerializeField] private GameObject panel;
        [SerializeField] private int totalRounds;
        [SerializeField] private int profitLoss;
        [SerializeField] private int wonRounds;
        [SerializeField] private TextMeshProUGUI totalRoundsText;
        [SerializeField] private TextMeshProUGUI profitLossText;
        [SerializeField] private TextMeshProUGUI wonRoundsText;
        [SerializeField] private Button closeButton;
        
        #endregion

        #region Private Variables

        private GameStatistics _gameStats;

        #endregion
    
        #endregion

        private void OnEnable() => SubscribeEvents();
        private void OnDisable() => UnSubscribeEvents();
        private void Start()
        {
            _gameStats = new GameStatistics(totalRounds, profitLoss, wonRounds);
            UpdateUI();
        }
        
        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onTurnResult += OnTurnResult;
            UISignals.Instance.onToggleStatics += OnToggleStatics;
            closeButton.onClick.AddListener(OnClickCloseButton);
        }

        private void UnSubscribeEvents()
        {
            CoreGameSignals.Instance.onTurnResult -= OnTurnResult;
            UISignals.Instance.onToggleStatics -= OnToggleStatics;
            closeButton.onClick.RemoveListener(OnClickCloseButton);
        }
        
        private void OnClickCloseButton()
        {
            panel.SetActive(false);
            background.enabled = false;
        }

        private void OnToggleStatics()
        {
            Debug.Log("Turn on or off panel");
            panel.SetActive(!panel.activeSelf);
            background.enabled = !background.enabled;
        }
        
        private void OnTurnResult(TurnResultParams turnResultParams)
        {
            _gameStats.AddRound(turnResultParams.Profit, turnResultParams.IsWon);
        }

        private void UpdateUI()
        {
            totalRoundsText.text = "Total Rounds: " + _gameStats.TotalRounds;
            wonRoundsText.text = "Won Rounds: " + _gameStats.WonRounds;
            profitLossText.text = "Profit/Loss: " + NumberFormatter.FormatWithCommas(_gameStats.ProfitLoss);
            profitLossText.color = _gameStats.ProfitLoss < 0 ? Color.red : Color.green;
        }
    }
}