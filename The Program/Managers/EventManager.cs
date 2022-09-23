using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.UI;
using TMPro;

public class EventManager : MonoBehaviour
{
    public UnityEvent OnDie;

    public ParticleSystem smoke;
    public Interactable bed;
    public Volume rp;
    public VolumeProfile jp;

    public float[] eventWeights;

    [Header("Times")]
    [SerializeField] private float timeTillFirstEvent;
    [SerializeField] private float minTimeBetweenAgressiveEvents, maxTimeBetweenAgressiveEvents;
    [SerializeField] private float minTimeBetweenPassiveEvents, maxTimeBetweenPassiveEvents;

    [Header("Noise")]
    [SerializeField] private float radioNoise; 
    [SerializeField] private float phoneNoise;

    [Header("Jumpscare")]
    [SerializeField] private Sprite windowImage;
    [SerializeField] private Sprite doorImage;
    [SerializeField] private string windowTip, doorTip;
    [SerializeField] private Image tipImage;
    [SerializeField] private TextMeshProUGUI tipText;

    [Header("Animators")]
    public Animator radio;
    public Animator phone;
    public Animator window;
    private Animator anim;
    public Animator jumpScare;

    [Header("Debug")]
    [Range(0, 1)]
    [SerializeField] private float spawnChange = 0.5f;

    [Header("Private")]
    private bool jumpScared = false;
    private bool isChecking = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
        
        StartCoroutine(StartEvents());
    }

    private void Update()
    {
        spawnChange = NoiseManager.instance.value / 100;

        if (NoiseManager.instance.isDead && !jumpScared)
        {
            jumpScared = true;
            AudioManager.instance.StopAllAudio();
            JumpScare(NoiseManager.instance.windowDeath);
        }

        if (isChecking && !bed.isActive)
            NoiseManager.instance.isDead = true;

        if (NoiseManager.instance.isDead)
            StopAllCoroutines();
    }

    #region Passive

    private IEnumerator BetweenPassiveEvents()
    {
        if (CheckPassive())
        {
            yield return new WaitForSeconds(Random.Range(minTimeBetweenPassiveEvents, maxTimeBetweenPassiveEvents));
           
            ChosePassiveEvent();

            StartCoroutine(BetweenPassiveEvents());
        }
        else
            StartCoroutine(AllActivePassive());
    }

    private IEnumerator AllActivePassive()
    {
        yield return new WaitForSeconds(5);
        StartCoroutine(BetweenPassiveEvents());
    }

    private bool CheckPassive()
    {
        if (phone.GetBool("On") == true && radio.GetBool("On") == true)
            return false;
        else
            return true;
    }

    private void ChosePassiveEvent()
    {
        var dropVariable = Random.value;

        if (dropVariable > spawnChange)
        {
            int passive = Random.Range(0, 2);

            if (passive == 0 && phone.GetBool("On") == false)
            {
                phone.SetBool("On", true);
                NoiseManager.instance.currMultiplier += phoneNoise;
                AudioManager.instance.Play("Phone");
            }
            else if (passive == 1 && radio.GetBool("On") == false)
            {
                radio.SetBool("On", true);
                AudioManager.instance.Play("Radio");
                NoiseManager.instance.currMultiplier += radioNoise;
            }
        }
    }
    #endregion

    #region Events
    private IEnumerator StartEvents()
    {
        yield return new WaitForSeconds(timeTillFirstEvent);

        StartCoroutine(BetweenAgressiveEvents());
        StartCoroutine(BetweenPassiveEvents());
    }

    private int GetRandomWeightedIndex(float[] itemWeights)
    {
        float sum = 0f;
        for (int i = 0; i < itemWeights.Length; i++)
        {
            sum += itemWeights[i];
        }
        float randomValue = Random.Range(0, sum);
        float tempSum = 0;

        for (int i = 0; i < 2; i++)
        {
            if (randomValue >= tempSum && randomValue < tempSum + itemWeights[i])
            {
                return i;
            }
            tempSum += itemWeights[i];
        }
        return 0;
    }
    #endregion

    #region Agressive

    private IEnumerator BetweenAgressiveEvents()
    {
        if (CheckAgressive())
        {
            yield return new WaitForSeconds(Random.Range(minTimeBetweenAgressiveEvents, maxTimeBetweenAgressiveEvents));

            ChoseAggresiveEvent();

            StartCoroutine(BetweenAgressiveEvents());
        }
        else
            StartCoroutine(AllActiveAgressive());
    }

    private IEnumerator AllActiveAgressive()
    {
        yield return new WaitForSeconds(5);
        StartCoroutine(BetweenAgressiveEvents());
    }

    private bool CheckAgressive()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Door") && window.GetCurrentAnimatorStateInfo(0).IsName("Window"))
            return false;
        else
            return true;
    }

    private void ChoseAggresiveEvent()
    {
        var dropVariable = Random.value;

        if (dropVariable < spawnChange)
        {
            int index = GetRandomWeightedIndex(eventWeights);

            if (index == 0 && !anim.GetCurrentAnimatorStateInfo(0).IsName("Door"))
            {
                anim.Play("Door");
                eventWeights[0] -= 10;
                eventWeights[1] += 10;
            }
            else if (index == 1 && !window.GetCurrentAnimatorStateInfo(0).IsName("Window"))
            {
                window.Play("Window");
                eventWeights[0] += 10;
                eventWeights[1] -= 10;
            }
        }
    }

    #endregion

    #region Extra
    public void Smoke(string play)
    {
        if (play == "Play")
            smoke.Play();
        else
            smoke.Stop();
    }

    public void PlayAudio(string nane)
    {
        AudioManager.instance.Play(nane);
    }

    public void BedCheck(int i)
    {
        if (i == 0)
            isChecking = true;
        else
            isChecking = false;
    }

    public void StopRadio()
    {
        radio.SetBool("On", false);
        NoiseManager.instance.StopAddingOverTime(radioNoise);
    }

    public void StopPhone()
    {
        phone.SetBool("On", false);
        NoiseManager.instance.StopAddingOverTime(phoneNoise);
    }

    public void JumpScare(bool window)
    {
        if (window)
        {
            tipImage.sprite = windowImage;
            tipText.text = windowTip;
        }
        else
        {
            tipImage.sprite = doorImage;
            tipText.text = doorTip;
        }

        rp.profile = jp;
        NoiseManager.instance.isDead = true;
        OnDie?.Invoke();
        jumpScare.Play("Jumpscare");
        AudioManager.instance.Play("JumpScare");
    }

    #endregion
}
