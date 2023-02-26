using System.Collections.Generic;
using GameStateMachine.GameStates;
using Signals;
using UnityEngine;
using Zenject;

namespace GameStateMachine
{
    public class GameStateMachine : MonoBehaviour
    {
        protected Dictionary<GameStateType, AbstractGameState> _gameStatesDictionary;
        
        private SignalBus _signalBus;
        
        private AbstractGameState _currentState;
        private GameStateType _currentStateType;
        
        [Inject]
        private void Init(SignalBus signalBus)
        {
            _signalBus = signalBus;
            
            InitStates();
        }


        private void InitStates()
        {
            _gameStatesDictionary = new Dictionary<GameStateType, AbstractGameState>
            {
                { GameStateType.Menu, new MenuState(this, _signalBus) },
                { GameStateType.Play, new PlayState(this, _signalBus) },
                { GameStateType.Pause, new PauseState(this, _signalBus) },
                { GameStateType.Defeat, new DefeatState(this, _signalBus) },
            };
        }

        public void ChangeState(GameStateType newStateType)
        {
            var prevStateType = _currentStateType;
            if (_currentState != null)
                _currentState.Exit();

            _currentState = _gameStatesDictionary[newStateType];
            _currentStateType = newStateType;
            _currentState.Enter();
            
            _signalBus.Fire(new OnGameStateChanged(prevStateType, newStateType));
        }

        private void Start()
        {
            ChangeState(GameStateType.Menu);
        }
    }
}