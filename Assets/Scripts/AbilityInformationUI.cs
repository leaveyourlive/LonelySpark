using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityInformationUI : MonoBehaviour
{
    [SerializeField] private GameObject combinationsPrefab; // ������ � ����� ��������
    [SerializeField] private Transform contentParent; // ������������ ������ Content (��� ScrollView)

    // private void Awake()
    // {
    //     var abilityList = AbilityInputManager.Instance.abilityList;
    //     foreach (var ability in abilityList)
    //     {
    //         GameObject newAbilityUI = Instantiate(combinationsPrefab, contentParent); // ������� ����� �������
    //         Text[] textComponents = newAbilityUI.GetComponentsInChildren<Text>(); // �������� ��� ���������� Text � �������

    //         if (textComponents.Length < 2)
    //         {
    //             Debug.LogError("� ������� ������ ���� ��� ���������� Text ��� ����� � ����������!");
    //             return;
    //         }

    //         // ��������� ������ ����� (��� �����������)
    //         textComponents[0].text = ability.name; // ������ ������ ��� ����� �����������

    //         // ��������� ������ ����� (����������)
    //         string translatedCombination = "";
    //         foreach (char c in ability.combination)
    //         {
    //             translatedCombination += AbilityNumberToKey(c);
    //         }
    //         textComponents[1].text = translatedCombination; // ������ ������ ��� ����������
    //         Canvas.ForceUpdateCanvases();
    //     }
    // }
    private void OnEnable()
    {
        var abilityList = AbilityInputManager.Instance.abilityList;

        // �������� ������������ ��������, ���� ��� ����
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject); // ������� ������ ��������
        }

        // ���������� ��� �������� ����� ��������� ��� ������ �����������
        foreach (var ability in abilityList)
        {
            GameObject newAbilityUI = Instantiate(combinationsPrefab, contentParent); // ������� ����� �������
            Text[] textComponents = newAbilityUI.GetComponentsInChildren<Text>(); // �������� ��� ���������� Text � �������

            if (textComponents.Length < 2)
            {
                Debug.LogError("� ������� ������ ���� ��� ���������� Text ��� ����� � ����������!");
                return;
            }

            // ��������� ������ ����� (��� �����������)
            textComponents[0].text = ability.name; // ������ ������ ��� ����� �����������

            // ��������� ������ ����� (����������)
            string translatedCombination = "";
            foreach (char c in ability.combination)
            {
                translatedCombination += AbilityNumberToKey(c);
            }
            textComponents[1].text = translatedCombination; // ������ ������ ��� ����������
            Canvas.ForceUpdateCanvases();
        }
    }

    // ����� ��� �������� ����� � �������
    string AbilityNumberToKey(char c)
    {
        switch (c)
        {
            case '1':
                return SettingsManager.GetKeyByAction(InputAction.AbilButton1).ToString();
            case '2':
                return SettingsManager.GetKeyByAction(InputAction.AbilButton2).ToString();
            case '3':
                return SettingsManager.GetKeyByAction(InputAction.AbilButton3).ToString();
            case '4':
                return SettingsManager.GetKeyByAction(InputAction.AbilButton4).ToString();
            default:
                return "";
        }
    }
}
