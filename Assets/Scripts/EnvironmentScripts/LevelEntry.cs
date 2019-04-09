using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEntry : MonoBehaviour
{
    public string sceneName;
    public OVRScreenFade screenfade;
    private float timer;
    void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player"){
            timer++;
            Debug.Log(timer);
            if(timer>200)
                screenfade.FadeOut(() => 
                    SceneManager.LoadScene(sceneName,LoadSceneMode.Single)
                );
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player") timer = 0;
    }
}
