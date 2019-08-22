﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LillyPad : MonoBehaviour
{
    [SerializeField]
    public Type type;
    [SerializeField]
    public float speed;
    [SerializeField]
    public int lillyNumber;
    [SerializeField]
    private Animation bounceAnimation;
    [SerializeField]
    private Animation splashAnimation;
    [SerializeField]
    private Transform lillyTransform;
    [SerializeField]
    private Animation flyAnimation;
    [SerializeField]
    private GameObject fly;
    [SerializeField]
    private Outline outline;
    
    public enum Type
    {
        Normal,
        Fly,
        Flower,
        Sink
    }

    private void Update()
    {
        if (GameManager.Instance.Playing)
            transform.Translate(Vector3.back * Time.deltaTime * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.transform.tag)
        {
            case "LillyDeadZone":
                gameObject.SetActive(false);
                break;
            case "JumpRangeShort":
                outline.enabled = true;
                break;
            case "JumpRangeLong":
                outline.enabled = true;
                break;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        switch (other.transform.tag)
        {
            case "JumpRangeShort":
                outline.enabled = false;
                break;
            case "JumpRangeLong":
                outline.enabled = false;
                break;
        }
    }

    public void SetLilly(float speed, int lilyNumber)
    {
        this.speed = speed;
        this.lillyNumber = lilyNumber;
        if (type == Type.Fly) fly.SetActive(true);
        if (type == Type.Sink) bounceAnimation.Play("LillyIdle");
    }

    public void SplashAnimate()
    {
        splashAnimation.Play();
        bounceAnimation.Play();
        if (type == Type.Fly) fly.SetActive(false);
    }

    public Transform GetLillyTransform()
    {
        return lillyTransform;
    }

    public static Type GetRandomType()
    {
        return (Type)Random.Range(0, 4);
    }

}
