using UnityEngine;
using Zenject;

namespace Installers
{
    public class GlobalManagersInstaller : MonoInstaller
    {
        [SerializeField] private GameStateMachine.GameStateMachine _gameStateMachine;

        private void OnValidate()
        {
            if (_gameStateMachine == null)
                _gameStateMachine = GetComponentInChildren<GameStateMachine.GameStateMachine>();
        }

        public override void InstallBindings()
        {
            Container.Bind<GameStateMachine.GameStateMachine>().FromInstance(_gameStateMachine).AsSingle().NonLazy();
        }
    }
}