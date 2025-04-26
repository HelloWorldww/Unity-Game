using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.DamoncStudios.Scripts
{
    public class FarmHouseManager : MonoBehaviour, ManagerLocation
    {
        [SerializeField] private Transform depositLocation;
        [SerializeField] private Deposit farmHouseDeposit;
        [SerializeField] private CartFarmer cart;

        [SerializeField] private FarmHouseWorkManager farmHouseWorkManagerPrefab;
        [SerializeField] private Transform farmHouseManagerPosition;

        public FarmHouseWorkManager WorkManager { get; set; }

        public Deposit FarmHouseDeposit => farmHouseDeposit;
        public Transform DepositLocation => depositLocation;
        public CartFarmer Cart => cart;

        public float timeRemaining, boostValue;
        public bool boostIsRunning;

        private void Start()
        {
            CreateManager();

            farmHouseDeposit.CurrentProducts = DataManager.Profile.farmHouse.DepositCurrentProducts;
            farmHouseDeposit.IsFarmHouse = true;
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
                }
            }
        }

        private void CreateManager()
        {
            WorkManager = Instantiate(farmHouseWorkManagerPrefab, farmHouseManagerPosition.position, Quaternion.identity);
            WorkManager.transform.SetParent(transform);
            WorkManager.CurrentManagerLocation = this;
            WorkManager.isFarmHouse = true;

            if (DataManager.Profile.farmHouse.HasManager)
            {
                WorkManager.ManagerAssigned = WorkManagerController.Instance.GetManager(DataManager.Profile.farmHouse.Manager.Split("-")[0],
                    DataManager.Profile.farmHouse.Manager.Split("-")[1]);
            }
        }

        public void ApplyManagerBoost()
        {
            switch (WorkManager.ManagerAssigned.BoostType)
            {
                case BoostType.Movement:
                    WorkManagerController.Instance.RunMovementBoost(cart, WorkManager.ManagerAssigned.BoostDuration, WorkManager.ManagerAssigned.BoostValue);
                    boostValue = WorkManager.ManagerAssigned.BoostValue;
                    boostIsRunning = true;
                    timeRemaining = WorkManager.ManagerAssigned.BoostDuration * 60;
                    break;

                case BoostType.Loading:
                    WorkManagerController.Instance.RunLoadingBoost(cart, WorkManager.ManagerAssigned.BoostDuration, WorkManager.ManagerAssigned.BoostValue);
                    boostValue = WorkManager.ManagerAssigned.BoostValue;
                    boostIsRunning = true;
                    timeRemaining = WorkManager.ManagerAssigned.BoostDuration * 60;
                    break;
            }
        }
    }
}