using Runtime.Enums;
using Runtime.Keys;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Signals
{
    public class UISignals: MonoBehaviour
    {
        #region Singleton
        public static UISignals Instance;
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

        public UnityAction onPlay = delegate { };
        public UnityAction onToggleStatics = delegate { };
        public UnityAction<bool> onToggleTest = delegate { };
        public UnityAction<ChipParams> onChooseChip = delegate { };
        public UnityAction onPrepareSpin = delegate { };
        public UnityAction onClearBets = delegate { };
    }
}