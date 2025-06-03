using UnityEngine;
using System.Linq;

public class UIToggle : MonoBehaviour
{
    [SerializeField] private GameObject UIObject;
    [HideInInspector] public bool isOpened = false;
    [SerializeField] private InputAction action;
    [SerializeField] private GameObject[] PanelsToToggle;

    private PlayerAttack playerAttack;

    private void Start()
    {
        playerAttack = FindObjectOfType<PlayerAttack>();
        if (UIObject.activeSelf)
        {
            isOpened = true;
            UIBlocker.NotifyUIOpened();
        }
    }

    private void Update()
    {
        if (SettingsManager.GetKeyDown(action))
        {
            if (!isOpened) {
                Open();
            }
            else {
                Close();
            }
        }
    }

    private void Open()
    {
        // Сначала прячем остальные, чтобы они вызвали NotifyUIClosed
        foreach (var other in FindObjectsOfType<UIToggle>().Where(u => u != this && u.isOpened))
            other.Close();

        isOpened = true;
        UIObject.SetActive(true);
        UIBlocker.NotifyUIOpened();

        playerAttack.canDoShit = false;
        foreach (var panel in PanelsToToggle) {
            panel.SetActive(false);
        }
    }

    private void Close()
    {
        isOpened = false;
        UIObject.SetActive(false);
        UIBlocker.NotifyUIClosed();

        playerAttack.canDoShit = true;
        foreach (var panel in PanelsToToggle) {
            panel.SetActive(true);
        }
    }
}