using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyMultLilyPad : LillyPad
{
    [SerializeField]
    private GameObject fly;
    [SerializeField]
    private float points;

    public override void SetLilly(float speed, int lilyNumber)
    {
        base.SetLilly(speed, lilyNumber);
        fly.SetActive(true);
    }

    public override void SplashAnimate()
    {
        base.SplashAnimate();
        fly.SetActive(false);
        GameManager.Instance.ShowNotification(string.Format("+{0}", points), transform.position);
        GameManager.Instance.AddFirefly(3);
        GameManager.Instance.AddToScore(points);

    }
}
