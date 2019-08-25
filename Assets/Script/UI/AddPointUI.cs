using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AddPointUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI notificationText;
    [SerializeField]
    private Animation animation;
    
    public void ShowPoints(string text, Vector2 position)
    {
        this.transform.position = position;
        notificationText.text = text;
        notificationText.enabled = true;
        animation.Play("Open");
        StartCoroutine(WaitForAnimationFinished());
    }

    private IEnumerator WaitForAnimationFinished()
    {
        yield return new WaitForSeconds(animation.clip.length);
        animation.Play("Close");
        notificationText.enabled = false;
    }
}
