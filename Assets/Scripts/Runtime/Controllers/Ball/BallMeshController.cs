using System;
using UnityEngine;

namespace Runtime.Controllers.Ball
{
    public class BallMeshController : MonoBehaviour
    {
        #region Self Variables
        
        #region Serialized Field Variables
        
        [SerializeField] private float rotationSpeed;
  
        #endregion

        #region Private Variables

        private bool _canRotate;

        #endregion

        #endregion

        private void Start()
        {
            _canRotate = true;
        }

        private void Update()
        {
            if (!_canRotate) return;
            transform.Rotate(Vector3.forward * (rotationSpeed * Time.deltaTime));
        }
        
        internal void StartRotation() => _canRotate = true;
        internal void StopRotation() => _canRotate = false;

        internal void ToggleMeshRenderer()
        {
            transform.GetComponent<MeshRenderer>().enabled = !transform.GetComponent<MeshRenderer>().enabled;
        }
    }
}