using Signals;
using Zenject;

namespace GameStateMachine.GameStates
{
    public class PlayState : AbstractGameState
    {
        public PlayState(GameStateMachine gameStateMachine, SignalBus signalBus) : base(gameStateMachine, signalBus)
        {
        }

        public override void Enter()
        {
            signalBus.Subscribe<OnGameStateChangeButtonClick>(OnGameStateChangeButtonClick);
            signalBus.Subscribe<OnPlayerDeath>(OnPlayerDeath);
        }

        public override void Exit()
        {
            signalBus.Unsubscribe<OnGameStateChangeButtonClick>(OnGameStateChangeButtonClick);
            signalBus.Unsubscribe<OnPlayerDeath>(OnPlayerDeath);
        }

        private void OnPlayerDeath()
        {
            gameStateMachine.ChangeState(GameStateType.Defeat);
        }
        
        private void OnGameStateChangeButtonClick(OnGameStateChangeButtonClick buttonClickEvent)
        {
            if (buttonClickEvent.buttonTargetType == GameStateType.Pause)
                gameStateMachine.ChangeState(buttonClickEvent.buttonTargetType);
            
        }
    }
}