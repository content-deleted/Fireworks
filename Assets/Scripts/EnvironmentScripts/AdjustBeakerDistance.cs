using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustBeakerDistance : MonoBehaviour
{
    void OnTriggerEnter (Collider c){
        Debug.Log(c.name);
        if(c.gameObject.layer == 9 ){
            Debug.Log("fuck");
        }
    }
}
