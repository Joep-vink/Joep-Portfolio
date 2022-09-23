using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    public GameObject pauseMenu, mapDisplay, bigMapText, deathMenu, Cameras;

    public Image currentGun;
    public TextMeshProUGUI gunName;

    public string mainMenuScene;

    private void Awake()
    {
        instance = this;
        Cameras = GameObject.Find("Cameras");
    }

    public void ReturnMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
        Destroy(player.instance.gameObject);
        Destroy(CameraController.instance.gameObject);
        Destroy(UIController.instance.Cameras);
    }

    public void Resume()
    {
        GameManager.instance.PauseUnpause();
    }
}
