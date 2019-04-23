using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulse : MonoBehaviour {
    public float speed = 10.0f;
    public Vector3 axis = new Vector3(1,1,1);
    public Vector3 offset = new Vector3(2,2,2); 
    public bool UseOriginalScale = false;
    private float ticks;
    void Start () {
        if(UseOriginalScale) offset = Vector3.Scale(offset, transform.localScale);
    }
	void Update () {
        ticks += Time.deltaTime * speed;
		transform.localScale = offset + axis * Mathf.Cos(ticks);
	}
}
