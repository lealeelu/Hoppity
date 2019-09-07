using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinkLilyPad : LillyPad
{
    public bool sinking = false;

    public override void SetLilly(float speed, int lilyNumber)
    {
        base.SetLilly(speed, lilyNumber);
        base.bounceAnimation.Play("LillyIdle");
        sinking = false;
    }

    public override void SplashAnimate()
    {
        base.SplashAnimate();
        sinking = true;
        AudioManager.Instance.PlaySink();
    }
}
