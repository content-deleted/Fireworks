using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public string text;
    void Update () {
        if(OVRInput.GetDown(OVRInput.Button.Any) || Input.GetKeyDown(KeyCode.Space) ) skip = true;
    }
    IEnumerator push()
    {
        foreach(char c in text.ToCharArray()){
            if(skip) {
                textObject.text = text;
                break;
            }
            textObject.text = textObject.text+c;
            yield return new WaitForSeconds(textSpeed);
        }
        UIDisplay.singleton.Locked = false;
    }
}
