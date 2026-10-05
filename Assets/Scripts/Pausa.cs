using UnityEngine;
using UnityEngine.SceneManagement;  

public class Pausa : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject pauseBoton;

    public void PauseGame()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        pauseBoton.SetActive(false);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        pauseBoton.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale= 1; 
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
