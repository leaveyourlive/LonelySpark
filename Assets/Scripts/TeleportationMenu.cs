using System.Collections;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TeleportationMenu : MonoBehaviour
{
    public GameObject menuPanel; // Ссылка на BackgroundPanel
    public Transform contentParent; // Ссылка на Content внутри Scroll View
    public GameObject campfireButtonPrefab; // Префаб кнопки костра

    private GameObject equipmentPanel;
    private static Vector3 targetPosition;

    public void OpenMenu()
    {
        menuPanel.SetActive(true);
        Time.timeScale = 0f;
        equipmentPanel = GameObject.FindObjectOfType<EquipmentPanel>().gameObject;
        equipmentPanel.SetActive(false);
        PopulateMenu();

        // Проверяем сцену, в которой находится игрок
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            string playerScene = player.scene.name;
            Debug.Log($"Игрок находится в сцене: {playerScene}");
        }
        else
        {
            Debug.LogWarning("Игрок не найден!");
        }
    }

    public void CloseMenu()
    {
        menuPanel.SetActive(false);
        Time.timeScale = 1f;
        equipmentPanel.SetActive(true);
    }

    void PopulateMenu()
    {
        // Очищаем старые элементы
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        foreach (var campfire in CampfireManager.Instance.activatedCampfires)
        {
            GameObject item = Instantiate(campfireButtonPrefab, contentParent);
            item.GetComponentInChildren<Text>().text = campfire.campfireName;

            // Добавляем действие на кнопку
            Button button = item.GetComponent<Button>();
            button.onClick.AddListener(() => OnCampfireSelected(campfire));
        }
    }

    void OnCampfireSelected(CampfireManager.CampfireData campfire)
    {
        CloseMenu();
        TeleportToCampfire(campfire);
    }

    void TeleportToCampfire(CampfireManager.CampfireData campfire)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        player.GetComponent<PlayerMovement>().enabled = true;

        if (player != null)
        {
            string playerScene = player.scene.name;
            Debug.Log($"Игрок сейчас в сцене: {playerScene}");

            if (campfire.sceneName != playerScene)
            {
                // Запускаем телепортацию с загрузкой сцены
                TeleportWithSceneLoad(campfire);
            }
            else
            {
                // Телепортируем внутри текущей сцены
                player.transform.position = campfire.position;
                Debug.Log($"Телепортирован к костру {campfire.campfireName} в сцене {campfire.sceneName}");
            }
        }
        else
        {
            Debug.LogWarning("Игрок не найден при попытке телепортации!");
        }
    }

    void TeleportWithSceneLoad(CampfireManager.CampfireData campfire)
    {
        // Запоминаем позицию для телепорта
        targetPosition = campfire.position;

        // Ищем игрока перед загрузкой сцены
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Ошибка: Игрок не найден перед загрузкой сцены!");
            return;
        }

        // Делаем игрока временно DontDestroyOnLoad
        DontDestroyOnLoad(player);

        // Подписываемся на событие загрузки сцены
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Загружаем новую сцену
        SceneManager.LoadScene(campfire.sceneName, LoadSceneMode.Single);
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // Перемещаем игрока в новую сцену
            player.transform.position = targetPosition;
            Debug.Log($"Игрок телепортирован в {scene.name} на {targetPosition}");

            // Удаляем DontDestroyOnLoad и привязываем игрока к текущей сцене
            SceneManager.MoveGameObjectToScene(player, scene);
            Debug.Log("Игрок теперь привязан к новой сцене и больше не DontDestroyOnLoad.");
        }
        else
        {
            Debug.LogError("Игрок не найден после загрузки сцены!");
        }

        // Отписываемся от события, чтобы не срабатывало постоянно
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

}