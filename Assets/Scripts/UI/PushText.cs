using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PushText : MonoBehaviour
{
    private Text textObject;
    
    void Start() {
        UIDisplay.singleton.Locked = true;
        textObject = GetComponent<Text>();
        StartCoroutine(push());
    }
    private bool skip = false;
    public float textSpeed;
    public List<string> messages = new List<string>();
    void Update () {
        if(OVRInput.GetDown(OVRInput.Button.Any) || Input.GetKeyDown(KeyCode.Space) ) skip = true;
    }
    
    public int waitBetweenMessages = 200;
    IEnumerator push()
    {
        foreach(string text in messages) {
            foreach(char c in text.ToCharArray()){
                if(skip) {
                    textObject.text = text;
                    skip = false;
                    break;
                }
                textObject.text = textObject.text+c;
                yield return new WaitForSeconds(textSpeed);
            }
            if(!text.Equals(messages.Last())){
                for(int i = 0; i < waitBetweenMessages; i++){
                    yield return new WaitForSeconds(textSpeed);
                    if(skip) {
                        skip = false;
                        break;
                    }
                }
                textObject.text = "";
            }
        }
        UIDisplay.singleton.Locked = false;
    }
}
