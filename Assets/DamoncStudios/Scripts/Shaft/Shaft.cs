using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.DamoncStudios.Scripts
{
    public class Shaft : MonoBehaviour, ManagerLocation
    {
        [Header("Prefab")]
        [SerializeField] private ShaftFarmer farmerPrefab;
        [SerializeField] private Deposit depositPrefab;

        [Header("Manager")]
        [SerializeField] private ShaftWorkManager shaftManagerPrefab;
        [SerializeField] private Transform shaftManagerPosition;

        [Header("Locations")]
        [SerializeField] private Transform harvestingLocation;
        [SerializeField] private Transform depositLocation;
        [SerializeField] private Transform depositCreationLocation;

        public int Index { get; set; }
        public int ShaftID { get; set; }
        public ProductType Type { get; set; }
        public Sprite ProductIcon { get; set; }
        public Transform HarvestingLocation => harvestingLocation;
        public Transform DepositLocation => depositLocation;
        public Deposit ShaftDeposit { get; set; }
        public ShaftUI ShaftUI { get; set; }

        private List<ShaftFarmer> _farmers = new List<ShaftFarmer>();

        public List<ShaftFarmer> Farmers => _farmers;

        public ShaftWorkManager WorkManager { get; set; }

        public float timeRemaining, boostValue;
        public bool boostIsRunning;
        public bool done;

        private void Awake()
        {
            ShaftUI = GetComponent<ShaftUI>();
        }

        void Start()
        {
            CreateFarmer();
            //CreateDeposit();
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

                    foreach (ShaftFarmer farmer in _farmers)
                    {
                        farmer.speedBoost = false;
                        farmer.perSecondBoost = false;
                    }
                }
            }
        }

        public void CreateFarmer()
        {
            ShaftFarmer newFarmer = Instantiate(farmerPrefab, depositLocation.position, Quaternion.identity);
            newFarmer.CurrentShaft = this;
            newFarmer.transform.SetParent(transform);

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
            else
            {
                if (Index > 2)
                {
                    newFarmer.HarvestCapacity *= (((Index + 1) * (ShaftManager.Instance.ShaftUpgradeMultiplier / 2)) * (ShaftManager.Instance.NewShaftCostMultiplier * 0.2f)) * (Index + 1);
                    newFarmer.HarvestPerSecond *= (((Index + 1) * (ShaftManager.Instance.ShaftUpgradeMultiplier / 2)) * (ShaftManager.Instance.NewShaftCostMultiplier * 0.2f)) * (Index + 1);
                }
                else if (Index > 1)
                {
                    newFarmer.HarvestCapacity *= (((Index + 1) * (ShaftManager.Instance.ShaftUpgradeMultiplier / 2)) * (ShaftManager.Instance.NewShaftCostMultiplier * 0.2f)) * (Index - 1);
                    newFarmer.HarvestPerSecond *= (((Index + 1) * (ShaftManager.Instance.ShaftUpgradeMultiplier / 2)) * (ShaftManager.Instance.NewShaftCostMultiplier * 0.2f)) * (Index - 1);
                }
                else if (Index > 0)
                {
                    newFarmer.HarvestCapacity *= (Index + 1) * (ShaftManager.Instance.ShaftUpgradeMultiplier / 2);
                    newFarmer.HarvestPerSecond *= (Index + 1) * (ShaftManager.Instance.ShaftUpgradeMultiplier / 2);
                }
            }

            _farmers.Add(newFarmer);
        }

        public void CreateDeposit(int index, bool isShaft, double depositAmount, Sprite productIcon)
        {
            ShaftDeposit = Instantiate(depositPrefab, depositCreationLocation.position, Quaternion.identity);
            ShaftDeposit.productIcon = productIcon;
            ShaftDeposit.CurrentProducts = depositAmount;
            ShaftDeposit.ShaftIndex = index;
            ShaftDeposit.IsShaft = isShaft;
            ShaftDeposit.transform.SetParent(transform);
        }

        private void CreateManager()
        {
            WorkManager = Instantiate(shaftManagerPrefab, shaftManagerPosition.position, Quaternion.identity);
            WorkManager.transform.SetParent(transform);
            WorkManager.CurrentManagerLocation = this;
            WorkManager.isShaft = true;
            WorkManager.shaftId = Index;

            if (DataManager.Profile.shafts[Index].HasManager)
            {
                WorkManager.ManagerAssigned = WorkManagerController.Instance.GetManager(DataManager.Profile.shafts[Index].Manager.Split("-")[0],
                    DataManager.Profile.shafts[Index].Manager.Split("-")[1]);
            }
        }

        public void ApplyManagerBoost()
        {
            switch (WorkManager.ManagerAssigned.BoostType)
            {
                case BoostType.Movement:
                    foreach (ShaftFarmer farmer in _farmers)
                    {
                        WorkManagerController.Instance.RunMovementBoost(farmer, WorkManager.ManagerAssigned.BoostDuration, WorkManager.ManagerAssigned.BoostValue);
                        boostValue = WorkManager.ManagerAssigned.BoostValue;
                        boostIsRunning = true;
                        timeRemaining = WorkManager.ManagerAssigned.BoostDuration * 60;
                    }
                    break;

                case BoostType.Loading:
                    foreach (ShaftFarmer farmer in _farmers)
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