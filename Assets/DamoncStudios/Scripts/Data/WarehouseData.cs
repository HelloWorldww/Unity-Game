using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.DamoncStudios.Scripts
{
    [Serializable()]
    public class WarehouseData
    {
        public int Level;
        public double UpgradeCost;
        public double DepositCurrentProducts;
        public string Manager;
        public bool HasManager;
    }
}