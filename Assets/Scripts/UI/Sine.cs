using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sine : MonoBehaviour
{
    public Vector3 motion;
    private float activeTime;
    public float speed;
    public bool localMovement = false;
    void Update()
    {
        activeTime += speed;
        if(localMovement) transform.localPosition = transform.localPosition + Mathf.Sin(activeTime) * motion;
        else transform.position = transform.position + Mathf.Sin(activeTime) * motion;
    }
}
