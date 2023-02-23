using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        
        [SerializeField] private float _maxSpeed = 15f;
        [SerializeField] private float _maxSpeedAcceleration = 35f;
        
        [SerializeField] private float _maxRotationAngle = 55f;
        
        private float _currentSpeedAcceleration;
        private float _currentTargetRotationAngle;

        private void Awake()
        {
            PlayerInput.PlayerHorizontalInput += HandleHorizontalInput;
            PlayerInput.PlayerVerticalInput += HandleVerticalInput;
        }
        
        private void OnDestroy()
        {
            PlayerInput.PlayerHorizontalInput -= HandleHorizontalInput;
            PlayerInput.PlayerVerticalInput -= HandleVerticalInput;
        }


        private void HandleVerticalInput(float input)
        {
            _currentSpeedAcceleration = _maxSpeedAcceleration * input;
        }

        private void HandleHorizontalInput(float input)
        {
            _currentTargetRotationAngle = _maxRotationAngle * input;
        }

        private void FixedUpdate()
        {
            ApplySpeedAcceleration();
            ApplyRotation();
        }


        private void ApplySpeedAcceleration()
        {
            var upVector = transform.up;
            var dotValue = Vector3.Dot(upVector, _rigidbody.velocity);
            if(dotValue >= _maxSpeed)
                return;
        
            var force = upVector * _currentSpeedAcceleration;
            _rigidbody.AddForce(force, ForceMode.Acceleration);
        }

        private void ApplyRotation()
        {
            _rigidbody.MoveRotation(Quaternion.Euler(0f, 0f, _currentTargetRotationAngle));
        }
    }
}