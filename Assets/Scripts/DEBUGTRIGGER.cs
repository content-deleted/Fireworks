using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DEBUGTRIGGER : MonoBehaviour
{
    public void OnTriggerEnter (Collider other) {
        if(other.tag == "Player"){
            GameStateManager.singleton.setDebug();
            SceneManager.LoadScene("FireworksCreation",LoadSceneMode.Single);
        }
    }
}
