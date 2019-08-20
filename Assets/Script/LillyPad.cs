using System.Collections;
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
    
    public enum Type
    {
        Normal,
        Fly,
        Flower,
        Small
    }

    private void Start()
    {
        if (type == Type.Fly) flyAnimation.wrapMode = WrapMode.Loop;
        splashAnimation.wrapMode = WrapMode.Once;
        bounceAnimation.wrapMode = WrapMode.Once;
    }

    private void Update()
    {
        if (GameManager.Instance.Playing)
            transform.Translate(Vector3.back * Time.deltaTime * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "LillyDeadZone")
        {
            gameObject.SetActive(false);
        }
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
        return (Type)Random.Range(0, 3);
    }
}
