using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameCountDown : MonoBehaviour
{
    public int minutesStart = 8;
    private Text text;
    public void Start () => text = GetComponent<Text>();
    public void Update () {
        var t = ((8 * 60) - Time.timeSinceLevelLoad);
        text.text =  $"{(int)(t/60)}:{(int)(t%60)}";
    }
}
