using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : MonoBehaviour
{
    private LillyPad currentLillyPad;

    private void Update()
    {
        if (currentLillyPad != null)
        {
            transform.position = currentLillyPad.gameObject.transform.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "DeadZone")
        {
            GameManager.Instance.EndGame();
        }
    }

    public void JumpToLillyPad(LillyPad lillyPad)
    {
        transform.position = lillyPad.gameObject.transform.position;
        currentLillyPad = lillyPad;
    }
}
