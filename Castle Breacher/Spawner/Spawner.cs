using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private ObjectPool pool;

    public SpawnerInfo spawnInfo;
    public SpawnManager sManager;

    public List<WaveSO> waves = new List<WaveSO>();

    private WaveSO currWave;

    private void Start()
    {
        sManager = GetComponentInParent<SpawnManager>();
        pool = GetComponent<ObjectPool>();
    }

    private void Update()
    {
        if (waves.Count > 0 && sManager.AllWaves[0] == waves[0] && !sManager.CanSpawn)
            StartCoroutine(BetweenWaves());
    }

    protected virtual IEnumerator BetweenWaves()
    {
        currWave = waves[0];
        sManager.CanSpawn = true;

        StartCoroutine(WaveSpawner());
        yield return null;
    }

    protected virtual IEnumerator WaveSpawner()
    {
        //int currPhase = 0; //only use this when using the mini wave system 
        WaveData wave = currWave.wave[0];

        while (sManager.CanSpawn)
        {
            for (int i = 0; i < wave.agents.Length; i++)
            {
                pool.objectToSpawn = wave.agents[i];
                var obj = pool.SpawnObject();

                yield return new WaitForSeconds(wave.timeBetweenSpawn);
            }

            yield return new WaitForSeconds(sManager.currentTimeBetweenSpawn);

            //Use this code if you wanna use the one wave system

            //At the end remove wave and sett values
            waves.RemoveAt(0);
            sManager.RemoveWaves();
            sManager.CanSpawn = false;

            //Use this code if you wanna use the mini wave system 

            /*if (currPhase == waves[0].wave.Length - 1)
            {
                agentCount = 0;
                yield return new WaitForSeconds(sManager.timeBetweenCardSpawn);

                //At the end remove wave and sett values
                waves.RemoveAt(0);
                sManager.allWaves.RemoveAt(0);
                sManager.canSpawn = false;
            }
            else if (agentCount == waves[0].wave[0].agents.Length)
            {
                yield return new WaitForSeconds(waves[0].wave[0].timeBetweenNextMiniWave);
            }*/
        }
    }
}
