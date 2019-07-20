using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LillyPad : MonoBehaviour
{
    public float speed;

    private void Update()
    {
        transform.Translate(Vector3.back * Time.deltaTime * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "LillyDeadZone")
        {
            gameObject.SetActive(false);
        }
    }
}
