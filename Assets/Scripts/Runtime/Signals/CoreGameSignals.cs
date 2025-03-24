using Runtime.Keys;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Signals
{
    public class CoreGameSignals: MonoBehaviour
    {
        #region Singleton
        public static CoreGameSignals Instance;
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }
        
        #endregion
        
        public UnityAction<string> onBetCollider = delegate { };
        public UnityAction<BetParams> onPlaceBet = delegate { };
        public UnityAction<int> onSpin = delegate { };
        public UnityAction<Transform> onGoTarget = delegate { };
        public UnityAction onBallStopped = delegate { };
        public UnityAction<int> onSpinResult = delegate { };
        public UnityAction<TurnResultParams> onTurnResult = delegate { };
        public UnityAction onStartNewTurn = delegate { };
    }
}