using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine.UI;

namespace Assets.DamoncStudios.Scripts
{
    public enum RewardType
    {
        Earning,
        Offline,
        Boost
    }

    public class AdsManager : Singleton<AdsManager>
    {
        internal static AdsManager wkr;
        Queue<Action> jobs = new Queue<Action>();

        [SerializeField] private bool testing;

        [Header("Components")]
        [SerializeField] private GameObject adsGameBtn;
        [SerializeField] private GameObject adsOfflineBtn;
        [SerializeField] private GameObject adsOfflineFreeBtn;
        [SerializeField] private GameObject adsOfflinePanel;
        [SerializeField] private GameObject offlineWindow;
        [SerializeField] private GameObject offersWindow;
        [SerializeField] private GameObject adsAdvisorsBtn;
        [SerializeField] private GameObject adsFreeAdvisorsBtn;
        [SerializeField] private GameObject confirmationPanel;
        [SerializeField] private TextMeshProUGUI confirmationPanelText;
        [SerializeField] private GameObject adsBoostBtn;
        //[SerializeField] private GameObject adsBoostContainer;
        [SerializeField] private TextMeshProUGUI adsBoostTMP;
        [SerializeField] private Image adsBoostImg;
        [SerializeField] private TextMeshProUGUI countdown;

        [Header("Farmers")]
        [SerializeField] private CartFarmer cart;
        [SerializeField] private Warehouse warehouse;

        public static bool offerActivated = false;

        private RewardType rewardType;

        public bool boostActive;

        bool timerIsRunning = false;
        float timeRemaining = 10;
        float waitTime;

        public bool countdownIsRunning = false;
        float countdownTimeRemaining = 600f;

        protected override void Awake()
        {
            base.Awake();

            wkr = this;

            SetWaitTime();
        }

        void Update()
        {
            while (jobs.Count > 0)
                jobs.Dequeue().Invoke();

            if (timerIsRunning)
            {
                if (timeRemaining <= waitTime)
                {
                    timeRemaining += Time.deltaTime;
                }
                else
                {
                    timeRemaining = 0;
                    timerIsRunning = false;


                    OffersManager.Instance.SetOfferRewardAmount();
                    adsGameBtn.SetActive(true);
                    if (DataManager.Profile.removeAds)
                        adsOfflineFreeBtn.SetActive(true);
                    else
                        adsOfflineBtn.SetActive(true);

                    System.Random rnd = new System.Random();
                    int r = rnd.Next(0, 100);
                }
            }

            if (countdownIsRunning)
            {
                if (countdownTimeRemaining > 0)
                {
                    countdownTimeRemaining -= Time.deltaTime;

                    float minutes = Mathf.FloorToInt(countdownTimeRemaining / 60);
                    float seconds = Mathf.FloorToInt(countdownTimeRemaining % 60);

                    string minutesTxt = minutes.ToString();
                    string secondsTxt = seconds.ToString();

                    if (minutes < 10)
                        minutesTxt = $"0{minutes}";

                    if (seconds < 10)
                        secondsTxt = $"0{seconds}";

                    countdown.text = $"{minutesTxt}:{secondsTxt}";
                    boostActive = true;
                }
                else
                {
                    countdown.text = "";
                    countdownTimeRemaining = 600f;
                    countdownIsRunning = false;
                    boostActive = false;
                    //adsBoostBtn.SetActive(true);
                    adsBoostTMP.gameObject.SetActive(true);
                    adsBoostImg.gameObject.SetActive(true);
                }
            }
        }

        internal void AddJob(Action newJob)
        {
            jobs.Enqueue(newJob);
        }

        public void ShowRewardedAdByType(int type)
        {
            if (boostActive && type == 2)
                return;

            switch (type)
            {
                case 0:
                    rewardType = RewardType.Earning;
                    break;
                case 1:
                    rewardType = RewardType.Offline;
                    break;
                case 2:
                    rewardType = RewardType.Boost;
                    break;
            }

            ShowRewardedAd();
        }

        private void ShowRewardedAd()
        {
            GetRewardByType();
        }

        private void GetRewardByType()
        {
            OffersManager.offersOpened = false;
            switch (rewardType)
            {
                case RewardType.Earning:
                    MoneyManager.Instance.AddMoney(OffersManager.Instance.rewardAmount);
                    break;
                case RewardType.Offline:
                    OfflineEarningsManager.isOfflineOpened = false;
                    MoneyManager.Instance.AddMoney(OfflineEarningsManager.Instance.earningsAmount * 2d);
                    break;
                case RewardType.Boost:
                    adsBoostTMP.gameObject.SetActive(false);
                    adsBoostImg.gameObject.SetActive(false);
                    countdownIsRunning = true;
                    break;
            }

            offerActivated = false;


            adsGameBtn.SetActive(false);
            offlineWindow.SetActive(false);
            offersWindow.SetActive(false);

            SetWaitTime();
        }

        private void SetWaitTime()
        {
            if (DataManager.Profile.removeAds)
                waitTime = UnityEngine.Random.Range(10, 25);
            else
                waitTime = UnityEngine.Random.Range(5, 15);
            timerIsRunning = true;
        }
    }
}