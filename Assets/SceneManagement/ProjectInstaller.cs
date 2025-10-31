using Infrastructure;
using Zenject;

namespace SceneManagement
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<SaveDataManager>().To<SaveDataManager>().AsSingle();
            Container.Bind<SceneManager>().To<SceneManager>().AsSingle();
        }
    }
}