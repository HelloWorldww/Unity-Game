using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.DamoncStudios.Scripts
{
    public class WorkManagerController : Singleton<WorkManagerController>
    {
        [SerializeField] private GameObject managerCardPrefab;
        [SerializeField] private Transform managerPanelContainer;
        [SerializeField] private List<WorkManagerInfo> availableManagers;
        [SerializeField] private List<WorkManagerInfo> managers;
        [SerializeField] private GameObject managerPanel;

        [Header("Assign Manager Card")]
        [SerializeField] private GameObject assignManagerCard;
        [SerializeField] private Image managerIcon;
        [SerializeField] private Image boostIcon;
        [SerializeField] private TextMeshProUGUI managerName;
        [SerializeField] private TextMeshProUGUI managerType;
        [SerializeField] private TextMeshProUGUI boostDuration;
        [SerializeField] private TextMeshProUGUI boostDescription;
        [SerializeField] private GameObject hireManagerBtn;

        [Header("Assign Manager Card")]
        [SerializeField] private double initialManagerCost = 200;
        [SerializeField] private double managerCostMultiplier = 2;
        [SerializeField] private TextMeshProUGUI hireCost;

        public static bool managersOpened;

        public GameObject ManagerCardPrefab => managerCardPrefab;

        public BaseWorkManager CurrentWorkManager { get; set; }

        public double CurrentManagerCost { get; set; }

        private List<WorkManagerCard> _workManagerCardsAssigned = new List<WorkManagerCard>();
        private List<WorkManagerCard> _managerCards = new List<WorkManagerCard>();

        private void Start()
        {
            if (DataManager.Profile.managers == null)
            {
                DataManager.Profile.managers = new List<WorkManagerInfo>();
            }

            CurrentManagerCost = initialManagerCost;

            if (DataManager.Profile.managers.Count > 0)
                LoadHiredManagers();
        }

        private void Update()
        {
            hireCost.text = CurrentManagerCost.CurrencyText();

            if (availableManagers.Count > 0 && MoneyManager.Instance.CurrentMoney >= CurrentManagerCost)
            {
                hireManagerBtn.GetComponent<Button>().interactable = true;
            }
            else
            {
                hireManagerBtn.GetComponent<Button>().interactable = false;
            }
        }

        public void OpenCloseManagerPanel(bool value)
        {
            managersOpened = value;
            managerPanel.SetActive(value);
        }

        #region Boost

        public void RunMovementBoost(BaseFarmer farmer, float duration, float value)
        {
            StartCoroutine(IEMovementBoost(farmer, duration * 60, value));
        }

        private IEnumerator IEMovementBoost(BaseFarmer farmer, float duration, float value)
        {
            farmer.speedBoost = true;
            farmer.boostValue = value;
            yield return new WaitForSeconds(duration);
            farmer.speedBoost = false;
        }

        public void RunLoadingBoost(BaseFarmer farmer, float duration, float value)
        {
            StartCoroutine(IELoadingBoost(farmer, duration * 60, value));
        }

        private IEnumerator IELoadingBoost(BaseFarmer farmer, float duration, float value)
        {
            farmer.perSecondBoost = true;
            farmer.boostValue = value;
            yield return new WaitForSeconds(duration);
            farmer.perSecondBoost = false;
        }

        #endregion

        public void HireManager()
        {
            if (availableManagers.Count > 0 && MoneyManager.Instance.CurrentMoney >= CurrentManagerCost)
            {
                GameObject managerCardGO = Instantiate(managerCardPrefab, managerPanelContainer);
                WorkManagerCard managerCard = managerCardGO.GetComponent<WorkManagerCard>();
                int random = UnityEngine.Random.Range(0, availableManagers.Count);
                WorkManagerInfo managerInfo = availableManagers[random];
                managerCard.SetupWorkManagerCard(managerInfo);
                //availableManagers.RemoveAt(random);

                WorkManagerInfo managerToSave = managerInfo;
                if (DataManager.Profile.managers.Count == 0)
                    managerToSave.Index = 0;
                else
                    managerToSave.Index = DataManager.Profile.managers.Count - 1;
                managerToSave.OriginalIndex = random;
                DataManager.Profile.managers.Add(managerToSave);

                MoneyManager.Instance.RemoveMoney(CurrentManagerCost);
                CurrentManagerCost *= managerCostMultiplier;

                DataManager.Instance.SaveUserProfile();
            }
        }

        public void UnassignManager()
        {
            if (CurrentWorkManager.isFarmHouse)
            {
                DataManager.Profile.farmHouse.HasManager = false;
                DataManager.Profile.farmHouse.Manager = null;
            }

            if (CurrentWorkManager.isShaft)
            {
                DataManager.Profile.shafts[CurrentWorkManager.shaftId].HasManager = false;
                DataManager.Profile.shafts[CurrentWorkManager.shaftId].Manager = null;
            }

            if (CurrentWorkManager.isWareHouse)
            {
                DataManager.Profile.wareHouse.HasManager = false;
                DataManager.Profile.wareHouse.Manager = null;
            }

            RestoreManagerCardAssigned();
            UpdateAssignManagerCard();
            DataManager.Instance.SaveUserProfile();
        }

        public void RestoreManagerCardAssigned()
        {
            WorkManagerCard managerCardAssigned = null;

            for (int i = 0; i < _workManagerCardsAssigned.Count; i++)
            {
                if (CurrentWorkManager.ManagerAssigned == _workManagerCardsAssigned[i]?.ManagerInfoAssigned)
                {
                    managerCardAssigned = _workManagerCardsAssigned[i];
                }
            }

            if (managerCardAssigned != null)
            {
                managerCardAssigned.gameObject.SetActive(true);
                _workManagerCardsAssigned.Remove(managerCardAssigned);
                CurrentWorkManager.ManagerAssigned = null;
                CurrentWorkManager.SetManager(true);
            }
        }

        private void UpdateAssignManagerCard()
        {
            if (CurrentWorkManager.ManagerAssigned != null)
            {
                assignManagerCard.SetActive(true);
                managerIcon.sprite = CurrentWorkManager.ManagerAssigned.ManagerIcon;
                boostIcon.sprite = CurrentWorkManager.ManagerAssigned.BoostIcon;
                managerName.text = CurrentWorkManager.ManagerAssigned.Name;
                managerType.text = CurrentWorkManager.ManagerAssigned.ManagerType.ToString();
                managerType.color = CurrentWorkManager.ManagerAssigned.LevelColor;
                boostDuration.text = $"Duration: {CurrentWorkManager.ManagerAssigned.BoostDuration} min";
                boostDescription.text = CurrentWorkManager.ManagerAssigned.BoostDescription;

                if (!CurrentWorkManager.isCardLinked)
                {
                    WorkManagerCard managerCard = GetManagerCard(CurrentWorkManager.ManagerAssigned);
                    _workManagerCardsAssigned.Add(managerCard);
                    CurrentWorkManager.isCardLinked = true;
                    //managerCard.gameObject.SetActive(false);
                }
            }
            else
            {
                assignManagerCard.SetActive(false);
            }
        }

        private void ManagerClicked(ManagerLocation location, BaseWorkManager workManager)
        {
            CurrentWorkManager = workManager;

            if (CurrentWorkManager.ManagerAssigned == null)
            {
                assignManagerCard.SetActive(false);
            }
            else
            {
                assignManagerCard.SetActive(true);
            }

            DataManager.Instance.SaveUserProfile();
            UpdateAssignManagerCard();
            OpenCloseManagerPanel(true);
        }

        public void WorkManagerCardAssigned(WorkManagerCard card)
        {
            if (CurrentWorkManager.ManagerAssigned != null)
                RestoreManagerCardAssigned();

            if (CurrentWorkManager.isFarmHouse)
            {
                DataManager.Profile.farmHouse.HasManager = true;
                DataManager.Profile.farmHouse.Manager = $"{card.ManagerInfoAssigned.Name}-{card.ManagerInfoAssigned.ManagerType}";
                DataManager.Instance.SaveUserProfile();
            }

            if (CurrentWorkManager.isShaft)
            {
                DataManager.Profile.shafts[CurrentWorkManager.shaftId].HasManager = true;
                DataManager.Profile.shafts[CurrentWorkManager.shaftId].Manager = $"{card.ManagerInfoAssigned.Name}-{card.ManagerInfoAssigned.ManagerType}";
                DataManager.Instance.SaveUserProfile();
            }

            if (CurrentWorkManager.isWareHouse)
            {
                DataManager.Profile.wareHouse.HasManager = true;
                DataManager.Profile.wareHouse.Manager = $"{card.ManagerInfoAssigned.Name}-{card.ManagerInfoAssigned.ManagerType}";
                DataManager.Instance.SaveUserProfile();
            }

            _workManagerCardsAssigned.Add(card);
            CurrentWorkManager.ManagerAssigned = card.ManagerInfoAssigned;
            CurrentWorkManager.SetManager(false);
            CurrentWorkManager.SetupBoostButton();
            UpdateAssignManagerCard();
        }

        private void OnEnable()
        {
            BaseWorkManager.OnManagerClicked += ManagerClicked;
            WorkManagerCard.OnAssignRequest += WorkManagerCardAssigned;
        }

        private void OnDisable()
        {
            BaseWorkManager.OnManagerClicked -= ManagerClicked;
            WorkManagerCard.OnAssignRequest -= WorkManagerCardAssigned;
        }

        private void LoadHiredManagers()
        {
            foreach (WorkManagerInfo manager in DataManager.Profile.managers)
            {
                GameObject managerCardGO = Instantiate(managerCardPrefab, managerPanelContainer);
                WorkManagerCard managerCard = managerCardGO.GetComponent<WorkManagerCard>();
                managerCard.SetupWorkManagerCard(managers[manager.OriginalIndex]);
                _managerCards.Add(managerCard);

                CurrentManagerCost *= managerCostMultiplier;
            }
        }

        public void HideSetManager(WorkManagerInfo manager)
        {
            foreach (WorkManagerCard card in _managerCards)
            {
                if (card.ManagerInfoAssigned == manager)
                {
                    card.gameObject.SetActive(false);
                    return;
                }
            }
        }

        public WorkManagerInfo GetManager(string name, string managerType)
        {
            foreach (WorkManagerInfo manager in managers)
            {
                if (manager.Name.Equals(name) && managerType == manager.ManagerType.ToString())
                {
                    return manager;
                }
            }

            return null;
        }

        private WorkManagerCard GetManagerCard(WorkManagerInfo workManager)
        {
            foreach (WorkManagerCard card in _managerCards)
            {
                if (card.ManagerInfoAssigned == workManager)
                {
                    return card;
                }
            }

            return null;
        }
    }
}