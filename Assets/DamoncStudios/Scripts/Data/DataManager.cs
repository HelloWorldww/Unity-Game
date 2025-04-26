using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BayatGames.SaveGameFree;

namespace Assets.DamoncStudios.Scripts
{
    public class DataManager : Singleton<DataManager>
    {
        public static GameUserProfile Profile = null;

        private string KEY_USER_DATA = "KEY_USER_DATA";

        public void SaveUserProfile()
        {
            SaveGame.Save(KEY_USER_DATA, Profile);
        }

        public void GetUserProfile()
        {
            Profile = SaveGame.Load<GameUserProfile>(KEY_USER_DATA);
        }

        public bool CanStartCart()
        {
            if (Profile.shafts != null)
            {
                if (Profile.shafts.Count > 0)
                {
                    foreach (UsersShaft shaft in Profile.shafts)
                    {
                        if (shaft.DepositCurrentProducts > 0)
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        public void SaveStorePurchaseRegistry(string product, float amount)
        {

        }
    }
}
