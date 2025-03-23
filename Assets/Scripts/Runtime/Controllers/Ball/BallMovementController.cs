using System;
using System.Collections;
using System.Threading;
using UnityEngine;

namespace Runtime.Controllers.Ball
{
    public class BallMovementController : MonoBehaviour
    {
        #region Self Variables
        
        #region Serialized Field Variables
        
        [SerializeField] private Transform targetSlot;
        [SerializeField] private Transform rouletteWheel;
        [SerializeField] private float initialSpeed  = 500f;
        [SerializeField] private float slowDownRate  = 0.99f;
        [SerializeField] private float stopThreshold = 0.05f;
        [SerializeField] private float ballForwardForceMultiplier = 2f;
        [SerializeField] private float pullAreaDot = 0.9f;
        [SerializeField] private float minBallSpeedToPull = 30f;
        [SerializeField] private float attractionForce = 10f;
        
        #endregion

        #region Private Variables

        private Rigidbody _rigidbody;
        private bool _isSpinning;
        private bool _isUsingPhysics;
        
        #endregion

        #endregion
        
        void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.useGravity = false;
            _rigidbody.drag = 0.1f;
            _rigidbody.angularDrag = 0.1f;
            Spin();
        }
        
        void Update()
        {
            if (!_isSpinning) return;
            if (_isUsingPhysics) return;
            // Rulet etrafında döndür
            transform.RotateAround(rouletteWheel.position, Vector3.up, initialSpeed * Time.deltaTime);
            // Hızı yavaşlat
            if (initialSpeed > minBallSpeedToPull)
            {
                initialSpeed -= slowDownRate;
            }
            if (initialSpeed > minBallSpeedToPull) return;
            if (!IsSlotNear()) return;
            EnablePhysicsMode();
        }

        private void FixedUpdate()
        {
            if (!_isSpinning) return;
            if (!_isUsingPhysics) return;
            ApplyAttractionForce();
            if (_rigidbody.velocity.magnitude < stopThreshold)
            {
                Debug.Log("Ball stopped spinning.");
                //_isSpinning = false;
            }
        }
        
        internal void Spin()
        {
            _isSpinning = true;
        }

        private void ApplyAttractionForce()
        {
            Debug.Log("Attract");
            Vector3 forceDirection = (targetSlot.position - transform.position).normalized;
            _rigidbody.AddForce(forceDirection * attractionForce, ForceMode.Acceleration);
        }

        private bool IsSlotNear()
        {
            Vector3 toSlot = (targetSlot.position - rouletteWheel.position).normalized;
            Vector3 toBall = (transform.position - rouletteWheel.position).normalized;
            return Vector3.Dot(toSlot, toBall) > pullAreaDot;
        }

        private void EnablePhysicsMode()
        {
            Debug.Log("Physics mode enabled.");
            _isUsingPhysics = true;
            _rigidbody.useGravity = true;
            // Mevcut hareket yönünü koruyarak kuvvet uygula
            Vector3 forwardForce = _rigidbody.velocity.normalized * ballForwardForceMultiplier;
            _rigidbody.AddForce(forwardForce, ForceMode.Impulse);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_isSpinning) return;
            if (!other.CompareTag("SlotMagnet")) return;
            Debug.Log("on Trigger");
            _rigidbody.drag = 5f;
            _rigidbody.angularDrag = 5f;
            //attractionForce *= 2;
        }
        
        private void GetRadiusOfObject()
        {
            Transform childTransform = gameObject.transform.Find("Mesh");
            if (childTransform != null)
            {
                Bounds bounds = childTransform.GetComponent<MeshRenderer>().bounds;
                float radius = bounds.extents.magnitude;  // En büyük ekseni almak için
                Debug.Log("Yarıçap: " + radius + " metre");
                Debug.Log("Child Nesnesindeki MeshRenderer bulundu.");
            }
            else
            {
                Debug.Log("Belirtilen isimde bir child bulunamadı.");
            }
        }
    }
}