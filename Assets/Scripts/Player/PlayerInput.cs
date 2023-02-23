using System;
using UnityEngine;

namespace Player
{
    public class PlayerInput : MonoBehaviour
    {
        public static event Action<float> PlayerVerticalInput;
        public static event Action<float> PlayerHorizontalInput;

        [SerializeField] private float _horizontalInputIncrease = 1.6f;
        [SerializeField] private float _horizontalInputDecrease = 4.5f;

        [SerializeField] private float _verticalInputGain = 0.6f;

        [SerializeField] private float _maxRotationInputValue = 200f;

        private float _cashedVerticalInput;
        private float _cashedHorizontalInput;
        
        private Vector2 _cashedTouchStartPosition;
        
        private void Update()
        {
            CheckForInput();
        }

        private void CheckForInput()
        {
            var hasTouches = Input.touchCount > 0;
            _cashedVerticalInput = hasTouches ? UpdateVerticalInput() : 0;
            PlayerHorizontalInput?.Invoke(_cashedHorizontalInput);

            if (hasTouches && Input.GetTouch(0).phase is not TouchPhase.Ended)
            {
                var touch = Input.GetTouch(0);
        
                if (touch.phase is TouchPhase.Began)
                    _cashedTouchStartPosition = touch.position;
                
                _cashedHorizontalInput = IncreaseHorizontalInputValue(ConvertTouchToHorizontalMoveInputValue(touch));
            }
            else
            {
                _cashedTouchStartPosition = Vector2.zero;
                _cashedHorizontalInput = DecreaseHorizontalInputValue();
            }
            
            PlayerVerticalInput?.Invoke(_cashedVerticalInput);
        }
        
        private float UpdateVerticalInput()
        {
            var value = Mathf.MoveTowards(_cashedVerticalInput, 
                1f, _verticalInputGain * Time.deltaTime);
            
            return value;
        }
        
        
        private float IncreaseHorizontalInputValue(float input)
        {
            var value = Mathf.MoveTowardsAngle(_cashedHorizontalInput, 
                input, _horizontalInputIncrease * Time.deltaTime);
            
            return value;
        }
        
        private float DecreaseHorizontalInputValue()
        {
            var value = Mathf.MoveTowardsAngle(_cashedHorizontalInput, 
                0, _horizontalInputDecrease * Time.deltaTime);
            
            return value;
        }
        
        private float ConvertTouchToHorizontalMoveInputValue(Touch touch)
        {
            var touchDelta = touch.position - _cashedTouchStartPosition;
        
            var clampedInput = Mathf.Clamp(touchDelta.x, -_maxRotationInputValue, _maxRotationInputValue);

            var normalizedInput = clampedInput / _maxRotationInputValue;
            return normalizedInput;
        }
    }
}