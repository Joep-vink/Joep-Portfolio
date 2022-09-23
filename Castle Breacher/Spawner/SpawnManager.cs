using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    public Color NotOccupied, Occupied;
    public Transform SpawnIconParent;

    [Range(0.1f, 4f)]
    public float[] TimeBetweenCardSpawn;
    [HideInInspector] public float currentTimeBetweenSpawn;

    public SpawnerInfo[] Spawners;

    public Transform CardParent;

    public List<WaveSO> AllWaves = new List<WaveSO>();

    [HideInInspector] public bool CanSpawn = false;

    private void Awake()
    {
        int c = 0;

        for (int i = 0; i < CardParent.childCount; i++)
        {
            if (CardParent.GetChild(i).gameObject.TryGetComponent(out Card card))
            {
                Spawners[c].currCard = card;
                card.Manager = this;
                card.SpawnerInfo = Spawners[i];
                c++;
            }
        }

        int s = 0;

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.TryGetComponent(out Spawner spawn))
            {
                Spawners[s].spawner = spawn;
                s++;
            }
        }
    }
     
    private void Start()
    {
        currentTimeBetweenSpawn = TimeBetweenCardSpawn[0];

        for (int i = 0; i < SpawnIconParent.childCount; i++)
        {
            SpawnIconParent.GetChild(i).GetComponent<Image>().color = NotOccupied;
        }
    }

    public void AddWaves(WaveSO wave)
    {
        AllWaves.Add(wave);

        for (int i = 0; i < AllWaves.Count; i++)
        {
            SpawnIconParent.transform.GetChild(i).GetComponent<Image>().color = Occupied;
        }
    }

    public void RemoveWaves()
    {
        AllWaves.RemoveAt(0);

        for (int i = AllWaves.Count ; i < 4; i++)
        {
            SpawnIconParent.transform.GetChild(i).GetComponent<Image>().color = NotOccupied;
        }
    }
}

[System.Serializable]
public class SpawnerInfo
{
    public Card currCard;
    public Spawner spawner;
}
