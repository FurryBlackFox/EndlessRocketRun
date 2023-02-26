using Settings.LevelBuilder;
using UnityEngine;
using Zenject;

namespace Level
{
    public class LevelChunk : MonoBehaviour
    {
        private Vector2 _leveBounds;
        
        public void Move(Vector3 moveVector, float fixedDeltaTime)
        {
            transform.position += moveVector * fixedDeltaTime;
        }

        public void OnSpawn()
        {
           
        }

        public void TryToInit(Vector2 levelBounds, SignalBus signalBus)
        {
            _leveBounds = levelBounds;
        }

        public bool CheckIsInGameBounds()
        {
            var position = transform.position;

            if (Mathf.Abs(position.x) > _leveBounds.x || Mathf.Abs(position.y) > _leveBounds.y)
                return false;

            return true;
        }

        public void OnDespawnStarted()
        {
           
        }

        public void OnDespawnFinished()
        {
            
        }
    }
}