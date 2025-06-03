using System;
using System.IO;
using System.Linq;
using UnityEngine;

public static class SettingsManager
{
    private static string SettingsPath = Application.persistentDataPath + "/keybinds.json";

    private static KeyBindSettings _settings;

    public static KeyBindSettings Settings
    {
        get
        {
            if (_settings == null) LoadSettings();
            return _settings;
        }
    }

    public static void LoadSettings()
    {
        if (File.Exists(SettingsPath))
        {
            string json = File.ReadAllText(SettingsPath);
            _settings = JsonUtility.FromJson<KeyBindSettings>(json);
        }
        else
        {
            _settings = new KeyBindSettings();
        }
    }

    public static event Action OnKeyBindChanged;

    public static void SaveSettings()
    {
        string json = JsonUtility.ToJson(_settings);
        File.WriteAllText(SettingsPath, json);

        OnKeyBindChanged?.Invoke();
    }

    public static bool GetKeyDown(InputAction action)
    {
        var bind = Settings.keyBinds.FirstOrDefault(b => b.action == action);
        return bind != null && Input.GetKeyDown(bind.keyCode);
    }

    public static bool GetKey(InputAction action)
    {
        var bind = Settings.keyBinds.FirstOrDefault(b => b.action == action);
        return bind != null && Input.GetKey(bind.keyCode);
    }

    public static bool GetKeyUp(InputAction action)
    {
        var bind = Settings.keyBinds.FirstOrDefault(b => b.action == action);
        return bind != null && Input.GetKeyUp(bind.keyCode);
    }

    public static KeyCode GetKeyByAction(InputAction action)
    {
        return Settings.keyBinds.FirstOrDefault(b => b.action == action).keyCode;
    }
}