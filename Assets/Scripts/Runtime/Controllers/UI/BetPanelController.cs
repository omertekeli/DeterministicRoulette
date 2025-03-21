using System;
using System.Collections.Generic;
using Runtime.Enums;
using Runtime.Signals;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Controllers.UI
{
    public class BetPanelController: MonoBehaviour
    {
        #region Self Variables

        #region SerializedField Variables

        [SerializeField] private List<Button> chipButtons;

        #endregion

        #endregion

        private void Awake()
        {
            SetButtonActions();
        }

        private void SetButtonActions()
        {
            if (chipButtons.Count < 1) return;
            foreach (var button in chipButtons)
            {
                button.onClick.AddListener(() => OnClickChip(button));
            }
        }

        private static void OnClickChip(Button button)
        {
            Debug.Log("Click on " + button.name);
            if (Enum.TryParse(button.name, out ChipTypes chipType))
                UISignals.Instance.onChooseChip?.Invoke(chipType);
        }

        private void OnEnable() => SubscribeEvents();
        private void OnDisable() => UnSubscribeEvents();
        
        private void SubscribeEvents()
        {
            
        }
        
        private void UnSubscribeEvents()
        {
            
        }
        
        private void ClearPlayerBets()
        {
            UISignals.Instance.onClearBets?.Invoke();   
        }
    }
}