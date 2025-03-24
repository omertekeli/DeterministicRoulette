using Runtime.Controllers.Ball;
using Runtime.Keys;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Managers
{
    public class BallManager: MonoBehaviour
    {
        #region Self Variables
        
        #region Serialized Field Variables
        
        [SerializeField] private GameObject ball;
        [SerializeField] private BallMovementController ballMovementController;
        [SerializeField] private BallMeshController ballMeshController;
        
        #endregion

        #endregion
        private void OnEnable() => SubscribeEvents();
        private void OnDisable() => UnSubscribeEvents();
        
        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onGoTarget += OnGoTarget;
            CoreGameSignals.Instance.onBallStopped += OnBallStopped;
            CoreGameSignals.Instance.onTurnResult += OnTurnResult;
        }
        
        private void UnSubscribeEvents()
        {
            CoreGameSignals.Instance.onGoTarget -= OnGoTarget;
            CoreGameSignals.Instance.onBallStopped -= OnBallStopped;
            CoreGameSignals.Instance.onTurnResult -= OnTurnResult;
        }
        
        private void OnTurnResult(TurnResultParams arg0)
        {
            ballMeshController.ToggleMeshRenderer();
            ballMovementController.Reset();
        }

        private void OnBallStopped()
        {
            ballMeshController.StopRotation();
        }

        private void OnGoTarget(Transform target)
        {
            ballMeshController.ToggleMeshRenderer();
            ballMovementController.Spin(target);
            ballMeshController.StartRotation();
        }
    }
}