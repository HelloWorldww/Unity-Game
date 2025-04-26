using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.DamoncStudios.Scripts
{
    public class OfflineEarningsManager : Singleton<OfflineEarningsManager>
    {
        [SerializeField] private GameObject offlineEarningsPanel;
        //[SerializeField] private TextMeshProUGUI btnMessage;
        [SerializeField] private TextMeshProUGUI earningsTMP;
        [SerializeField] private TextMeshProUGUI timeTMP;

        public long seconds { get; set; }
        public string absentTime { get; set; }

        private double earnings;

        public double earningsAmount;

        private readonly string OFFLINE_TIME_KEY = "OFFLINE_TIME_KEY";

        public static bool isOfflineOpened;

        private void Start()
        {
            isOfflineOpened = false;
            CalculateOfflineTime();
            CalculateEarnings();
            ShowEarnings();
        }

        public void ClaimEarnings()
        {
            offlineEarningsPanel.SetActive(false);
            MoneyManager.Instance.AddMoney(earnings);
            seconds = 0;
            earnings = 0;
            earningsAmount = 0;
            isOfflineOpened = false;
        }

        private void ShowEarnings()
        {
            isOfflineOpened = true;
            if (seconds > 0 && earnings > 0d)
            {
                if (offlineEarningsPanel != null)
                    offlineEarningsPanel.SetActive(true);
                earningsTMP.text = $"${earnings.CurrencyText()}";
                timeTMP.text = absentTime;
            }
        }

        private void CalculateEarnings()
        {
            if (seconds > 0)
            {
                if (seconds >= 43200)
                {
                    seconds = 43200; // 12 hours
                }

                float multiplier = getMultiplier();

                if (MoneyManager.Instance.CurrentMoney == 0)
                {
                    earnings = 10;
                }
                else
                {
                    earnings = multiplier * MoneyManager.Instance.CurrentMoney;
                }

                if (earnings <= 10)
                    earnings = 10;

                earningsAmount = earnings;
            }
        }

        private float getMultiplier()
        {
            float multiplier = seconds / 43200f;

            return multiplier;
        }

        private void CalculateOfflineTime()
        {

            string time = PlayerPrefs.GetString(OFFLINE_TIME_KEY, string.Empty);

            if (!string.IsNullOrEmpty(time))
            {
                DateTime savedTime = DateTime.FromBinary(Convert.ToInt64(time));
                TimeSpan difference = DateTime.Now.Subtract(savedTime);
                seconds = Mathf.FloorToInt((float)difference.TotalSeconds);
                absentTime = $"{difference.Hours:00}:{difference.Minutes:00}:{difference.Seconds:00}";
            }

        }

        public void SaveGameTime()
        {
            string offlineTime = DateTime.Now.ToBinary().ToString();

            PlayerPrefs.SetString(OFFLINE_TIME_KEY, offlineTime);
            PlayerPrefs.Save();

            /*FirebaseManager.Profile.lastTimePlayed = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            FirebaseManager.Profile.offlineTime = offlineTime;
            FirebaseManager.Instance.SaveUserProfile(false);*/

        }

        private void OnApplicationFocus(bool focus)
        {
            if (!focus)
            {
                SaveGameTime();
            }
        }

        private void OnDisable()
        {
            SaveGameTime();
        }
    }
}