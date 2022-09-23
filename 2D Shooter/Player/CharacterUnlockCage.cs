using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterUnlockCage : MonoBehaviour
{
    private bool canUnlock = false;
    public GameObject message;

    [SerializeField] private CharacterSelector[] charSelects;
    private CharacterSelector playerToUnlock;

    public SpriteRenderer cagedSR;

    private void Start()
    {
        playerToUnlock = charSelects[Random.Range(0, charSelects.Length)];

        cagedSR.sprite = playerToUnlock.playerToSpawn.GetComponentInChildren<SpriteRenderer>().sprite;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canUnlock)
        {
            PlayerPrefs.SetInt(playerToUnlock.playerToSpawn.name, 1);

            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            canUnlock = true;
            message.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            canUnlock = false;
            message.SetActive(false);
        }
    }
}
