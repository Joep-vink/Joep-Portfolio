using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemDropper : MonoBehaviour
{
    public List<ItemSpawnData> itemsToDrop = new List<ItemSpawnData>();
    float[] itemWeights;

    [Range(0,1)]
    [SerializeField] private float dropChange = 0.5f;

    //Chest
    [SerializeField] private SpriteRenderer theSprite;
    [SerializeField] private Sprite chestOpen;
    [SerializeField] private bool isChest = false, canOpen, isOpen = false;
    [SerializeField] private GameObject notification;
    public float scaleSpeed = 2;

    private void Start()
    {
        itemWeights = itemsToDrop.Select(item => item.rate).ToArray();
    }

    public void DropItem()
    {
        var dropVariable = UnityEngine.Random.value;
        if (dropVariable < dropChange)
        {
            int index = GetRandomWeightedIndex(itemWeights);

            if (isChest)
            {
                GameObject go = Instantiate(itemsToDrop[index].itemPrefab, transform.position, Quaternion.identity);
                go.transform.SetParent(theSprite.gameObject.transform);
                itemsToDrop.Remove(itemsToDrop[index]);
            }
            else
            {
                Instantiate(itemsToDrop[index].itemPrefab, transform.position, Quaternion.identity);
            }
        }
    }

    private int GetRandomWeightedIndex(float[] itemWeights)
    {
        float sum = 0f;
        for (int i = 0; i < itemWeights.Length; i++)
        {
            sum += itemWeights[i];
        }
        float randomValue = UnityEngine.Random.Range(0, sum);
        float tempSum = 0;

        for (int i = 0; i < itemsToDrop.Count; i++)
        {
            if (randomValue >= tempSum && randomValue < tempSum + itemWeights[i])
            {
                return i;
            }
            tempSum += itemWeights[i];
        }
        return 0;
    }
    // Chest
    private void Update()
    {
        if (isChest && canOpen && !isOpen)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                theSprite.sprite = chestOpen;

                if (itemsToDrop.Count > 0)
                {
                    DropItem();
                    isOpen = true;

                    transform.localScale = new Vector3(1.2f, 1.2f, 1);
                }
            }
        }

        if (isOpen && isChest)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.one, Time.deltaTime * scaleSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Player" && isChest && !isOpen)
        {
            notification.SetActive(true);
            canOpen = true;
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag == "Player" && isChest)
        {
            notification.SetActive(false);
            canOpen = true;
        }
    }
}

[Serializable]
public struct ItemSpawnData
{
    [Range(0,1)]
    public float rate;
    public GameObject itemPrefab;
}
