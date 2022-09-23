using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCenter : MonoBehaviour
{
    public bool openWhenEnemiesCleared;

    public List<GameObject> enemies = new List<GameObject>();

    public Room theRoom;
    public EnemySpawner spawnEnemies;

    private void Start()
    {
        spawnEnemies = GetComponentInChildren<EnemySpawner>();

        if (openWhenEnemiesCleared)
        {
            theRoom.closeWhenEntered = true;
        }
    }

    private void Update()
    {
        if (theRoom.roomActive && openWhenEnemiesCleared)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i] == null)
                {
                    enemies.RemoveAt(i);
                    i--;
                }
            }
        }

        if (theRoom.roomActive)
        {
            if (enemies.Count == 0 && openWhenEnemiesCleared)
            {
                theRoom.OpenDoors();
                theRoom.roomActive = false;
            }
        }
    }
}
