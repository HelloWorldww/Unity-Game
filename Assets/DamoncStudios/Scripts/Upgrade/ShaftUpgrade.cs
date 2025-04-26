using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.DamoncStudios.Scripts
{
    public class ShaftUpgrade : BaseUpgrade
    {
        private int counter;

        protected override void ExecuteUpgrade()
        {
            if (CurrentLevel % 10 == 0)
            {
                _shaft.CreateFarmer();
            }

            if (CurrentLevel % 10 == 0)
            {
                _shaft.Farmers[0].HarvestCapacity *= 2;
                _shaft.Farmers[0].HarvestPerSecond *= 2;
                _shaft.Farmers[0].MoveSpeed *= MoveSpeedMultiplier;
                counter++;
            }
            else
            {
                _shaft.Farmers[0].HarvestCapacity *= CollectCapacityMultiplier;
                _shaft.Farmers[0].HarvestPerSecond *= CollectPerSecondMultiplier;
            }

            for (int i = 1; i < _shaft.Farmers.Count; i++)
            {
                _shaft.Farmers[i].HarvestCapacity = _shaft.Farmers[0].HarvestCapacity;
                _shaft.Farmers[i].HarvestPerSecond = _shaft.Farmers[0].HarvestPerSecond;
                _shaft.Farmers[i].MoveSpeed = _shaft.Farmers[0].MoveSpeed;
            }
        }

        protected override void ExecuteLoadUpgrade()
        {
            _shaft.Farmers[0].HarvestCapacity *= CollectCapacityMultiplier;
            _shaft.Farmers[0].HarvestPerSecond *= CollectPerSecondMultiplier;

            if (CurrentLevel % 10 == 0)
            {
                _shaft.Farmers[0].MoveSpeed *= MoveSpeedMultiplier;
                counter++;
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
                    _shaft.Farmers[i].HarvestCapacity = _shaft.Farmers[0].HarvestCapacity;
                    _shaft.Farmers[i].HarvestPerSecond = _shaft.Farmers[0].HarvestPerSecond;
                    _shaft.Farmers[i].MoveSpeed = _shaft.Farmers[0].MoveSpeed;
                }
            }
            catch (Exception e) { }
        }

        protected override IEnumerator Creating()
        {
            yield return new WaitForSeconds(0.8f);
            _shaft.CreateFarmer();
        }
    }
}