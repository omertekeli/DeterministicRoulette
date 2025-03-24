using System;
using Runtime.Managers;
using Runtime.Signals;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Handlers
{
    public class UIButtonEffectHandler: MonoBehaviour
    {
        #region Self Variables

        #region Public Variables

        public static event Action<UIButtonEffectHandler> onButtonEffectTriggered;

        #endregion
        
        #region Serialized Variables
        
        [SerializeField] private bool hasEffectSubscription = false;
        [SerializeField] private bool enableMove = false;
        [SerializeField] private bool enableScale = false;
        [SerializeField] private Vector2 moveVector = new Vector2(0, 50);
        [SerializeField] private float scaleFactor = 1.2f;
        [SerializeField] private float duration = 0.2f;
        [SerializeField] private RectTransform target;

        #endregion

        #region Private Variables
        
        private Button _button;
        private Vector2 _originalPos;
        private Vector2 _originalSize;
        private bool _isDefaultSize;
        private bool _isDefaultPos;
        private UIEffectManager _effectManager;

        #endregion

        #endregion
        
        private void Awake()
        {
            _button = GetComponent<Button>();
            FindReferences();
        }
        private void FindReferences()
        {
            _effectManager = FindObjectOfType<UIEffectManager>();
        }

        private void OnEnable() => SubscribeEvents();

        private void OnDisable() => UnSubscribeEvents();
        private void Start() => SetVariableValues();
        
        private void UnSubscribeEvents()
        {
            _button.onClick.RemoveListener(TryToActions);
            if (hasEffectSubscription) 
                onButtonEffectTriggered  -= OnButtonEffectTriggered ;
        }

        private void SubscribeEvents()
        {
            _button.onClick.AddListener(TryToActions);
            if (hasEffectSubscription) 
                onButtonEffectTriggered  += OnButtonEffectTriggered ;
        }

        private void TryToActions()
        {
            TryToInvoke();
            TryToMove();
            TryToScale();
        }
        
        private void TryToInvoke()
        {
            if (_isDefaultPos || _isDefaultSize)
            {
                onButtonEffectTriggered?.Invoke(this);
            }
        }
        
        private void OnButtonEffectTriggered(UIButtonEffectHandler sender)
        {
            if (sender != this) 
                ResetAll();
        }

        private void TryToScale()
        {
            if (!enableScale) return;
            if (_isDefaultSize) ScaleToTarget();
            else ResetScale();
        }

        private void ScaleToTarget()
        {
            var targetScale = target.sizeDelta * scaleFactor;
            _effectManager.ScaleUI(target, targetScale, duration);
            _isDefaultSize = false;
        }

        private void TryToMove()
        {
            if (!enableMove) return;
            if (_isDefaultPos) MoveToTarget();
            else ResetPosition();
        }

        private void MoveToTarget()
        {
            var targetPos = target.anchoredPosition + moveVector;
            _effectManager.MoveUI(target, targetPos, duration);
            _isDefaultPos = false;
        }

        private void SetVariableValues()
        {
            _isDefaultSize = true;
            _isDefaultPos = true;
            _originalPos = target.anchoredPosition;
            _originalSize = target.sizeDelta;
        }

        public void ResetAll()
        {
            ResetPosition();
            ResetScale();
        }
        
        private void ResetScale()
        {
            if (!enableScale) return;
            _effectManager.ResetScale(target, _originalSize, duration);
            _isDefaultSize = true;
        }

        private void ResetPosition()
        {
            if (!enableMove) return;
            _effectManager.ResetPosition(target, _originalPos, duration);
            _isDefaultPos = true;
        }
    }
}