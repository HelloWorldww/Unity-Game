using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.DamoncStudios.Scripts
{
    [Serializable()]
    public class GameUserProfile
    {
        public string username;
        public string uid;
        public double totalEarnings;
        public double maxEarningsReached;
        public double sessionMaxEarningsReached;
        public List<UsersShaft> shafts;
        public List<WorkManagerInfo> managers;
        public FarmHouseData farmHouse;
        public WarehouseData wareHouse;
        public string offlineTime;
        public bool firstPurchaseCompleted;
        public bool removeAds;
        public bool multiplier;
        public string deviceId;
        public bool update;
        public string createdDate;
        public string lastTimePlayed;
    }
}