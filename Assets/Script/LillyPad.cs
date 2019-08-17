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
    private Animator animator;
    [SerializeField]
    private Animation splashAnimation;
    [SerializeField]
    private Transform lillyTransform;
    [SerializeField]
    private Animation flyAnimation;
    
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
        animator.SetTrigger("Splash");
    }

    public Transform GetLillyTransform()
    {
        return lillyTransform;
    }
}
