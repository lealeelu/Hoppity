using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField]
    private AudioSource source;

    [SerializeField]
    private AudioClip hop;
    [SerializeField]
    private AudioClip click;
    [SerializeField]
    private AudioClip sink;
    [SerializeField]
    private AudioClip gulp;

    [SerializeField]
    private AudioClip[] splashClips;
    [SerializeField]
    private AudioSource[] bgmSources;

    [SerializeField]
    private AudioSource death;
    [SerializeField]
    private AudioSource super;

    public void PlayButtonClick()
    {
        source.PlayOneShot(click);
    }

    public void PlayHop()
    {
        source.PlayOneShot(hop);
    }

    public void PlaySplash()
    {
        int clipid = Random.Range(0, splashClips.Length);
        source.PlayOneShot(splashClips[clipid]);
    }

    public void PlaySink()
    {
        source.PlayOneShot(sink);
    }

    public void PlayGulp()
    {
        source.PlayOneShot(gulp);
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
            death.Play();
       }
    }

    public void PlaySuperMode()
    {
        super.Play();
    }

    public void StopSuperMode()
    {
        super.Stop();
    }
}
