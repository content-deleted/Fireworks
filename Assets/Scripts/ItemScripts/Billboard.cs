using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    void Start () {
        transform.LookAt(Camera.main.transform);
        GetComponent<Renderer>().material.renderQueue = 4999;
    }
    void LateUpdate() {
        GetComponent<Renderer>().material.renderQueue = 4999;
    }
}
