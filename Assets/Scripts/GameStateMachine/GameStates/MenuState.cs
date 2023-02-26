using Signals;
using Zenject;

namespace GameStateMachine.GameStates
{
    public class MenuState : AbstractGameState
    {
        public MenuState(GameStateMachine gameStateMachine, SignalBus signalBus) : base(gameStateMachine, signalBus)
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
                    gameStateMachine.ChangeState(buttonClickEvent.buttonTargetType);
                    break;
            }
        }
    }
}