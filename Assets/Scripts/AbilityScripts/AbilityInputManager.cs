using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityInputManager : MonoBehaviour
{
    public static AbilityInputManager Instance {get; private set;}

    [System.Serializable]
    public class AbilityCombination
    {
        public string name;
        public string combination;
        public Sprite abilityIcon;
        public Ability ability;
    }
    public List<AbilityCombination> abilityList;

    [SerializeField] private float timeToMatchCombo = 2f;
    [SerializeField] private float comboHoldTime = 5f;
    [SerializeField] private float currentTimeout;
    //private bool abilityInputEnabled = true;
    private float lastInputTime;

    [SerializeField] private KeyCode confirmKey;
    [SerializeField] private KeyCode abilButton1;
    [SerializeField] private KeyCode abilButton2;
    [SerializeField] private KeyCode abilButton3;
    [SerializeField] private KeyCode abilButton4;
    private List<KeyCode> validKeys;

    [SerializeField] private bool canPressButtonsAfterAbilityFound = true;

    private void OnEnable()
    {
        SettingsManager.OnKeyBindChanged += UpdateKeys;
        UpdateKeys();
    }

    private void OnDisable()
    {
        SettingsManager.OnKeyBindChanged -= UpdateKeys;
    }

    private void UpdateKeys()
    {
        confirmKey = SettingsManager.GetKeyByAction(InputAction.ConfirmAbility);
        abilButton1 = SettingsManager.GetKeyByAction(InputAction.AbilButton1);
        abilButton2 = SettingsManager.GetKeyByAction(InputAction.AbilButton2);
        abilButton3 = SettingsManager.GetKeyByAction(InputAction.AbilButton3);
        abilButton4 = SettingsManager.GetKeyByAction(InputAction.AbilButton4);
        validKeys = new() {abilButton1, abilButton2, abilButton3, abilButton4, confirmKey};
    }

    private const int maxkeys = 4;
    private List<KeyCode> currentInput = new();
    private AbilityCombination currentAbility;
    public Image combinationIcon; // UI элемент картинки
    public Text inputText; // UI элемент

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 

            UpdateKeys();
            currentTimeout = timeToMatchCombo;
        }
        else { 
            Destroy(gameObject); 
        }
    }

    private void Update()
    {   
        if (currentInput.Count > 0 && Time.time > currentTimeout + lastInputTime)
        {
            ResetCombination();
        }

        if (Input.anyKeyDown)
        {
            foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key) && IsValidKey(key))
                {
                    if (key == confirmKey) // currentAbility != null уже в ActivateAbility();
                    {
                        ActivateAbility();
                        return;
                    }

                    //                                                                  
                    if (currentInput.Count < maxkeys && key != confirmKey && canPressButtonsAfterAbilityFound)
                    {
                        currentInput.Add(key);
                        Debug.Log($"добавлена кнопка {key}, размер массива {currentInput.Count}");
                        inputText.text += key.ToString();
                        lastInputTime = Time.time;
                        CheckCombination();
                    }
                }
            }
        }
    }

    private void ChangeTimeout()
    {
        currentTimeout = currentAbility != null ? comboHoldTime : timeToMatchCombo;
    }

    private bool IsValidKey(KeyCode key)
    {
        return validKeys.Contains(key) || key == confirmKey;
    }

    private void CheckCombination()
    {
        foreach (var abilityCombination in abilityList)
        {
            if (MatchesCombination(abilityCombination.combination))
            {
                currentAbility = abilityCombination;
                combinationIcon.sprite = abilityCombination.abilityIcon;
                ChangeTimeout();
                return;
            }
            else
            {
                currentAbility = null;
                combinationIcon.sprite = default;
                ChangeTimeout();
            }
        }
    }

    // private IEnumerator ComboFoundRoutine(AbilityCombination abilityCombination)
    // {
    //     currentAbility = abilityCombination;
    //     combinationIcon.sprite = abilityCombination.abilityIcon;

    //     yield return new WaitForSeconds(comboHoldTime);

    //     ResetCombination();
    // }

    private bool MatchesCombination(string combination)
    {
        if (currentInput.Count != combination.Length) 
            return false;
        
        for (int i = 0; i < combination.Length; i++)
        {
            //if (currentInput[i] != validKeys[combination[i]-1]) return false;
            
            // Получаем числовое значение символа
            if (!char.IsDigit(combination[i]))
            {
                Debug.LogError($"Недопустимый символ в комбинации: {combination[i]}");
                return false;
            }

            int buttonIndex = combination[i] - '1'; // Конвертируем '1' в 0, '2' в 1 и т.д.
            
            // Проверяем границы массива
            if (buttonIndex < 0 || buttonIndex >= validKeys.Count - 1) // -1 чтобы исключить confirmKey
            {
                Debug.LogError($"Неверный индекс кнопки: {buttonIndex}");
                return false;
            }

            // Сравниваем с текущим вводом
            if (currentInput[i] != validKeys[buttonIndex])
            {
                return false;
            }
        }
        
        return true;
    }

    private void ActivateAbility()
    {
        Debug.Log("Вызван ActivateAbility()");

        if (currentAbility != null && currentAbility.ability != null)
        {
            Debug.Log("Используется непустая способность");
            currentAbility.ability.Use();
            ResetCombination();
        }
        // else отодрать игрока в жопу
    }

    private void ResetCombination()
    {
        currentInput.Clear();
        inputText.text = "";
        currentAbility = null;
        combinationIcon.sprite = default;
        ChangeTimeout();
    }
}