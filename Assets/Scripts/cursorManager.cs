using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance;
    private Camera mainCam;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        StartCoroutine(DelayedFindCamera()); // Ожидаем пару кадров перед поиском камеры
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(DelayedFindCamera()); // Обновляем камеру после смены сцены
    }

    private IEnumerator DelayedFindCamera()
    {
        yield return new WaitForSeconds(0.1f); // Даем время сцене и объектам загрузиться

        mainCam = null; // Сбрасываем, чтобы пересканировать

        GameObject cameraObject = GameObject.FindGameObjectWithTag("MainCamera");

        if (cameraObject != null && cameraObject.activeInHierarchy)
        {
            mainCam = cameraObject.GetComponent<Camera>();
        }
        else
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                mainCam = player.GetComponentInChildren<Camera>(true); // true ищет и в отключённых объектах
            }
        }

        if (mainCam == null)
        {
            Debug.LogError("Ошибка: Камера с тегом 'MainCamera' не найдена!");
        }
        else
        {
            Debug.Log($"Камера найдена: {mainCam.name}");
        }
    }

    void Update()
    {
        if (mainCam == null) return;

        CursorManagement();
    }

    private void CursorManagement()
    {
        if (mainCam != null)
        {
            Vector3 targetPosition = mainCam.ScreenToWorldPoint(Input.mousePosition);
            targetPosition.z = 0f;
            transform.position = targetPosition;
        }
    }
}
