using System;
using System.Collections.Generic;
using GameStateMachine.GameStates;
using Settings;
using Signals;
using UnityEngine;
using Zenject;

namespace Player
{
    public class PlayerVfxController : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _deathVfx;

        [SerializeField] private ParticleSystem _engineVfx;
        [SerializeField] private Vector3 _engineVfxMinScale = Vector3.one;
        [SerializeField] private Vector3 _engineVfxMaxScale = 2 * Vector3.one;
        
        [SerializeField] private List<ParticleSystem> _brokenPlayerVfx;
        
        
        
        private SignalBus _signalBus;

        [Inject]
        private void Init(SignalBus signalBus)
        {
            _signalBus = signalBus;
        
            _signalBus.Subscribe<OnPlayerInputPerformed>(OnPlayerInputPerformed);
            _signalBus.Subscribe<OnPlayerDeath>(OnPlayerDeath);
            _signalBus.Subscribe<OnGameStateChanged>(OnGameStateChanged);
        }

        private void OnPlayerDeath(OnPlayerDeath obj)
        {
            ChangeEngineVfxState(false);
            
            _deathVfx.Play();
            ChangeBrokenVfxState(true);
        }

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<OnPlayerInputPerformed>(OnPlayerInputPerformed);
            _signalBus.Unsubscribe<OnPlayerDeath>(OnPlayerDeath);
            _signalBus.Unsubscribe<OnGameStateChanged>(OnGameStateChanged);
        }

        private void OnGameStateChanged(OnGameStateChanged changeEvent)
        {
            switch (changeEvent.currentStateType)
            {
                case GameStateType.Menu:
                    ChangeBrokenVfxState(false);
                    ChangeEngineVfxState(true);
                    break;
                case GameStateType.Play when changeEvent.prevStateType is GameStateType.Pause:
                    _engineVfx.Play();
                    break;
                case GameStateType.Pause:
                    _engineVfx.Pause();
                    break;
            }
        }

        private void OnPlayerInputPerformed(OnPlayerInputPerformed inputEvent)
        {
            _engineVfx.transform.localScale = Vector3.Lerp(_engineVfxMinScale, _engineVfxMaxScale, inputEvent.vertical);
        }

        private void ChangeBrokenVfxState(bool state)
        {
            foreach (var vfx in _brokenPlayerVfx)
            {
                vfx.gameObject.SetActive(state);
            }
        }

        private void ChangeEngineVfxState(bool state)
        {
            _engineVfx.gameObject.SetActive(state);
        }
    }
}