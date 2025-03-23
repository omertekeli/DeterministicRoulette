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
            CoreGameSignals.Instance.onSpin += OnSpin;
            CoreGameSignals.Instance.onGoTarget += OnGoTarget;
            CoreGameSignals.Instance.onStopBall += OnStopBall;
        }
        
        private void UnSubscribeEvents()
        {
            CoreGameSignals.Instance.onSpin -= OnSpin;
            CoreGameSignals.Instance.onGoTarget += OnGoTarget;
            CoreGameSignals.Instance.onStopBall += OnStopBall;
        }

        private void OnStopBall()
        {
            //ballMovementController.StopBall();
        }

        private void OnGoTarget(GameObject target)
        {
            ballMeshController.StopRotation();
            //ballMovementController.GoTarget(target);
        }

        private void OnSpin(int targetNumber)
        {
            ballMovementController.Spin();
            ballMeshController.StartRotation();
        }
    }
}