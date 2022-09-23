using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : Interactable
{
    [SerializeField] private Cloth left, right;

    public float timeItTakesToClose;

    private float zAcceleration;
    private Animator anim;

    private bool iscounting = false;
    public float currTime;

    private void Start()
    {
        anim = GetComponent<Animator>();
        zAcceleration = left.externalAcceleration.z;
    }

    private void Update()
    {
        if (iscounting && anim.GetCurrentAnimatorStateInfo(0).IsName("Window"))
            currTime += Time.deltaTime;

        if (currTime >= timeItTakesToClose)
        {
            anim.Play("Idle");
            currTime = 0;
        }
    }

    public override void Interact()
    {
        iscounting = true;
        OnInteract?.Invoke();
        StartCoroutine(NoiseManager.instance.AddValue(3, 0.5f));
        isActive = true;
        AudioManager.instance.Play("Curtain");
        left.externalAcceleration = new Vector3(0, 0, 0.5f);
        right.externalAcceleration = new Vector3(0, 0, -0.5f);
    }

    public override void StopInteraction()
    {
        iscounting = false;
        StartCoroutine(NoiseManager.instance.AddValue(3, 0.5f));
        isActive = false;
        OnLeave?.Invoke();
        AudioManager.instance.Play("Curtain");
        left.externalAcceleration = new Vector3(0, 0, zAcceleration);
        right.externalAcceleration = new Vector3(0, 0, -zAcceleration);
    }

    public void Die()
    {
        NoiseManager.instance.windowDeath = true;
        NoiseManager.instance.isDead = true;
    }
}
