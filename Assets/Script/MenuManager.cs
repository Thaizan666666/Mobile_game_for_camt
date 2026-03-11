using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void StartGame()
    {
        SoundManager.StopAllLoops();
        SoundManager.PlayGameMusic();
        SceneManager.LoadScene("InGame");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    public void MainMenu()
    {
        SoundManager.StopAllLoops();
        Debug.Log("MainMaun");
        SceneManager.LoadScene("Menu");
    }
    public void WinSecen()
    {
        SoundManager.StopAllLoops();
        SoundManager.PlayWinMusic();
        Debug.Log("MainMaun");
        SceneManager.LoadScene("Win");
    }
}