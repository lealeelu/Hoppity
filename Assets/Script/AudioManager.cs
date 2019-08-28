using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField]
    private AudioSource source;

    [SerializeField]
    private AudioSource[] bgmSources;

    [SerializeField]
    private AudioClip[] splashClips;

    public void PlaySplash()
    {
        int clipid = Random.Range(0, splashClips.Length);
        source.PlayOneShot(splashClips[clipid]);
    }

    public void PlayBG(bool play = true)
    {
       if (play)
       {
            double startTime = AudioSettings.dspTime + 0.2;
            for (int i = 0; i < bgmSources.Length; i++)
            {
                bgmSources[i].PlayScheduled(startTime);
                startTime += (double)bgmSources[i].clip.samples / bgmSources[i].clip.frequency;
            }
       }
       else
       {
            for (int i = 0; i < bgmSources.Length; i++)
            {
                bgmSources[i].Stop();
            }
       }
    }
}
