using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnGizmo : MonoBehaviour
{
    public float radius = 2f;
    public Color color = Color.red;

    void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, radius);
    }

}
