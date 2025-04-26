using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.DamoncStudios.Scripts
{
    public class MoneyManager : Singleton<MoneyManager>
    {
        [SerializeField] private double testMoney = 0;

        public double CurrentMoney { get; set; }

        private void Start()
        {
            DataManager.Profile.sessionMaxEarningsReached = DataManager.Profile.totalEarnings;

            if (testMoney != 0)
                CurrentMoney = testMoney;
            else
                CurrentMoney = DataManager.Profile.totalEarnings;
        }

        public void AddMoney(double amount)
        {
            CurrentMoney += amount;
            DataManager.Profile.totalEarnings = CurrentMoney;

            if (DataManager.Profile.sessionMaxEarningsReached <= CurrentMoney || DataManager.Profile.sessionMaxEarningsReached == 0)
                DataManager.Profile.sessionMaxEarningsReached = CurrentMoney;
        }

        public void RemoveMoney(double amount)
        {
            if (amount <= CurrentMoney)
            {
                CurrentMoney -= amount;
                DataManager.Profile.totalEarnings = CurrentMoney;
            }
        }
    }
}
