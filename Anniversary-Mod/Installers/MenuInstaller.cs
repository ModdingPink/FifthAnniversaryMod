using FifthAnniversary.UI;
using Zenject;

namespace FifthAnniversary.Installers
{
    class MenuInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<DownloadSettings>().FromNewComponentOnRoot().AsSingle();
        }
    }
}
