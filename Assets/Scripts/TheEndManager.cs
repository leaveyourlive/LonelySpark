using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private AudioSource backgroundMusic;

    private void Start()
    {
        PlayMusic();
    }

    private void PlayMusic()
    {
        if (backgroundMusic != null && !backgroundMusic.isPlaying)
        {
            backgroundMusic.Play();
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
