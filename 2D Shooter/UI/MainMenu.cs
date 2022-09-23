using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string levelLoad;

    public void StartGame()
    {
        SceneManager.LoadScene(levelLoad);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
