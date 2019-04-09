using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MsgTimer : MonoBehaviour
{

    [System.Serializable]
    public class message
    {
        public List<string> subMessages;
    }

    [SerializeField]
    public List<message> messages = new List<message> ();
    public float timeTillNextMessage = 10;
    public static float timer;

    public void Start() => timer = timeTillNextMessage;

    void Update() {
        timer-=Time.deltaTime;
        if(timer <= 0){
            Debug.Log("Pushed Msg");
            timer = timeTillNextMessage;
            var ourMessage = getNewMessage();
            PlayerTextUI.singleton.helpMessages.AddRange(ourMessage);
            PlayerTextUI.singleton.startPush();
        }
    }

    private List<string>  getNewMessage () {
        var rand = new System.Random();
        var notCollected = messages.Where((n, i) => !GameStateManager.singleton.hasCollected((GameStateManager.elements)i));
        return notCollected.ElementAt(rand.Next(notCollected.Count())).subMessages;
    }
}