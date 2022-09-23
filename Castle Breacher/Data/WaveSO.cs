using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Wave")]
public class WaveSO : ScriptableObject
{
    public WaveData[] wave;
}

[System.Serializable]
public class WaveData
{
    public GameObject[] agents;

    public float timeBetweenSpawn;
    public float timeBetweenNextMiniWave;
}
