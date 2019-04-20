using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamNode : MonoBehaviour
{
    void OnTriggerStay(Collider other)  {
        if(other.tag == "Player") {
            NodeBasedCamera.currentNode = this.transform;
        }
    }
}
