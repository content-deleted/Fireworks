using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class HelpTexts : MonoBehaviour
{
    [SerializeField]
    public List<string> helpMessages = new List<string>();

    public bool triggerOnce;

    void Start()
    {
        helpMessages.Add("this is a test");
        helpMessages.Add("this is a test2");
        helpMessages.Add("this is a test3");

        for (int i = helpMessages.Count - 1; i >= 0; i--)
        {
            helpMessages.RemoveAt(i);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            PlayerTextUI.singleton.helpMessages.AddRange(helpMessages);
            PlayerTextUI.singleton.startPush();

            if (triggerOnce){
                Destroy(this);
            }
        }
    }
}