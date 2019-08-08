using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private float JSIncreasePerSec = 0.01f;

    private LillyPad currentLillyPad;
    private LillyPad jumpingLillyPad;
    private bool jumping = false;
    //This will reflect the accuracy of the jump so we can reward the player
    //with a faster jump
    private float jumpingSpeedBase = 2;
    private float jumpSpeedIncreaseCounter;
    private float distanceTraveled = 0;
    private Vector3 targetJumpLocation;
    private Vector3 oldLocation;
    private float startingAnimationLength = 0.625f;
    private float currentAnimationLength = 0.625f;
    private float currentJumpTime = 0;
    
    private void Update()
    {
        if (!GameManager.Instance.Playing) return;
        if (jumping)
        {
            distanceTraveled += jumpingSpeedBase * Time.smoothDeltaTime * 10;
            GameManager.Instance.UpdateScore(distanceTraveled);

            if (currentJumpTime > currentAnimationLength)
            {
                currentLillyPad = jumpingLillyPad;
                jumping = false;
            }
            else
            {
                currentJumpTime += Time.deltaTime;
                transform.position = Vector3.Lerp(oldLocation, targetJumpLocation, currentJumpTime / currentAnimationLength);
            }
        }
        else
        {
            if (currentLillyPad != null)
            {
                transform.position = currentLillyPad.gameObject.transform.position;
            }
            //Every second we aren't jumping, increase the jump speed
            jumpSpeedIncreaseCounter += Time.smoothDeltaTime;
            if (jumpSpeedIncreaseCounter > 1f)
            {
                currentAnimationLength -= JSIncreasePerSec;
                animator.speed += JSIncreasePerSec;
                Debug.Log(string.Format("Animator Speed: {0} Animation Length: {1}", animator.speed, currentAnimationLength));
            }
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
        targetJumpLocation = lillyPad.gameObject.transform.position + new Vector3(0, 0, -lillyPad.speed * currentAnimationLength);

        //face that direction
        transform.LookAt(targetJumpLocation);

        //set that as the jumptarget and ignore where the lilly is currently
        animator.SetTrigger("jump");
    }

    //This is run at the start of every game, so this is practicly the startgame()
    public void TeleportToLillyPad(LillyPad lillyPad)
    {
        distanceTraveled = 0;
        currentAnimationLength = startingAnimationLength;
        animator.speed = 1;
        GameManager.Instance.UpdateScore(distanceTraveled);
        currentLillyPad = lillyPad;
        transform.position = currentLillyPad.gameObject.transform.position;
    }
}
