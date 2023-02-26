using System.Collections.Generic;
using Level;
using Settings.LevelBuilder;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "PlayerSettings", menuName = "App/PlayerSettings", order = 0)]
    public class PlayerSettings : ScriptableObject
    {
        [field: SerializeField, Header("Input")]  public float HorizontalInputIncreasePerSec { get; private set; } = 7f;
        [field: SerializeField] public float HorizontalInputDecreasePerSec { get; private set; } = 2f;
        [field: SerializeField] public float VerticalInputGainPerSec { get; private set; } = 1.8f;
        [field: SerializeField] public float MaxRotationInputValue { get; private set; } = 200f;
        
        
        [field: SerializeField, Header("Movement")]  public float MaxSpeed { get; private set; } = 15f;
        [field: SerializeField] public float MaxSpeedAcceleration { get; private set; } = 60f;
        [field: SerializeField] public float MaxRotationAngle { get; private set; } = 65f;

        
        [field: SerializeField, Header("Defeat Explosion")] public Vector2 ExplosionVelocityForceRange { get; private set; }
        [field: SerializeField] public Vector2 ExplosionRotationForceRange { get; private set; }
        [field: SerializeField] public float ExplosionVelocityForceMaxRandomAngle { get; private set; } = 15f;

        
        [field: SerializeField, Header("Additional Rotation Animation")]
        public float MaxRollRotationAngle { get; private set; } = 45f;
        [field: SerializeField] public float MaxPitchRotationAngle { get; private set; } = 7.5f;
    }
}