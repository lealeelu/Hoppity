using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField]
    private AudioSource source;

    [SerializeField]
    private AudioSource bgmIntro;
    [SerializeField]
    private double loopStartAdjust = 0.15;
    [SerializeField]
    private AudioSource bgmLoop;

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
            double introDuration = (double)bgmIntro.clip.samples / bgmIntro.clip.frequency;
            double startTime = AudioSettings.dspTime + 0.2;
            bgmIntro.PlayScheduled(startTime);
            bgmLoop.PlayScheduled(startTime + introDuration - loopStartAdjust);
       }
       else
       {
            bgmIntro.Stop();
            bgmLoop.Stop();
       }
    }
}
