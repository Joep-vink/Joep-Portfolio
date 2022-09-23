using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoiseManager : MonoBehaviour
{
    public static NoiseManager instance;

    private void Awake()
    {
        instance = this;
    }

    public float speed;

    private Slider noiseBar;
    [HideInInspector] public float value;
    [HideInInspector] private bool minus = true;
    private bool addOverTime = false;
    public float currMultiplier = 0;

    public bool windowDeath = false;
    public bool isDead = false;

    private void Start()
    {
        noiseBar = GetComponent<Slider>();
        value = noiseBar.minValue;

        StartCoroutine(AddOverTime());
    }

    private void Update()
    {
        noiseBar.value = value;

        value = CalculateValue();

        value = Mathf.Clamp(value, 0, 100);
        currMultiplier = Mathf.Clamp(currMultiplier, 0, Mathf.Infinity);

        if (currMultiplier <= 0)
            addOverTime = false;
        else
            addOverTime = true;
    }

    private float CalculateValue()
    {
        float f = value;

        if (minus && !addOverTime)
            f -= speed * Time.deltaTime;

        return f;
    }

    public IEnumerator AddOverTime()
    {
        while (true)
        {
            value += currMultiplier / 10;
            yield return new WaitForSeconds(0.1f);

            yield return null;
        }
    }

    public void StopAddingOverTime(float amount)
    {
        currMultiplier -= amount;
    }

    public IEnumerator AddValue(float amount, float timeItTakes)
    {
        minus = false;
        float oldValue = value;

        float a = amount + oldValue;

        float startTime = Time.time;
        while (Time.time < startTime + timeItTakes)
        {
            value = Mathf.Lerp(oldValue, a, (Time.time - startTime) / timeItTakes);
            yield return null;
        }

        minus = true;
    }
}
