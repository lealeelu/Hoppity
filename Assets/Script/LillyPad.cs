using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LillyPad : MonoBehaviour
{
    [SerializeField]
    public float speed;
    [SerializeField]
    public int lillyNumber;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Transform lillyTransform;

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
        animator.SetTrigger("Splash");
    }

    public Transform GetLillyTransform()
    {
        return lillyTransform;
    }
}
