using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenOptions()
    {

    }
}
