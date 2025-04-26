using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.DamoncStudios.Scripts
{
    public enum ManagerType
    {
        Junior,
        Senior,
        Executive
    }

    public enum BoostType
    {
        Movement,
        Loading
    }

    [CreateAssetMenu]
    public class WorkManagerInfo : ScriptableObject
    {
        [Header("Manager Info")]
        public ManagerType ManagerType;
        public Color LevelColor;

        public int Index;
        public int OriginalIndex;
        public string Name;
        public BoostType BoostType;
        public Sprite BoostIcon;
        public Sprite Manager;
        public Sprite ManagerIcon;
        public float BoostDuration;
        public string BoostDescription;
        public float BoostValue;
    }
}