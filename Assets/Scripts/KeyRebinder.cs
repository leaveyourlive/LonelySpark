using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class KeyRebinder : MonoBehaviour
{
    [System.Serializable]
    public class KeyBindUI
    {
        public InputAction action;
        public Text actionNameText;
        public Text currentKeyText;
        public Button rebindButton;
    }

    public KeyBindUI[] keyBindUI;
    public GameObject rebindOverlay;

    private InputAction currentRebindingAction;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        InitializeUI();
        UpdateAllUI();
    }

    void InitializeUI()
    {
        foreach (var uiElement in keyBindUI)
        {
            uiElement.rebindButton.onClick.AddListener(() => 
                StartRebind(uiElement.action));
        }
    }

    void UpdateAllUI()
    {
        foreach (var uiElement in keyBindUI)
        {
            var bind = SettingsManager.Settings.keyBinds
                .FirstOrDefault(b => b.action == uiElement.action);
            
            if (bind != null)
            {
                uiElement.currentKeyText.text = bind.keyCode.ToString();
                uiElement.actionNameText.text = uiElement.action.ToString();
            }
        }
    }

    public void StartRebind(InputAction action)
    {
        currentRebindingAction = action;
        StartCoroutine(RebindKeyCoroutine());
    }

    private IEnumerator RebindKeyCoroutine()
    {
        rebindOverlay.SetActive(true);
        yield return null; // Пропускаем текущий кадр

        while (true)
        {
            if (Input.anyKeyDown)
            {
                foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(keyCode) && keyCode != KeyCode.Mouse0)
                    {
                        var existingBind = SettingsManager.Settings.keyBinds
                            .FirstOrDefault(b => b.keyCode == keyCode);

                        if (existingBind != null)
                        {
                            // Обработка конфликта клавиш
                            Debug.Log($"Key {keyCode} уже используется для {existingBind.action}");
                            rebindOverlay.SetActive(false);
                            yield break;
                        }

                        var bind = SettingsManager.Settings.keyBinds
                            .FirstOrDefault(b => b.action == currentRebindingAction);
                        
                        if (bind != null)
                        {
                            bind.keyCode = keyCode;
                            SettingsManager.SaveSettings();
                            UpdateAllUI();
                        }

                        rebindOverlay.SetActive(false);
                        yield break;
                    }
                }
            }
            yield return null;
        }
    }

    public void ResetToDefaults()
    {
        SettingsManager.Settings.SetDefault();
        SettingsManager.SaveSettings();
        UpdateAllUI();
    }
}