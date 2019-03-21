using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PauseManager : MonoBehaviour
{
    public static PauseManager singleton;
    public GameObject PauseMenu;
    public GameObject HUD;
    void Start()
    {
        Paused = false;
        singleton = this;
    }
    private bool paused = false;
    public bool Paused {
        get => paused;
        set {
            paused = value;
            PauseMenu.SetActive(value);
            HUD.SetActive(!value);
            Time.timeScale = (value) ? 0 : 1;
            holdTime = 0;

            if(value) PlayerState.singleton.switchLocked = true;
            else StartCoroutine(unlockDelay());
        }
    }
    public IEnumerator unlockDelay () {
        yield return new WaitForSeconds(0.5f);
        PlayerState.singleton.switchLocked = false;
    }
    private float holdTime = 0;
    void Update()
    {
        if(!paused) {
            if(Input.GetKey(KeyCode.Escape) || OVRInput.Get(OVRInput.Button.Back)){
                holdTime++;
                if(holdTime>50){
                    Paused = true;
                }
            }
            else {
                holdTime=0;
            }
               
        }
        else {
            if(Input.GetKeyDown(KeyCode.Escape) || OVRInput.GetDown(OVRInput.Button.Back)) {
                Paused = false;
            }
        }
    }
}
