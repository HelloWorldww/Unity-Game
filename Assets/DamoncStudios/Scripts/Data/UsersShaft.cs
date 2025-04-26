using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.DamoncStudios.Scripts
{
    [Serializable()]
    public class UsersShaft
    {
        public int Index;
        public int ShaftIndex;
        public int Level;
        public double LevelUpgradeCost;
        public double Cost;
        public double UpgradeCost;
        public double DepositCurrentProducts;
        public string Manager;
        public bool HasManager;
    }
}