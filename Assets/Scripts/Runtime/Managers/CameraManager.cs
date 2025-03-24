using System;
using System.Collections;
using System.Collections.Generic;
using Runtime.Enums;
using Runtime.Keys;
using Runtime.Models;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Managers
{
    public class CameraManager: MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private Camera targetCamera;
        [SerializeField] private List<CameraState> cameraStates = new List<CameraState>();
        
        #endregion

        #region Private Variables

        private bool _isTweening;
        private float _transitionDuration;

        #endregion
        
        #endregion
        
        private void OnEnable() => SubscribeEvents();
        
        private void OnDisable() => UnSubscribeEvents();
        
        private void Start()
        {
            _isTweening = false;
            _transitionDuration = 1f;
        }
        
        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onTurnResult += OnTurnResult;
            UISignals.Instance.onPrepareSpin += OnPrepareSpin;
            UISignals.Instance.onPlay += OnPlay;
            UISignals.Instance.onChooseChip += OnChooseChip;
        }

        private void OnChooseChip(ChipParams arg0)
        {
            _transitionDuration = 1f;
            SwitchToCamera(GameStates.Bet);
        }

        private void OnTurnResult(TurnResultParams arg0)
        {
            _transitionDuration = 1f;
            SwitchToCamera(GameStates.Bet);
        }
        
        private void OnPrepareSpin()
        {
            _transitionDuration = 1f;
            SwitchToCamera(GameStates.Spin);
        }

        private void OnPlay()
        {
            _transitionDuration = 1f;
            SwitchToCamera(GameStates.Idle);
        }

        private void UnSubscribeEvents()
        {
            CoreGameSignals.Instance.onTurnResult -= OnTurnResult;
            UISignals.Instance.onPrepareSpin -= OnPrepareSpin;
            UISignals.Instance.onPlay -= OnPlay;
        }

        private void SwitchToCamera(GameStates state)
        {
            bool found = TryFindCameraState(state, out var matchedState);
            if (found)
            {
                StartCoroutine(TweenToCamera(matchedState));
            }
        }

        private bool TryFindCameraState(GameStates state, out CameraState matchedState)
        {
            matchedState = cameraStates.Find(cs => cs.GameState == state);
            return matchedState != null;
        }
        
        IEnumerator TweenToCamera(CameraState targetState)
        {
            if (_isTweening) yield break;
            _isTweening = true;

            float elapsedTime = 0f;

            Vector3 startPosition = targetCamera.transform.position;
            Quaternion startRotation = targetCamera.transform.rotation;
            float startFOV = targetCamera.fieldOfView;
            float startOrthoSize = targetCamera.orthographicSize;
            
            while (elapsedTime < _transitionDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.SmoothStep(0, 1, elapsedTime / _transitionDuration);
                
                targetCamera.transform.position = Vector3.Lerp(startPosition, targetState.Position, t);
                targetCamera.transform.rotation = Quaternion.Slerp(startRotation, targetState.Rotation, t);
                
                if (targetCamera.orthographic)
                    targetCamera.orthographicSize = Mathf.Lerp(startOrthoSize, targetState.OrthographicSize, t);
                else
                    targetCamera.fieldOfView = Mathf.Lerp(startFOV, targetState.FieldOfView, t);

                yield return null;
            }
            
            targetCamera.orthographic = targetState.IsOrthographic;
            
            _isTweening = false;
        }
    }
}