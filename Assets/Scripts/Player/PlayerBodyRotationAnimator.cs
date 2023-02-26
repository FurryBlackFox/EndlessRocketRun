using Settings;
using Signals;
using UnityEngine;
using Zenject;

namespace Player
{
    public class PlayerBodyRotationAnimator : MonoBehaviour
    {
        private PlayerSettings _playerSettings;
        private SignalBus _signalBus;

        [Inject]
        private void Init(PlayerSettings playerSettings, SignalBus signalBus)
        {
            _playerSettings = playerSettings;
            _signalBus = signalBus;
        
            _signalBus.Subscribe<OnPlayerInputPerformed>(OnPlayerInputPerformed);
            _signalBus.Subscribe<OnPlayerDeath>(OnPlayerDeath);
        }

        private void OnPlayerDeath(OnPlayerDeath obj)
        {
            SetRotationInputValues(0f, 0f);
        }

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<OnPlayerInputPerformed>(OnPlayerInputPerformed);
            _signalBus.Unsubscribe<OnPlayerDeath>(OnPlayerDeath);
        }
    
        private void OnPlayerInputPerformed(OnPlayerInputPerformed inputEvent)
        {
            SetRotationInputValues(inputEvent.horizontal, inputEvent.vertical);
        }

        private void SetRotationInputValues(float horizontal, float vertical)
        {
            var rollRotation = Mathf.LerpUnclamped(0, _playerSettings.MaxRollRotationAngle, horizontal);
            var pitchRotation = Mathf.Lerp(0, _playerSettings.MaxPitchRotationAngle, vertical);
            transform.localRotation = Quaternion.Euler(pitchRotation, -rollRotation, 0f);
        }

   
    }
}
