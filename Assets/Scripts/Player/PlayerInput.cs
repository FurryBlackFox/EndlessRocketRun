using System;
using Settings;
using Signals;
using UnityEngine;
using Zenject;

namespace Player
{
    public class PlayerInput : MonoBehaviour
    {
        private float _cashedVerticalInput;
        private float _cashedHorizontalInput;
        
        private Vector2 _cashedTouchStartPosition;

        private bool _isInputEnabled;
        private OnPlayerInputPerformed _cashedInputEvent = new OnPlayerInputPerformed();
        
        private PlayerSettings _playerSettings;
        private SignalBus _signalBus;

        [Inject]
        private void Init(PlayerSettings playerSettings, SignalBus signalBus)
        {
            _playerSettings = playerSettings;
            _signalBus = signalBus;
        }

        public void ChangeInputEnabledState(bool state)
        {
            _isInputEnabled = state;
        }

        public void ResetValues()
        {
            _cashedHorizontalInput = 0f;
            _cashedVerticalInput = 0f;
            _cashedTouchStartPosition = Vector2.zero;
        }
        
        private void Update()
        {
            if(_isInputEnabled)
                CheckForInput();
        }

        private void CheckForInput()
        {
            var hasTouches = Input.touchCount > 0;
            _cashedVerticalInput = hasTouches ? UpdateVerticalInput() : 0;
            _cashedInputEvent.vertical = _cashedVerticalInput;

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

            _cashedInputEvent.horizontal = _cashedHorizontalInput;
            _signalBus.Fire(_cashedInputEvent);
        }
        
        private float UpdateVerticalInput()
        {
            var value = Mathf.MoveTowards(_cashedVerticalInput, 
                1f, _playerSettings.VerticalInputGainPerSec * Time.deltaTime);
            
            return value;
        }
        
        
        private float IncreaseHorizontalInputValue(float input)
        {
            var value = Mathf.MoveTowardsAngle(_cashedHorizontalInput, 
                input, _playerSettings.HorizontalInputIncreasePerSec * Time.deltaTime);
            
            return value;
        }
        
        private float DecreaseHorizontalInputValue()
        {
            var value = Mathf.MoveTowardsAngle(_cashedHorizontalInput, 
                0, _playerSettings.HorizontalInputDecreasePerSec * Time.deltaTime);
            
            return value;
        }
        
        private float ConvertTouchToHorizontalMoveInputValue(Touch touch)
        {
            var touchDelta = touch.position - _cashedTouchStartPosition;

            var clampedInput = Mathf.Clamp(touchDelta.x, -_playerSettings.MaxRotationInputValue,
                _playerSettings.MaxRotationInputValue);

            var normalizedInput = clampedInput / _playerSettings.MaxRotationInputValue;
            return normalizedInput;
        }
    }
}