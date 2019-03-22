using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager singleton;
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
    [SerializeField]
    private GameObject dex;

    bool [] collected = new bool[11];
    Sprite [] UI_Sprites;
    void Awake()
    {
        if(singleton == null) singleton = this;
        else DestroyImmediate(gameObject);

        UI_Sprites = Resources.LoadAll<Sprite>($"UI_Sprite_");
    }
    public static string getElementName(elements e) => Enum.GetName(typeof(elements),e);
    public bool hasCollected(elements e) => collected[(int)e];
    public void collect(elements e) {
        if(hasCollected(e)) return;
        
        collected[(int)e] = true;
        var g = dex.transform.Find($"Entry ({(int)e})");
        
        g.GetComponent<SpriteRenderer>().sprite = UI_Sprites[(int)e];
        g.localScale = Vector3.one * 4;
    }

    // Just incase, I thought about abstracting the gamestate based on scene but its probably bad 
    //Scene scene = SceneManager.GetActiveScene();
}
