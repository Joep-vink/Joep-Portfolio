using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
public class AgentRenderer : MonoBehaviour
{
    protected SpriteRenderer spriteRenderer;
    [SerializeField] private SpriteRenderer hand;

    [field: SerializeField]
    public UnityEvent<int> OnBackWardMovement { get; set; }

    public bool isBoss = false;
    public Transform bossShotPoint;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void FaceDirection(Vector2 pointerInput)
    {
        var direction = (Vector3)pointerInput - transform.position;
        var result = Vector3.Cross(Vector2.up, direction);

        if (result.z > 0)
        {
            if (isBoss)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                bossShotPoint.rotation = Quaternion.Euler(0, 0, 180);
            }
            else 
                spriteRenderer.flipX = true;
        }
        else if (result.z < 0)
        {
            if (isBoss)
            {
                transform.localScale = new Vector3(1, 1, 1);
                bossShotPoint.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
                spriteRenderer.flipX = false;
        }
    }

    public void CheckIfBackWardMovement(Vector2 movementVector)
    {
        float angle = 0;
        if (spriteRenderer.flipX == true)
        {
            hand.flipX = true;
            angle = Vector2.Angle(-transform.right, movementVector);
        }
        else
        {
            hand.flipX = false;
            angle = Vector2.Angle(transform.right, movementVector);
        }

        if (angle > 90)
        {
            OnBackWardMovement?.Invoke(-1);
        }
        else
        {
            OnBackWardMovement?.Invoke(1);
        }
    }
}
