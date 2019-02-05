using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* 
 * This is a singleton which stores stateful information on the player not contained in other classes
 */
public class PlayerState : MonoBehaviour
{
    public static GameObject Player;
    public static PlayerState singleton;

    public bool pointerMode = false;
    public GameObject PlayerControlModel;
    public GameObject PointerModel;

    public void Awake () {
        Player = gameObject;
        singleton = this;
    } 

    public void Update () {
        if(OVRInput.GetDown(OVRInput.Button.Back)) {
            pointerMode = !pointerMode;

            // Set the model to a different one
            PlayerControlModel.SetActive(!pointerMode);
            PointerModel.SetActive(pointerMode);
        }
    }
}
