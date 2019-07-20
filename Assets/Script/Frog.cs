using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "DeadZone")
        {
            GameManager.Instance.EndGame();
        }
    }
}
