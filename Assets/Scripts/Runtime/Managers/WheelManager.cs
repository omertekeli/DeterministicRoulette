using System;
using System.Collections.Generic;
using Runtime.Controllers.Wheel;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Managers
{
    public class WheelManager: MonoBehaviour
    {
        
        #region Self Variables
        
        #region Serialized Field Variables
        
        [SerializeField] private WheelSlotController wheelSlotController;
        [SerializeField] private WheelMovementController wheelMovementController;
        
        #endregion

        #endregion
        private void OnEnable() => SubscribeEvents();
        private void OnDisable() => UnSubscribeEvents();

        private void Start()
        {
            wheelMovementController.SetRotatingState(true);
        }
        
        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onSpin += OnSpin;
        }
        
        private void UnSubscribeEvents()
        {
            CoreGameSignals.Instance.onSpin -= OnSpin;
        }

        private void OnSpin(int targetNumber)
        {
            var slotObject = wheelSlotController.GetSlot(targetNumber);
            if (slotObject)
                CoreGameSignals.Instance.onGoTarget?.Invoke(slotObject);

        }


    }
}