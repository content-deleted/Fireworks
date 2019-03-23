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
    public PointerController playerPointer;

    // Controls if the player can switch their modes
    public bool switchLocked = false;

    public void Awake () {
        Player = gameObject;
        singleton = this;
        setController();
    }

    // Move out
    public void Start () {
        OVRManager.display.displayFrequency = 72.0f;
    }

    public void Update () {
        if(!switchLocked && !playerPointer.creationMode && OVRInput.GetDown(OVRInput.Button.Back)) {
            pointerMode = !pointerMode;
            setController();
        }
    }

    public void setController () {
         // Set the model to a different one
        PlayerControlModel.SetActive(!pointerMode);
        PointerModel.SetActive(pointerMode);

        // Activate light cursor
        playerPointer.lightCursor.SetActive(pointerMode);
    }
}
