using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.DamoncStudios.Scripts
{
    public enum ProductType
    {
        tree,
        ground,
        bush
    }

    [CreateAssetMenu(fileName = "HarvestSpot")]
    [Serializable()]
    public class ShaftData : ScriptableObject
    {
        public int Index;
        public string Product;
        public string ProductType;
        public Sprite ProductIcon;
    }
}