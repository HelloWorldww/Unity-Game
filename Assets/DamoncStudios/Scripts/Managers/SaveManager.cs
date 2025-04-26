using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.DamoncStudios.Scripts
{
    public class SaveManager : MonoBehaviour
    {
        private void Start()
        {
            InvokeRepeating("SaveGameRoutine", 10f, 15f);
        }

        private void SaveGameRoutine()
        {
            DataManager.Instance.SaveUserProfile();
        }
    }
}