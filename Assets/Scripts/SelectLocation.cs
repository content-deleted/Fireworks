using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectLocation : MonoBehaviour
{
    public Vector3 altPosition;
    void Start()
    {
        if(GameStateManager.singleton.gameStarted) {
            transform.localPosition = altPosition;
        }
    }
}
