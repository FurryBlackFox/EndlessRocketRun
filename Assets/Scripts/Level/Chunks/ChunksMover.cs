using System;
using System.Collections;
using System.Collections.Generic;
using Settings.LevelBuilder;
using Signals;
using UnityEngine;
using Zenject;

namespace Level
{
    public class ChunksMover : MonoBehaviour
    {
        private bool _isAbleToMoveChunks;
        
        private LevelSettings _levelSettings;
        private SignalBus _signalBus;

        private float _cashedAdditionalMoveSpeed;
        private bool _isAdditionalSpeedActive;
        
        [Inject]
        private void Init(LevelSettings levelSettings, SignalBus signalBus)
        {
            _levelSettings = levelSettings;
            _signalBus = signalBus;
            
            _signalBus.Subscribe<OnPlayerInteractionWithBoostZone>(OnPlayerInteractionWithBoostZone);
        }

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<OnPlayerInteractionWithBoostZone>(OnPlayerInteractionWithBoostZone);
        }
        
        private void OnPlayerInteractionWithBoostZone(OnPlayerInteractionWithBoostZone interactionEvent)
        {
            _isAdditionalSpeedActive = interactionEvent.enteredZone;
        }
        public void ChangeMovementAvailabilityState(bool newState)
        {
            _isAbleToMoveChunks = newState;
        }

        public void TryToMoveChunks(IEnumerable<LevelChunk> chunks)
        {
            if (!_isAbleToMoveChunks)
                return;
            
            if(_isAdditionalSpeedActive || _cashedAdditionalMoveSpeed > 0)
                UpdateAdditionalSpeed();

            UpdateChunksPositions(chunks);
        }

        private void UpdateAdditionalSpeed()
        {
            var targetAdditionalMoveSpeed = _isAdditionalSpeedActive ? _levelSettings.MaxAdditionalBoostSpeed : 0f;
            
            var targetDeltaValue = _isAdditionalSpeedActive
                ? _levelSettings.AdditionalBoostSpeedIncreasePerSec
                : _levelSettings.AdditionalBoostSpeedDecreasePerSec;
            
            _cashedAdditionalMoveSpeed = Mathf.MoveTowards(_cashedAdditionalMoveSpeed, targetAdditionalMoveSpeed,
                targetDeltaValue * Time.fixedDeltaTime);
        }

        private void UpdateChunksPositions(IEnumerable<LevelChunk> chunks)
        {
            var moveSpeed = _levelSettings.DefaultGameSpeed + _cashedAdditionalMoveSpeed;
            var moveVector = Vector3.down * moveSpeed;
            
            foreach (var chunk in chunks)
            {
                chunk.Move(moveVector, Time.fixedDeltaTime);
            }
        }
    }
}