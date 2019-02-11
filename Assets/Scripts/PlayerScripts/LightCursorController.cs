using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCursorController : MonoBehaviour
{
    float time;
    public float Scale;
    public float Speed = 0.1f;
    void Update()
    {
        time += Speed;
        var scale = Mathf.Cos(time);
        transform.localScale = Scale * scale * Vector3.one;
    }
}
