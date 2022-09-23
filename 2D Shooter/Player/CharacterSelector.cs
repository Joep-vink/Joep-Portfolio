using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    private bool canSelect;

    public GameObject message;

    public player playerToSpawn;

    public bool ShouldUnlock;

    private void Start()
    {
        if (ShouldUnlock)
        {
            if (PlayerPrefs.HasKey(playerToSpawn.name))
            {
                if (PlayerPrefs.GetInt(playerToSpawn.name) == 1)
                {
                    gameObject.SetActive(true);
                }
                else
                    gameObject.SetActive(false);
            }
            else
                gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (canSelect)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Vector3 playerPos = player.instance.transform.position;

                Destroy(player.instance.gameObject);

                player newPlayer = Instantiate(playerToSpawn, playerPos, playerToSpawn.transform.rotation);
                player.instance = newPlayer;

                gameObject.SetActive(false);
                canSelect = false;

                CameraController.instance.target = newPlayer.transform;

                CharacterSelectManager.instance.activePlayer = newPlayer;
                CharacterSelectManager.instance.activeCharSelect.gameObject.SetActive(true);
                CharacterSelectManager.instance.activeCharSelect = this;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            canSelect = true;
            message.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            canSelect = false;
            message.gameObject.SetActive(false);
        }
    }
}
