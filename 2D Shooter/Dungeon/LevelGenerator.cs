using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{
    public GameObject layoutRoom;
    private GameObject endRoom, shopRoom, chestRoom;
    private List<GameObject> layoutRoomObject = new List<GameObject>();
    private List<GameObject> generatedOutlines = new List<GameObject>();

    public Color startColor, endColor, shopColor, chestColor;

    public LayerMask roomLayer;

    public int distanceToEnd, minDisShop, maxDisShop, minDisChest, maxDisChest;

    public bool includeShop, includeChests;

    public Transform generatorPoint;

    public float xOffset = 18, yOffset = 10;

    public RoomPrefabs rooms;

    public RoomCenter centerStart, centerEnd, centerShop, centerGoldChest;
    public RoomCenter[] potentialCenters;

    public enum Direcrion
    {
        up,
        right,
        down,
        left
    };

    public Direcrion selectedDirection;

    private void Start()
    {
        Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation).GetComponent<SpriteRenderer>().color = startColor;

        selectedDirection = (Direcrion)Random.Range(0, 4);
        MoveGenerationPoint();

        for (int i = 0; i < distanceToEnd; i++)
        {
            GameObject newRoom = Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation);

            layoutRoomObject.Add(newRoom);

            if (i + 1 == distanceToEnd)
            {
                newRoom.GetComponent<SpriteRenderer>().color = endColor;
                layoutRoomObject.RemoveAt(layoutRoomObject.Count - 1);

                endRoom = newRoom;
            }
            
            selectedDirection = (Direcrion)Random.Range(0, 4);
            MoveGenerationPoint();

            while (Physics2D.OverlapCircle(generatorPoint.position, .2f, roomLayer))
            {
                MoveGenerationPoint();
            }
        }

        //Spawn Shop
        if (includeShop)
        {
            int shopSelecter = Random.Range(minDisShop, (layoutRoomObject.Count - 1) - maxDisShop);
            shopRoom = layoutRoomObject[shopSelecter];
            layoutRoomObject.RemoveAt(shopSelecter);
            shopRoom.GetComponent<SpriteRenderer>().color = shopColor;
        }

        //Spawn Chest
        if (includeChests)
        {
            int chestSelector = Random.Range(minDisChest, (layoutRoomObject.Count - 1) - maxDisChest);
            chestRoom = layoutRoomObject[chestSelector];
            layoutRoomObject.RemoveAt(chestSelector);
            chestRoom.GetComponent<SpriteRenderer>().color = chestColor;
        }

        //Create room outline
        CreateRoomOutline(Vector3.zero);

        foreach (GameObject room in layoutRoomObject)
        {
            CreateRoomOutline(room.transform.position);
        }
        CreateRoomOutline(endRoom.transform.position);

        if (includeShop)
        {
            CreateRoomOutline(shopRoom.transform.position);
        }

        if (includeChests)
        {
            CreateRoomOutline(chestRoom.transform.position);
        }


        foreach (GameObject outline in generatedOutlines)
        {
            bool generateCenter = true;

            if (outline.transform.position == Vector3.zero)
            {
                Instantiate(centerStart, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
                generateCenter = false;
            }
            if (outline.transform.position == endRoom.transform.position)
            {
                Instantiate(centerEnd, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
                generateCenter = false;
            }

            if (includeShop)
            {
                if (outline.transform.position == shopRoom.transform.position)
                {
                    Instantiate(centerShop, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
                    generateCenter = false;
                }
            }

            if (includeChests)
            {
                if (outline.transform.position == chestRoom.transform.position)
                {
                    Instantiate(centerGoldChest, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
                    generateCenter = false;
                }
            }

            if (generateCenter)
            {
                int centerSelect = Random.Range(0, potentialCenters.Length);

                Instantiate(potentialCenters[centerSelect], outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
            }
        }
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.Backspace))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
#endif
    }

    public void MoveGenerationPoint()
    {
        switch (selectedDirection)
        {
            case Direcrion.up:
                generatorPoint.position += new Vector3(0f, yOffset, 0f);
                break;
            case Direcrion.right:
                generatorPoint.position += new Vector3(xOffset, 0f, 0f);
                break;
            case Direcrion.down:
                generatorPoint.position += new Vector3(0f, -yOffset, 0f);
                break;
            case Direcrion.left:
                generatorPoint.position += new Vector3(-xOffset, 0f, 0f);
                break;
        }
    }

    public void CreateRoomOutline(Vector3 roomPos)
    {
        bool roomAbove = Physics2D.OverlapCircle(roomPos + new Vector3(0f, yOffset, 0), 0.2f, roomLayer);
        bool roomBelow = Physics2D.OverlapCircle(roomPos + new Vector3(0f, -yOffset, 0), 0.2f, roomLayer);
        bool roomLeft = Physics2D.OverlapCircle(roomPos + new Vector3(-xOffset, 0f, 0), 0.2f, roomLayer);
        bool roomRight = Physics2D.OverlapCircle(roomPos + new Vector3(xOffset, 0f, 0), 0.2f, roomLayer);

        int directionCount = 0;
        if (roomAbove)
        {
            directionCount++;
        }
        if (roomBelow)
        {
            directionCount++;
        }
        if (roomLeft)
        {
            directionCount++;
        }
        if (roomRight)
        {
            directionCount++;
        }

        switch (directionCount)
        {
            case 0:
                Debug.LogError("Found No Room Exits!!");
                break;
            case 1:
                if (roomAbove)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleUp, roomPos, transform.rotation));
                }
                if (roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleDown, roomPos, transform.rotation));
                }
                if (roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleLeft, roomPos, transform.rotation));
                }
                if (roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleRight, roomPos, transform.rotation));
                }
                break;
            case 2:
                if (roomAbove && roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleUpDown, roomPos, transform.rotation));
                }
                if (roomLeft && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleLeftRight, roomPos, transform.rotation));
                }
                if (roomAbove && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleUpRight, roomPos, transform.rotation));
                }
                if (roomRight && roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleRightDown, roomPos, transform.rotation));
                }
                if (roomLeft && roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleDownLeft, roomPos, transform.rotation));
                }
                if (roomLeft && roomAbove)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleLeftUp, roomPos, transform.rotation));
                }
                break;
            case 3:
                if (roomAbove && roomBelow && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleUpRightDown, roomPos, transform.rotation));
                }
                if (roomRight && roomBelow && roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleRightDownLeft, roomPos, transform.rotation));
                }
                if (roomAbove && roomBelow && roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleDownLeftUp, roomPos, transform.rotation));
                }
                if (roomLeft && roomAbove && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleLeftUpRight, roomPos, transform.rotation));
                }
                break;
            case 4:
                if (roomAbove && roomBelow && roomRight && roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.fourway, roomPos, transform.rotation));
                }
                break;
        }
    }
}

[System.Serializable]
public class RoomPrefabs
{
    public GameObject singleUp, singleDown, singleLeft, singleRight,
        doubleUpDown, doubleLeftRight, doubleUpRight, doubleRightDown, doubleDownLeft, doubleLeftUp,
        tripleUpRightDown, tripleRightDownLeft, tripleDownLeftUp, tripleLeftUpRight,
        fourway;
}
