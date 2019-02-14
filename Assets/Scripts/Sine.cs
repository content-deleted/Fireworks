using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sine : MonoBehaviour
{
    public Vector3 motion;
    private float activeTime;
    public float speed;
    void Update()
    {
        activeTime += speed;
        transform.position = transform.position + Mathf.Sin(activeTime) * motion;
    }
}
