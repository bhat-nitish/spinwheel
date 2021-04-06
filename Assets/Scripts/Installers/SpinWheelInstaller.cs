using Spinwheel.Models;
using Spinwheel.Presenters;
using Spinwheel.Services;
using Zenject;

namespace Installers
{
    public class SpinWheelInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ISpinWheelModel>().To<SpinWheelModel>().AsSingle();
            Container.Bind<ISpinWheelPresenter>().To<SpinWheelPresenter>().AsSingle();
            Container.Bind<ISpinWheelService>().To<SpinWheelService>().AsSingle();
        }
    }
}
