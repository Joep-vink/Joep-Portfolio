using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{
    public UnityEvent OnLevelComplete;
    
    public UnityEvent OnWin;

    public Slider progSlider;

    [Header("GridSettings")]
    public float xOffset, yOffset;

    public Levels[] levels;

    [Header("Tiles")]
    public Tile objToSpawn;
    public GameObject startEndTile;
    public GameObject badTile;

    public Tile[,] tiles;

    [Header("Debug")]
    public bool findDistance = false;
    public bool generateNewLevel = false;
    public List<Tile> path = new List<Tile>();

    [Header("Private")]
    private List<GameObject> badTiles = new List<GameObject>();
    private Tile finnishTile, startTile;
    private int currLevel;
    private Tile currTile;

    private int amountPerLevelUp;
    private int progress;

    private void Awake()
    {
        GenerateLevel();
        amountPerLevelUp = 100 / levels.Length;
    }

    private void OnEnable()
    {
        progSlider.value = progress;

        if (currTile == finnishTile)
        {
            generateNewLevel = true;
        }
    }

    private void Update()
    {
        if (generateNewLevel)
        {
            StartCoroutine(NewLevelCoroutine());
            generateNewLevel = false;
        }

        if (findDistance) //For debug 
        {
            SetDistance();
            SetPath();
            findDistance = false;
        }
    }
     
    private IEnumerator NewLevelCoroutine()
    {
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        GenerateLevel();
    }

    /// <summary>
    /// Generate a level and check if the level is beatable with the other functions
    /// /// </summary>
    private void GenerateLevel()
    {
        tiles = new Tile[levels[currLevel].grid.x, levels[currLevel].grid.y]; //Make a new 2d array

        for (int i = 0; i < levels[currLevel].grid.x; i++) //Spawn the tiles and add them to the array
        {
            for (int o = 0; o < levels[currLevel].grid.y; o++)
            {
                Tile obj = Instantiate(objToSpawn, transform.position + new Vector3(i * xOffset, o * yOffset, 0), Quaternion.identity, transform);

                tiles[i, o] = obj; //Add to the list

                //Set the values of the tile
                obj.x = i;
                obj.y = o;

                obj.name = string.Format("cube " + "{0}" + " - " + "{1}", i, o);
            }
        }

        //Add a finnish
        finnishTile = tiles[levels[currLevel].grid.x - 1, Random.Range(0, levels[currLevel].grid.y)];
        Instantiate(startEndTile, finnishTile.transform.position, Quaternion.identity, transform);

        //Add a start
        startTile = tiles[0, Random.Range(0, levels[currLevel].grid.y)];
        Instantiate(startEndTile, startTile.transform.position, Quaternion.identity, transform);
        AddBadTiles();

        InitialSetUp();
        SetDistance();
        SetPath();

        NewInteractableTile(0, startTile.y); //Set start tile
    }

    private void AddBadTiles()
    {
        for (int i = 0; i < levels[currLevel].badTileCount; i++)
        {
            int x = Random.Range(0, levels[currLevel].grid.x);
            int y = Random.Range(0, levels[currLevel].grid.y);

            if (tiles[x, y] != startTile && tiles[x, y] != finnishTile && !tiles[x, y].isBad)
            {
                GameObject go = Instantiate(badTile, tiles[x, y].transform.position, Quaternion.identity, transform);
                badTiles.Add(go);
                tiles[x, y].isBad = true;
            }
            else
                i--;
        }
    }

    #region AI

    /// <summary>
    /// Run InitialSetUp 
    /// </summary>
    private void SetDistance()
    {
        InitialSetUp();
        int x = startTile.x;
        int y = startTile.y;
        int[] testArray = new int[levels[currLevel].grid.y * levels[currLevel].grid.x];

        for (int step = 1; step < levels[currLevel].grid.y * levels[currLevel].grid.x; step++)
        {
            foreach (Tile obj in tiles)
            {
                if (obj && obj.visited == step - 1)
                    TestFourDirections(obj.x, obj.y, step);
            }
        }
    }

    /// <summary>
    /// Test all four directions
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="step"></param>
    private void TestFourDirections(int x, int y, int step)
    {
        if (TestDirection(x, y, -1, 1)) //Test Up
            SetVisited(x, y + 1, step);

        if (TestDirection(x, y, -1, 2)) //Test Right
            SetVisited(x + 1, y, step);

        if (TestDirection(x, y, -1, 3)) //Test Down
            SetVisited(x, y - 1, step);

        if (TestDirection(x, y, -1, 4)) //Test Left
            SetVisited(x - 1, y, step);
    }

    /// <summary>
    /// Check if the level is beatable if not generate a new level
    /// </summary>
    private void SetPath()
    {
        int step;
        int x = finnishTile.x;
        int y = finnishTile.y;
        List<Tile> tempList = new List<Tile>();
        path.Clear();

        if (tiles[finnishTile.x, finnishTile.y] && tiles[finnishTile.x, finnishTile.y].visited > 0)
        {
            path.Add(tiles[x, y]);
            step = tiles[x, y].visited - 1;

            for (int i = step; step > -1; step--)
            {
                if (TestDirection(x, y, step, 1))
                    tempList.Add(tiles[x, y + 1]);
                if (TestDirection(x, y, step, 2))
                    tempList.Add(tiles[x + 1, y]);
                if (TestDirection(x, y, step, 3))
                    tempList.Add(tiles[x, y - 1]);
                if (TestDirection(x, y, step, 4))
                    tempList.Add(tiles[x - 1, y]);

                Tile tempTile = FindClosest(tiles[finnishTile.x, finnishTile.y].transform, tempList);
                path.Add(tempTile);
                x = tempTile.x;
                y = tempTile.y;

                tempList.Clear();
            }
        }
        else
        {
            print("Cant reach Finnish");
            
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }

            GenerateLevel();
        }
    }

    /// <summary>
    /// Set all tiles visited to -1 except the startTile
    /// </summary>
    private void InitialSetUp()
    {
        foreach (Tile obj in tiles)
        {
            obj.visited = -1;
        }

        startTile.visited = 0;
    }

    /// <summary>
    /// Test all the directions and return true if 1 direction meeds the criteria
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="step"></param>
    /// <param name="direction"></param>
    /// <returns></returns>
    bool TestDirection(int x, int y, int step, int direction)
    {
        //Int directions : 1 up, 2 right, 3 down, 4 left
        switch (direction)
        {
            case 1:
                if (y + 1 < levels[currLevel].grid.y && tiles[x, y + 1] && tiles[x, y + 1].visited == step && !tiles[x,y].isBad)
                    return true;
                else
                    return false;
            case 2:
                if (x + 1 < levels[currLevel].grid.x && tiles[x + 1, y] && tiles[x + 1, y].visited == step && !tiles[x, y].isBad)
                    return true;
                else
                    return false;
            case 3:
                if (y - 1 > -1 && tiles[x, y - 1] && tiles[x, y - 1].visited == step && !tiles[x, y].isBad)
                    return true;
                else
                    return false;
            case 4:
                if (x - 1 > -1 && tiles[x - 1, y] && tiles[x - 1, y].visited == step && !tiles[x, y].isBad)
                    return true;
                else
                    return false;
        }
        return false;
    }

    /// <summary>
    /// Change the visited to the step
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="step"></param>
    private void SetVisited(int x, int y, int step)
    {
        if (tiles[x, y])
            tiles[x, y].visited = step;
    }

    /// <summary>
    /// Find the closest tile in the given list and return it
    /// </summary>
    /// <param name="targetLocation"></param>
    /// <param name="list"></param>
    /// <returns></returns>
    Tile FindClosest(Transform targetLocation, List<Tile> list)
    {
        float currDistance = Mathf.Infinity;
        int indexNumb = 0;

        for (int i = 0; i < list.Count; i++)
        {
            if (Vector3.Distance(targetLocation.position, list[i].transform.position) < currDistance && !list[i].isBad)
            {
                currDistance = Vector3.Distance(targetLocation.position, list[i].transform.position);
                indexNumb = i;
            }
        }
        return list[indexNumb];
    }

    #endregion

    /// <summary>
    /// Sets the new tile active and calculates the possible directions
    /// </summary>
    /// <param name="X"></param>
    /// <param name="Y"></param>
    public void NewInteractableTile(int X, int Y)
    {
        tiles[X, Y].canInteract = true; //Set the tile active

        Tile tile = tiles[X, Y]; //The current tile
        currTile = tiles[X, Y];

        int t = 0;

        if (tile.y + 1 != levels[currLevel].grid.y && tiles[X, Y + 1].isAvailable && !tiles[X, Y + 1].isBad && tile != finnishTile)//Up
        {
            t++;
            tile.neighbourTiles.Add(tiles[tile.x, tile.y + 1]);
            tile.buttons[0].interactable = true;
        }
        else
            tile.buttons[0].interactable = false;

        if (tile.x + 1 != levels[currLevel].grid.x && tiles[X + 1, Y].isAvailable && !tiles[X + 1, Y].isBad && tile != finnishTile) //Right
        {
            t++;
            tile.neighbourTiles.Add(tiles[tile.x + 1, tile.y]);
            tile.buttons[1].interactable = true;
        }
        else
            tile.buttons[1].interactable = false;

        if (tile.y != 0 && tiles[X, Y - 1].isAvailable && !tiles[X, Y - 1].isBad && tile != finnishTile)     //Down
        {
            t++;
            tile.neighbourTiles.Add(tiles[tile.x, tile.y - 1]);
            tile.buttons[2].interactable = true;
        }
        else
            tile.buttons[2].interactable = false;

        if (tile.x != 0 && tiles[X - 1, Y].isAvailable && !tiles[X - 1, Y].isBad && tile != finnishTile)     //Left
        {
            t++;
            tile.neighbourTiles.Add(tiles[tile.x - 1, tile.y]);
            tile.buttons[3].interactable = true;
        }
        else
            tile.buttons[3].interactable = false;

        if (t == 0 && tile != finnishTile)
            generateNewLevel = true;

        //when you finnish
        if (tile == finnishTile)
        {
            AudioManager.instance.Play("Confetti");
            progress += amountPerLevelUp;
            
            if (currLevel == levels.Length - 1)
            {
                SceneManager.LoadScene("Win");
                StartCoroutine(UpdateProgressBar());
            }
            else
            {
                StartCoroutine(UpdateProgressBar());
                OnLevelComplete?.Invoke();
                currLevel++;
                generateNewLevel = true;
            }
        }
    }

    public IEnumerator UpdateProgressBar()
    {
        float oldValue = progSlider.value;

        float startTime = Time.time;
        while (Time.time < startTime + 1)
        {
            progSlider.value = Mathf.Lerp(oldValue, progress, (Time.time - startTime) / 1);
            yield return null;
        }
    }
}

[System.Serializable]
public class Levels
{
    public Vector2Int grid;

    public int badTileCount;
}
