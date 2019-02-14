using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UIDisplay : MonoBehaviour
{
    public static UIDisplay singleton;
    public void Awake() {
        foreach(GameObject g in elements) g.SetActive(false);
        singleton = this;
    }
    public List<GameObject> elements = new List<GameObject>();

    public GameObject PressAnyButton;

    public bool Locked = false;
    public bool Active = false;
    public int currentElement;
    public bool StartFromBeginning = true;
    public void Start () {
        if(StartFromBeginning) {
            Advance();
            Active = true;
        }
    }

    void Update()
    {
        if( (OVRInput.GetDown(OVRInput.Button.Any) || Input.GetKeyDown(KeyCode.Space)) && !Locked) Advance();
    }
    void Advance() {
        if(currentElement > elements.Count) return;
        if(currentElement > 0) elements[currentElement-1].SetActive(false);

        if(currentElement == elements.Count) {
            PressAnyButton.SetActive(false);
            Active = false;
            currentElement++;
            return;
        }

        elements[currentElement].SetActive(true);
        currentElement++;
    }
}
