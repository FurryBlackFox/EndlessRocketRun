using System;
using Level;
using UnityEngine;

namespace Settings.LevelBuilder
{
    [Serializable]
    public class LevelChunkData
    {
        [field: SerializeField] public LevelChunk ChunkPrefab { get; private set; }
    }
}