using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public PlayerStats playerStats;
    public InventoryManager inventoryManager;
    // Другие менеджеры (синглтоны)

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            SaveSystem.LoadPlayerData(playerStats);
            //LoadInventory
        }
        else
        {
            Destroy(gameObject);
        }
    }
}