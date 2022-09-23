using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://www.youtube.com/watch?v=3xXlnSetHPM source
public class CameraScaler : MonoBehaviour
{
    public SpriteRenderer rink;

    public float maxSize;

    private void Awake()
    {
        float orthoSize = rink.bounds.size.x * Screen.height / Screen.width * 0.5f;

        if (orthoSize > maxSize)
            Camera.main.orthographicSize = maxSize;
        else
            Camera.main.orthographicSize = orthoSize;
    }
}
