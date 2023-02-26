using Settings;
using Settings.LevelBuilder;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class SettingsInstaller : MonoInstaller
    {
        [SerializeField] private PlayerSettings _playerSettings;
        [SerializeField] private LevelSettings _levelSettings;

        public override void InstallBindings()
        {
            Container.Bind<PlayerSettings>().FromScriptableObject(_playerSettings).AsSingle();
            Container.Bind<LevelSettings>().FromScriptableObject(_levelSettings).AsSingle();
        }
    }
}