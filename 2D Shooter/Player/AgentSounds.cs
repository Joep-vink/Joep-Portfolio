using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentSounds : AudioPlayer
{
    [SerializeField] private AudioClip hitClip = null, deathClip = null, voiceLineClip = null, reloadClip = null;

    public void PlayReloadSound()
    {
        PlayClip(reloadClip);
    }

    public void PlayHitSound()
    {
        PlayClipWithVariablePitch(hitClip);
    }

    public void PlayDeathSound()
    {
        PlayClip(deathClip);
    }

    public void PlayVoiceSound()
    {
        PlayClipWithVariablePitch(voiceLineClip);
    }
}
