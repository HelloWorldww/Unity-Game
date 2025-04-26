using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.DamoncStudios.Scripts
{
    public class WarehouseUI : MonoBehaviour
    {
        public static Action<WarehouseUpgrade> OnUpgradeRequest;

        [SerializeField] private TextMeshProUGUI currentLevelTMP;

        private WarehouseUpgrade _warehouseUpgrade;
        bool upgradeDone = false;

        private void Start()
        {
            _warehouseUpgrade = GetComponent<WarehouseUpgrade>();
        }

        private void Update()
        {
            if (_warehouseUpgrade.isReady && !upgradeDone)
            {
                upgradeDone = true;
                if (DataManager.Profile.wareHouse != null && DataManager.Profile.wareHouse.Level > 0)
                    LoadWareHouseUpgrades(DataManager.Profile.wareHouse.Level - 1);
            }
        }

        private void UpgradeCompleted(BaseUpgrade upgrade)
        {
            if (_warehouseUpgrade == upgrade)
            {
                currentLevelTMP.text = upgrade.CurrentLevel.ToString();
                DataManager.Instance.SaveUserProfile();
            }
        }

        public void OpenWareHouseUpgradePanel()
        {
            OnUpgradeRequest?.Invoke(_warehouseUpgrade);
        }

        private void OnEnable()
        {
            WarehouseUpgrade.OnUpgradeCompleted += UpgradeCompleted;
        }

        private void OnDisable()
        {
            WarehouseUpgrade.OnUpgradeCompleted -= UpgradeCompleted;
        }

        public void LoadWareHouseUpgrades(int level)
        {
            _warehouseUpgrade.LoadUpgrade(level);
        }
    }
}