using UnityEngine;
using UnityEngine.SceneManagement;

//code borrowed and modified by Hooson on youtube https://www.youtube.com/watch?v=tfzwyNS1LUY
public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject mainMenu;

    private int sceneToContinue;
    private int currentSceneIndex;
    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Home()
    {
        Time.timeScale = 1f;
        mainMenu.SetActive(true);
    }
}
