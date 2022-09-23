using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public List<Button> buttons = new List<Button>();
    public List<Tile> neighbourTiles = new List<Tile>();

    public bool canInteract = false, isAvailable = true;

    public GameObject pipe;

    public int visited = -1;
    public int x, y;
    public bool isBad = false;

    public UnityEvent OnClick;

    public LevelGenerator generator;

    private void Awake()
    {
        generator = GetComponentInParent<LevelGenerator>();

        if (!canInteract)
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].interactable = false;
            }
        }
    }

    public void Clicked(int index)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].interactable = false;
        }

        isAvailable = false;

        if (index == 0)
        {
            generator.NewInteractableTile(x, y + 1);
            Instantiate(pipe, transform.position + new Vector3(0, 0.045f, 0), Quaternion.Euler(0, 0, 90), transform);
        }
        else if (index == 1)
        {
            generator.NewInteractableTile(x + 1, y);
            Instantiate(pipe, transform.position + new Vector3(0.045f, 0, 0), Quaternion.identity, transform);
        }
        else if (index == 2)
        {
            generator.NewInteractableTile(x, y - 1);
            Instantiate(pipe, transform.position + new Vector3(0, -0.045f, 0), Quaternion.Euler(0, 0, 90), transform);
        }
        else if (index == 3)
        {
            generator.NewInteractableTile(x - 1, y);
            Instantiate(pipe, transform.position + new Vector3(-0.045f, 0, 0), Quaternion.identity, transform);
        }
    }
}
