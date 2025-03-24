using System.Collections;
using Runtime.Keys;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Managers
{
    public class GameManager: MonoBehaviour
    {
        #region Self Variables

        #region SerializeField Variables

        [SerializeField] private float newTurnDelayTime = 3f;

        #endregion

        #endregion
        private void OnEnable() => SubscribeEvents();
        private void OnDisable() => UnSubscribeEvents();

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onTurnResult += OnTurnResult;
        }

        private void UnSubscribeEvents()
        {
            CoreGameSignals.Instance.onTurnResult += OnTurnResult;
        }

        private void OnTurnResult(TurnResultParams arg0)
        {
            StartCoroutine(StartNewTurn(newTurnDelayTime));
        }

        private IEnumerator StartNewTurn(float delayTime)
        {
            yield return new WaitForSeconds(delayTime);
            CoreGameSignals.Instance.onStartNewTurn?.Invoke();
        }
    }
}