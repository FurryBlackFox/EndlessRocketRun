using Signals;
using Zenject;

namespace GameStateMachine.GameStates
{
    public class DefeatState : AbstractGameState
    {
        public DefeatState(GameStateMachine gameStateMachine, SignalBus signalBus) : base(gameStateMachine, signalBus)
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
            if (buttonClickEvent.buttonTargetType == GameStateType.Menu)
                gameStateMachine.ChangeState(buttonClickEvent.buttonTargetType);
            
        }
    }
}