using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyItem : Item
{
    [SerializeField] private int moneyValue;
    public override void Activate()
    {
        PlayerStats.Instance.AddMoney(moneyValue);
    }
}