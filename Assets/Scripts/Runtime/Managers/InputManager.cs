using System.Collections.Generic;
using Runtime.Controllers;
using Runtime.Signals;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Runtime.Managers
{
    public class InputManager : MonoBehaviour
    {
        private Camera _mainCamera;
        private RaycastHit[] _raycastHits = new RaycastHit[1];

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if (IsPointerOverUIElement()) return;
            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.RaycastNonAlloc(ray, _raycastHits) <= 0)
            {
                CoreGameSignals.Instance.onBetCollider?.Invoke(""); 
            }
            else
            {
                if (_raycastHits[0].collider)
                {
                    var betName = _raycastHits[0].collider.GetComponent<BetColliderController>().GetBetName();
                    CoreGameSignals.Instance.onBetCollider?.Invoke(betName);
                }
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