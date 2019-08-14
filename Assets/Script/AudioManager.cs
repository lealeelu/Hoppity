using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : Singleton<AudioManager>
{
    [SerializeField]
    private AudioSource source;

    [SerializeField]
    private AudioSource bgm;

    [SerializeField]
    private AudioClip[] splashClips;

    public void PlaySplash()
    {
        int clipid = Random.Range(0, splashClips.Length);
        source.PlayOneShot(splashClips[clipid]);
    }

    public void PlayFlowingWater(bool play = true)
    {
       if (play)
       {
            bgm.Play();
       }
       else
       {
            bgm.Stop();
       }
    }
}
