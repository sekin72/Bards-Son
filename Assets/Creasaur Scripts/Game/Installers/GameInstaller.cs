using Assets.Scripts.Game.Managers;
using Installer;
using TouchSystem;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private GameObject _touchInteractor;
    [SerializeField] private PopupController _popupController;
    [SerializeField] private Timer.CreaCoroutine _creaCoroutine;
    [SerializeField] private PoolManager _poolManager;
    [SerializeField] private SimulationAreaManager _simualtionAreaManager;
    [SerializeField] private ShopManager _shopManager;
    [SerializeField] private ParkingLot _parkingLot;
    [SerializeField] private SpecialEventManager _specialEventManager;
    [SerializeField] private SpawnManager _spawnManager;
    [SerializeField] private CameraMovement _cameraMovement;
    [SerializeField] private NavMeshSurface _navMeshSurface;
    [SerializeField] private SpecialEvent[] _specialEvents;


    public override void InstallBindings()
    {

#if UNITY_EDITOR
        _touchInteractor.AddComponent(typeof(TouchSimulator));
#else
        _touchInteractor.AddComponent(typeof(TouchInteractor));
#endif
        Container.BindInstance(_spawnManager);
        Container.BindInstance(_navMeshSurface);
        Container.BindInstance(_cameraMovement);
        Container.BindInstance(_poolManager);
        Container.BindInstance(_parkingLot);
        Container.BindInstance(_specialEvents);
        Container.BindInstance(_specialEventManager);
        Container.Bind<ShopQueueController>().AsTransient();
        Container.Bind<GameDataManager>().AsSingle();
        Container.BindInstance(_creaCoroutine);
        Container.BindInstance(_simualtionAreaManager);
        Container.BindInstance(_shopManager);
        Container.BindInstance(_touchInteractor.GetComponent<TouchInteractor>());
        Container.BindInstance(_popupController.GetComponent<PopupController>());
        ContainerManager.GameSceneContainer = Container;
    }
}
