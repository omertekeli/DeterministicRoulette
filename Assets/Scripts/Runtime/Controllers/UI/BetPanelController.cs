using System;
using System.Collections;
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

        [SerializeField] private GameObject container;
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

        private IEnumerator ActivatePanel()
        {
            //TODO: get duration via camera data
            yield return new WaitForSeconds(1f);
            container.SetActive(true);
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
            CoreGameSignals.Instance.onTurnResult += OnTurnResult;
            //TODO: remove onOpenPanel, after implementing save system. Now, bet panel has to be active, dont inistiate it
            CoreUISignals.Instance.onOpenPanel += OnOpenPanel;
        }

        private void OnOpenPanel(UIPanelTypes panel, int arg1)
        {
            if (panel == UIPanelTypes.Bet)
            {
                StartCoroutine(ActivatePanel());
                
            }
            else
            {
                container.SetActive(false); 
            }
        }

        private void OnClickSpinButton()
        {
            container.SetActive(false);
            spinButton.interactable = false;
            ResetChipButtons();
            UISignals.Instance.onPrepareSpin?.Invoke();
        }

        private void ResetChipButtons()
        {
            foreach (var button in chipButtons)
            {
                button.GetComponent<UIButtonEffectHandler>().ResetAll();
            }
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
            CoreGameSignals.Instance.onTurnResult -= OnTurnResult;
            CoreUISignals.Instance.onOpenPanel -= OnOpenPanel;
        }

        private void OnTurnResult(TurnResultParams arg0)
        {
            _selectedChipAmount = 0;
            _turnTotalBetAmount = 0;
            _playerTotalChips += arg0.EarnedChipAmount;
            SetTexts();
        }

        private void OnPlaceBet(BetParams betParams)
        {
            if (_selectedChipAmount <= 0) return;
            _turnTotalBetAmount += _selectedChipAmount;
            _playerTotalChips -= _selectedChipAmount;
            SetTexts();
            spinButton.interactable = true;
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
            UISignals.Instance.onToggleStatics?.Invoke();
        }
        
        private void OnClickClearButton()
        {
            UISignals.Instance.onClearBets?.Invoke();
            _playerTotalChips += _turnTotalBetAmount;
            _turnTotalBetAmount = 0;
            SetTexts();
            spinButton.interactable = false;
        }
        private void SetTexts()
        {
            playerBalanceText.text = NumberFormatter.FormatWithCommas(_playerTotalChips);
            playerBetsText.text = "Total Bet: " + NumberFormatter.FormatWithCommas(_turnTotalBetAmount);
        }
    }
}