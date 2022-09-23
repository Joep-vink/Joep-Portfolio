using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefab;
    [SerializeField] private List<GameObject> SpawnPoints;
    [SerializeField] public int count = 20;
    [SerializeField] private float minDelay = 0.8f, maxDelay = 1.5f;

    private RoomCenter room;

    IEnumerator SpawnCorotoutine()
    {
        while (count > 0)
        {
            count--;
            var randomIndex = Random.Range(0, SpawnPoints.Count);

            var randomOffset = Random.insideUnitCircle;
            var spawnPoint = SpawnPoints[randomIndex].transform.position + (Vector3)randomOffset;

            SpawnEnemy(spawnPoint);
            
            var randomTime = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(randomTime);
        }
    }

    private void SpawnEnemy(Vector3 spawnPoint)
    {
        GameObject go = Instantiate(enemyPrefab[Random.Range(0, enemyPrefab.Length)], spawnPoint, Quaternion.identity);
        room.enemies.Add(go);
    }

    private void Start()
    {
        room = GetComponentInParent<RoomCenter>();

        StartCoroutine(SpawnCorotoutine());

        /*if (SpawnPoints.Count > 0)
        {
            foreach (var spawnpoint in SpawnPoints)
            {
                SpawnEnemy(spawnpoint.transform.position);
            }
        }*/
    }
}
