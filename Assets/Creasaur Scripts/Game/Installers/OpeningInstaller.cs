using Assets.Scripts.Game.SceneChangers;
using Environment;
using Helpers.Utils;
using Installer;
using UnityEngine;
using Zenject;

public class OpeningInstaller : MonoInstaller
{
    [SerializeField] ClientEnvironment clientEnvironment;

    public override void InstallBindings()
    {
        Container.ParentContainers[0].BindInstance(clientEnvironment);
        Container.ParentContainers[0].Bind<PrefsManager>().AsSingle().NonLazy();
        Container.ParentContainers[0].Bind<UserDataManager>().AsSingle().NonLazy();
        ContainerManager.MainContainer = Container.ParentContainers[0];
    }
}
