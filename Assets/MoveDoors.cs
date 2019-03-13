﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDoors : MonoBehaviour
{
    public GameObject bookRight;
    public GameObject bookLeft;

    public float max = 1;
    public float dt = 0.001f;

    private float leftStart;
    private float rightStart;
    void Start()
    {
        rightStart=bookRight.transform.position.z;
        leftStart=bookLeft.transform.position.z;
    }
    public bool open = false;

    void OnTriggerEnter(Collider c) {
        Debug.Log("coll");
       if(c.tag == "Player" && !open){
           StartCoroutine(moveShelves(true));
           StartCoroutine(moveShelves(false));
       } 
    }
    IEnumerator moveShelves (bool right) {
        Debug.Log("start");
        if(right){
            while(bookRight.transform.position.z < rightStart+max) {
                Debug.Log("coright");
                bookRight.transform.position += new Vector3(0,0,dt);
                yield return new WaitForSeconds(0.000001f);
            }
        }
        else {
            while(bookLeft.transform.position.z > leftStart-max) {
                Debug.Log("coleft");
                bookLeft.transform.position -= new Vector3(0,0,dt);
                yield return new WaitForSeconds(0.000001f);
            }
        }
    }
}