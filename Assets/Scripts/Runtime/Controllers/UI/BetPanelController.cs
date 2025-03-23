using System;
using System.Collections.Generic;
using Runtime.Enums;
using Runtime.Handlers;
using Runtime.Keys;
using Runtime.Signals;
using Runtime.Utilies;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Controllers.UI
{
    public class BetPanelController: MonoBehaviour
    {
        #region Self Variables

        #region SerializedField Variables

        [SerializeField] private List<Button> chipButtons;
        [SerializeField] private Button clearButton;
        [SerializeField] private Button statisticsButton;
        [SerializeField] private Button testButton;
        [SerializeField] private Button spinButton;
        [SerializeField] private TextMeshProUGUI testButtonText;
        [SerializeField] private TextMeshProUGUI playerBetsText;
        [SerializeField] private TextMeshProUGUI playerBalanceText;
        
        #endregion

        #region Private Variables

        private bool _isOnTest;
        private int _playerTotalChips;
        private int _turnTotalBetAmount;
        private int _selectedChipAmount;
        private Dictionary<ChipTypes, int> _chipData;
        
        #endregion
        
        #endregion

        private void Awake()
        {
            _chipData = ChipDataHandler.ChipData;
            var playerData = PlayerDataHandler.PlayerData;
            _playerTotalChips = playerData.Chips;
        }
        
        private void OnEnable() => SubscribeEvents();
        private void OnDisable() => UnSubscribeEvents();

        private void Start()
        {
            SetVariableValues();
            SetTexts();
            ToggleTestUIComponents();
        }

        private void SetVariableValues()
        {
            _isOnTest = false;
            _turnTotalBetAmount = 0;
            _selectedChipAmount = 0;
        }
        
        private void SubscribeEvents()
        {
            foreach (var button in chipButtons)
            {
                button.onClick.AddListener(() => OnClickChip(button));
            }
            clearButton.onClick.AddListener(OnClickClearButton);
            statisticsButton.onClick.AddListener(OnClickStatisticsButton);
            testButton.onClick.AddListener(OnClickTestButton);
            spinButton.onClick.AddListener(OnClickSpinButton);
            CoreGameSignals.Instance.onPlaceBet += OnPlaceBet;
        }

        private void OnClickSpinButton()
        {
            UISignals.Instance.onPrepareSpin?.Invoke();
        }

        private void UnSubscribeEvents()
        {
            foreach (var button in chipButtons)
            {
                button.onClick.RemoveListener(() => OnClickChip(button));
            }
            clearButton.onClick.RemoveListener(OnClickClearButton);
            statisticsButton.onClick.RemoveListener(OnClickStatisticsButton);
            testButton.onClick.RemoveListener(OnClickTestButton);
            CoreGameSignals.Instance.onPlaceBet -= OnPlaceBet;
        }

        private void OnPlaceBet(BetParams betParams)
        {
            _turnTotalBetAmount += _selectedChipAmount;
            _playerTotalChips -= _selectedChipAmount;
            SetTexts();
        }

        private void OnClickTestButton()
        {   
            _isOnTest = !_isOnTest;
            ToggleTestUIComponents();
            UISignals.Instance.onToggleTest?.Invoke(_isOnTest);
        }

        private void ToggleTestUIComponents()
        {
            ColorBlock colors = testButton.colors;
            if (_isOnTest)
            {
                testButtonText.text = "Test: ON";
                colors.normalColor = Color.green;
                colors.pressedColor = new Color(0, 0.7f, 0);
                colors.highlightedColor = new Color(0.6f, 1f, 0.6f);
                colors.selectedColor = new Color(0.2f, 0.8f, 0.2f);
            }
            else
            {
                testButtonText.text = "Test: OFF";
                colors.normalColor = Color.red;
                colors.pressedColor = new Color(0.7f, 0, 0);
                colors.selectedColor = new Color(0.8f, 0, 0);
                colors.highlightedColor = new Color(1f, 0.6f, 0.6f);
            }
            testButton.colors = colors;
        }

        private void OnClickChip(Button button)
        {
            Debug.Log("Click on " + button.name);
            if (!Enum.TryParse(button.name, out ChipTypes chipType)) return;
            _selectedChipAmount = _chipData.GetValueOrDefault(chipType, 0);
            UISignals.Instance.onChooseChip?.Invoke(new ChipParams()
            {
                ChipType = chipType,
                Amount = _selectedChipAmount
            });
        }

        private void OnClickStatisticsButton()
        {
            Debug.Log("Toggle Statistics");
            UISignals.Instance.onToggleStatics?.Invoke();
        }
        
        private void OnClickClearButton()
        {
            Debug.Log("Clear Player Bets");
            UISignals.Instance.onClearBets?.Invoke();
            _playerTotalChips += _turnTotalBetAmount;
            _turnTotalBetAmount = 0;
            SetTexts();
        }
        private void SetTexts()
        {
            playerBalanceText.text = NumberFormatter.FormatWithCommas(_playerTotalChips);
            playerBetsText.text = "Total Bet: " + NumberFormatter.FormatWithCommas(_turnTotalBetAmount);
        }
    }
}