using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.DamoncStudios.Scripts
{
    public class ShaftFarmer : BaseFarmer
    {
        public Shaft CurrentShaft { get; set; }

        public Vector3 HarvestingLocation => new Vector3(CurrentShaft.HarvestingLocation.position.x, transform.position.y);
        public Vector3 DepositLocation => new Vector3(CurrentShaft.DepositLocation.position.x, transform.position.y);

        private int idleAnimation = Animator.StringToHash("Idle");
        private int walkAnimation = Animator.StringToHash("Walk");
        private int harvestingAnimation = Animator.StringToHash("Harvesting");
        private int harvestingTreeAnimation = Animator.StringToHash("HarvestingTree");
        private int depositingAnimation = Animator.StringToHash("Depositing");

        bool started = false;

        /*public override void OnClick()
        {
            MoveFarmer(HarvestingLocation);
        }*/

        private void Update()
        {
            if (_animator != null && !started /*&& CurrentShaft.index == 0*/)
            {
                MoveFarmer(HarvestingLocation);
                started = true;

                foreach (GameObject basketProduct in BasketProducts)
                {
                    basketProduct.GetComponent<SpriteRenderer>().sprite = CurrentShaft.ProductIcon;
                }
            }
        }

        protected override void MoveFarmer(Vector3 newPosition)
        {
            base.MoveFarmer(newPosition);

            _animator.SetTrigger(walkAnimation);
        }

        protected override void Harvest()
        {
            if (CurrentShaft.Type == ProductType.tree)
                _animator.SetTrigger(harvestingTreeAnimation);
            else
                _animator.SetTrigger(harvestingAnimation);

            float perSecondValue;

            if (perSecondBoost)
                perSecondValue = HarvestPerSecond * boostValue;
            else
                perSecondValue = HarvestPerSecond;

            float harvestTime = (float)(HarvestCapacity / perSecondValue);
            StartCoroutine(IEHarvest(HarvestCapacity, harvestTime));
        }

        protected override IEnumerator IEHarvest(double products, float harvestTime)
        {
            yield return new WaitForSeconds(harvestTime);

            if (DataManager.Profile.multiplier)
                products *= 2;

            if (AdsManager.Instance.boostActive)
                products *= 2;

            CurrentProducts = products;
            ChangeGoal();
            RotateFarmer(-1);
            _animator.SetTrigger(depositingAnimation);
            MoveFarmer(DepositLocation);
        }

        protected override void Deposit()
        {
            CurrentShaft.ShaftDeposit.DepositProducts(CurrentProducts);

            CurrentProducts = 0;
            ChangeGoal();
            RotateFarmer(1);
            _animator.SetTrigger(walkAnimation);
            MoveFarmer(HarvestingLocation);
        }
    }
}