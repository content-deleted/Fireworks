using System.Collections;
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
       if(c.tag == "Player" && !open){
           StartCoroutine(moveShelves(true));
           StartCoroutine(moveShelves(false));
       } 
    }
    IEnumerator moveShelves (bool right) {
        if(right){
            while(bookRight.transform.position.z < rightStart+max) {
                if(this.transform.localPosition.z > 0.2671) this.transform.localPosition -= new Vector3(0,0,0.0001f);
                bookRight.transform.position += new Vector3(0,0,dt);
                yield return new WaitForSeconds(0.000001f);
            }
        }
        else {
            while(bookLeft.transform.position.z > leftStart-max) {
                bookLeft.transform.position -= new Vector3(0,0,dt);
                yield return new WaitForSeconds(0.000001f);
            }
        }
    }
}
