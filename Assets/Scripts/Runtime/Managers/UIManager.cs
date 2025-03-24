using System;
using Runtime.Enums;
using Runtime.Keys;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Managers
{
    public class UIManager : MonoBehaviour
    {
        //TODO: Arrange UIManager to use layers, after implementing save system
        private void OnEnable() => SubscribeEvents();
        private void OnDisable() => UnSubscribeEvents();
        
        private void Start()
        {
            CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Start, 1);
        }
        
        private void SubscribeEvents()
        {
            UISignals.Instance.onPlay += OnPlay;
            CoreGameSignals.Instance.onSpinResult += OnSpinResult;
            CoreGameSignals.Instance.onTurnResult += OnTurnResult;
            CoreGameSignals.Instance.onStartNewTurn += OnStartNewTurn;
        }

        private void UnSubscribeEvents()
        {
            UISignals.Instance.onPlay -= OnPlay;
            CoreGameSignals.Instance.onSpinResult -= OnSpinResult;
            CoreGameSignals.Instance.onTurnResult -= OnTurnResult;
            CoreGameSignals.Instance.onStartNewTurn -= OnStartNewTurn;
        }

        private void OnStartNewTurn()
        {
            //1000 is invalid, placeholder value
            CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Bet, 1000);
        }

        private void OnPlay()
        {
            CoreUISignals.Instance.onClosePanel?.Invoke(1);
            //1000 is invalid, placeholder value
            CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Bet, 1000);
        }
        
        private void OnSpinResult(int winningNumber)
        {
            //1000 is invalid, placeholder value
            CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.SpinResult, 1000);
        }
        
        private void OnTurnResult(TurnResultParams arg0)
        {
            //1000 is invalid, placeholder value
            CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.TurnResult, 1000);
        }
    }
}