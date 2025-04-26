using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.DamoncStudios.Scripts
{
    public class ContactManager : MonoBehaviour
    {
        [SerializeField] private GameObject contactPanel;

        public void OpenContactPanel()
        {
            contactPanel.SetActive(true);
        }

        public void CloseContactPanel()
        {
            contactPanel.SetActive(false);
        }

        public void SendEmail()
        {
            string toEmail = "damonc.studios@gmail.com";
            string emailSubject = "User Support";
            string emailBody = "Damonc Studios Team \n\nI would like to report that: \n\n";

            emailSubject += $" - {DataManager.Profile.uid}";

            emailSubject = System.Uri.EscapeUriString(emailSubject);
            emailBody = System.Uri.EscapeUriString(emailBody);
            Application.OpenURL("mailto:" + toEmail + "?subject=" + emailSubject + "&body=" + emailBody);
        }

        public void OpenNews()
        {
            Application.OpenURL("https://damonc-studios.web.app/idle-farmer");
        }
    }
}