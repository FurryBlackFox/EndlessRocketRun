using Signals;
using Zenject;

namespace GameStateMachine.GameStates
{
    public class PauseState : AbstractGameState
    {
        public PauseState(GameStateMachine gameStateMachine, SignalBus signalBus) : base(gameStateMachine, signalBus)
        {
        }

        public override void Enter()
        {
            signalBus.Subscribe<OnGameStateChangeButtonClick>(OnGameStateChangeButtonClick);
        }

        public override void Exit()
        {
            signalBus.Unsubscribe<OnGameStateChangeButtonClick>(OnGameStateChangeButtonClick);
        }

        private void OnGameStateChangeButtonClick(OnGameStateChangeButtonClick buttonClickEvent)
        {
            switch (buttonClickEvent.buttonTargetType)
            {
                case GameStateType.Play:
                case GameStateType.Menu:
                    gameStateMachine.ChangeState(buttonClickEvent.buttonTargetType);
                    break;
            }
        }
    }
}