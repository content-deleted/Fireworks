using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationIndicator : MonoBehaviour
{
    private List<Renderer> rend = new List<Renderer>();
    public Renderer playerRend;
    private bool showArrow = false;

    public float fadeSpeed = 0.05f;
    void Start()
    {
        rend.Add(transform.GetChild(0).GetComponent<Renderer>());
        rend.Add(transform.GetChild(0).GetChild(0).GetComponent<Renderer>());
        foreach(var ren in rend) ren.material.SetFloat("_Alpha", 0);
    }
    public Vector3 rot;
    private float ticker;
    public float offsetSpeed;
    public bool facePlayer;
    void LateUpdate()
    {
        ticker += offsetSpeed;
        if(!playerRend.isVisible && !UIDisplay.singleton.Active) {
            showArrow = true;
            
            //put the arrow rotate around
            var dirToPlayer = transform.parent.InverseTransformDirection((playerRend.transform.position - transform.parent.position).normalized)/(10+(Mathf.Cos(ticker)+1) );
            transform.localPosition = new Vector3(dirToPlayer.x,dirToPlayer.y,0.25f);
            
            var lookVec = transform.parent.InverseTransformDirection((playerRend.transform.position - transform.parent.position).normalized*5);
            lookVec.z = 0.25f;
            lookVec = transform.parent.TransformPoint(lookVec);
            transform.LookAt(lookVec,-Camera.main.transform.forward);
            //transform.Rotate(Vector3.up*180,Space.Self);
        }
        else showArrow = false;

        var a = rend[0].material.GetFloat("_Alpha");

        if(showArrow && a < 1 )  foreach(var ren in rend) ren.material.SetFloat("_Alpha", a+fadeSpeed);
        else if(a > 0)  foreach(var ren in rend) ren.material.SetFloat("_Alpha", a-fadeSpeed);
    }


    // Consider we may want to show if the char is on screen that they are behind an object
    bool onScreen(Transform t) {
        var screenPos = Camera.main.WorldToViewportPoint(t.position);
        return screenPos.x > 0 && screenPos.x < 1 && screenPos.y > 0 && screenPos.y < 1;
    }
}
