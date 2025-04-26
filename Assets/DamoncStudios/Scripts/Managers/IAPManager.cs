using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.DamoncStudios.Scripts
{
    public class IAPManager : MonoBehaviour
    {
        [SerializeField] private GameObject firstPurchaseOption;
        [SerializeField] private GameObject removeAdsOption;
        [SerializeField] private GameObject increasedEarningsOption;
        [SerializeField] private GameObject firstPurchaseBtn;
        [SerializeField] private GameObject removeAdsBtn;
        [SerializeField] private GameObject doubleProfitsBtn;
        [SerializeField] private GameObject newHireAdvisorBtn;
        [SerializeField] private GameObject storeOfferPanel;

        public static string KEY_FIRST_PURCHASE = "KEY_FIRST_PURCHASE";
        public static string KEY_REMOVE_ADS = "KEY_REMOVE_ADS";
        public static string KEY_DOUBLE_PROFITS = "KEY_DOUBLE_PROFITS";

        public static bool recentlyBought;

        private void Start()
        {
            if (DataManager.Profile.firstPurchaseCompleted)
            {
                firstPurchaseOption.SetActive(false);
            }

            if (DataManager.Profile.removeAds)
            {
                removeAdsOption.SetActive(false);
            }

            if (DataManager.Profile.multiplier)
            {
                increasedEarningsOption.SetActive(false);
            }
        }

        public void OnFirstPurchaseCompleted()
        {
            DataManager.Instance.SaveStorePurchaseRegistry("firstpurchase", 0.99f);

            DataManager.Profile.firstPurchaseCompleted = true;

            DataManager.Instance.SaveUserProfile();

            firstPurchaseOption.SetActive(false);
            firstPurchaseBtn.transform.localPosition = Vector3.right * 2750f;
            storeOfferPanel.transform.localPosition = Vector3.right * 2750f;
            SettingsManager.Instance.OpenConfirmationAlert("Thank you for your incredible support! A new manager has arrived!");
        }

        public void OnRemoveAdsPurchaseCompleted()
        {
            DataManager.Instance.SaveStorePurchaseRegistry("removeads", 6.99f);

            DataManager.Profile.removeAds = true;

            DataManager.Instance.SaveUserProfile();

            removeAdsOption.SetActive(false);
            storeOfferPanel.transform.localPosition = Vector3.right * 2750f;
            removeAdsBtn.transform.localPosition = Vector3.right * 2750f;
            SettingsManager.Instance.OpenConfirmationAlert("Thank you for your incredible support! Ads have been removed!");
        }

        public void OnDoubleProfitsPurchaseCompleted()
        {
            DataManager.Instance.SaveStorePurchaseRegistry("x2earningsmultiplier", 3.99f);

            DataManager.Profile.multiplier = true;

            DataManager.Instance.SaveUserProfile();

            increasedEarningsOption.SetActive(false);
            storeOfferPanel.transform.localPosition = Vector3.right * 2750f;
            doubleProfitsBtn.transform.localPosition = Vector3.right * 2750f;
            SettingsManager.Instance.OpenConfirmationAlert("Thank you for your incredible support! All your earnings have been increased by 2!");
        }

        public void OnHireNewAdvisorPurchaseCompleted()
        {
            DataManager.Instance.SaveStorePurchaseRegistry("hirenewadvisor", 1.99f);

            //StartCoroutine(AdvisorManager.Instance.IEPickRandom(false));

            newHireAdvisorBtn.transform.localPosition = Vector3.right * 2750f;
            recentlyBought = true;
            SettingsManager.Instance.OpenConfirmationAlert("Thank you for your incredible support! A new manager has arrived!");
        }

        public void OnDonationCompleted()
        {
            DataManager.Instance.SaveStorePurchaseRegistry("donate", 0.99f);

            SettingsManager.Instance.OpenConfirmationAlert("Thank you for your incredible support! This will help us a lot to keep making games!");
        }

        public void OpenStoreOfferPanel()
        {
            storeOfferPanel.SetActive(true);
        }

        public void CloseStoreOfferPanel()
        {
            storeOfferPanel.SetActive(false);
        }

        /*public void RedeemGiftCode()
        {
            GiftCodePanel.SetActive(false);
            Debug.Log("ENTERED CODE (" + giftCodeField.text + ")");

            GiftCode giftCode = GetGiftCodeByCode(giftCodeField.text);

            Debug.Log($"CODES >>> {JsonUtility.ToJson(giftCode)}");

            if (giftCode == null)
            {
                descriptionTMP.text = $"Code {giftCodeField.text} doesn't exist!";
                GiftRedeemedPanel.SetActive(true);
                return;
            }

            if (UserGiftClaimed(giftCode.userIds))
            {
                descriptionTMP.text = $"Code {giftCodeField.text} already claimed!";
                GiftRedeemedPanel.SetActive(true);
                return;
            }

            if (!giftCode.isEnabled)
            {
                descriptionTMP.text = $"Code {giftCodeField.text} isn't enabled!";
                GiftRedeemedPanel.SetActive(true);
                return;
            }

            if (giftCode.claimedTimes > giftCode.availableClaims)
            {
                descriptionTMP.text = $"Code {giftCodeField.text} isn't available for more claims!";
                GiftRedeemedPanel.SetActive(true);
                return;
            }

            if (giftCode.claimedTimes == 0)
                giftCode.claimedTimes = 1;
            else
                giftCode.claimedTimes++;

            if (giftCode.userIds == null)
                giftCode.userIds = new List<string>();

            giftCode.userIds.Add(FirebaseManager.Profile.uid);


            FirebaseManager.Instance.UpdateGiftCode(giftCode);

            switch (FirebaseManager.Instance.GetCodeType(giftCode.codeType))
            {
                case CodeType.firstPurchaseFree:

                    FirebaseManager.Profile.firstPurchaseCompleted = true;

                    FirebaseManager.Instance.SaveUserProfile(false);

                    StartCoroutine(AdvisorManager.Instance.IEPickRandom(true));
                    firstPurchaseOption.transform.localPosition = Vector3.right * 2750f;
                    firstPurchaseBtn.transform.localPosition = Vector3.right * 2750f;

                    descriptionTMP.text = "Congratulations a LEGENDARY advisor will arrive soon!";
                    GiftRedeemedPanel.SetActive(true);
                    firstPurchaseOption.SetActive(false);

                    break;

                case CodeType.removeAdsFree:

                    FirebaseManager.Profile.removeAds = true;

                    FirebaseManager.Instance.SaveUserProfile(false);

                    removeAdsBtn.transform.localPosition = Vector3.right * 2750f;

                    descriptionTMP.text = "Congratulations Ads have been removed!";
                    GiftRedeemedPanel.SetActive(true);
                    removeAdsOption.SetActive(false);

                    break;

                case CodeType.doubleProfitsFree:

                    FirebaseManager.Profile.multiplier = true;

                    FirebaseManager.Instance.SaveUserProfile(false);

                    doubleProfitsBtn.transform.localPosition = Vector3.right * 2750f;

                    descriptionTMP.text = "Congratulations your earnings have been increased x2";
                    GiftRedeemedPanel.SetActive(true);
                    increasedEarningsOption.SetActive(false);

                    break;

                case CodeType.hireAdvisorFree:

                    StartCoroutine(AdvisorManager.Instance.IEPickRandom(false));

                    newHireAdvisorBtn.transform.localPosition = Vector3.right * 2750f;

                    descriptionTMP.text = "Congratulations a new advisor will arrive soon!";
                    GiftRedeemedPanel.SetActive(true);

                    break;

                case CodeType.freeCash:

                    EarningsManager.Instance.AddEarnings(giftCode.amount);
                    FirebaseManager.Instance.SaveUserProfile(false);

                    descriptionTMP.text = $"Congratulations you have earned ${giftCode.amount.CurrencyText()}";
                    GiftRedeemedPanel.SetActive(true);
                    break;
            }
        }*/

        public void HideFirstPurchaseOption()
        {
            firstPurchaseOption.SetActive(false);
        }

        /* public void CloseGiftRedeemedPanel()
         {
             GiftRedeemedPanel.SetActive(false);
         }

         private GiftCode GetGiftCodeByCode(string code)
         {
             foreach (GiftCode giftCode in FirebaseManager.Instance.codes)
             {
                 if (giftCode.code.Equals(giftCodeField.text))
                 {
                     return giftCode;
                 }
             }

             return null;
         }

         private bool UserGiftClaimed(List<string> userIds)
         {
             if (userIds == null || userIds.Count <= 0)
                 return false;

             foreach (string uid in userIds)
             {
                 if (uid == FirebaseManager.Profile.uid)
                 {
                     return true;
                 }
             }

             return false;
         }*/
    }
}