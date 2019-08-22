using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateTextureOffset : MonoBehaviour
{
    [SerializeField]
    private Material material;
    [SerializeField]
    [Range(0, 2)]
    private float speed = 1;
    [SerializeField]
    private Color color;

    void Update()
    {
        material.mainTextureOffset = new Vector2(material.mainTextureOffset.x,
            material.mainTextureOffset.y + Time.deltaTime * speed);
    }
}
