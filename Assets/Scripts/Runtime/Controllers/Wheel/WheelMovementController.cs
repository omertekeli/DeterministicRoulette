using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Runtime.Controllers.Wheel
{
    public class WheelMovementController : MonoBehaviour
    {
        #region Self Variables
        
        #region Serialized Field Variables
        
        [SerializeField] private Transform wheelTransform;
        [SerializeField] private float rouletteRotationSpeed;
        
        #endregion
        
        #region Private Variables
        
        private bool _canRotating;
        
        #endregion
        
        #endregion
        private void FixedUpdate()
        {
            if (!_canRotating) return;
            //since the ball will go to the target with physics, this process should be with fixed update
            wheelTransform.Rotate(Vector3.up * (rouletteRotationSpeed * Time.fixedDeltaTime), Space.World);;
        }
        
        internal void SetRotatingState(bool canRotating) => _canRotating = canRotating;
    }
}

