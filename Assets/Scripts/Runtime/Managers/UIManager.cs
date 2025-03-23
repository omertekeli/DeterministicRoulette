﻿using Runtime.Enums;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Managers
{
    public class UIManager : MonoBehaviour
    {
        private void OnEnable() => SubscribeEvents();
        private void OnDisable() => UnSubscribeEvents();
        
        private void SubscribeEvents()
        {
            //CoreGameSignals.Instance.onTurnResult += OnTurnResult;
        }

        private void OnTurnResult()
        {
            //CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Bet, 1);
        }

        private void OnSpinResult(int winningNumber)
        {
            //CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Result, 3);
        }

        private void UnSubscribeEvents()
        {
            //CoreGameSignals.Instance.onSpinResult -= OnSpinResult;
        }


        
    }
}