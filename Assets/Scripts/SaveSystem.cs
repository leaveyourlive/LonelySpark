using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string playerDataPath = Application.persistentDataPath + "/playerdata.json";

    public static void SavePlayerData(PlayerStats playerStats)
    {
        PlayerData data = new PlayerData
        {
            Vigor = playerStats.Vigor,
            Strength = playerStats.Strength,
            Endurance = playerStats.Endurance,
            Agility = playerStats.Agility,
            Willpower = playerStats.Willpower,
            Intelligence = playerStats.Intelligence,
            Faith = playerStats.Faith,

            maxHealth = playerStats.maxHealth,
            maxStamina = playerStats.maxStamina,
            maxMana = playerStats.maxMana,
            currentHealth = playerStats.currentHealth,
            currentStamina = playerStats.currentStamina,
            currentMana = playerStats.currentMana,
            // staminaRegenRate = playerStats.staminaRegenRate,
            // canRegenerateStamina = playerStats.canRegenerateStamina,

            money = playerStats.money,
            armor = playerStats.armor,
        };

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(playerDataPath, json);
    }

    public static void LoadPlayerData(PlayerStats playerStats)
    {
        if (File.Exists(playerDataPath))
        {
            string json = File.ReadAllText(playerDataPath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);

            playerStats.Vigor = data.Vigor;
            playerStats.Strength = data.Strength;
            playerStats.Endurance = data.Endurance;
            playerStats.Agility = data.Agility;
            playerStats.Willpower = data.Willpower;
            playerStats.Intelligence = data.Intelligence;
            playerStats.Faith = data.Faith;

            playerStats.maxHealth = data.maxHealth;
            playerStats.maxStamina = data.maxStamina;
            playerStats.maxMana = data.maxMana;
            playerStats.currentHealth = data.currentHealth;
            playerStats.currentStamina = data.currentStamina;
            playerStats.currentMana = data.currentMana;
            // playerStats.staminaRegenRate = data.staminaRegenRate;
            // playerStats.canRegenerateStamina = data.canRegenerateStamina;

            playerStats.money = data.money;
            playerStats.armor = data.armor;
        }
        else
        {
            Debug.LogWarning("Файл данных игрока не найден!");
        }
    }
}