using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.DamoncStudios.Scripts
{
    public class WarehouseUpgrade : BaseUpgrade
    {
        private int counter;

        protected override void ExecuteUpgrade()
        {
            if (CurrentLevel % 10 == 0)
            {
                _warehouse.AddHelper();
            }

            foreach (WarehouseFarmer farmer in _warehouse.Farmers)
            {
                if (CurrentLevel % 10 == 0)
                {
                    farmer.HarvestCapacity *= 2;
                    farmer.HarvestPerSecond *= 2;
                    farmer.MoveSpeed *= MoveSpeedMultiplier;
                }
                else
                {
                    farmer.HarvestCapacity *= CollectCapacityMultiplier;
                    farmer.HarvestPerSecond *= CollectPerSecondMultiplier;
                }
            }
        }

        protected override void ExecuteLoadUpgrade()
        {
            if (CurrentLevel % 10 == 0)
            {
                _warehouse.Farmers[0].HarvestCapacity *= 2;
                _warehouse.Farmers[0].HarvestPerSecond *= 2;
                _warehouse.Farmers[0].MoveSpeed *= MoveSpeedMultiplier;
                counter++;
            }
            else
            {
                _warehouse.Farmers[0].HarvestCapacity *= CollectCapacityMultiplier;
                _warehouse.Farmers[0].HarvestPerSecond *= CollectPerSecondMultiplier;
            }
        }

        protected override IEnumerator ExecuteCreate()
        {
            for (int i = 0; i < counter; i++)
            {
                yield return StartCoroutine(Creating());
            }

            counter = 0;

            try
            {
                for (int i = 1; i < _shaft.Farmers.Count; i++)
                {
                    _warehouse.Farmers[i].HarvestCapacity = _warehouse.Farmers[0].HarvestCapacity;
                    _warehouse.Farmers[i].HarvestPerSecond = _warehouse.Farmers[0].HarvestPerSecond;
                    _warehouse.Farmers[i].MoveSpeed = _warehouse.Farmers[0].MoveSpeed;
                }
            }
            catch (Exception e) { }
        }

        protected override IEnumerator Creating()
        {
            yield return new WaitForSeconds(1.2f);
            _warehouse.AddHelper();
        }
    }
}