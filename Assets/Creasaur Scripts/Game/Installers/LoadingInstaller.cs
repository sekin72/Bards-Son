using Installer;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class LoadingInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        //Container.ParentContainers[0].Bind<BallControl>();
        SceneManager.LoadScene("Game");
    }
}