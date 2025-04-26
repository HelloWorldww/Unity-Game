using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.DamoncStudios.Scripts
{
    public class FarmHouseUI : MonoBehaviour
    {
        public static Action<FarmHouseUpgrade> OnUpgradeRequest;

        [SerializeField] private TextMeshProUGUI depositProductsTMP;
        [SerializeField] private TextMeshProUGUI currentLevelTMP;
        [SerializeField] private GameObject packages;

        private FarmHouseManager _farmHouse;
        private FarmHouseUpgrade _farmHouseUpgrade;
        bool upgradeDone = false;

        void Start()
        {
            _farmHouse = GetComponent<FarmHouseManager>();
            _farmHouseUpgrade = GetComponent<FarmHouseUpgrade>();
        }

        void Update()
        {
            depositProductsTMP.text = _farmHouse.FarmHouseDeposit.CurrentProducts.CurrencyText();

            if (_farmHouse.FarmHouseDeposit.CurrentProducts > 0)
                packages.SetActive(true);
            else
                packages.SetActive(false);

            if (_farmHouseUpgrade.isReady && !upgradeDone)
            {
                upgradeDone = true;
                if (DataManager.Profile.farmHouse != null && DataManager.Profile.farmHouse.Level > 0)
                    LoadFarmHouseUpgrades(DataManager.Profile.farmHouse.Level - 1);
            }
        }

        public void OpenFarmHouseUpgrade()
        {
            OnUpgradeRequest?.Invoke(_farmHouseUpgrade);
        }

        private void UpgradeCompleted(BaseUpgrade upgrade)
        {
            if (_farmHouseUpgrade == upgrade)
            {
                currentLevelTMP.text = upgrade.CurrentLevel.ToString();
                DataManager.Instance.SaveUserProfile();
            }
        }

        private void OnEnable()
        {
            FarmHouseUpgrade.OnUpgradeCompleted += UpgradeCompleted;
        }

        private void OnDisable()
        {
            FarmHouseUpgrade.OnUpgradeCompleted -= UpgradeCompleted;
        }

        public void LoadFarmHouseUpgrades(int level)
        {
            _farmHouseUpgrade.Upgrade(level);
        }
    }
}