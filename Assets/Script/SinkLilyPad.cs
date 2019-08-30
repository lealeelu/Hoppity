using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinkLilyPad : LillyPad
{
    public override void SetLilly(float speed, int lilyNumber)
    {
        base.SetLilly(speed, lilyNumber);
        base.bounceAnimation.Play("LillyIdle");
    }

    public override void SplashAnimate()
    {
        base.SplashAnimate();
        AudioManager.Instance.PlaySink();
    }
}
