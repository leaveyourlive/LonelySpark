using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyUI : MonoBehaviour
{
    [SerializeField] private Text MoneyUIText;

    private void Update()
    {
        MoneyUIText.text = PlayerStats.Instance.money.ToString();
    }
}
