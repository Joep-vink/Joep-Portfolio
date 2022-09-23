using UnityEngine;

public class PlayerTouchInput : MonoBehaviour
{
    public LayerMask layerToHit;

    private void Update()
    {
        TouchInput();
    }

    private void TouchInput()
    {
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 rayPos = Camera.main.ScreenToWorldPoint(touch.position);
            RaycastHit2D[] hit = Physics2D.RaycastAll(rayPos, Vector2.zero, Mathf.Infinity, layerToHit);

            for (int i = 0; i < hit.Length; i++)
            {
                Debug.Log(hit[i].collider.name);
                if (hit[i].transform.GetComponent<FeedBack>())
                    hit[i].transform.GetComponent<FeedBack>().OnClick?.Invoke();
            }
            
        }
    }
}
