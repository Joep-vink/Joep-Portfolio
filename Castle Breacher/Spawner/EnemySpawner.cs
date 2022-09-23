using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public waves[] Waves;
    private int _lvl = 0;

    [Range(0.1f, 4)]
    public float MinTimeBetweenCardSpawn;
    [Range(0.1f, 4)]
    public float MaxTimeBetweenCardSpawn;

    public ObjectPool[] Pools;    //All the objectPools

    [Header("Private variables")]
    private int _index;           //Index of the wave its gonna spawn
    private WaveSO _currWave;     //The current wave 
    private float[] _itemWeights; //All the percentages of the spawnchanges

    private void Start()
    {
        _itemWeights = Waves.Select(item => item.rate).ToArray(); //Save all the values

        for (int i = 0; i < transform.childCount; i++)
        {
            Pools[i] = transform.GetChild(i).GetComponent<ObjectPool>();
        }

        StartCoroutine(SpawnWave());
    }

    /// <summary>
    /// Gets a random item from the list
    /// </summary>
    /// <param name="itemWeights"></param>
    /// <returns></returns>
    private int GetRandomWeightedIndex(float[] itemWeights)
    {
        float sum = 0f;
        for (int i = 0; i < itemWeights.Length; i++)
        {
            sum += itemWeights[i];
        }
        float randomValue = Random.Range(0, sum);
        float tempSum = 0;

        for (int i = 0; i < Waves.Length; i++)
        {
            if (randomValue >= tempSum && randomValue < tempSum + itemWeights[i])
            {
                return i;
            }
            tempSum += itemWeights[i];
        }
        return 0;
    }

    private IEnumerator SpawnWave()
    {
        while (true)
        {
            //Get a wave to spawn 
            _index = GetRandomWeightedIndex(_itemWeights);
            _currWave = Waves[_index].miniWave;

            for (int i = 0; i < _currWave.wave[0].agents.Length; i++)
            {
                Pools[_index].objectToSpawn = _currWave.wave[0].agents[i]; //set the spawn object for the objectPool
                var obj = Pools[_index].SpawnObject(); //Soawn the object

                yield return new WaitForSeconds(_currWave.wave[0].timeBetweenSpawn);
            }

            yield return new WaitForSeconds(Random.Range(MinTimeBetweenCardSpawn, MaxTimeBetweenCardSpawn));
        }
    }
}

[System.Serializable]
public struct waves
{
    [Range(0, 1)]
    public float rate;       //The change of the wave to be chosen
    public WaveSO miniWave;  //The wave
}
