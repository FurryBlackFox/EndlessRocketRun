using GameStateMachine.GameStates;
using Signals;
using UnityEngine;
using Zenject;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private PlayerMovement _playerMovement;

        private SignalBus _signalBus;

        
        [Inject]
        private void Init(SignalBus signalBus)
        {
            _signalBus = signalBus;
            
            _signalBus.Subscribe<OnGameStateChanged>(OnGameStateChanged);
        }

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<OnGameStateChanged>(OnGameStateChanged);
        }

        private void OnValidate()
        {
            if (_playerInput == null)
                _playerInput = GetComponentInChildren<PlayerInput>();

            if (_playerMovement == null)
                _playerMovement = GetComponentInChildren<PlayerMovement>();
        }
        
        private void OnGameStateChanged(OnGameStateChanged stateChangedEvent)
        {
            switch (stateChangedEvent.currentStateType)
            {
                case GameStateType.Menu:
                    _playerInput.ChangeInputEnabledState(false);
                    _playerMovement.SetIsKinematicEnabledState(true);
                    _playerMovement.ResetValues();
                    _playerMovement.ChangeMoveAvailabilityState(false);
                    _playerMovement.SetGravityEnabledState(false);
                    _playerInput.ResetValues();
                    break;
                case GameStateType.Play:
                    _playerMovement.SetIsKinematicEnabledState(false);
                    _playerInput.ChangeInputEnabledState(true);
                    _playerMovement.ChangeMoveAvailabilityState(true);
                    _playerMovement.SetGravityEnabledState(true);
                    break;
                case GameStateType.Pause:
                    _playerInput.ChangeInputEnabledState(false);
                    _playerMovement.SetIsKinematicEnabledState(true);
                    _playerMovement.ChangeMoveAvailabilityState(false);
                    break;
                case GameStateType.Defeat:
                    _playerInput.ChangeInputEnabledState(false);
                    //_playerMovement.ChangeMoveAvailabilityState(false);
                    _playerMovement.ChangeMoveAvailabilityState(false);
                    _playerMovement.Explode(transform.position);
                    break;
            }
        }
    }
}