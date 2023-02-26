using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using Settings.LevelBuilder;
using UnityEngine;
using Zenject;

namespace Level
{
    public class ChunksFactory : MonoBehaviour
    {
        public Queue<LevelChunk> SpawnedChunks { get; private set; } = new Queue<LevelChunk>();

        private LevelChunk _firstChunk;

        private LevelSettings _levelSettings;
        private SignalBus _signalBus;

        [Inject]
        private void Init(SignalBus signalBus, LevelSettings levelSettings)
        {
            _levelSettings = levelSettings;
            _signalBus = signalBus;
        }
        
        public void FixedTick()
        {
            TryToSpawnChunk(false);
            TryToDespawnLastChunk();
        }

        private void TryToForceDespawnAllChunks()
        {
            StopAllCoroutines();

            while (SpawnedChunks.Count > 0)
            {
                ForceDespawnLastChunk();
            }
        }

        public void InitialSpawn()
        {
            TryToForceDespawnAllChunks();

            var spawnResult = false;
            do
            {
                spawnResult = TryToSpawnChunk(true);
            } while (spawnResult);
        }

        private bool TryToSpawnChunk(bool defaultChunk)
        {
            var spawnPosition = GetNewChunkSpawnPosition();
            if (spawnPosition.y > _levelSettings.LevelBounds.y)
                return false;

            var targetChunkPrefab = defaultChunk
                ? _levelSettings.DefaultChunkPrefab
                : _levelSettings.GetRandomLevelChunkPrefab();

            SpawnChunk(spawnPosition, targetChunkPrefab);
            return true;
        }

        private Vector3 GetNewChunkSpawnPosition()
        {
            var spawnPosition = Vector3.zero;
            if (!_firstChunk)
            {
                spawnPosition.y -= (_levelSettings.LevelBounds.y / 2);
                return spawnPosition;
            }
            
            spawnPosition = _firstChunk.transform.position;
            spawnPosition.y += _levelSettings.ChunkSpawnOffset;
            return spawnPosition;
        }

        private void SpawnChunk(Vector3 spawnPosition, LevelChunk targetChunkPrefab)
        {
            var newChunk = LeanPool.Spawn(targetChunkPrefab, spawnPosition, Quaternion.identity, transform);
            
            newChunk.TryToInit(_levelSettings.LevelBounds, _signalBus);
            newChunk.OnSpawn();

            _firstChunk = newChunk;

            SpawnedChunks.Enqueue(_firstChunk);
        }

        private void ForceDespawnLastChunk()
        {
            var targetChunk = SpawnedChunks.Peek();
            StartCoroutine(DespawnChunk(targetChunk));
        }

        private bool TryToDespawnLastChunk()
        {
            if (SpawnedChunks.Count == 0)
                return false;

            var targetPlatform = SpawnedChunks.Peek();
            if (targetPlatform.CheckIsInGameBounds())
                return false;


            StartCoroutine(DespawnChunk(targetPlatform));
            return true;
        }

        private IEnumerator DespawnChunk(LevelChunk targetChunk)
        {
            if (SpawnedChunks.Peek() != targetChunk)
            {
                Debug.LogError("DESPAWN ERROR");
                yield break;
            }

            SpawnedChunks.Dequeue();
            
            LeanPool.Despawn(targetChunk);
            
            if (_firstChunk == targetChunk)
                _firstChunk = null;
        }
    }
}