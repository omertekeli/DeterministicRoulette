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
        [SerializeField] private float ballStopThreshold = 0.05f;
        [SerializeField] private float ballForwardForceMultiplier = 2f;
        [SerializeField] private float pullAreaDot = 0.85f;
        [SerializeField] private float minBallSpeedToPull = 150f; //Must be greater than wheel rotation speed
        [SerializeField] private float attractionForce = 20f;
        
        #endregion

        #region Private Variables

        private Vector3 _ballLaunchPosition;
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
            _ballLaunchPosition = transform.position;
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

        void Update()
        {
            if (!_isSpinning) return;
            if (_isUsingPhysics) return;
            transform.RotateAround(rouletteWheel.position, Vector3.up, initialBallSpeed * Time.deltaTime);
            if (initialBallSpeed > minBallSpeedToPull)
            {
                initialBallSpeed -= ballSpeedSlowDownRate;
            }
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
        
        internal void PrepareSpin()
        {
            initialBallSpeed = 500f;
            _isUsingPhysics = false;
            _isOnSlot = false;
            transform.SetParent(null);
            transform.position = _ballLaunchPosition;
        }
        
        internal void Spin()
        {
            _isSpinning = true;
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
            if (!_isSpinning) return;
            if (!other.CompareTag("SlotMagnet")) return;
            StopBall();
        }

        private void StopBall()
        {
            _rigidbody.drag = 5f;
            _rigidbody.angularDrag = 5f;
            _isOnSlot = true;
            transform.SetParent(_targetSlot);
            CoreGameSignals.Instance.onBallStopped?.Invoke();
        }
    }
}