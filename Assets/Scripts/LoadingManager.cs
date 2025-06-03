using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingScreen : MonoBehaviour
{
    public CanvasGroup fadeCanvasGroup;
    public float waitBeforeLoad = 5f;
    public float fadeDuration = 2f;
    public string sceneToLoad = "GameScene";

    public AudioSource backgroundMusic;
    public float musicFadeDuration = 1.5f;

    void Awake()
    {
        // Защитим музыку от уничтожения при переходе
        if (backgroundMusic != null)
        {
            if (FindObjectsOfType<LoadingScreen>().Length > 1)
            {
                Destroy(gameObject); // если дубликат — убираем
                return;
            }

            DontDestroyOnLoad(backgroundMusic.gameObject);
        }
    }

    void Start()
    {
        // Начальный Fade-In (из черного в прозрачное)
        StartCoroutine(FadeInRoutine());
    }

    IEnumerator FadeInRoutine()
    {
        float t = fadeDuration;
        while (t > 0f)
        {
            t -= Time.deltaTime;
            fadeCanvasGroup.alpha = t / fadeDuration;
            yield return null;
        }

        fadeCanvasGroup.alpha = 0f;

        // Подождали до загрузки
        yield return new WaitForSeconds(waitBeforeLoad);

        // Теперь fade-out + загрузка
        StartCoroutine(LoadSceneRoutine());
    }

    IEnumerator LoadSceneRoutine()
    {
        float t = 0f;

        // Плавное затемнение
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeCanvasGroup.alpha = t / fadeDuration;
            yield return null;
        }

        // Плавно затушим музыку, если есть
        if (backgroundMusic != null)
        {
            yield return StartCoroutine(FadeOutMusic(backgroundMusic, musicFadeDuration));
        }

        // Грузим сцену
        SceneManager.LoadScene(sceneToLoad);
    }

    IEnumerator FadeOutMusic(AudioSource audioSource, float duration)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0f)
        {
            audioSource.volume -= startVolume * Time.deltaTime / duration;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}
