using System;
using Runtime.Signals;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Controllers.UI
{
    public class StartPanelController: MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private Button startButton;

        #endregion

        #endregion

        private void OnEnable() => SubscribeEvents();

        private void SubscribeEvents()
        {
            startButton.onClick.AddListener(OnClickStartButton);
        }

        private void OnDisable() => UnSubscribeEvents();

        private void UnSubscribeEvents()
        {
            startButton.onClick.RemoveListener(OnClickStartButton);
        }

        private void OnClickStartButton()
        {
            UISignals.Instance.onPlay?.Invoke();
        }
    }
}