using GameStateMachine.GameStates;
using Signals;
using UnityEngine;
using Zenject;

namespace Level
{
    public class ChanksManager : MonoBehaviour
    {
        [SerializeField] private ChunksFactory _chunksFactory;
        [SerializeField] private ChunksMover _chunksMover;
        
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
            if (_chunksMover == null)
                _chunksMover = GetComponentInChildren<ChunksMover>();

            if (_chunksFactory == null)
                _chunksFactory = GetComponentInChildren<ChunksFactory>();
        }

        
        private void OnGameStateChanged(OnGameStateChanged stateChangedEvent)
        {
            switch (stateChangedEvent.currentStateType)
            {
                case GameStateType.Menu:
                    _chunksFactory.InitialSpawn();
                    _chunksMover.ChangeMovementAvailabilityState(false);
                    break;
                case GameStateType.Play:
                    _chunksMover.ChangeMovementAvailabilityState(true);
                    break;
                case GameStateType.Pause:
                    _chunksMover.ChangeMovementAvailabilityState(false);
                    break;
                case GameStateType.Defeat:
                    //_chunksMover.ChangeMovementAvailabilityState(false);
                    break;
            }
        }

        private void FixedUpdate()
        {
            _chunksMover.TryToMoveChunks(_chunksFactory.SpawnedChunks);
            
            _chunksFactory.FixedTick();
        }
        
    }
}