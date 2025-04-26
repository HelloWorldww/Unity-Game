using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.DamoncStudios.Scripts
{
    public class FarmHouseUpgrade : BaseUpgrade
    {
        protected override void ExecuteUpgrade()
        {
            if (CurrentLevel + 1 % 10 == 0)
            {
                _farmHouse.Cart.HarvestCapacity *= 2;
                _farmHouse.Cart.HarvestPerSecond *= 2;
                _farmHouse.Cart.MoveSpeed *= MoveSpeedMultiplier;
            }
            else
            {
                _farmHouse.Cart.HarvestCapacity *= CollectCapacityMultiplier;
                _farmHouse.Cart.HarvestPerSecond *= CollectPerSecondMultiplier;
            }
        }
    }
}