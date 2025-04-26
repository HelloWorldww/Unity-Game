using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.DamoncStudios.Scripts
{
    public class BaseUpgrade : MonoBehaviour
    {
        public static Action<BaseUpgrade> OnUpgradeCompleted;

        [Header("Upgrade")]
        [SerializeField] private float collectCapacityMultiplier = 2;
        [SerializeField] private float collectPerSecondMultiplier = 2;
        [SerializeField] private float moveSpeedMultiplier = 2;

        [Header("Cost")]
        [SerializeField] private double initialUpgradeCost = 600;
        [SerializeField] private float upgradeCostMultiplier = 2;

        public float CollectCapacityMultiplier => collectCapacityMultiplier;
        public float CollectPerSecondMultiplier => collectPerSecondMultiplier;
        public float MoveSpeedMultiplier => moveSpeedMultiplier;
        public float UpgradeCostMultiplier => upgradeCostMultiplier;

        public int CurrentLevel { get; set; }
        public double UpgradeCost { get; set; }
        public double BoostLevel { get; set; }

        public Warehouse _warehouse;
        public FarmHouseManager _farmHouse;
        public Shaft _shaft;
        private int _currentNextBoostLevel = 1;
        private int _nextBoostResetValue = 1;

        public bool isReady = false;
        public bool loading = true;

        private void Start()
        {
            _shaft = GetComponent<Shaft>();
            _farmHouse = GetComponent<FarmHouseManager>();
            _warehouse = GetComponent<Warehouse>();

            CurrentLevel = 1;
            UpgradeCost = initialUpgradeCost;
            BoostLevel = 10;
        }

        private void Update()
        {
            if ((_shaft != null || _farmHouse != null || _warehouse != null) && loading)
            {
                isReady = true;
                loading = false;

                if (_shaft != null)
                {
                    if (_shaft.Index > 2)
                    {
                        UpgradeCost = ((initialUpgradeCost * (_shaft.Index + 1) * ShaftManager.Instance.ShaftUpgradeMultiplier) * (ShaftManager.Instance.NewShaftCostMultiplier * 0.2f)) * (_shaft.Index + 1);
                    }
                    else if (_shaft.Index > 1)
                    {
                        UpgradeCost = ((initialUpgradeCost * (_shaft.Index + 1) * ShaftManager.Instance.ShaftUpgradeMultiplier) * (ShaftManager.Instance.NewShaftCostMultiplier * 0.2f)) * (_shaft.Index - 1);
                    }
                    else if (_shaft.Index > 0)
                        UpgradeCost = initialUpgradeCost * (_shaft.Index + 1) * ShaftManager.Instance.ShaftUpgradeMultiplier;
                    else
                        UpgradeCost = initialUpgradeCost;
                }
            }
        }

        public void Upgrade(int amount)
        {
            if (amount > 0)
            {
                for (int i = 0; i < amount; i++)
                {
                    UpgradeCompleted();
                    ExecuteUpgrade();
                }
            }
        }

        public void LoadUpgrade(int amount)
        {
            if (amount > 0)
            {
                for (int i = 0; i < amount; i++)
                {
                    UpgradeCompleted();
                    ExecuteLoadUpgrade();
                }
                StartCoroutine(ExecuteCreate());
            }
        }

        private void UpgradeCompleted()
        {
            MoneyManager.Instance.RemoveMoney(UpgradeCost);
            UpgradeCost *= upgradeCostMultiplier;
            CurrentLevel++;

            if (_shaft != null)
            {
                DataManager.Profile.shafts[_shaft.Index].UpgradeCost = UpgradeCost;
                DataManager.Profile.shafts[_shaft.Index].Level = CurrentLevel;
            }
            else if (_farmHouse != null)
            {
                DataManager.Profile.farmHouse.UpgradeCost = UpgradeCost;
                DataManager.Profile.farmHouse.Level = CurrentLevel;
            }
            else if (_warehouse != null)
            {
                if (DataManager.Profile.wareHouse == null)
                    DataManager.Profile.wareHouse = new WarehouseData();
                DataManager.Profile.wareHouse.UpgradeCost = UpgradeCost;
                DataManager.Profile.wareHouse.Level = CurrentLevel;
            }

            UpdateNextBoostLevel();
            OnUpgradeCompleted?.Invoke(this);
        }

        protected virtual void ExecuteUpgrade()
        {

        }

        protected virtual void ExecuteLoadUpgrade()
        {

        }

        protected virtual IEnumerator ExecuteCreate()
        {
            yield return null;
        }

        protected virtual IEnumerator Creating()
        {
            yield return null;
        }

        protected void UpdateNextBoostLevel()
        {
            _currentNextBoostLevel++;
            _nextBoostResetValue++;

            if (_currentNextBoostLevel == BoostLevel)
            {
                _nextBoostResetValue = 1;
                BoostLevel += 10;
            }
        }

        public float GetNextBoostProgress()
        {
            return (float)_nextBoostResetValue / 10;
        }

        public void SetUpgradeData(int level, double upgradeCost)
        {
            CurrentLevel = level;
            UpgradeCost = upgradeCost;

            if (_shaft != null)
                StartCoroutine(SetShaftUpgradeData(level));
            else if (_farmHouse != null)
                SetFarmHouseUpgradeData();
            else if (_warehouse != null)
                SetWareHouseUpgradeData();
        }

        public IEnumerator SetShaftUpgradeData(int level)
        {
            int counter = 0;
            for (int i = 1; i <= level; i++)
            {
                _shaft.Farmers[0].HarvestCapacity *= CollectCapacityMultiplier;
                _shaft.Farmers[0].HarvestPerSecond *= CollectPerSecondMultiplier;

                if (i % 10 == 0)
                {
                    _shaft.Farmers[0].MoveSpeed *= MoveSpeedMultiplier;
                    counter++;
                }
            }

            yield return StartCoroutine(ExecuteShaftCreate(counter));

            for (int i = 1; i < _shaft.Farmers.Count; i++)
            {
                _shaft.Farmers[i].HarvestCapacity = _shaft.Farmers[0].HarvestCapacity;
                _shaft.Farmers[i].HarvestPerSecond = _shaft.Farmers[0].HarvestPerSecond;
                _shaft.Farmers[i].MoveSpeed = _shaft.Farmers[0].MoveSpeed;
            }

            UpgradeCompleted();
        }

        private IEnumerator ExecuteShaftCreate(int counter)
        {
            for (int i = 0; i < counter; i++)
            {
                yield return StartCoroutine(Creating());
            }
        }

        public void SetFarmHouseUpgradeData()
        {

        }

        public void SetWareHouseUpgradeData()
        {

        }
    }
}