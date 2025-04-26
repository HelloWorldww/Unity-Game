using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Assets.DamoncStudios.Scripts
{
    public class UpgradeManager : MonoBehaviour
    {
        [SerializeField] private GameObject upgradeContainer;
        [SerializeField] private Image panelFarmerImage;
        [SerializeField] private TextMeshProUGUI panelTitle;
        [SerializeField] private TextMeshProUGUI level;
        [SerializeField] private TextMeshProUGUI nextBoost;
        [SerializeField] private TextMeshProUGUI upgradeCost;
        [SerializeField] private Image progressBar;

        [Header("Upgrade Buttons")]
        [SerializeField] private Button upgradeButton;
        [SerializeField] private GameObject[] upgradeButtons;
        [SerializeField] private Color disableColorBtn;
        [SerializeField] private Color enableColorBtn;

        [Header("Stats")]
        [SerializeField] private GameObject[] stats;

        [Header("Stat Title")]
        [SerializeField] public TextMeshProUGUI Stat1Title;
        [SerializeField] public TextMeshProUGUI Stat2Title;
        [SerializeField] public TextMeshProUGUI Stat3Title;
        [SerializeField] public TextMeshProUGUI Stat4Title;

        [Header("Stat Values")]
        [SerializeField] public TextMeshProUGUI Stat1CurrentValue;
        [SerializeField] public TextMeshProUGUI Stat2CurrentValue;
        [SerializeField] public TextMeshProUGUI Stat3CurrentValue;
        [SerializeField] public TextMeshProUGUI Stat4CurrentValue;

        [Header("Stat New Values")]
        [SerializeField] public TextMeshProUGUI Stat1NewValue;
        [SerializeField] public TextMeshProUGUI Stat2NewValue;
        [SerializeField] public TextMeshProUGUI Stat3NewValue;
        [SerializeField] public TextMeshProUGUI Stat4NewValue;

        [Header("Stat Icon")]
        [SerializeField] public Image Stat1Icon;
        [SerializeField] public Image Stat2Icon;
        [SerializeField] public Image Stat3Icon;
        [SerializeField] public Image Stat4Icon;

        [Header("Panel Info")]
        [SerializeField] private UpgradePanelInfo shaftFarmerInfo;
        [SerializeField] private UpgradePanelInfo farmHouseInfo;
        [SerializeField] private UpgradePanelInfo wareHouseInfo;

        private Shaft _currentShaft;
        private UpgradePanelInfo _currentPanelInfo;
        private BaseUpgrade _currentUpgrade;
        private BaseFarmer _currentFarmer;
        private int _currentActiveButton;
        private int _farmerCount;

        private double updatedUpgradeCost;

        public int UpgradeAmount { get; set; }

        private void Update()
        {
            if (_currentUpgrade != null)
            {
                Stat1CurrentValue.text = $"{((_currentUpgrade.CurrentLevel) / 10) + 1}";

                if (MoneyManager.Instance.CurrentMoney >= updatedUpgradeCost)
                {
                    upgradeButton.interactable = true;
                }
                else
                {
                    upgradeButton.interactable = false;
                }
            }
        }

        public void OpenCloseUpgradeContainer(bool status)
        {
            UpgradeX1(false);
            upgradeContainer.SetActive(status);
        }

        public void Upgrade()
        {
            if (MoneyManager.Instance.CurrentMoney >= _currentUpgrade.UpgradeCost)
            {
                _currentUpgrade.Upgrade(UpgradeAmount);
                UpdatePanelValues();
                RefreshUpgradeAmount();
            }
        }

        #region Upgrade Buttons
        public void UpgradeX1(bool animate)
        {
            ActivateButton(0, animate);
            upgradeButtons[0].GetComponent<Button>().interactable = false;
            upgradeButtons[1].GetComponent<Button>().interactable = true;
            upgradeButtons[2].GetComponent<Button>().interactable = true;
            upgradeButtons[3].GetComponent<Button>().interactable = true;
            UpgradeAmount = CanUpgradeManyTimes(1, _currentUpgrade) ? 1 : 0;
            upgradeCost.text = GetUpgradeCost(1, _currentUpgrade).CurrencyText();
            UpdatePanelValues(1, _currentUpgrade);
        }

        public void UpgradeX10(bool animate)
        {
            ActivateButton(1, animate);
            upgradeButtons[0].GetComponent<Button>().interactable = true;
            upgradeButtons[1].GetComponent<Button>().interactable = false;
            upgradeButtons[2].GetComponent<Button>().interactable = true;
            upgradeButtons[3].GetComponent<Button>().interactable = true;
            UpgradeAmount = CanUpgradeManyTimes(10, _currentUpgrade) ? 10 : 0;
            upgradeCost.text = GetUpgradeCost(10, _currentUpgrade).CurrencyText();
            UpdatePanelValues(10, _currentUpgrade);
        }

        public void UpgradeX50(bool animate)
        {
            ActivateButton(2, animate);

            upgradeButtons[0].GetComponent<Button>().interactable = true;
            upgradeButtons[1].GetComponent<Button>().interactable = true;
            upgradeButtons[2].GetComponent<Button>().interactable = false;
            upgradeButtons[3].GetComponent<Button>().interactable = true;
            UpgradeAmount = CanUpgradeManyTimes(50, _currentUpgrade) ? 50 : 0;
            upgradeCost.text = GetUpgradeCost(50, _currentUpgrade).CurrencyText();
            UpdatePanelValues(50, _currentUpgrade);
        }

        public void UpgradeMax(bool animate)
        {
            ActivateButton(3, animate);
            upgradeButtons[0].GetComponent<Button>().interactable = true;
            upgradeButtons[1].GetComponent<Button>().interactable = true;
            upgradeButtons[2].GetComponent<Button>().interactable = true;
            upgradeButtons[3].GetComponent<Button>().interactable = false;
            UpgradeAmount = CalculateUpgradeCount(_currentUpgrade);
            upgradeCost.text = GetUpgradeCost(UpgradeAmount, _currentUpgrade).CurrencyText();
            UpdatePanelValues(UpgradeAmount, _currentUpgrade);
        }

        private int CalculateUpgradeCount(BaseUpgrade upgrade)
        {
            int count = 0;
            double currentGold = MoneyManager.Instance.CurrentMoney;
            double currentUpgradeCost = upgrade.UpgradeCost;

            if (MoneyManager.Instance.CurrentMoney >= currentUpgradeCost)
            {
                for (double i = currentGold; i >= 0; i -= currentUpgradeCost)
                {
                    count++;
                    currentUpgradeCost *= upgrade.UpgradeCostMultiplier;
                }
            }

            return count;
        }

        private bool CanUpgradeManyTimes(int upgradeAmount, BaseUpgrade upgrade)
        {
            int count = CalculateUpgradeCount(upgrade);
            if (count >= upgradeAmount)
            {
                return true;
            }

            return false;
        }

        private double GetUpgradeCost(int amount, BaseUpgrade upgrade)
        {
            double cost = 0d;
            double currentUpgradeCost = upgrade.UpgradeCost;

            for (int i = 0; i < amount; i++)
            {
                cost += currentUpgradeCost;
                currentUpgradeCost *= upgrade.UpgradeCostMultiplier;
            }

            updatedUpgradeCost = cost;

            return cost;
        }

        private void ActivateButton(int index, bool animateButton)
        {
            for (int i = 0; i < upgradeButtons.Length; i++)
            {
                upgradeButtons[i].GetComponent<Image>().color = disableColorBtn;
            }
            _currentActiveButton = index;

            upgradeButtons[index].GetComponent<Image>().color = enableColorBtn;

            if (animateButton)
                upgradeButtons[index].transform.DOPunchPosition(transform.localPosition + new Vector3(0f, -5f, 0f), 0.5f).Play();
        }

        private void RefreshUpgradeAmount()
        {
            switch (_currentActiveButton)
            {
                case 0:
                    UpgradeX1(false);
                    break;
                case 1:
                    UpgradeX10(false);
                    break;
                case 2:
                    UpgradeX50(false);
                    break;
                case 3:
                    UpgradeMax(false);
                    break;
            }
        }
        #endregion

        private void UpdatePanelValues()
        {
            upgradeCost.text = _currentUpgrade.UpgradeCost.CurrencyText();
            level.text = $"Level {_currentUpgrade.CurrentLevel}";
            progressBar.DOFillAmount(_currentUpgrade.GetNextBoostProgress(), 0.5f).Play();
            nextBoost.text = $"Next Boost at Level {_currentUpgrade.BoostLevel}";

            // Move Speed
            float farmerMoveSpeed = _currentFarmer.MoveSpeed;
            float walkSpeedUpgraded = Mathf.Abs((float)(farmerMoveSpeed - (farmerMoveSpeed * _currentUpgrade.MoveSpeedMultiplier)));

            // Collect Per Second
            double farmerCollectPerSecond = _currentFarmer.HarvestPerSecond;
            double collectPerSecondUpgraded = Mathf.Abs((float)(farmerCollectPerSecond - (farmerCollectPerSecond * _currentUpgrade.CollectPerSecondMultiplier)));

            // Collect Capacity
            double farmerCollectCapacity = _currentFarmer.HarvestCapacity; // 200 - (200 * 2)
            double collectCapacityUpgraded = Mathf.Abs((float)(farmerCollectCapacity - (farmerCollectCapacity * _currentUpgrade.CollectCapacityMultiplier)));

            /*if (_currentPanelInfo.Location == Locations.FarmHouse)
            {
                //Current Values
                Stat1CurrentValue.text = $"{_currentFarmer.HarvestCapacity.CurrencyText()}";
                Stat2CurrentValue.text = $"{Mathf.Round(_currentFarmer.MoveSpeed * 100.0f) * 0.01f}";
                Stat3CurrentValue.text = $"{Convert.ToDouble(_currentFarmer.HarvestPerSecond).CurrencyText()}";
                Stat4CurrentValue.text = $"{_currentFarmer.HarvestCapacity.CurrencyText()}";

                // Farmer stat
                Stat1NewValue.text = $"+{collectCapacityUpgraded.CurrencyText()}";
                Stat2NewValue.text = (_currentUpgrade.CurrentLevel + 1) % 10 == 0 ? $"-{Mathf.Round(walkSpeedUpgraded * 100.0f) * 0.01f} /s" : "0";
                Stat3NewValue.text = $"+{collectPerSecondUpgraded.CurrencyText()}";
            }
            else
            {*/
            //Current Values
            Stat1CurrentValue.text = $"{_farmerCount}";
            Stat2CurrentValue.text = $"{Mathf.Round(_currentFarmer.MoveSpeed * 100.0f) * 0.01f}";
            Stat3CurrentValue.text = $"{Convert.ToDouble(_currentFarmer.HarvestPerSecond).CurrencyText()}";
            Stat4CurrentValue.text = $"{_currentFarmer.HarvestCapacity.CurrencyText()}";

            // Farmer stat
            Stat1NewValue.text = (_currentUpgrade.CurrentLevel + 1) % 10 == 0 ? "+1" : "+0";
            Stat2NewValue.text = (_currentUpgrade.CurrentLevel + 1) % 10 == 0 ? $"-{Mathf.Round(walkSpeedUpgraded * 100.0f) * 0.01f} /s" : "0";
            Stat3NewValue.text = $"+{collectPerSecondUpgraded.CurrencyText()}";
            Stat4NewValue.text = $"+{collectCapacityUpgraded.CurrencyText()}";
            //}
        }

        private void UpdatePanelValues(int amount, BaseUpgrade upgrade)
        {
            // Move Speed
            float farmerMoveSpeed = _currentFarmer.MoveSpeed;

            // Collect Per Second
            double farmerCollectPerSecond = _currentFarmer.HarvestPerSecond;

            // Collect Capacity
            double farmerCollectCapacity = _currentFarmer.HarvestCapacity; // 200 - (200 * 2)

            /*if (_currentPanelInfo.Location == Locations.FarmHouse)
            {
                float moveSpeed = 0f;

                for (int i = 0; i < amount; i++)
                {
                    farmerCollectCapacity *= upgrade.CollectCapacityMultiplier;
                    farmerCollectPerSecond *= upgrade.CollectPerSecondMultiplier;
                }

                for (int i = 0; i < Mathf.Abs(amount / 10); i++)
                {
                    moveSpeed += farmerMoveSpeed - (farmerMoveSpeed * upgrade.MoveSpeedMultiplier);
                    farmerMoveSpeed *= upgrade.MoveSpeedMultiplier;
                }

                // Farmer stat
                Stat1NewValue.text = $"+{(farmerCollectCapacity - _currentFarmer.HarvestCapacity).CurrencyText()}";
                Stat2NewValue.text = (amount > 1) ? $"-{Mathf.Round(moveSpeed * 100.0f) * 0.01f} /s" : "0";
                Stat3NewValue.text = $"+{(farmerCollectPerSecond - _currentFarmer.HarvestPerSecond).CurrencyText()}";
            }
            else
            {*/
            float moveSpeed = 0f;

            for (int i = 0; i < amount; i++)
            {
                if (i % 10 != 0)
                {
                    farmerCollectCapacity *= upgrade.CollectCapacityMultiplier;
                    farmerCollectPerSecond *= upgrade.CollectPerSecondMultiplier;
                }
            }

            for (int i = 0; i < Mathf.Abs(amount / 10); i++)
            {
                farmerCollectCapacity *= 2;
                farmerCollectPerSecond *= 2;
                moveSpeed += farmerMoveSpeed - (farmerMoveSpeed * upgrade.MoveSpeedMultiplier);
                farmerMoveSpeed *= upgrade.MoveSpeedMultiplier;
            }

            // Farmer stat
            Stat1NewValue.text = amount > 1 ? $"+{amount / 10}" : "+0";
            Stat2NewValue.text = amount > 1 ? $"-{Mathf.Round(moveSpeed * 100.0f) * 0.01f} /s" : "0";
            Stat3NewValue.text = $"+{(farmerCollectPerSecond - _currentFarmer.HarvestPerSecond).CurrencyText()}";
            Stat4NewValue.text = $"+{(farmerCollectCapacity - _currentFarmer.HarvestCapacity).CurrencyText()}";
            //}
        }

        private void UpdateUpgradeInfo()
        {
            /*if (_currentPanelInfo.Location == Locations.FarmHouse)
                stats[3].SetActive(false);
            else*/
            stats[3].SetActive(true);

            panelTitle.text = _currentPanelInfo.PanelTitle;
            panelFarmerImage.sprite = _currentPanelInfo.PanelMinerIcon;

            Stat1Title.text = _currentPanelInfo.Stat1Title;
            Stat2Title.text = _currentPanelInfo.Stat2Title;
            Stat3Title.text = _currentPanelInfo.Stat3Title;
            Stat4Title.text = _currentPanelInfo.Stat4Title;

            Stat1Icon.sprite = _currentPanelInfo.Stat1Icon;
            Stat2Icon.sprite = _currentPanelInfo.Stat2Icon;
            Stat3Icon.sprite = _currentPanelInfo.Stat3Icon;
            Stat4Icon.sprite = _currentPanelInfo.Stat4Icon;
        }

        private void ShaftUpgradeRequest(Shaft selectedShaft, ShaftUpgrade selectedUpgrade)
        {
            _farmerCount = selectedShaft.Farmers.Count;
            _currentFarmer = selectedShaft.Farmers[0];
            _currentUpgrade = selectedUpgrade;
            _currentPanelInfo = shaftFarmerInfo;

            UpdateUpgradeInfo();
            UpdatePanelValues();
            OpenCloseUpgradeContainer(true);
        }

        private void FarmHouseUpgradeRequest(FarmHouseUpgrade selectedUpgrade)
        {
            _currentFarmer = selectedUpgrade.GetComponent<FarmHouseManager>().Cart;
            _currentPanelInfo = farmHouseInfo;
            _currentUpgrade = selectedUpgrade;

            UpdateUpgradeInfo();
            UpdatePanelValues();
            OpenCloseUpgradeContainer(true);
        }

        private void WareHouseUpgradeRequest(WarehouseUpgrade selectedUpgrade)
        {
            _farmerCount = selectedUpgrade.GetComponent<Warehouse>().Farmers.Count;
            _currentFarmer = selectedUpgrade.GetComponent<Warehouse>().Farmers[0];
            _currentPanelInfo = wareHouseInfo;
            _currentUpgrade = selectedUpgrade;

            UpdateUpgradeInfo();
            UpdatePanelValues();
            OpenCloseUpgradeContainer(true);
        }

        private void OnEnable()
        {
            ShaftUI.OnUpgradeRequest += ShaftUpgradeRequest;
            FarmHouseUI.OnUpgradeRequest += FarmHouseUpgradeRequest;
            WarehouseUI.OnUpgradeRequest += WareHouseUpgradeRequest;
        }

        private void OnDisable()
        {
            ShaftUI.OnUpgradeRequest -= ShaftUpgradeRequest;
            FarmHouseUI.OnUpgradeRequest -= FarmHouseUpgradeRequest;
            WarehouseUI.OnUpgradeRequest -= WareHouseUpgradeRequest;
        }
    }
}