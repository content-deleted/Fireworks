using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectWhichToUse : MonoBehaviour
{
    public bool useWhenGameAlreadyStarted = true;
    void Awake()
    {
        var started = GameStateManager.singleton?.gameStarted ?? false;
        if(useWhenGameAlreadyStarted != started)
            GameObject.Destroy(this.gameObject);
    }
}
