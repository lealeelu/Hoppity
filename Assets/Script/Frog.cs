using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private float splashPlayAheadPercent = 0.80f;

    private LillyPad currentLillyPad;
    private LillyPad jumpingLillyPad;
    private bool jumping = false;
    public bool canJump = true;
    private bool splashPlayed = false;
    //This will reflect the accuracy of the jump so we can reward the player
    //with a faster jump
    private float jumpingSpeedBase = 2;
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

            //Something with this timing logic is off.
            if (currentJumpTime > currentAnimationLength)
            {
                jumpingLillyPad.SplashAnimate();
                if (jumpingLillyPad.type == LillyPad.Type.Flower)
                {
                    animator.SetTrigger("Slip");
                    canJump = false;
                    StartCoroutine(WaitForSlipEnd());
                }
                if (jumpingLillyPad.type == LillyPad.Type.Fly)
                {
                    GameManager.Instance.flyCountBar.Increment();
                    if (GameManager.Instance.flyCountBar.ReachedMax())
                    {
                        ActivateSuperMode();
                        GameManager.Instance.flyCountBar.SetValue(0);
                    }
                }

                currentLillyPad = jumpingLillyPad;
                jumping = false;
                splashPlayed = false;
            }
            currentJumpTime += Time.deltaTime;
            if (!splashPlayed && currentJumpTime/currentAnimationLength > splashPlayAheadPercent)
            {
                AudioManager.Instance.PlaySplash();
                splashPlayed = true;
            }
            transform.position = Vector3.Lerp(oldLocation, targetJumpLocation, currentJumpTime / currentAnimationLength);
        }
        else
        {
            if (currentLillyPad != null)
            {
                transform.position = currentLillyPad.GetLillyTransform().position;
            }            
        }

        //Increase Jump Speed
        currentAnimationLength = startingAnimationLength * (1 - GameManager.Instance.CurrentDifficulty);
        animator.speed = 1 + GameManager.Instance.CurrentDifficulty;
        Debug.Log(string.Format("{0} {1}", currentAnimationLength, animator.speed));

    }

    IEnumerator WaitForSlipEnd()
    {
        //wait for the length of the slip animation
        yield return new WaitForSeconds(0.833f);
        canJump = true;
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
        if (lillyPad.lillyNumber < currentLillyPad.lillyNumber
            || lillyPad.lillyNumber > currentLillyPad.lillyNumber + 1
            || jumping
            || !canJump) return;

        //Setting up variables for new jump
        jumping = true;
        splashPlayed = false;
        jumpingLillyPad = lillyPad;
        currentJumpTime = 0;
        oldLocation = transform.position;

        //calculate where the lilly will be in 0.625f
        targetJumpLocation = lillyPad.GetLillyTransform().position + new Vector3(0, 0, -lillyPad.speed * currentAnimationLength);

        //face that direction
        transform.LookAt(targetJumpLocation);

        //set that as the jumptarget and ignore where the lilly is currently
        animator.SetTrigger("jump");
    }

    //This is run at the start of every game, so this is practicly the startgame()
    public void TeleportToLillyPad(LillyPad lillyPad)
    {
        //reset variables
        distanceTraveled = 0;
        currentAnimationLength = startingAnimationLength;
        animator.speed = 1;
        GameManager.Instance.UpdateScore(distanceTraveled);

        currentLillyPad = lillyPad;
        transform.position = currentLillyPad.GetLillyTransform().position;

        //face forward
        transform.LookAt(transform.position + new Vector3(0, 0, 5));
    }

    public void ActivateSuperMode()
    {

    }
}
