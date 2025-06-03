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
        // ������� ������ �� ����������� ��� ��������
        if (backgroundMusic != null)
        {
            if (FindObjectsOfType<LoadingScreen>().Length > 1)
            {
                Destroy(gameObject); // ���� �������� � �������
                return;
            }

            DontDestroyOnLoad(backgroundMusic.gameObject);
        }
    }

    void Start()
    {
        // ��������� Fade-In (�� ������� � ����������)
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

        // ��������� �� ��������
        yield return new WaitForSeconds(waitBeforeLoad);

        // ������ fade-out + ��������
        StartCoroutine(LoadSceneRoutine());
    }

    IEnumerator LoadSceneRoutine()
    {
        float t = 0f;

        // ������� ����������
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeCanvasGroup.alpha = t / fadeDuration;
            yield return null;
        }

        // ������ ������� ������, ���� ����
        if (backgroundMusic != null)
        {
            yield return StartCoroutine(FadeOutMusic(backgroundMusic, musicFadeDuration));
        }

        // ������ �����
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
