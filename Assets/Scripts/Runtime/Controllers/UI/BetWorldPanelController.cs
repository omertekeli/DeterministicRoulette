using System;
using System.Collections.Generic;
using Runtime.Handlers;
using Runtime.Keys;
using Runtime.Signals;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Controllers.UI
{
    public class BetWorldPanelController : MonoBehaviour
    {
        #region Self Variables

        #region SerializeField Variables

        [SerializeField] private List<Image> highlightImages;

        #endregion
        
        #region Private Variables
        
        private Dictionary<string, Image> _highlightImageMap;
        private Image _activeImage;
        private Dictionary<string, int[]> _betData;
        private string _betName;

        #endregion

        #endregion

        private void Awake()
        {
            _highlightImageMap = new Dictionary<string, Image>();
            foreach (var image in highlightImages)
            {
                _highlightImageMap[image.name] = image;
                image.gameObject.SetActive(false);
            }
        }

        private void Start()
        {
            _betData = BetDataHandler.BetData;
        }

        private void OnEnable() => SubscribeEvents();
        private void OnDisable() => UnSubscribeEvents();

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onBetCollider += OnBetCollider;
        }
        
        private void UnSubscribeEvents()
        {
            CoreGameSignals.Instance.onBetCollider -= OnBetCollider;
        }
        
        private void OnBetCollider(string betName)
        {
            if (betName == "")
            {
                ToggleHighlight(false);
                return;
            }
            
            if (_betName == betName) return;
            _betName = betName;
            ToggleHighlight(false);
            
            var numbers = _betData[betName];
            Debug.Log("Numbers: " + string.Join(", ", numbers));
            foreach (var number in numbers)
            {
                if (_highlightImageMap.TryGetValue(number.ToString(), out Image newImage))
                {
                    Debug.Log("Activate image: " + newImage.name);
                    newImage.gameObject.SetActive(true);
                    Debug.Log($"After activation: {newImage.gameObject.activeSelf}");
                }
            }
        }

        private void ToggleHighlight(bool isActive)
        {
            foreach (var image in highlightImages)
            {
                image.gameObject.SetActive(isActive);
            }
        }
        
    }
}