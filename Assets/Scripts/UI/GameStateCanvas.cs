using System.Collections.Generic;
using GameStateMachine.GameStates;
using Signals;
using UnityEngine;
using Zenject;

namespace UI
{
    public class GameStateCanvas : MonoBehaviour
    {
        [SerializeField] private List<GameStateType> _enabledOnGameStateTypesList;

        private SignalBus _signalBus;

        private bool _currentEnabledState;
        
        [Inject]
        private void Init(SignalBus signalBus)
        {
            _signalBus = signalBus;
            
            _signalBus.Subscribe<OnGameStateChanged>(OnGameStateChanged);
            
            TryToChangeState(false, true);
        }

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<OnGameStateChanged>(OnGameStateChanged);
        }

        private void OnGameStateChanged(OnGameStateChanged onGameStateChangedEvent)
        {
            var currentGameStateType = onGameStateChangedEvent.currentStateType;
            var newState = _enabledOnGameStateTypesList.Contains(currentGameStateType);

            TryToChangeState(newState);
        }

        private void TryToChangeState(bool newState, bool force = false)
        {
            if (_currentEnabledState == newState && !force)
                return;

            _currentEnabledState = newState;
            
            gameObject.SetActive(_currentEnabledState);
        }
    }
}