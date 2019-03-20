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