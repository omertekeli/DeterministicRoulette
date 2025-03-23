using System.Collections.Generic;
using Runtime.Controllers;
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
        
        //Oyun Statelerini ayarla

        private void Update()
        {
            if (!_isBetState) return;
            if (IsPointerOverUIElement()) return;
            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.RaycastNonAlloc(ray, _raycastHits) <= 0)
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