using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {
    void OnCollisionEnter(Collision c){
        if(c.transform.tag == "Player")
            c.transform.parent = this.transform;
    }
    void OnCollisionExit(Collision c) {
        if(c.transform.tag == "Player"&& c.transform.parent == transform.parent)
            c.transform.parent = null;
    }
}
