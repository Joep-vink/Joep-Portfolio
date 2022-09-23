using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAudio : AudioPlayer
{
    [SerializeField] private AudioClip shootBullet = null, outOfBulletClip = null;

    public void PlayerShootSound()
    {
        PlayClip(shootBullet);
    }

    public void PlayerNoBulletsSound()
    {
        PlayClip(outOfBulletClip);
    }
}
