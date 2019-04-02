using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MsgTimer : MonoBehaviour
{
    [SerializeField]
    public List<string> messages = new List<string>();
    void Awake()
    {
        var rand = new System.Random();
        var notCollected = messages.Where((n, i) => !GameStateManager.singleton.hasCollected((GameStateManager.elements)i));
        var ourMessage = notCollected.ElementAt(rand.Next(notCollected.Count()));
    }
}