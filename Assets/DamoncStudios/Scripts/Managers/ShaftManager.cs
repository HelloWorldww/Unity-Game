using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.DamoncStudios.Scripts
{
    public class ShaftManager : Singleton<ShaftManager>
    {
        [SerializeField] private Camera cam;

        [Header("Shaft Config")]
        [SerializeField] private Shaft shaftPrefab;
        [SerializeField] private float newShaftYPosition;
        [SerializeField] private double newShaftCost = 5000;
        [SerializeField] private float newShaftCostMultiplier = 10;
        [SerializeField] private float shaftUpgradeMultiplier = 85.62f;
        [SerializeField] private List<Shaft> shafts;
        [SerializeField] private List<ShaftData> shaftsData;

        int _currentShaftIndex;

        public List<Shaft> Shafts => shafts;

        public double ShaftCost { get; set; }

        public float NewShaftCostMultiplier => newShaftCostMultiplier;
        public float ShaftUpgradeMultiplier => shaftUpgradeMultiplier;

        private void Start()
        {
            ShaftCost = newShaftCost;
            _currentShaftIndex = 0;

            if (DataManager.Profile.shafts == null || DataManager.Profile.shafts.Count == 0)
            {
                DataManager.Profile.shafts = new List<UsersShaft>();

                UsersShaft newShaft = GenerateNewShaft(shaftsData[0]);
                DataManager.Profile.shafts.Add(newShaft);
            }

            shafts[_currentShaftIndex].GetComponent<ShaftUI>().SetShaftUI(0, ShaftCost, DataManager.Profile.shafts[0].Level - 1, DataManager.Profile.shafts[0].LevelUpgradeCost);
            shafts[_currentShaftIndex].CreateDeposit(_currentShaftIndex, true, DataManager.Profile.shafts[0].DepositCurrentProducts, shaftsData[0].ProductIcon);
            shafts[_currentShaftIndex].GetComponent<ShaftUI>().SetHarvestSpots(GetProductType(shaftsData[0].ProductType), shaftsData[0].ProductIcon);

            LoadShafts();
        }

        public void AddShaft()
        {
            Transform lastShaft = shafts[_currentShaftIndex].transform;
            Vector3 newShaftPos = new Vector3(lastShaft.position.x, lastShaft.position.y - newShaftYPosition);
            Shaft newShaft = Instantiate(shaftPrefab, newShaftPos, Quaternion.identity);

            _currentShaftIndex++;
            ShaftCost *= newShaftCostMultiplier;

            System.Random rnd = new System.Random();
            int r = rnd.Next(0, shaftsData.Count);

            ShaftData shaftData = shaftsData[r];
            UsersShaft newUsersShaft = GenerateNewShaft(shaftData);
            newUsersShaft.Cost = ShaftCost;
            DataManager.Profile.shafts.Add(newUsersShaft);

            newShaft.GetComponent<ShaftUI>().SetShaftUI(_currentShaftIndex, ShaftCost, newUsersShaft.Level, newUsersShaft.UpgradeCost);
            newShaft.CreateDeposit(_currentShaftIndex, true, newUsersShaft.DepositCurrentProducts, shaftData.ProductIcon);
            newShaft.GetComponent<ShaftUI>().SetHarvestSpots(GetProductType(shaftData.ProductType), shaftData.ProductIcon);

            shafts.Add(newShaft);

            if (_currentShaftIndex > 0)
            {
                CameraScroll.Instance.ExpandMapRenderer();
            }

            DataManager.Instance.SaveUserProfile();
        }

        public void LoadShafts()
        {

            for (int i = 0; i < DataManager.Profile.shafts.Count; i++)
            {
                if (i > 0)
                {
                    Transform lastShaft = shafts[_currentShaftIndex].transform;
                    Vector3 newShaftPos = new Vector3(lastShaft.position.x, lastShaft.position.y - newShaftYPosition);
                    Shaft newShaft = Instantiate(shaftPrefab, newShaftPos, Quaternion.identity);

                    _currentShaftIndex++;
                    ShaftCost *= newShaftCostMultiplier;
                    DataManager.Profile.shafts[i].Cost = ShaftCost;

                    newShaft.GetComponent<ShaftUI>().SetShaftUI(i, ShaftCost, DataManager.Profile.shafts[i].Level - 1, DataManager.Profile.shafts[i].LevelUpgradeCost);
                    newShaft.CreateDeposit(i, true, DataManager.Profile.shafts[i].DepositCurrentProducts, shaftsData[DataManager.Profile.shafts[i].ShaftIndex].ProductIcon);
                    newShaft.GetComponent<ShaftUI>().SetHarvestSpots(GetProductType(shaftsData[DataManager.Profile.shafts[i].ShaftIndex].ProductType),
                        shaftsData[DataManager.Profile.shafts[i].ShaftIndex].ProductIcon);
                    shafts.Add(newShaft);

                    /*if (i > 0)
                    {
                        CameraScroll.Instance.ExpandMapRenderer();
                    }*/
                    CameraScroll.Instance.ExpandMapRenderer();
                }

            }

            cam.transform.localPosition = CameraScroll.cameraPos;
        }

        private ProductType GetProductType(string type)
        {
            switch (type)
            {
                case "tree":
                    return ProductType.tree;

                case "bush":
                    return ProductType.bush;

                case "ground":
                    return ProductType.ground;
            }

            return ProductType.tree;
        }

        private UsersShaft GenerateNewShaft(ShaftData shaftData)
        {
            UsersShaft newShaft = new UsersShaft();
            newShaft.Index = DataManager.Profile.shafts.Count;
            newShaft.ShaftIndex = shaftData.Index;
            newShaft.Level = 0;
            newShaft.LevelUpgradeCost = 600;
            newShaft.Cost = ShaftCost;
            newShaft.UpgradeCost = 600;
            newShaft.DepositCurrentProducts = 0;
            newShaft.Manager = "";
            newShaft.HasManager = false;

            return newShaft;
        }
    }
}