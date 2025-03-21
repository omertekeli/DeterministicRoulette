using Runtime.Enums;
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
        public UnityAction onSpin = delegate { };
        public UnityAction onClearBets = delegate { };
        public UnityAction onToggleTest = delegate { };
        public UnityAction onOpenIstatics = delegate { };
        public UnityAction<ChipTypes> onChooseChip = delegate { };
    }
}