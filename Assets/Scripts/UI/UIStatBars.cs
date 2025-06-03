using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIStatBars : MonoBehaviour
{
    public Slider healthBar;
    public Slider staminaBar;
    public Slider manaBar;

    public PlayerStats stats;

    // private RectTransform rth;
    // private RectTransform rts;
    // private RectTransform rtm;

    // private void Start()
    // {
    //     rth = healthBar.GetComponentInParent<RectTransform>();
    //     rts = healthBar.GetComponentInParent<RectTransform>();
    //     rtm = healthBar.GetComponentInParent<RectTransform>();
    // }

    private void Update()
    {
        UpdateSliderValue();
        //UpdateBarSize();
    }

    private void UpdateSliderValue()
    {
        healthBar.value = stats.currentHealth / stats.maxHealth;
        healthBar.GetComponentInChildren<Text>().text = stats.currentHealth.ToString();

        staminaBar.value = stats.currentStamina / stats.maxStamina;
        staminaBar.GetComponentInChildren<Text>().text = stats.currentStamina.ToString();

        manaBar.value = stats.currentMana / stats.maxMana;
        manaBar.GetComponentInChildren<Text>().text = stats.currentMana.ToString();
    }

    // private void UpdateBarSize()
    // {

    // }
}