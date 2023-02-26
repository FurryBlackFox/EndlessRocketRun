using System.Collections.Generic;
using Level;
using UnityEngine;

namespace Settings.LevelBuilder
{
    [CreateAssetMenu(fileName = "LevelSettings", menuName = "App/LevelSettings", order = 0)]
    public class LevelSettings : ScriptableObject
    {
        [SerializeField, Header("Prefabs")] private List<LevelChunkData> _chunkData = new List<LevelChunkData>();
        [field: SerializeField] public LevelChunk DefaultChunkPrefab { get; private set; }
        
        [field: SerializeField, Header("Constants")] public float ChunkSpawnOffset { get; private set; } = 15f;
        [field: SerializeField] public Vector2 LevelBounds { get; private set; } = 10f * Vector2.one;

        [field: SerializeField, Header("Chunks Movement")] public float DefaultGameSpeed { get; private set; } = 5f;
        [field: SerializeField] public float MaxAdditionalBoostSpeed { get; private set; } = 15f;
        [field: SerializeField] public float AdditionalBoostSpeedIncreasePerSec { get; private set; } = 5f;
        [field: SerializeField] public float AdditionalBoostSpeedDecreasePerSec { get; private set; } = 5f;

        public LevelChunk GetRandomLevelChunkPrefab()
        {
            var randomValue = Random.Range(0, _chunkData.Count);
            return _chunkData[randomValue].ChunkPrefab;
        }
    }
}