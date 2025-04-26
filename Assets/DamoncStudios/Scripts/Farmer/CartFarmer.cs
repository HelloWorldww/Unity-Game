using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.DamoncStudios.Scripts
{
    public class CartFarmer : BaseFarmer
    {
        [SerializeField] private FarmHouseManager farmHouse;
        public Vector3 DepositLocation => new Vector3(transform.position.x, farmHouse.DepositLocation.position.y);

        private Deposit _currentShaftDeposit;
        private int _currentShaftIndex = -1;

        bool started, taskDone = false;

        bool isHarvesting = false;
        float harvestingTimeRemaining = 0;
        float harvestingTime = 0;

        private void Update()
        {
            try
            {
                if (DataManager.Instance.CanStartCart() && !started)
                {
                    started = true;
                    MoveToNextLocation();
                }
            }
            catch {
            }

            if (started && taskDone)
                if (CanStartCart())
                {
                    MoveToNextLocation();
                }

            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            //   MoveToNextLocation();
            //}

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

        private void MoveToNextLocation()
        {
            taskDone = false;
            _currentShaftIndex++;
            Shaft currentShaft = ShaftManager.Instance.Shafts[_currentShaftIndex];
            _currentShaftDeposit = currentShaft.ShaftDeposit;
            Vector3 shaftDepositPos = currentShaft.DepositLocation.position;
            Vector3 fixedPos = new Vector3(transform.position.x, shaftDepositPos.y - 0.235f);
            MoveFarmer(fixedPos);
        }

        protected override void Harvest()
        {
            if (_currentShaftIndex == ShaftManager.Instance.Shafts.Count - 1 && !_currentShaftDeposit.CanCollectProducts)
            {
                ChangeGoal();
                MoveFarmer(DepositLocation);
                _currentShaftIndex = -1;
                return;
            }

            double amountToCollect = _currentShaftDeposit.CollectProducts(this);

            if (AdsManager.Instance.boostActive)
                if (_currentShaftDeposit.CollectProducts(this) * 2 > _currentShaftDeposit.CurrentProducts)
                {
                    amountToCollect = _currentShaftDeposit.CurrentProducts;
                }
                else
                {
                    amountToCollect = _currentShaftDeposit.CollectProducts(this) * 2;
                }

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

        protected override IEnumerator IEHarvest(double products, float collectTime)
        {
            if (products > 0 && Slider != null)
            {
                Slider.gameObject.SetActive(true);
                Slider.value = 0;
                harvestingTime = collectTime;
                isHarvesting = true;
            }

            yield return new WaitForSeconds(collectTime);

            if (AdsManager.Instance.boostActive)
            {
                CurrentProducts += products;
                _currentShaftDeposit.RemoveProducts(products);
            }
            else
            {
                if (CurrentProducts < HarvestCapacity && products <= (HarvestCapacity - CurrentProducts))
                {
                    CurrentProducts += products;
                    _currentShaftDeposit.RemoveProducts(products);
                }
            }


            if (products > 0)
                ShowCartItem(true);

            yield return new WaitForSeconds(0.5f);

            if (CurrentProducts < HarvestCapacity && _currentShaftIndex != ShaftManager.Instance.Shafts.Count - 1)
            {
                MoveToNextLocation();
            }
            else
            {
                ChangeGoal();
                MoveFarmer(DepositLocation);
                _currentShaftIndex = -1;
            }
        }

        protected override void Deposit()
        {
            if (CurrentProducts <= 0)
            {
                ChangeGoal();
                MoveToNextLocation();
                return;
            }

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
                Slider.value = 0;
                harvestingTime = depositTime;
                isHarvesting = true;
            }

            yield return new WaitForSeconds(depositTime);

            farmHouse.FarmHouseDeposit.DepositProducts(CurrentProducts);

            ShowCartItem(false);

            CurrentProducts = 0;

            ChangeGoal();

            taskDone = true;

            DataManager.Instance.SaveUserProfile();
        }

        public bool CanStartCart()
        {
            foreach (Shaft shaft in ShaftManager.Instance.Shafts)
            {
                if (shaft.ShaftDeposit.CurrentProducts > 0f)
                {
                    return true;
                }
            }
            return false;
        }
    }
}