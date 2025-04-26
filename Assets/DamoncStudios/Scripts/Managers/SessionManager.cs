using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Linq;

namespace Assets.DamoncStudios.Scripts
{
    public class SessionManager : Singleton<SessionManager>
    {
        internal static SessionManager wkr;
        Queue<Action> jobs = new Queue<Action>();

        public bool isGuest = false;
        public bool isGuestSync = false;

        public string GUEST_KEY = "isGuest";

        protected override void Awake()
        {
            base.Awake();

            wkr = this;
        }

        void Start()
        {
            LoadingManager.Instance.UpdateLoading(0.12f, "Loading...", false);

            ManageUserData();
        }

        private void Update()
        {
            while (jobs.Count > 0)
                jobs.Dequeue().Invoke();
        }

        internal void AddJob(Action newJob)
        {
            jobs.Enqueue(newJob);
        }

        public void ManageUserData()
        {
            isGuest = true;
            LoadingManager.Instance.UpdateLoading(0.53f, "Retrieving Guest Session Data", false);

            HandleGuestSession();
        }

        private void HandleGuestSession()
        {
            DataManager.Instance.GetUserProfile();

            string guestId = DataManager.Profile == null ? generateID() : DataManager.Profile.username.Split("-")[1];

            if (DataManager.Profile == null)
            {
                PlayerPrefs.SetInt(GUEST_KEY, 1);
                DataManager.Profile = new GameUserProfile();
                DataManager.Profile.uid = SystemInfo.deviceUniqueIdentifier;
                DataManager.Profile.deviceId = SystemInfo.deviceUniqueIdentifier;
                DataManager.Profile.createdDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                DataManager.Profile.username = $"Guest-{guestId}";
                DataManager.Profile.maxEarningsReached = 0;

                DataManager.Instance.SaveUserProfile();
            }
            else
            {
                guestId = DataManager.Profile.username.Split("-")[1];
            }

            LoadingManager.Instance.UpdateLoading(1f, $"Guest Session - {guestId}", true);
        }

        public string generateID()
        {
            StringBuilder builder = new StringBuilder();
            Enumerable
               .Range(65, 26)
                .Select(e => ((char)e).ToString())
                .Concat(Enumerable.Range(97, 26).Select(e => ((char)e).ToString()))
                .Concat(Enumerable.Range(0, 10).Select(e => e.ToString()))
                .OrderBy(e => Guid.NewGuid())
                .Take(11)
                .ToList().ForEach(e => builder.Append(e));
            return builder.ToString(); ;
        }
    }
}