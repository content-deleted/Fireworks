using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float speed;
    public Vector3 Axis;
    void Update() =>
        transform.Rotate(Axis * speed,Space.World);
}
