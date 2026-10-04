using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject optionsMenu;
    public GameObject Menu;

    public void OpenOptionsPanel()
    {
        optionsMenu.SetActive(true);
        Menu.SetActive(false);
    }

    public void OpenMenuPanel()
    {
        optionsMenu.SetActive(false);
        Menu.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Hand Landmark Detection");
    }
}
