using System;
using Runtime.Enums;
using Runtime.Keys;
using Runtime.Signals;
using Runtime.Utilies;
using TMPro;
using UnityEngine;

namespace Runtime.Controllers.UI
{
    public class TurnResultPanelController: MonoBehaviour
    {
        #region Self Variables

        #region Serialized Field Variables

        [SerializeField] private GameObject container;
        [SerializeField] private TextMeshProUGUI earnedChipText;
        [SerializeField] private TextMeshProUGUI profitText;
        [SerializeField] private TextMeshProUGUI resultText;
        
        #endregion
        
        #endregion

        private void OnEnable()
        {
            SubscribeEvents();
        }
        
        private void OnDisable()
        {
            UnSubscribeEvents();
        }
        
        private void UnSubscribeEvents()
        {
            CoreUISignals.Instance.onOpenPanel -= OnOpenPanel;
            CoreGameSignals.Instance.onTurnResult -= OnTurnResult;
        }

        private void SubscribeEvents()
        {
            CoreUISignals.Instance.onOpenPanel += OnOpenPanel;
            CoreGameSignals.Instance.onTurnResult += OnTurnResult;
        }

        private void OnOpenPanel(UIPanelTypes panel, int layer)
        {
            if (panel == UIPanelTypes.TurnResult)
            {
                container.SetActive(true);
                
            }
            else
            {
                container.SetActive(false); 
            }
        }

        private void OnTurnResult(TurnResultParams arg0)
        {
            earnedChipText.text = "Earned " + NumberFormatter.FormatWithCommas(arg0.EarnedChipAmount) + " Chips";
            profitText.text = "Loss / Profit: " + NumberFormatter.FormatWithCommas(arg0.Profit);
            resultText.text = arg0.IsWon ? "YOU WON" : "YOU LOST";
        }
    }
}