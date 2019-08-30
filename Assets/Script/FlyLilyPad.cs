using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyLilyPad : LillyPad
{
    [SerializeField]
    private GameObject fly;
    [SerializeField]
    private float points;
    [SerializeField]
    private int flyCount;

    private bool recievedPoints;

    public override void SetLilly(float speed, int lilyNumber)
    {
        base.SetLilly(speed, lilyNumber);
        fly.SetActive(true);
        recievedPoints = false;
    }

    public override void SplashAnimate()
    {
        base.SplashAnimate();
        fly.SetActive(false);
        if (!recievedPoints)
        {
            recievedPoints = true;
            AudioManager.Instance.PlayGulp();
            GameManager.Instance.ShowNotification(string.Format("+{0}", points), transform.position);
            GameManager.Instance.AddFirefly(flyCount);
            GameManager.Instance.AddToScore(points);
        }        
    }
}
