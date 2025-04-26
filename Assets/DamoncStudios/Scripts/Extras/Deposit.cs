using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.DamoncStudios.Scripts
{
    public class Deposit : MonoBehaviour
    {
        [SerializeField] private List<GameObject> products;

        public double CurrentProducts { get; set; }
        public int ShaftIndex { get; set; }
        public bool IsShaft { get; set; }
        public bool IsFarmHouse { get; set; }

        public bool CanCollectProducts => CurrentProducts > 0;

        public Sprite productIcon;

        public void DepositProducts(double amount)
        {
            CurrentProducts += amount;

            if (IsShaft)
                DataManager.Profile.shafts[ShaftIndex].DepositCurrentProducts = CurrentProducts;
            else if (IsFarmHouse)
                DataManager.Profile.farmHouse.DepositCurrentProducts = CurrentProducts;

            foreach (GameObject product in products)
            {
                product.GetComponent<SpriteRenderer>().sprite = productIcon;
                product.SetActive(true);
            }

            //DataManager.Instance.SaveUserProfile(false);
        }

        public void RemoveProducts(double amount)
        {
            if (amount <= CurrentProducts)
            {
                if (IsShaft)
                    DataManager.Profile.shafts[ShaftIndex].DepositCurrentProducts = CurrentProducts;
                else if (IsFarmHouse)
                    DataManager.Profile.farmHouse.DepositCurrentProducts = CurrentProducts;

                CurrentProducts -= amount;
            }

            if (CurrentProducts < 1)
            {
                foreach (GameObject product in products)
                {
                    product.SetActive(false);
                }
            }

            DataManager.Instance.SaveUserProfile();
        }

        public double CollectProducts(BaseFarmer farmer)
        {
            double cartCapacity = farmer.HarvestCapacity - farmer.CurrentProducts;
            return EvaluateAmountToCollect(cartCapacity);
        }

        public double EvaluateAmountToCollect(double cartCollectCapacity)
        {
            if (cartCollectCapacity <= CurrentProducts)
            {
                return cartCollectCapacity;
            }

            return CurrentProducts;
        }
    }
}