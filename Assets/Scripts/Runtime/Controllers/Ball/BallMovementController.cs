using System;
using System.Collections;
using System.Threading;
using Runtime.Signals;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;

namespace Runtime.Controllers.Ball
{
    /// <summary>
    /// It manages the ball movement. First launch the ball and then spin it.
    /// Pull area is controlled with a dot product.
    /// After decreasing the speed of the ball, it will be able to pull if slot is near.
    /// </summary>

    public class BallMovementController : MonoBehaviour
    {
        #region Self Variables
        
        #region Serialized Field Variables
        
        [SerializeField] private Transform rouletteWheel;
        [SerializeField] private float initialBallSpeed  = 500f;
        [SerializeField] private float ballSpeedSlowDownRate  = 0.5f;
        [SerializeField] private float ballStopThreshold = 0.1f;
        [SerializeField] private float ballForwardForceMultiplier = 2f;
        [SerializeField] private float pullAreaDot = 0.85f;
        [SerializeField] private float minBallSpeedToPull = 150f; //Must be greater than wheel rotation speed
        [SerializeField] private float attractionForce = 20f;
        
        #endregion

        #region Private Variables

        private Matrix4x4 _ballLaunchMatrix4X4;
        private Transform _targetSlot;
        private Rigidbody _rigidbody;
        private bool _isSpinning;
        private bool _isUsingPhysics;
        private bool _isOnSlot;
        
        #endregion

        #endregion
        
        void Start()
        {
            SetVariableValues();
            SetRigidbodyProperties();
        }

        private void SetVariableValues()
        {
            _ballLaunchMatrix4X4 = transform.localToWorldMatrix;
            _rigidbody = GetComponent<Rigidbody>();
            _isSpinning = false;
            _isUsingPhysics = false;
            _isOnSlot = false;
        }

        private void SetRigidbodyProperties()
        {
            _rigidbody.useGravity = false;
            _rigidbody.drag = 0.1f;
            _rigidbody.angularDrag = 0.1f;
        }

        //TODO:Create formula to increase reality by changing pullAreaDot, attractionForce, minBallSpeedToPull
        void Update()
        {
            if (!_isSpinning) return;
            if (_isUsingPhysics) return;
            transform.RotateAround(rouletteWheel.position, Vector3.up, initialBallSpeed * Time.deltaTime);
            if (initialBallSpeed > minBallSpeedToPull)
            {
                initialBallSpeed -= ballSpeedSlowDownRate;
            }
            
            //TODO: Use force the ball go to the center but slowl
            
            if (initialBallSpeed > minBallSpeedToPull) return;
            if (!IsSlotNear()) return;
            EnablePhysicsMode();
        }

        private void FixedUpdate()
        {
            if (!_isSpinning) return;
            if (!_isUsingPhysics) return;
            ApplyAttractionForce();
            if (_rigidbody.velocity.magnitude < ballStopThreshold && _isOnSlot)
            {
                _isSpinning = false;
            }
        }
        
        internal void Spin(Transform target)
        {
            _targetSlot = target;
            _rigidbody.isKinematic = false;
            _isSpinning = true;
        }
        
        internal void Reset()
        {
            _rigidbody.isKinematic = true;
            _isSpinning = false;
            transform.position = _ballLaunchMatrix4X4.GetColumn(3);
            transform.rotation = _ballLaunchMatrix4X4.rotation;
            transform.localScale = _ballLaunchMatrix4X4.lossyScale;
            initialBallSpeed = 500f;
            _isUsingPhysics = false;
            _isOnSlot = false;
            _rigidbody.useGravity = false;
            _rigidbody.drag = 0.1f;
            _rigidbody.angularDrag = 0.1f;
        }
        private void ApplyAttractionForce()
        {
            Vector3 forceDirection = (_targetSlot.position - transform.position).normalized;
            _rigidbody.AddForce(forceDirection * attractionForce, ForceMode.Acceleration);
        }

        private bool IsSlotNear()
        {
            Vector3 toSlot = (_targetSlot.position - rouletteWheel.position).normalized;
            Vector3 toBall = (transform.position - rouletteWheel.position).normalized;
            return Vector3.Dot(toSlot, toBall) > pullAreaDot;
        }

        private void EnablePhysicsMode()
        { 
            _isUsingPhysics = true;
            _rigidbody.useGravity = true;
            Vector3 forwardForce = _rigidbody.velocity.normalized * ballForwardForceMultiplier;
            _rigidbody.AddForce(forwardForce, ForceMode.Impulse);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_isOnSlot) return;
            if (!other.CompareTag("SlotMagnet")) return;
            StopBall();
        }

        private void StopBall()
        {
            _isOnSlot = true;
            _rigidbody.drag = 5;
            _rigidbody.angularDrag = 5f;
            CoreGameSignals.Instance.onBallStopped?.Invoke();
        }
    }
}