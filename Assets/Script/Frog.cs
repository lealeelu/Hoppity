using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    private LillyPad currentLillyPad;
    private LillyPad jumpingLillyPad;
    private bool jumping = false;
    //This will reflect the accuracy of the jump so we can reward the player
    //with a faster jump
    private float jumpingSpeedBase = 2;
    private float distanceTraveled = 0;
    private Vector3 targetJumpLocation;
    private Vector3 oldLocation;
    private float animationLength = 0.625f;
    private float currentJumpTime = 0;
    
    private void Update()
    {
        if (jumping)
        {
            distanceTraveled += jumpingSpeedBase * Time.deltaTime * 10;
            GameManager.Instance.UpdateScore(distanceTraveled);

            if (currentJumpTime > animationLength)
            {
                currentLillyPad = jumpingLillyPad;
                jumping = false;
            }
            else
            {
                currentJumpTime += Time.deltaTime;
                transform.position = Vector3.Lerp(oldLocation, targetJumpLocation, currentJumpTime / animationLength);
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
        //MAYBE: add speed accuracy to jump
        if (currentLillyPad.lillyNumber + 1 != lillyPad.lillyNumber
            || jumping) return;

        //Setting up variables for new jump
        jumping = true;
        jumpingLillyPad = lillyPad;
        currentJumpTime = 0;
        oldLocation = transform.position;

        //calculate where the lilly will be in 0.625f
        targetJumpLocation = lillyPad.gameObject.transform.position + new Vector3(0, 0, -lillyPad.speed * animationLength);

        //face that direction
        transform.LookAt(targetJumpLocation);

        //set that as the jumptarget and ignore where the lilly is currently
        animator.SetTrigger("jump");
    }

    public void TeleportToLillyPad(LillyPad lillyPad)
    {
        distanceTraveled = 0;
        GameManager.Instance.UpdateScore(distanceTraveled);
        currentLillyPad = lillyPad;
    }
}
