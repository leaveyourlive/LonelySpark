using UnityEngine;
using UnityEngine.UI;

public class StatsPanelController : MonoBehaviour
{
    [Header("Attribute Texts")]
    public Text vigorText;        // Отображает Vigor
    public Text strengthText;     // Отображает Strength
    public Text enduranceText;    // Отображает Endurance
    public Text agilityText;      // Отображает Agility
    public Text willpowerText;    // Отображает Willpower
    public Text intelligenceText; // Отображает Intelligence
    public Text faithText;        // Отображает Faint

    [Header("Resource Sliders")]
    public Text currentToMaxHealth;
    public Text currentToMaxStamina;
    public Text currentToMaxMana;

    private PlayerStats stats;

    private void Awake()
    {
        // Получаем Singleton-инстанс
        stats = PlayerStats.Instance;
    }

    private void Update()
    {
        if (isActiveAndEnabled)
        {
            RefreshStatsDisplay();
        }
    }

    // // Когда панель включается в иерархии (через SetActive(true))
    // private void OnEnable()
    // {
    //     RefreshStatsDisplay();
    // }

    /// <summary>
    /// Обновляет все надписи и ползунки в панели
    /// </summary>
    public void RefreshStatsDisplay()
    {
        // Атрибуты
        vigorText.text        = $"Vigor: {stats.Vigor}";
        strengthText.text     = $"Strength: {stats.Strength}";
        enduranceText.text    = $"Endurance: {stats.Endurance}";
        agilityText.text      = $"Agility: {stats.Agility}";
        willpowerText.text    = $"Willpower: {stats.Willpower}";
        intelligenceText.text = $"Intelligence: {stats.Intelligence}";
        faithText.text        = $"Faint: {stats.Faith}";

        currentToMaxHealth.text  = $"{stats.currentHealth.ToString("N2")} / {stats.maxHealth}";
        currentToMaxStamina.text = $"{stats.currentStamina.ToString("N2")} / {stats.maxStamina}";
        currentToMaxMana.text    = $"{stats.currentMana.ToString("N2")} / {stats.maxMana}";
    }
}