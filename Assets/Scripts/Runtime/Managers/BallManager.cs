using Runtime.Controllers.Ball;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Managers
{
    public class BallManager: MonoBehaviour
    {
        #region Self Variables
        
        #region Serialized Field Variables
        
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
            UISignals.Instance.onPrepareSpin += OnPrepareSpin;
        }
        
        private void UnSubscribeEvents()
        {
            CoreGameSignals.Instance.onGoTarget += OnGoTarget;
            CoreGameSignals.Instance.onBallStopped += OnBallStopped;
            UISignals.Instance.onPrepareSpin -= OnPrepareSpin;
        }

        private void OnBallStopped()
        {
            ballMeshController.StopRotation();
        }

        private void OnPrepareSpin()
        {
            ballMovementController.PrepareSpin();
        }

        private void OnGoTarget(GameObject target)
        {
            ballMeshController.StartRotation();
            ballMovementController.Spin();
        }
    }
}