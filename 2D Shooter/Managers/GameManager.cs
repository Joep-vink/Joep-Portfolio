using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private Texture2D corsorTexture = null;

    public float waitToLoad = 4f;
    public string nextLevel;
    public bool isPaused = false, canShoot = true, isAllowedToShoot = true, isBossRoom = false;

    public int coins;

    public Transform startPoint;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //Player setup
        player.instance.UiHealth = FindObjectOfType<UIHealth>();
        CharacterTracker.instance.maxHealth = player.instance.maxHealth;
        CharacterTracker.instance.currentHealth = player.instance.health;

        player.instance.UiHealth.Initialize(player.instance.maxHealth);
        player.instance.UiHealth.UpdateUI(player.instance.health);

        PlayerWeapon.instance.UIAmmo = GetComponentInChildren<UIAmmo>();
        PlayerWeapon.instance.SwitchGun();

        coins = CharacterTracker.instance.currentCoins;
        UICoins.instance.UpdateCoins(coins);

        player.instance.transform.position = startPoint.position;

        //Camera
        AgentInput.instance.mainCamera = Camera.main;
        CameraController.instance.gameObject.transform.position = new Vector3(startPoint.position.x, startPoint.position.y, -10);

        //Manager
        Time.timeScale = 1;

        SetCursorIcon();

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("LevelBoss"))
        {
            isBossRoom = true;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            DeleteSave();
        }
    }

    private void SetCursorIcon()
    {
        Cursor.SetCursor(corsorTexture, new Vector2(corsorTexture.width / 2f, corsorTexture.height / 2f), CursorMode.Auto);
    }

    public void RestartLevel()
    {
        DOTween.KillAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GetCoins(float amount)
    {
        int i = (int)amount;
        coins += i;
        UICoins.instance.UpdateCoins(coins);
    }

    public void SpendCoins(int amount)
    {
        coins -= amount;
        UICoins.instance.UpdateCoins(coins);

        if (coins < 0)
        {
            coins = 0;
        }
    }

    public void PauseUnpause()
    {
        if (!isPaused)
        {
            UIController.instance.pauseMenu.SetActive(true);

            isPaused = true;

            Time.timeScale = 0;
        }
        else
        {
            UIController.instance.pauseMenu.SetActive(false);

            isPaused = false;

            Time.timeScale = 1;
        }
    }

    public IEnumerator LevelEnd()
    {
        isAllowedToShoot = false;

        yield return new WaitForSeconds(waitToLoad);

        CharacterTracker.instance.currentCoins = coins;
        CharacterTracker.instance.currentHealth = player.instance.health;
        CharacterTracker.instance.maxHealth = player.instance.maxHealth;

        SceneManager.LoadScene(nextLevel);
    }

    public void DeleteSave()
    {
        PlayerPrefs.DeleteAll();
    }
}
