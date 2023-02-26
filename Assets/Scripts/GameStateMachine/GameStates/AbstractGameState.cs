using Zenject;

namespace GameStateMachine.GameStates
{
    public enum GameStateType
    {
        None = 0,
        Menu = 1 << 0,
        Play = 1 << 1,
        Pause= 1 << 2,
        Defeat = 1 << 3,
    }
    
    public abstract class AbstractGameState
    {
        protected GameStateMachine gameStateMachine;
        protected SignalBus signalBus;

        public AbstractGameState(GameStateMachine gameStateMachine, SignalBus signalBus)
        {
            this.gameStateMachine = gameStateMachine;
            this.signalBus = signalBus;
        }

        public abstract void Enter();

        public abstract void Exit();
    }
}