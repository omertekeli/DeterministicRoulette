using Runtime.Enums;
using Runtime.Keys;
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
            CoreGameSignals.Instance.onTurnResult += OnTurnResult;
        }

        private void OnTurnResult(TurnResultParams arg0)
        {
            //Kazanıp kazanmadığı sonrasın newStartTurn Atalım
            CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Bet, 1);
        }

        private void UnSubscribeEvents()
        {
            CoreGameSignals.Instance.onSpinResult -= OnSpinResult;
        }
        
        private void OnSpinResult(int winningNumber)
        {
            CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.SpinResult, 3);
        }




        
    }
}