using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager singleton;
    public bool debug = false;
    public enum elements {
        Sr,
        Cu,
        Ba,
        C,
        Na,
        Ca,
        S,
        Cl,
        K,
        N,
        O,
    }
    [Serializable]
    public class chemical {
        public List<elements> elements = new List<elements>();
        public Sprite chemSprite;
        public bool crafted = false;
    }
    public List<chemical> chemicals = new List<chemical>();
    public bool purpleCrafted = false;
    public bool canMoveOn = true;
    public Sprite purpleSprite;
    private GameObject dex;
    public bool gameStarted = false;
    bool [] collected = new bool[11];
    public Sprite [] UI_Sprites;
    void Awake()
    {
        if(singleton == null) singleton = this;
        else {
            DestroyImmediate(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        var Sprites = Resources.LoadAll<Sprite>($"UI_Sprite_");

        // debug for having all elements collected
        if(debug) setDebug();
    }
    public void setDebug() { for(int i = 0; i <11; i++) collected[i] = true; }
    void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        dex = GameObject.Find("OurOVRrig").transform.Find("PauseCanvas/PokeDex").gameObject;

        for(int i = 0; i < collected.Length; i++)
            if(collected[i])
                replaceDexEntry(i);
    }

    public static string getElementName(elements e) => Enum.GetName(typeof(elements),e);
    public bool hasCollected(elements e) => collected[(int)e];
    public void collect(elements e) {
        if(hasCollected(e)) return;
        
        collected[(int)e] = true;
        replaceDexEntry((int)e);
    }

    private void replaceDexEntry(int entry) {
        var g = dex.transform.Find($"Entry ({entry})");
        g.GetComponent<SpriteRenderer>().sprite = UI_Sprites[entry];
        g.localScale = Vector3.one * 4;
    }

    
    // Check if this was the last one
    public IEnumerator checkForAllChemicals () {
        yield return new WaitForEndOfFrame();
        var uncraftedChem = GameStateManager.singleton.chemicals.Where(x => !x.crafted);
        if(!uncraftedChem.Any()) {
            Debug.Log("no uncrafted");
            canMoveOn = true;
            PlayerState.singleton.transform.Find("Arrow").gameObject.SetActive(true);

            PlayerTextUI.singleton.helpMessages.Add("Congradulations! Looks like you're finished making all the fireworks!");
            PlayerTextUI.singleton.helpMessages.Add("Click on me when you're ready and I'll prepare them for launch.");
            PlayerTextUI.singleton.startPush();
        } else {
            var chemicalsLeft = uncraftedChem.Where(c => !c.elements.Where(e => !hasCollected(e)).Any()).Any();
            if(!chemicalsLeft) {
                Debug.Log("no avalible");
                canMoveOn = true;
                PlayerState.singleton.transform.Find("Arrow").gameObject.SetActive(true);
                
                PlayerTextUI.singleton.helpMessages.Add("Looks like thats all the types of fireworks you can make with elements you found.");
                PlayerTextUI.singleton.helpMessages.Add("You can try and find more next time but for now lets enjoy these colors!");
                PlayerTextUI.singleton.helpMessages.Add("Click on me when you're ready and I'll prepare them for launch.");
                PlayerTextUI.singleton.startPush();

            }
        }
    }

    // Just incase, I thought about abstracting the gamestate based on scene but its probably bad 
    //Scene scene = SceneManager.GetActiveScene();
}
