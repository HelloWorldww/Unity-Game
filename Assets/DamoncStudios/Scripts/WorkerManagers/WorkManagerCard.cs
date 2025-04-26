using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.DamoncStudios.Scripts
{
    public class WorkManagerCard : MonoBehaviour
    {
        public static Action<WorkManagerCard> OnAssignRequest;

        [SerializeField] private Image managerIcon;
        [SerializeField] private Image boostIcon;
        [SerializeField] private TextMeshProUGUI managerName;
        [SerializeField] private TextMeshProUGUI managerType;
        [SerializeField] private TextMeshProUGUI boostDuration;
        [SerializeField] private TextMeshProUGUI boostDescription;

        public WorkManagerInfo ManagerInfoAssigned { get; set; }

        public void SetupWorkManagerCard(WorkManagerInfo managerInfo)
        {
            ManagerInfoAssigned = managerInfo;
            managerIcon.sprite = managerInfo.ManagerIcon;
            boostIcon.sprite = managerInfo.BoostIcon;
            managerName.text = managerInfo.Name;
            managerType.text = managerInfo.ManagerType.ToString();
            managerType.color = managerInfo.LevelColor;
            boostDuration.text = $"Duration: {managerInfo.BoostDuration} min";
            boostDescription.text = managerInfo.BoostDescription;
        }

        public void AssignManager()
        {
            OnAssignRequest?.Invoke(this);
            gameObject.SetActive(false);
        }
    }
}