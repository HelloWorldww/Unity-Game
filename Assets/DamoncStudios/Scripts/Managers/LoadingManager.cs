using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using TMPro;
using BayatGames.SaveGameFree;

namespace Assets.DamoncStudios.Scripts
{
    public class LoadingManager : Singleton<LoadingManager>
    {
        internal static LoadingManager wkr;
        Queue<Action> jobs = new Queue<Action>();

        [SerializeField] private bool deleteAll;
        [SerializeField] private TextMeshProUGUI versionTMP;

        [Header("Businesses")]
        [SerializeField] private GameObject loadingPanel;
        [SerializeField] private TextMeshProUGUI idTMP;
        [SerializeField] private GameObject playGameSection;
        [SerializeField] private Slider slider;

        protected override void Awake()
        {
            base.Awake();

            wkr = this;

            versionTMP.text = $"v {Application.version}";

            slider.value = 0f;

            if (deleteAll)
                SaveGame.DeleteAll();
        }

        private void Update()
        {
            while (jobs.Count > 0)
                jobs.Dequeue().Invoke();
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        internal void AddJob(Action newJob)
        {
            jobs.Enqueue(newJob);
        }

        public void UpdateLoading(float progress, string message, bool complete)
        {
            slider.value = progress;
            idTMP.text = message;

            if (complete)
            {
                CompleteLoading();
            }
        }

        public void CompleteLoading()
        {
            loadingPanel.SetActive(false);
            playGameSection.SetActive(true);
        }

        public void StartGame()
        {
            if (DataManager.Profile.farmHouse == null)
                DataManager.Profile.farmHouse = new FarmHouseData();
            if (DataManager.Profile.wareHouse == null)
                DataManager.Profile.wareHouse = new WarehouseData();
            if (DataManager.Profile.shafts == null)
                DataManager.Profile.shafts = new List<UsersShaft>();
            if (DataManager.Profile.managers == null)
                DataManager.Profile.managers = new List<WorkManagerInfo>();

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
        }

    }
}