using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Debug.Log("����� �� ����...");
        Application.Quit();
    }

    public void OpenOptions()
    {
        Debug.Log("�������� ��������...");
    }
}

