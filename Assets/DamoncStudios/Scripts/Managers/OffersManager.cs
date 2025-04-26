using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.DamoncStudios.Scripts
{
    public class OffersManager : Singleton<OffersManager>
    {
        [Header("Offers Panel")]
        [SerializeField] private GameObject offersPanel;
        [SerializeField] private TextMeshProUGUI titleTMP;
        [SerializeField] private Image storeIcon;
        [SerializeField] private TextMeshProUGUI descriptionTMP;
        [SerializeField] private TextMeshProUGUI costTMP;
        [SerializeField] private List<Sprite> offersImgs;
        [SerializeField] private GameObject firstPurchaseIAP;
        [SerializeField] private GameObject removeAdsIAP;
        [SerializeField] private GameObject increaseEarningsIAP;
        [SerializeField] private GameObject claimBtn;
        [SerializeField] private GameObject freeClaimBtn;
        [SerializeField] private Sprite[] sprites;
        [SerializeField] private Sprite moneySprite;


        private float[] multipliers = { 0.15f, 0.25f, 0.35f, 0.47f, 0.65f, 0.86f, 1.04f };

        public double rewardAmount;

        public bool isSpeedAd = false;

        public static bool offersOpened;

        private void Start()
        {
            offersOpened = false;
        }

        public void OpenOffersPanel()
        {
            firstPurchaseIAP.SetActive(false);
            removeAdsIAP.SetActive(false);
            increaseEarningsIAP.SetActive(false);

            isSpeedAd = false;
            offersOpened = true;
            storeIcon.sprite = moneySprite;

            if (!AdsManager.offerActivated)
            {
                AdsManager.offerActivated = true;
                int randomManager = Random.Range(0, sprites.Length);
                storeIcon.sprite = sprites[randomManager];
                costTMP.text = $"+ ${rewardAmount.CurrencyText()}";
            }
            else
            {
                if (rewardAmount < 1)
                {
                    SetOfferRewardAmount();
                }
                costTMP.text = $"+ ${rewardAmount.CurrencyText()}";
            }

            titleTMP.text = "VIP INVESTOR HAS ARRIVED!";

            if (DataManager.Profile.removeAds)
            {
                claimBtn.SetActive(false);
                freeClaimBtn.SetActive(true);
                descriptionTMP.text = "Make this VIP Investor spend a LOT!";
            }
            else
            {
                claimBtn.SetActive(true);
                freeClaimBtn.SetActive(false);
                descriptionTMP.text = "Watch a video to make this VIP Investor spend a LOT!";
            }

            offersPanel.SetActive(true);
        }

        public void OpenFirstPurchaseOfferPanel()
        {
            claimBtn.SetActive(false);
            freeClaimBtn.SetActive(false);

            offersOpened = true;
            storeIcon.sprite = offersImgs[0];

            firstPurchaseIAP.SetActive(true);
            removeAdsIAP.SetActive(false);
            increaseEarningsIAP.SetActive(false);

            titleTMP.text = "First Purchase!";
            descriptionTMP.text = "Let a new random EXECUTIVE manager join your farm for only:";
            costTMP.text = "$0.99";

            if (offersPanel.activeSelf)
            {
                offersPanel.transform.localPosition = Vector3.zero;
            }
            else
            {
                offersPanel.SetActive(true);
            }
        }

        public void OpenRemoveAdsOfferPanel()
        {
            claimBtn.SetActive(false);
            freeClaimBtn.SetActive(false);

            offersOpened = true;
            storeIcon.sprite = offersImgs[1];

            firstPurchaseIAP.SetActive(false);
            removeAdsIAP.SetActive(true);
            increaseEarningsIAP.SetActive(false);

            titleTMP.text = "No Ads Offer!";
            descriptionTMP.text = "Get instant benefits without ads for only:";
            costTMP.text = "$3.99";

            if (offersPanel.activeSelf)
            {
                offersPanel.transform.localPosition = Vector3.zero;
            }
            else
            {
                offersPanel.SetActive(true);
            }
        }

        public void OpenIncreasedEarningsOfferPanel()
        {
            claimBtn.SetActive(false);
            freeClaimBtn.SetActive(false);

            offersOpened = true;
            storeIcon.sprite = offersImgs[2];

            firstPurchaseIAP.SetActive(false);
            removeAdsIAP.SetActive(false);
            increaseEarningsIAP.SetActive(true);

            titleTMP.text = "x2 Cash Forever!";
            descriptionTMP.text = "Get x2 increased cash for only:";
            costTMP.text = "$2.99";

            if (offersPanel.activeSelf)
            {
                offersPanel.transform.localPosition = Vector3.zero;
            }
            else
            {
                offersPanel.SetActive(true);
            }
        }

        public void DeclineOffer()
        {
            AdsManager.offerActivated = false;
            offersOpened = false;
            offersPanel.SetActive(false);
        }

        public void SetOfferRewardAmount()
        {
            int randomMultiplier = Random.Range(0, multipliers.Length - 1);

            if (DataManager.Profile.sessionMaxEarningsReached < 950)
            {
                rewardAmount = 650 * multipliers[randomMultiplier];
            }
            else
            {
                rewardAmount = (DataManager.Profile.sessionMaxEarningsReached * 0.32f) * multipliers[randomMultiplier];
            }
        }

        /*public void SetOfferRewardAmount()
        {
            int randomMultiplier = Random.Range(0, multipliers.Length - 1);

            if (DataManager.Profile.sessionMaxEarningsReached < 950)
            {
                rewardAmount = 650 * multipliers[randomMultiplier];
            }
            else
            {
                rewardAmount = (DataManager.Profile.sessionMaxEarningsReached * 0.63f) * multipliers[randomMultiplier];
            }
        }*/
    }
}