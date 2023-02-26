using GameStateMachine.GameStates;

namespace Signals
{
    public class OnGameStateChanged
    {
        public readonly GameStateType prevStateType;
        public readonly GameStateType currentStateType;
        
        public OnGameStateChanged(GameStateType prevStateType, GameStateType currentStateType)
        {
            this.prevStateType = prevStateType;
            this.currentStateType = currentStateType;
        }
    }
    
    public class OnGameStateChangeButtonClick
    {
        public readonly GameStateType buttonTargetType;

        public OnGameStateChangeButtonClick(GameStateType buttonTargetType)
        {
            this.buttonTargetType = buttonTargetType;
        }
    }

    public class OnPlayerDeath { }
    
    public class OnPlayerInteractionWithBoostZone
    {
        public readonly bool enteredZone;

        public OnPlayerInteractionWithBoostZone(bool enteredZone)
        {
            this.enteredZone = enteredZone;
        }
    }
    
    public class OnPlayerInputPerformed
    {
        public float vertical;
        public float horizontal;

    }
    
}