using Installer;
using System.Collections;
using System.Collections.Generic;
using Timer;
using UnityEngine;
using Zenject;
using System.Linq;

namespace Assets.Scripts.Game.Managers
{
    public class SpawnManager : MonoBehaviour
    {
        private CreaCoroutine _timer;
        private PoolManager _objectPooler;
        private SimulationAreaManager _simulationAreaManager;
        protected ShopManager _shopManager;

        private ParkingLot _parkingLot;
        private int _advertisingLevel { get { return _parkingLot.ParkingLotData.AdvertisementLevel; } } //take from config
        private float _currentTime;
        private double _carsEntry { get { return 0.8f * _advertisingLevel + 7.2f; } }
        public bool CustomerExistsInTheArea;

        [Inject]
        public void Setup(ParkingLot lot)
        {
            _parkingLot = lot;
        }

        public void Start()
        {
            _currentTime = 60f;
            CustomerExistsInTheArea = false;

            var container = ContainerManager.GameSceneContainer;
            _timer = container.TryResolve<CreaCoroutine>();
            _objectPooler = container.TryResolve<PoolManager>();
            _simulationAreaManager = container.TryResolve<SimulationAreaManager>();
            _shopManager = container.TryResolve<ShopManager>();
        }

        public void Update()
        {
            _currentTime += Time.deltaTime;
            if (_currentTime >= (60 / _carsEntry))
            {
                CallCustomer();
                _currentTime = 0f;
            }
        }


        private void CallCustomer()
        {
            Customer customer = (Customer)_objectPooler.GetActiveObjectFromPool();

            var position = _simulationAreaManager.EntryPos;

            if (customer != null)
            {
                customer.transform.position = position;
                customer.transform.rotation = customer.transform.rotation;
                customer.Enable();
                var shop = _shopManager.ShopGameObjectsList[UnityEngine.Random.Range(0, _shopManager.ShopGameObjectsList.Count(x => x.gameObject.activeInHierarchy))];
                customer.SetTargetShopAndPatrol(shop);
            }
        }

        public void CallAGroupofCustomers(int number, Vector3 position)
        {
            StartCoroutine(CallCustomersOneByOne(number, position));
        }

        private IEnumerator CallCustomersOneByOne(int number, Vector3 position)
        {
            while (number > 0)
            {
                CallCustomer(position);
                number--;
                yield return new WaitForSeconds(0.2f);
            }
        }

        private void CallCustomer(Vector3 position)
        {
            Customer customer = (Customer)_objectPooler.GetActiveObjectFromPool();
            if (customer != null)
            {
                customer.transform.position = position;
                customer.transform.rotation = customer.transform.rotation;
                customer.Enable();
                var shop = _shopManager.ShopGameObjectsList[UnityEngine.Random.Range(0, _shopManager.ShopGameObjectsList.Count)];
                customer.SetTargetShopAndPatrol(shop);
            }
        }
    }
}
