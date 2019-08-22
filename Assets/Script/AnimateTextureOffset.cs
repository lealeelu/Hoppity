using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateTextureOffset : MonoBehaviour
{
    [SerializeField]
    private Material material;
    [SerializeField]
    [Range(0, 2)]
    private float ySpeed = 1;
    [SerializeField]
    [Range(0, 2)]
    private float xSpeed = 1;

    private void Start()
    {
        material.mainTextureOffset = new Vector2(0,0);
    }

    void Update()
    {
        material.mainTextureOffset = new Vector2(material.mainTextureOffset.x + Time.deltaTime * xSpeed,
            material.mainTextureOffset.y + Time.deltaTime * ySpeed);
    }
}
