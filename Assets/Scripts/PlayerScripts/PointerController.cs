using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerController : MonoBehaviour
{
    const int layerNum = 1 << 9; 
    void Update()
    {
        if( PlayerState.singleton.pointerMode && OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) ){ 
            RaycastHit hit;
            // Test
            if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, layerNum))
            {
                Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.yellow);
            }
        }
    }
}
