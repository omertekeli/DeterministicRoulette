using System.Collections;
using System.Collections.Generic;
using Runtime.Enums;
using Runtime.Models;
using UnityEngine;

namespace Runtime.Managers
{
    public class CameraManager: MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private Camera targetCamera;
        [SerializeField] private List<CameraState> cameraStates = new List<CameraState>();
        [SerializeField] private float transitionDuration = 1f;
        
        #endregion

        #region Private Variables

        private bool _isTweening = false;

        #endregion
        
        #endregion
        
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
            
            while (elapsedTime < transitionDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.SmoothStep(0, 1, elapsedTime / transitionDuration);
                
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