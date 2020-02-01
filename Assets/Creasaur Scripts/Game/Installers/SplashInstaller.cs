using Installer;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class SplashInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        //Container.ParentContainers[0].Bind<BallControl>();
        SceneManager.LoadScene("Opening");
    }
}