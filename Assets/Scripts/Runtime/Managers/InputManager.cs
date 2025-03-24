using System;
using System.Collections.Generic;
using Runtime.Controllers;
using Runtime.Keys;
using Runtime.Signals;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Runtime.Managers
{
    public class InputManager : MonoBehaviour
    {
        #region Self Variables

        #region Private Variables

        private bool _isBetState;
        private Camera _mainCamera;
        private RaycastHit[] _raycastHits = new RaycastHit[1];

        #endregion

        #endregion

        private void Awake()
        {
            _mainCamera = Camera.main;
            _isBetState = false;
        }

        private void OnEnable() => SubscribeEvents();

        private void OnDisable() => UnSubscribeEvents();

        private void UnSubscribeEvents()
        {
            UISignals.Instance.onPrepareSpin -= OnPrepareSpin;
            UISignals.Instance.onChooseChip -= OnChooseChip;
        }

        private void SubscribeEvents()
        {
            UISignals.Instance.onPrepareSpin += OnPrepareSpin;
            UISignals.Instance.onChooseChip += OnChooseChip;
        }

        private void OnPrepareSpin() => _isBetState = false;
        private void OnChooseChip(ChipParams arg0) => _isBetState = true;

        private void Update()
        {
            if (!_isBetState) return;
            if (IsPointerOverUIElement()) return;
            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            int layerMask = ~LayerMask.GetMask("Ignore Raycast");
            if (Physics.RaycastNonAlloc(ray, _raycastHits, Mathf.Infinity, layerMask) <= 0)
            {
                CoreGameSignals.Instance.onBetCollider?.Invoke(""); 
            }
            else
            {
                if (!_raycastHits[0].collider) return;
                var betName = _raycastHits[0].collider.GetComponent<BetColliderController>().GetBetName();
                CoreGameSignals.Instance.onBetCollider?.Invoke(betName);
            }
        }
        
        private bool IsPointerOverUIElement()
        {
            var eventData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            return results.Count > 0;
        }
    }
}