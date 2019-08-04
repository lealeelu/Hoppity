using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : MonoBehaviour
{
    private LillyPad currentLillyPad;
    private LillyPad jumpingLillyPad;
    private bool jumping = false;
    //This will reflect the accuracy of the jump so we can reward the player
    //with a faster jump
    private float jumpingSpeedBase = 2;
    private float distanceTraveled = 0;

    private void Update()
    {
        if (jumping)
        {
            distanceTraveled += jumpingSpeedBase * Time.deltaTime * 10;
            GameManager.Instance.UpdateScore(distanceTraveled);
            float jumpSpeed = jumpingSpeedBase *  jumpingLillyPad.speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, jumpingLillyPad.transform.position, jumpSpeed);
            if (Vector3.Distance(transform.position, jumpingLillyPad.transform.position) < 0.001f)
            {
                currentLillyPad = jumpingLillyPad;
                jumping = false;
            }
        }
        if (!jumping && currentLillyPad != null)
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

    public void JumpToLillyPad(LillyPad lillyPad, float speed)
    {
        //add speed accuracy to jump
        jumping = true;
        jumpingLillyPad = lillyPad;
    }

    public void TeleportToLillyPad(LillyPad lillyPad)
    {
        currentLillyPad = lillyPad;
    }
}
