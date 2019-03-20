using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationIndicator : MonoBehaviour
{
    private Renderer rend;
    public Renderer playerRend;
    private bool showArrow = false;

    public float fadeSpeed = 0.05f;
    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material.SetFloat("_Alpha", 0);
    }
    public Vector3 rot;
    private float ticker;
    public float offsetSpeed;
    void LateUpdate()
    {
        ticker += offsetSpeed;
        if(!playerRend.isVisible) {
            showArrow = true;
            
            //put the arrow rotate around
            var dirToPlayer = transform.parent.InverseTransformDirection((playerRend.transform.position - transform.parent.position).normalized)/(10+(Mathf.Cos(ticker)+1) );
            transform.localPosition = new Vector3(dirToPlayer.x,dirToPlayer.y,0.25f);
            transform.LookAt(transform.parent.TransformDirection(-dirToPlayer*2),transform.parent.forward );
            transform.Rotate(rot,Space.Self);
        }
        else showArrow = false;

        var a = rend.material.GetFloat("_Alpha");

        if(showArrow && a < 1 ) rend.material.SetFloat("_Alpha", a+fadeSpeed);
        else if(a > 0) rend.material.SetFloat("_Alpha", a-fadeSpeed);
    }


    // Consider we may want to show if the char is on screen that they are behind an object
    bool onScreen(Transform t) {
        var screenPos = Camera.main.WorldToViewportPoint(t.position);
        return screenPos.x > 0 && screenPos.x < 1 && screenPos.y > 0 && screenPos.y < 1;
    }
}
