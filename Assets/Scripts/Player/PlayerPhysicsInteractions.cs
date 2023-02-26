using System;
using System.Collections.Generic;
using Level.Zones;
using Settings;
using Signals;
using UnityEngine;
using Zenject;

namespace Player
{
    public class PlayerPhysicsInteractions : MonoBehaviour
    {
        private SignalBus _signalBus;

        [Inject]
        private void Init(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<DeathZone>(out _))
            {
                _signalBus.Fire<OnPlayerDeath>();
                return;
            }
            
            if (other.TryGetComponent<BoostZone>(out _))
            {
                _signalBus.Fire(new OnPlayerInteractionWithBoostZone(true));
                return;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<BoostZone>(out _))
            {
                _signalBus.Fire(new OnPlayerInteractionWithBoostZone(false));
                return;
            }
        }
    }
}