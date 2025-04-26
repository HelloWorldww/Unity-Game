using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.DamoncStudios.Scripts
{
    public class Warehouse : MonoBehaviour, ManagerLocation
    {
        [Header("Prefab")]
        [SerializeField] private WarehouseFarmer helperPrefab;

        [Header("Extras")]
        [SerializeField] private Deposit farmHouseDeposit;
        [SerializeField] private Transform farmHouseDepositLocation;
        [SerializeField] private Transform warehouseDepositLocation;

        [SerializeField] private StoreWorkManager warehouseWorkManagerPrefab;
        [SerializeField] private Transform warehouseManagerPosition;

        public StoreWorkManager WorkManager { get; set; }

        private List<WarehouseFarmer> _farmers = new List<WarehouseFarmer>();

        public List<WarehouseFarmer> Farmers => _farmers;

        public float timeRemaining, boostValue;
        public bool boostIsRunning;

        void Start()
        {
            AddHelper();
            CreateManager();
        }

        private void Update()
        {
            if (boostIsRunning)
            {
                if (timeRemaining > 0)
                {
                    timeRemaining -= Time.deltaTime;

                    float minutes = Mathf.FloorToInt(timeRemaining / 60);
                    float seconds = Mathf.FloorToInt(timeRemaining % 60);

                    string minutesTxt = minutes.ToString();
                    string secondsTxt = seconds.ToString();

                    if (minutes < 10)
                        minutesTxt = $"0{minutes}";

                    if (seconds < 10)
                        secondsTxt = $"0{seconds}";

                    WorkManager.CountdownTMP.text = $"{minutesTxt}:{secondsTxt}";
                }
                else
                {
                    timeRemaining = 0;
                    boostIsRunning = false;
                    WorkManager.StopCountDown();

                    foreach (WarehouseFarmer farmer in _farmers)
                    {
                        farmer.speedBoost = false;
                        farmer.perSecondBoost = false;
                    }
                }
            }
        }

        public void AddHelper()
        {
            WarehouseFarmer newFarmer = Instantiate(helperPrefab, warehouseDepositLocation.position, Quaternion.identity);
            newFarmer.FarmHouseDeposit = farmHouseDeposit;
            newFarmer.FarmHouseDepositLocation = new Vector3(farmHouseDepositLocation.position.x, warehouseDepositLocation.position.y);
            newFarmer.WarehouseLocation = new Vector3(warehouseDepositLocation.position.x, warehouseDepositLocation.position.y);

            if (_farmers.Count > 0)
            {
                newFarmer.HarvestCapacity = _farmers[0].HarvestCapacity;
                newFarmer.HarvestPerSecond = _farmers[0].HarvestPerSecond;
                newFarmer.MoveSpeed = _farmers[0].MoveSpeed;

                if (boostIsRunning)
                {
                    if (_farmers[0].perSecondBoost)
                    {
                        newFarmer.perSecondBoost = true;
                    }
                    else
                    {
                        newFarmer.speedBoost = true;
                    }

                    newFarmer.boostValue = boostValue;
                }
            }

            _farmers.Add(newFarmer);
        }

        private void CreateManager()
        {
            WorkManager = Instantiate(warehouseWorkManagerPrefab, warehouseManagerPosition.position, Quaternion.identity);
            WorkManager.transform.SetParent(transform);
            WorkManager.transform.localScale = new Vector3(-1, 1, 1);
            WorkManager.CurrentManagerLocation = this;
            WorkManager.isWareHouse = true;

            if (DataManager.Profile.wareHouse.HasManager)
            {
                WorkManager.ManagerAssigned = WorkManagerController.Instance.GetManager(DataManager.Profile.wareHouse.Manager.Split("-")[0],
                    DataManager.Profile.wareHouse.Manager.Split("-")[1]);
            }
        }

        public void ApplyManagerBoost()
        {
            switch (WorkManager.ManagerAssigned.BoostType)
            {
                case BoostType.Movement:
                    foreach (WarehouseFarmer farmer in _farmers)
                    {
                        WorkManagerController.Instance.RunMovementBoost(farmer, WorkManager.ManagerAssigned.BoostDuration, WorkManager.ManagerAssigned.BoostValue);
                        boostValue = WorkManager.ManagerAssigned.BoostValue;
                        boostIsRunning = true;
                        timeRemaining = WorkManager.ManagerAssigned.BoostDuration * 60;
                    }
                    break;

                case BoostType.Loading:
                    foreach (WarehouseFarmer farmer in _farmers)
                    {
                        WorkManagerController.Instance.RunLoadingBoost(farmer, WorkManager.ManagerAssigned.BoostDuration, WorkManager.ManagerAssigned.BoostValue);
                        boostValue = WorkManager.ManagerAssigned.BoostValue;
                        boostIsRunning = true;
                        timeRemaining = WorkManager.ManagerAssigned.BoostDuration * 60;
                    }
                    break;
            }
        }
    }
}