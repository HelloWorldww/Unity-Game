using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.DamoncStudios.Scripts
{
    public class SettingsManager : Singleton<SettingsManager>
    {
        // Panel
        [Header("Panel")]
        [SerializeField] private GameObject settingsPanel;
        [SerializeField] private GameObject userProfile;

        //User profile variables
        [Header("Profile")]
        [SerializeField] private TMP_Text usernameText;
        [SerializeField] private TMP_Text idText;
        [SerializeField] private Image profileImg;

        [Header("Confirmation Alert")]
        [SerializeField] private GameObject ConfirmationAlertPanel;
        [SerializeField] private TextMeshProUGUI confirmationTMP;

        public static bool settingsOpened;

        public void OpenSettingsPanel()
        {
            settingsPanel.transform.localPosition = Vector3.zero;
            settingsOpened = true;

            LoadUserProfile();
        }

        public void CloseSettingsPanel()
        {
            settingsPanel.transform.localPosition = Vector3.right * 3000;
            settingsOpened = false;
        }

        private void LoadUserProfile()
        {

            usernameText.text = $"Username: {DataManager.Profile.username}";
            idText.text = $"ID: {DataManager.Profile.uid}";

            /* if (DataManager.Profile.profileAdvisor != null && DataManager.Profile.profileAdvisor != "")
             {
                 AdvisorCard advisor = AdvisorManager.Instance.GetProfileAdvisorCard(FirebaseManager.Profile.profileAdvisor);

                 if (advisor != null)
                 {
                     SetProfileImage(advisor.Advisor.Icon);
                 }
             }*/

            userProfile.SetActive(true);
        }

        public void SetProfileImage(Sprite icon)
        {
            profileImg.sprite = icon;
        }

        public void GetUserID()
        {
            OpenConfirmationAlert($"User ID {DataManager.Profile.uid} copied to clipboard!");

            TextEditor textEditor = new TextEditor();
            textEditor.text = DataManager.Profile.uid;
            textEditor.SelectAll();
            textEditor.Copy();
        }

        public void GetDeviceID()
        {
            OpenConfirmationAlert($"Device ID {SystemInfo.deviceUniqueIdentifier} copied to clipboard!");

            TextEditor textEditor = new TextEditor();
            textEditor.text = SystemInfo.deviceUniqueIdentifier;
            textEditor.SelectAll();
            textEditor.Copy();
        }

        public void OpenConfirmationAlert(string message)
        {
            ConfirmationAlertPanel.SetActive(true);
            confirmationTMP.text = message;
        }

        public void CloseConfirmationAlert()
        {
            ConfirmationAlertPanel.SetActive(false);
            settingsOpened = false;
        }
    }
}