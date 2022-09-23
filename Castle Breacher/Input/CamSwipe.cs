using UnityEngine;

public class CamSwipe : MonoBehaviour
{
    private Camera cam;

    [Header("Clamp camera Variables")]
    private float mapMinX, mapMaxX, mapMinY, mapMaxY;
    [SerializeField] private SpriteRenderer camBounds;

    [Header("Move Variables")]
    private Vector3 dragOrigin;
    private bool touched = false;
    private Vector2 firstTouchPrevPos, secondTouchPrevPos;
    private Touch touch;

    [Header("Zoom Variables")]
    [SerializeField] private float zoomStep;
    [SerializeField] private float minCamSize;
    private float maxCamSize;
    private float prefTouchDifference, currTouchDifference, zoomModifier;

    private void Awake()
    {
        mapMinX = camBounds.transform.position.x - camBounds.bounds.size.x / 2;
        mapMaxX = camBounds.transform.position.x + camBounds.bounds.size.x / 2;

        mapMinY = camBounds.transform.position.y - camBounds.bounds.size.y / 2;
        mapMaxY = camBounds.transform.position.y + camBounds.bounds.size.y / 2;
    }
    

    private void Start()
    {
        cam = GetComponent<Camera>();
        maxCamSize = cam.orthographicSize;

        cam.transform.position = ClampCamera(cam.transform.position);
    }

    private void Update()
    {
        if (Input.touchCount >= 0)
            PanCamera();

        if (Input.touchCount == 2)
            Zoom();
    }

    /// <summary>
    /// Move the camera if you hold your finger down
    /// </summary>
    private void PanCamera()
    {
        if (Input.touchCount == 1)
        {
            touch = Input.GetTouch(0);

            if (!touched)
            {
                dragOrigin = cam.ScreenToWorldPoint(touch.position);
                touched = true;
            }
        }
        else 
            touched = false;

        if (touch.phase == TouchPhase.Moved)
        {
            Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(touch.position);

            Debug.Log(difference);

            cam.transform.position = ClampCamera(cam.transform.position + difference);
        }
    }

    /// <summary>
    /// Zooms in and out if there are 2 fingers on the screens
    /// </summary>
    private void Zoom()
    {
        Touch firsTouch = Input.GetTouch(0);
        Touch secondTouch = Input.GetTouch(1);

        firstTouchPrevPos = firsTouch.position - firsTouch.deltaPosition;
        secondTouchPrevPos = secondTouch.position - secondTouch.deltaPosition;

        prefTouchDifference = (firstTouchPrevPos - secondTouchPrevPos).magnitude;
        currTouchDifference = (firsTouch.position - secondTouch.position).magnitude;

        zoomModifier = (firsTouch.deltaPosition - secondTouch.deltaPosition).magnitude * zoomStep;
        
        if (prefTouchDifference > currTouchDifference)
            cam.orthographicSize += zoomModifier;
        if (prefTouchDifference < currTouchDifference)
            cam.orthographicSize -= zoomModifier;

        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minCamSize, maxCamSize);
        cam.transform.position = ClampCamera(cam.transform.position);
    }

    /// <summary>
    /// Stops the camera from moving outside of the camBounds sprite
    /// </summary>
    /// <param name="targetPosition"></param>
    /// <returns></returns>
    private Vector3 ClampCamera(Vector3 targetPosition)
    {
        float camHeight = cam.orthographicSize;
        float camWidth = cam.orthographicSize * cam.aspect;

        float minX = mapMinX + camWidth;
        float maxX = mapMaxX - camWidth;
        float minY = mapMinY + camHeight;
        float maxY = mapMaxY - camHeight;

        float newX = Mathf.Clamp(targetPosition.x, minX, maxX);
        float newY = Mathf.Clamp(targetPosition.y, minY, maxY);

        return new Vector3(newX, newY, targetPosition.z);
    }
}
