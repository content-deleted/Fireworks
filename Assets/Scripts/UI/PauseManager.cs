using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System;


public class PauseManager : MonoBehaviour
{
    public static PauseManager singleton;
    private GameObject PauseMenu;
    private GameObject HUD;
    private GameObject Eyetracker;
    private OVRScreenFade screenfade;
    void Awake() => ((Application.platform == RuntimePlatform.Android) ? GetComponent<OVRInputModule>() : GetComponent<StandaloneInputModule>() as MonoBehaviour).enabled = true;
    void Start()
    {
        // Get the gameobject references
        var rig = GameObject.Find("OurOVRrig");
        PauseMenu = rig.transform.Find("PauseCanvas").gameObject;
        Eyetracker = rig.transform.Find("TrackingSpace/CenterEyeAnchor").gameObject;
        HUD = Eyetracker.transform.Find("HudCanvas").gameObject;
        screenfade = Eyetracker.GetComponent<OVRScreenFade>();

        singleton = this;
        Paused = false;
    }
    private bool paused = false;
    
    public event Action onUnPause;
    public event Action onPause;
    public float TimeSpentPaused;
    private float pauseStartTime;
    public bool Paused {
        get => paused;
        set {
            PauseMenu.SetActive(value);
            HUD.SetActive(!value);
            Time.timeScale = (value) ? 0 : 1;
            holdTime = 0;

            if(value) {
                PlayerState.singleton.switchLocked = true;

                var f = Camera.main.transform.forward;
                f.y = 0;
                PauseMenu.transform.position = Camera.main.transform.position + f.normalized/5;
                PauseMenu.transform.LookAt(PauseMenu.transform.position + f.normalized);

                if(!paused) {
                    pauseStartTime = Time.unscaledTime;
                    onPause?.Invoke();
                }
            }
            else {
                StartCoroutine(unlockDelay());
                if(paused) {
                    TimeSpentPaused += Time.unscaledTime-pauseStartTime;
                    onUnPause?.Invoke();
                }
            }

            paused = value;
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
    
    public void ResetGame () {
        Paused = false;
        screenfade.FadeOut(() => SceneManager.LoadScene("Main", LoadSceneMode.Single));
    }
}
