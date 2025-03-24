using System.Collections.Generic;
using System.Linq;
using Runtime.Handlers;
using Runtime.Managers;
using Runtime.Signals;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Controllers.UI
{ public class ResultPanelController: MonoBehaviour
    {
        #region Self Variables
    
        #region Serialized Field Variables

        [SerializeField] private Image winningNumberImage;
        [SerializeField] private TextMeshProUGUI winningNumberText;
        [SerializeField] private Vector2 targetPos;
        
        #endregion

        #region Private Variables

        private Dictionary<string, int[]> _betData;
        private UIEffectManager _effectManager;
        private Transform _defaultTransform;

        #endregion
        
        #endregion
        
        private void OnEnable() => SubscribeEvents();
        private void OnDisable() => UnsubscribeEvents();

        private void Start()
        {
            _betData = BetDataHandler.BetData;
            _defaultTransform = transform;
            FindReferences();
        }

        private void FindReferences()
        {
            _effectManager = FindObjectOfType<UIEffectManager>();
        }
        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onSpinResult += OnSpinResult;
        }
        private void UnsubscribeEvents()
        {
            CoreGameSignals.Instance.onSpinResult -= OnSpinResult;
        }

        private void OnSpinResult(int winningNumber)
        {
            winningNumberText.text = winningNumber.ToString();
            var image = winningNumberImage.GetComponent<Image>();
            if (image)
            {
                if (winningNumber == 0)
                {
                    image.color = Color.green;
                }
                else if (_betData.TryGetValue("Red", out var redNumbers))
                {
                    if (redNumbers.Contains(winningNumber))
                    {
                        image.color = Color.red;
                    }
                }
                else if (_betData.TryGetValue("Black", out var blackNumbers))
                {
                    if (blackNumbers.Contains(winningNumber))
                    {
                        image.color = Color.black;
                    }
                } 
            }
            _effectManager.MoveAndBack(winningNumberImage.rectTransform, targetPos, 0.5f);
        }
        
    }
}