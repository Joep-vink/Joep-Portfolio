using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollisions : MonoBehaviour
{
    public ParticleSystem ps;
    List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();

    public float damage = 0.5f;

    // Use this for initialization
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        ps.trigger.AddCollider(player.instance.particleCollider.transform);
    }

    public void PlayParticle()
    {
        if (!ps.isPlaying)
        {
            ps.Play();
        }
    }

    void OnParticleTrigger()
    {
        int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);

        for (int i = 0; i < numEnter; i++)
        {
            ParticleSystem.Particle p = enter[i];
            player.instance.GetHit(damage, gameObject);
            enter[i] = p;
        }

        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
    }
}
