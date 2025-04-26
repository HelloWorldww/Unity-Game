using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

namespace Assets.DamoncStudios.Scripts
{
    public class ShaftUI : MonoBehaviour
    {
        public static Action<Shaft, ShaftUpgrade> OnUpgradeRequest;

        [SerializeField] private TextMeshProUGUI depositGold;
        [SerializeField] private TextMeshProUGUI shaftID;
        [SerializeField] private TextMeshProUGUI shaftLevel;
        [SerializeField] private TextMeshProUGUI newShaftCost;
        [SerializeField] private GameObject newShaftButton;
        [SerializeField] private List<GameObject> groundSpots;
        [SerializeField] private List<GameObject> treeSpots;
        [SerializeField] private List<GameObject> bushSpots;
        [SerializeField] private List<GameObject> treeProducts;
        [SerializeField] private List<GameObject> bushProducts;
        [SerializeField] private List<GameObject> groundProducts;

        private Shaft _shaft;
        private ShaftUpgrade _shaftUpgrade;
        bool upgradeDone = false;
        int shaftCurrentLevel;
        double shaftUpgradeCost;
        bool isLoadDone = false;

        private void Awake()
        {
            _shaft = GetComponent<Shaft>();
            _shaftUpgrade = GetComponent<ShaftUpgrade>();
            newShaftButton.GetComponent<Button>().onClick.AddListener(AddShaft);
        }

        private void Update()
        {
            try { depositGold.text = $"{_shaft.ShaftDeposit.CurrentProducts.CurrencyText()}"; } catch { }

            if (MoneyManager.Instance.CurrentMoney >= ShaftManager.Instance.ShaftCost)
            {
                newShaftButton.GetComponent<Button>().interactable = true;
            }
            else
            {
                newShaftButton.GetComponent<Button>().interactable = false;
            }

            if (_shaftUpgrade.isReady && !upgradeDone)
            {
                upgradeDone = true;
                if (shaftCurrentLevel > 0)
                    LoadShaftUpgrades(shaftCurrentLevel);
            }
        }

        public void AddShaft()
        {
            MoneyManager.Instance.RemoveMoney(ShaftManager.Instance.ShaftCost);
            newShaftButton.SetActive(false);
            ShaftManager.Instance.AddShaft();
        }

        public void SetShaftUI(int id, double cost, int level, double upgradeCost)
        {
            _shaft.Index = id;
            shaftID.text = (id + 1).ToString();
            newShaftCost.text = cost.CurrencyText();
            shaftCurrentLevel = level;
            shaftUpgradeCost = upgradeCost;

            try
            {
                if (id == DataManager.Profile.shafts.Count - 1)
                {
                    newShaftButton.SetActive(true);
                }
                else
                {
                    newShaftButton.SetActive(false);
                }
            }
            catch (Exception e) { }
        }

        public void SetNewShaftCost(double newCost)
        {
            newShaftCost.text = newCost.ToString();
        }

        public void SetHarvestSpots(ProductType type, Sprite productSprite)
        {
            _shaft.Type = type;
            _shaft.ProductIcon = productSprite;
            List<GameObject> spots = new List<GameObject>();
            List<GameObject> products = new List<GameObject>();

            switch (type)
            {
                case ProductType.tree:
                    spots = treeSpots;
                    products = treeProducts;
                    break;

                case ProductType.bush:
                    spots = bushSpots;
                    products = bushProducts;
                    break;

                case ProductType.ground:
                    spots = groundSpots;
                    products = groundProducts;
                    break;
            }

            foreach (GameObject product in products)
            {
                product.gameObject.SetActive(true);
                product.gameObject.GetComponent<SpriteRenderer>().sprite = productSprite;
            }

            foreach (GameObject spot in spots)
            {
                spot.gameObject.SetActive(true);
            }
        }

        public void OpenUpgradeContainer()
        {
            OnUpgradeRequest?.Invoke(_shaft, _shaftUpgrade);
        }

        private void UpgradeCompleted(BaseUpgrade upgrade)
        {
            if (_shaftUpgrade == upgrade)
            {
                shaftLevel.text = upgrade.CurrentLevel.ToString();
                if (isLoadDone)
                    DataManager.Instance.SaveUserProfile();

                if (shaftCurrentLevel == upgrade.CurrentLevel)
                    isLoadDone = true;
            }
        }

        private void OnEnable()
        {
            ShaftUpgrade.OnUpgradeCompleted += UpgradeCompleted;
        }

        private void OnDisable()
        {
            ShaftUpgrade.OnUpgradeCompleted -= UpgradeCompleted;
        }

        public void LoadShaftUpgrades(int level)
        {
            _shaftUpgrade.LoadUpgrade(level);
        }
    }
}