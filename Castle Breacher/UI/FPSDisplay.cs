using UnityEngine;
using TMPro;

public class FPSDisplay : MonoBehaviour
{
    public TextMeshProUGUI FPSText;

    [Range(0.1f, 1)]
    public float PollingTime = 1;

    private float _time;
    private int _frameCount;

    private void Update()
    {
        _time += Time.deltaTime;

        _frameCount++;

        if (_time >= PollingTime)
        {
            int frameRate = Mathf.RoundToInt(_frameCount / _time);
            FPSText.text = frameRate.ToString() + " FPS";

            _time -= PollingTime;
            _frameCount = 0;
        }
    }
}
