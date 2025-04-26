using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

namespace Assets.DamoncStudios.Scripts
{
    public interface ManagerLocation
    {
        void ApplyManagerBoost();
    }

    public class BaseWorkManager : MonoBehaviour
    {
        [SerializeField] private Sprite manager;
        [SerializeField] private Sprite transparentManager;
        [SerializeField] private GameObject boostButton;
        [SerializeField] private Image boostIcon;
        [SerializeField] private GameObject countdownContainer;
        [SerializeField] private TextMeshProUGUI countdownTMP;

        public TextMeshProUGUI CountdownTMP => countdownTMP;

        public ManagerLocation CurrentManagerLocation { get; set; }
        public BaseWorkManager CurrentWorkManager { get; set; }
        public WorkManagerInfo ManagerAssigned { get; set; }

        public static Action<ManagerLocation, BaseWorkManager> OnManagerClicked;

        public bool isFarmHouse = false;
        public bool isShaft = false;
        public int shaftId;
        public bool isWareHouse = false;
        public bool isCardLinked = false;
        bool done = false;

        private void Start()
        {
            if (ManagerAssigned != null)
                SetManager(false);
            else
                SetManager(true);

            HideBoostButton();
        }

        private void Update()
        {
            if (ManagerAssigned != null && !done)
            {
                boostButton.SetActive(true);
                boostIcon.sprite = ManagerAssigned.BoostIcon;
                done = true;
                WorkManagerController.Instance.HideSetManager(ManagerAssigned);
            }
        }

        public void SetManager(bool isEmpty)
        {
            if (!isEmpty)
            {
                SpriteBone[] bones = manager.GetBones();
                GetComponent<SpriteSkin>().gameObject.GetComponent<SpriteRenderer>().sprite = /*manager*/ ManagerAssigned.Manager;
                GetComponent<SpriteSkin>().gameObject.GetComponent<SpriteRenderer>().sprite.SetBones(bones);
            }
            else
            {
                HideBoostButton();
                GetComponent<SpriteSkin>().gameObject.GetComponent<SpriteRenderer>().sprite = transparentManager;
            }
        }

        public void RunBoost()
        {
            boostButton.SetActive(false);
            countdownContainer.SetActive(true);
            CurrentManagerLocation?.ApplyManagerBoost();
        }

        private void OnMouseDown()
        {
            if (!OffersManager.offersOpened && !OfflineEarningsManager.isOfflineOpened)
            {
                CurrentWorkManager = this;
                OnManagerClicked?.Invoke(CurrentManagerLocation, CurrentWorkManager);
            }
        }

        private void HideBoostButton()
        {
            boostButton.SetActive(false);
            if (countdownContainer != null)
                countdownContainer.SetActive(false);
        }

        public void SetupBoostButton()
        {
            if (ManagerAssigned != null)
            {
                boostButton.SetActive(true);
                boostIcon.sprite = ManagerAssigned.BoostIcon;
            }
        }

        public void StopCountDown()
        {
            countdownContainer.SetActive(false);
            boostButton.SetActive(true);
        }

    }
}