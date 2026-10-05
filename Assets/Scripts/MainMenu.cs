using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject MenuOpciones;
    public GameObject Menu;

    public void OpenOptionsPanel()
    {
        MenuOpciones.SetActive(true);
        Menu.SetActive(false);
    }

    public void OpenMenuPanel()
    {
        MenuOpciones.SetActive(false);
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
