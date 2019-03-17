using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UIDisplay : MonoBehaviour
{
    [System.Serializable]
    public class UIElement {
        public GameObject gameObject;
        public bool activeOnStart;
        public bool activeOnEnd;
        public bool runImmidiate;
        public string GameObjectFromResources;
    }

    public static UIDisplay singleton;
    public void Awake() {
        foreach(UIElement e in elements) e.gameObject.SetActive(false);
        singleton = this;
    }
    public List<UIElement> elements = new List<UIElement>();

    public GameObject PressAnyButton;

    private bool locked = false;
    public bool Locked {
        get => locked;
        set {
            locked = value;
            PressAnyButton.SetActive(!locked);
        }
    }
    private bool active = false;
    public bool Active {
        get => active;
        set => active = value;
    }
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
        if(currentElement > 0) elements[currentElement-1].gameObject.SetActive(elements[currentElement-1].activeOnEnd);

        if(currentElement == elements.Count) {
            PressAnyButton.SetActive(false);
            currentElement++;
            StartCoroutine( delayedActivation ());
            return;
        }

        elements[currentElement].gameObject.SetActive(elements[currentElement].activeOnStart);
        currentElement++;
        
        while(currentElement < elements.Count && elements[currentElement].runImmidiate){
            elements[currentElement].gameObject.SetActive(true);
            elements.RemoveAt(currentElement);
        }
    }

    IEnumerator delayedActivation () {
        yield return new WaitForSeconds(0.5f);
        Active = false;
    }
}
