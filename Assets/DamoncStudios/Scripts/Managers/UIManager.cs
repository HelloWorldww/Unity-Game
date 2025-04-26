using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.DamoncStudios.Scripts
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI totalMoneyTMP;
        [SerializeField] private Sprite[] warehouseSprites;
        [SerializeField] private GameObject warehouse;

        void Update()
        {
            totalMoneyTMP.text = MoneyManager.Instance.CurrentMoney.CurrencyText();

            if (MoneyManager.Instance.CurrentMoney < 1)
            {
                warehouse.GetComponent<SpriteRenderer>().sprite = warehouseSprites[0];
            }
            else
            {
                warehouse.GetComponent<SpriteRenderer>().sprite = warehouseSprites[1];
            }
        }
    }
}