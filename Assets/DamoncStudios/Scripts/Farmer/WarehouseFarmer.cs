using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.DamoncStudios.Scripts
{
    public class WarehouseFarmer : BaseFarmer
    {
        public Deposit FarmHouseDeposit { get; set; }
        public Vector3 FarmHouseDepositLocation { get; set; }
        public Vector3 WarehouseLocation { get; set; }

        bool started = false;

        private int _walkAnimation = Animator.StringToHash("HelperWalk");
        private int _idleAnimation = Animator.StringToHash("HelperIdle");

        bool isHarvesting = false;
        float harvestingTimeRemaining = 0;
        float harvestingTime = 0;

        private void Update()
        {
            if (_animator != null && !started /*&& FarmHouseDeposit.CurrentProducts > 0*/)
            {
                RotateFarmer(-1);
                MoveFarmer(FarmHouseDepositLocation);
                started = true;
            }

            if (isHarvesting)
            {
                if (harvestingTimeRemaining < harvestingTime)
                {
                    Slider.value = harvestingTimeRemaining / harvestingTime;
                    harvestingTimeRemaining += Time.deltaTime;
                }
                else
                {
                    harvestingTimeRemaining = 0;
                    isHarvesting = false;
                    Slider.gameObject.SetActive(false);
                }
            }
        }

        protected override void MoveFarmer(Vector3 newPosition)
        {
            base.MoveFarmer(newPosition);
            _animator.SetBool(_walkAnimation, true);
        }

        protected override void Harvest()
        {
            if (!FarmHouseDeposit.CanCollectProducts)
            {
                RotateFarmer(1);
                ChangeGoal();
                MoveFarmer(WarehouseLocation);
                return;
            }

            _animator.SetBool(_walkAnimation, false);
            _animator.SetBool(_idleAnimation, true);
            double amountToCollect;

            if (AdsManager.Instance.boostActive)
                if (FarmHouseDeposit.CollectProducts(this) * 2 > FarmHouseDeposit.CurrentProducts)
                {
                    amountToCollect = FarmHouseDeposit.CurrentProducts;
                }
                else
                {
                    amountToCollect = FarmHouseDeposit.CollectProducts(this) * 2;
                }
            else
                amountToCollect = FarmHouseDeposit.CollectProducts(this);

            float collectTime;

            float perSecondValue;

            if (perSecondBoost)
                perSecondValue = HarvestPerSecond * boostValue;
            else
                perSecondValue = HarvestPerSecond;

            if (AdsManager.Instance.boostActive)
                collectTime = (float)((amountToCollect / 2) / perSecondValue);
            else
                collectTime = (float)(amountToCollect / perSecondValue);

            StartCoroutine(IEHarvest(amountToCollect, collectTime));
        }

        protected override IEnumerator IEHarvest(double products, float harvestTime)
        {
            if (products > 0 && Slider != null)
            {
                Slider.gameObject.SetActive(true);
                Slider.transform.localScale = new Vector3(-Mathf.Abs(Slider.transform.localScale.x), Slider.transform.localScale.y, Slider.transform.localScale.z);
                Slider.value = 0;
                harvestingTime = harvestTime;
                isHarvesting = true;
            }

            yield return new WaitForSeconds(harvestTime);

            if (products > FarmHouseDeposit.CurrentProducts)
                products = FarmHouseDeposit.CurrentProducts;

            CurrentProducts = products;
            FarmHouseDeposit.RemoveProducts(products);
            _animator.SetBool(_idleAnimation, false);
            _animator.SetBool(_walkAnimation, true);

            RotateFarmer(1);
            ChangeGoal();

            MoveFarmer(WarehouseLocation);
        }

        protected override void Deposit()
        {
            if (CurrentProducts <= 0)
            {
                RotateFarmer(-1);
                ChangeGoal();
                MoveFarmer(FarmHouseDepositLocation);
                return;
            }

            _animator.SetBool(_walkAnimation, false);
            _animator.SetBool(_idleAnimation, true);
            float depositTime;

            if (AdsManager.Instance.boostActive)
                depositTime = (float)((CurrentProducts / 2) / HarvestPerSecond);
            else
                depositTime = (float)(CurrentProducts / HarvestPerSecond);

            StartCoroutine(IEDeposit(depositTime));
        }

        protected override IEnumerator IEDeposit(float depositTime)
        {
            if (CurrentProducts > 0 && Slider != null)
            {
                Slider.gameObject.SetActive(true);
                Slider.transform.localScale = new Vector3(Mathf.Abs(Slider.transform.localScale.x), Slider.transform.localScale.y, Slider.transform.localScale.z);
                Slider.value = 0;
                harvestingTime = depositTime;
                isHarvesting = true;
            }

            yield return new WaitForSeconds(depositTime);

            MoneyManager.Instance.AddMoney(CurrentProducts);
            CurrentProducts = 0;

            RotateFarmer(-1);
            ChangeGoal();
            MoveFarmer(FarmHouseDepositLocation);
        }
    }
}