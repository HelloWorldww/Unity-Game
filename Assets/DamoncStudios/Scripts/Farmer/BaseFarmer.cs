using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.DamoncStudios.Scripts
{
    public class BaseFarmer : MonoBehaviour, IClickable
    {
        [SerializeField] protected float moveSpeed;

        [SerializeField] protected GameObject basket;
        [SerializeField] protected List<GameObject> basketProducts;
        [SerializeField] protected GameObject hoe;
        [SerializeField] protected GameObject trolleyCartEmpty;
        [SerializeField] protected GameObject trolleyCart;
        [SerializeField] protected GameObject package;
        [SerializeField] protected GameObject package2;
        [SerializeField] protected GameObject package3;

        [Header("Initial Values")]
        [SerializeField] private double initialHarvestCapacity;
        [SerializeField] private float initialHarvestPerSecond;

        [SerializeField] private Slider slider;

        public double CurrentProducts { get; set; }
        public double HarvestCapacity { get; set; }
        public float HarvestPerSecond { get; set; }
        public bool isTimeToHarvest { get; set; }
        public bool FarmerClicked { get; set; }
        public float MoveSpeed { get; set; }

        public List<GameObject> BasketProducts => basketProducts;

        public Slider Slider => slider;

        protected Animator _animator;

        public bool speedBoost, perSecondBoost;
        public float boostValue;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            isTimeToHarvest = true;

            HarvestCapacity = initialHarvestCapacity;
            HarvestPerSecond = initialHarvestPerSecond;
            MoveSpeed = moveSpeed;
        }

        protected virtual void MoveFarmer(Vector3 newPosition)
        {
            float speed;

            if (speedBoost)
                speed = MoveSpeed / boostValue;
            else
                speed = MoveSpeed;

            transform.DOMove(newPosition, speed).SetEase(Ease.Linear).OnComplete((() =>
            {
                if (isTimeToHarvest)
                {
                    Harvest();
                }
                else
                {
                    Deposit();
                }
            })).Play();
        }

        protected virtual void Harvest()
        {

        }

        protected virtual IEnumerator IEHarvest(double products, float harvestTime)
        {
            yield return null;
        }

        protected virtual void Deposit()
        {

        }

        protected virtual IEnumerator IEDeposit(float depositTime)
        {
            yield return null;
        }

        protected void ChangeGoal()
        {
            isTimeToHarvest = !isTimeToHarvest;
        }

        public void ShowCartItem(bool value)
        {
            if (package != null && package2 != null && package3 != null)
            {
                package.SetActive(value);
                package3.SetActive(value);
                package2.SetActive(value);
            }
        }

        protected void RotateFarmer(int direction)
        {
            if (direction == 1)
            {
                if (basket != null)
                    basket.SetActive(false);
                if (hoe != null)
                    hoe.SetActive(true);

                if (trolleyCartEmpty != null)
                    trolleyCartEmpty.SetActive(false);
                if (trolleyCart != null)
                {
                    if (CurrentProducts > 0)
                        trolleyCart.SetActive(true);
                    else
                        trolleyCartEmpty.SetActive(true);
                }

                transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                if (basket != null)
                    basket.SetActive(true);
                if (hoe != null)
                    hoe.SetActive(false);

                if (trolleyCart != null)
                    trolleyCart.SetActive(false);
                if (trolleyCartEmpty != null)
                    trolleyCartEmpty.SetActive(true);

                transform.localScale = new Vector3(-1, 1, 1);
            }
        }

        public virtual void OnClick()
        {

        }

        /*private void OnMouseDown()
        {
            if (!FarmerClicked)
            {
                OnClick();
                FarmerClicked = true;
            }
        }*/
    }
}