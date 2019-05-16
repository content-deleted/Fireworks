using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEntry : MonoBehaviour
{
    public string sceneName;
    public OVRScreenFade screenfade;
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player"){
                screenfade.FadeOut(() => 
                    SceneManager.LoadScene(sceneName,LoadSceneMode.Single)
                );
        }
    }
}
