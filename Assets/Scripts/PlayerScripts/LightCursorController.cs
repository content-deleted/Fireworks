using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCursorController : MonoBehaviour
{
    float time;
    void Update()
    {
        time += 0.1f;
        var scale = Mathf.Cos(time);
        transform.localScale = 2 * scale * Vector3.one;
    }
}
