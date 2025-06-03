using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInput : MonoBehaviour
{
    public static TestInput Instance {get; private set;}

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            Debug.Log("corner");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            Debug.Log("HUI");
            SaveSystem.SavePlayerData(PlayerStats.Instance);
        }
        if (Input.GetKeyDown(KeyCode.F6))
        {
            SaveSystem.LoadPlayerData(PlayerStats.Instance);
        }
        if (Input.GetKeyDown(KeyCode.F7))
        {
            FindObjectOfType<KeyRebinder>().GetComponent<KeyRebinder>().ResetToDefaults();
            Debug.Log("Default key settings");
        }
    }
}
