using System;
using Settings;
using Signals;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        
        private bool _isAbleToMove;
        
        private float _currentSpeedAcceleration;
        private float _currentTargetRotationAngle;

        private Vector3 _startRotation;
        private Vector3 _startPosition;
        private float _startAngularDrag;
        private RigidbodyConstraints _startRigidbodyConstraints;
        
        private PlayerSettings _playerSettings;
        private SignalBus _signalBus;

        [Inject]
        private void Init(PlayerSettings playerSettings, SignalBus signalBus)
        {
            _playerSettings = playerSettings;
            _signalBus = signalBus;
            
            _signalBus.Subscribe<OnPlayerInputPerformed>(OnPlayerInputPerformed);
        }

        private void OnValidate()
        {
            if (_rigidbody == null)
                _rigidbody = GetComponentInChildren<Rigidbody>();
        }

        private void Awake()
        {
            SetStartValues();
        }
        
        private void OnDestroy()
        {
            _signalBus.Unsubscribe<OnPlayerInputPerformed>(OnPlayerInputPerformed);
        }

        private void OnPlayerInputPerformed(OnPlayerInputPerformed inputEvent)
        {
            _currentSpeedAcceleration = _playerSettings.MaxSpeedAcceleration * inputEvent.vertical;
            _currentTargetRotationAngle = _playerSettings.MaxRotationAngle * inputEvent.horizontal;
        }

        private void SetStartValues()
        {
            _startPosition = _rigidbody.position;
            _startRotation = _rigidbody.rotation.eulerAngles;
            _startAngularDrag = _rigidbody.angularDrag;
            _startRigidbodyConstraints = _rigidbody.constraints;

            _isAbleToMove = true;
        }

        private void FixedUpdate()
        {
            if(!_isAbleToMove)
                return;
            
            ApplySpeedAcceleration();
            ApplyRotation();
        }


        private void ApplySpeedAcceleration()
        {
            var upVector = transform.up;
            var dotValue = Vector3.Dot(upVector, _rigidbody.velocity);
            if(dotValue >= _playerSettings.MaxSpeed)
                return;
        
            var force = upVector * _currentSpeedAcceleration;
            _rigidbody.AddForce(force, ForceMode.Acceleration);
        }

        private void ApplyRotation()
        {
            _rigidbody.MoveRotation(Quaternion.Euler(0f, 0f, _currentTargetRotationAngle));
        }

        public void ResetValues()
        {
            _currentSpeedAcceleration = 0f;
            _currentTargetRotationAngle = 0f;
            
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            
            _rigidbody.angularDrag = _startAngularDrag;
            _rigidbody.constraints = _startRigidbodyConstraints;
            
            _rigidbody.rotation = Quaternion.Euler(_startRotation);
            _rigidbody.position = _startPosition;
        }

        public void ChangeMoveAvailabilityState(bool state)
        {
            _isAbleToMove = state;
        }

        public void Explode(Vector3 position)
        {
            var velocityForce = Random.Range(_playerSettings.ExplosionVelocityForceRange.x,
                _playerSettings.ExplosionVelocityForceRange.y);
            var velocityForceAngle = Random.Range(-_playerSettings.ExplosionVelocityForceMaxRandomAngle,
                _playerSettings.ExplosionVelocityForceMaxRandomAngle);
            var velocityForceDirection = Quaternion.Euler(0f, 0f, velocityForceAngle) * Vector3.up;

            var rotationForce = Random.Range(_playerSettings.ExplosionRotationForceRange.x,
                _playerSettings.ExplosionRotationForceRange.y);
            var rotationDirection = Random.insideUnitSphere;

            _rigidbody.AddForce(velocityForce * velocityForceDirection, ForceMode.VelocityChange);

            _rigidbody.angularDrag = 0f;
            _rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;
            
            _rigidbody.AddRelativeTorque(rotationForce * rotationDirection, ForceMode.VelocityChange);
        }

        public void SetGravityEnabledState(bool state)
        {
            _rigidbody.useGravity = state;
        }
        
        public void SetIsKinematicEnabledState(bool state)
        {
            _rigidbody.isKinematic = state;
        }
    }
}