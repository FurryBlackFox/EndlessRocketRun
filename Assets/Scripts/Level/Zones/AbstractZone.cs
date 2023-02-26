using UnityEngine;

namespace Level.Zones
{
    [RequireComponent(typeof(Collider))]
    public abstract class AbstractZone : MonoBehaviour
    {
        [SerializeField] protected Collider _collider;
        [SerializeField] protected Color _gizmosColor = Color.red;

        protected virtual void OnValidate()
        {
            if (_collider == null)
                _collider = GetComponent<Collider>();
        }

        protected virtual void OnDrawGizmos()
        {
            Gizmos.color = _gizmosColor;
            var colliderBounds = _collider.bounds;
            var size = Vector3.Scale(colliderBounds.size, transform.localScale);
            Gizmos.DrawCube(colliderBounds.center, size);
        }
    }
}
