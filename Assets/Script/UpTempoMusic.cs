using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpTempoMusic : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private float speedMultiplier = 1;
    
    void Update()
    {
        audioSource.pitch = GameManager.Instance.CurrentDifficulty * speedMultiplier;
    }
}
