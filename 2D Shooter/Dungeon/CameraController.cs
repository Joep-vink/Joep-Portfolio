using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public float moveSpeed;

    public Transform target;

    public Camera mainCam, bigMapCam;

    public bool bigMapActive;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        target = FindObjectOfType<player>().transform;
    }

    private void Update()
    {
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, target.position.y, transform.position.z), moveSpeed);
        }

        if (Input.GetKeyDown(KeyCode.M) && !GameManager.instance.isBossRoom)
        {
            if (!bigMapActive)
            {
                ActivateBigMap();
            }
            else
            {
                DeactivateBigMap();
            }
        }
    }

    public void ChangeTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void ActivateBigMap()
    {
        if (!GameManager.instance.isPaused)
        {
            bigMapActive = true;

            bigMapCam.enabled = true;
            mainCam.enabled = false;

            GameManager.instance.canShoot = false;

            Time.timeScale = 0;

            UIController.instance.mapDisplay.SetActive(false);
            UIController.instance.bigMapText.SetActive(true);
        }
    }

    public void DeactivateBigMap()
    {
        if (!GameManager.instance.isPaused)
        {
            bigMapActive = false;

            bigMapCam.enabled = false;
            mainCam.enabled = true;

            GameManager.instance.canShoot = true;

            Time.timeScale = 1;

            UIController.instance.mapDisplay.SetActive(true);
            UIController.instance.bigMapText.SetActive(false);
        }
    }
}

