using Signals;
using Zenject;

namespace Installers
{
    public class SignalsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            
            Container.DeclareSignal<OnGameStateChanged>();
            Container.DeclareSignal<OnGameStateChangeButtonClick>();

            Container.DeclareSignal<OnPlayerInputPerformed>();
            Container.DeclareSignal<OnPlayerDeath>();
            Container.DeclareSignal<OnPlayerInteractionWithBoostZone>();
        }
    }
}