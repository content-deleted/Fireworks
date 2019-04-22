using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameStateManager;
using System.Linq;
using UnityEngine.UI;
public class ElementCombine : MonoBehaviour
{
    public static List<elements> combined = new List<elements>();
    public static float displayTime = 5;

    /// <summary>
    /// Adds an element to the list of combined elements
    /// The we check if its valid or not and update the UI 
    /// Provides feedback and stores crafted elements
    /// </summary>
    /// <param name="Element to be added"></param>
    public static void addElement(elements e){
        combined.Add(e);
        bool valid = false;
        foreach(chemical c in GameStateManager.singleton.chemicals){
            // store a mutable copy of our list
            List<elements> chemComp = c.elements.ToList();

            // iterate through the list of combined elements and check if they are in the chemical
            // remove elements as we go to ensure that something with multiple of the same elements is handled
            for(int i =0; i< combined.Count; i++){
                if(!chemComp.Contains(combined[i])){
                    break;
                }
                else if(i==combined.Count-1)
                    valid=true;
                chemComp.Remove(combined[i]);
            }
            if(valid){
                // while something may be valid, it may not have all the requirements to create a full chemical
                if(!chemComp.Any()) craftChemical(c);
                break;
            }
        }
        if(!valid){
            // clear combined
            combined = new List<elements>();
            // show feedback
            showFeedback(Resources.Load<Sprite>("X"));
            AudioManager.singleton.audioSource.PlayOneShot(AudioManager.singleton.wrong, 0.7F);
        }

        // update feedback 
        t.GetChild(0).GetComponent<Text>().text = combined.Any() ? toString() : "";
    }
    
    public static void craftChemical(chemical chem){
        chem.crafted = true;
        showFeedback(chem.chemSprite);
        AudioManager.singleton.audioSource.PlayOneShot(AudioManager.singleton.correct, 0.7F);

        // clear combined
        combined = new List<elements>();
    }

    /// <summary>
    /// This method generates a string for the stored list of elements (combined)
    /// It counts duplicate elmeents together and formates them
    /// IE instead of printing ClBaCl -> BaCl2
    /// </summary>
    /// <returns> String version of current *chemical* </returns>
    public static string toString(){
        var sorted = combined.OrderBy(x => (int)x);
        string r = sorted.First().ToString();
        int inc = 0;
        for(int i = 1; i < sorted.Count(); i++){
            if(sorted.ElementAt(i-1) == sorted.ElementAt(i)) inc++;
            else {
                r += getElementName(sorted.ElementAt(i));
                if(inc>0) r += (inc+1).ToString();
            }
        }
        if(inc>0) r += (inc+1).ToString();
        return r;
    }

    /// <summary>
    /// Creates a new instance of the UI display 
    /// </summary>
    /// <param name="Sprite to set"></param>
    public static void showFeedback(Sprite sprite){
        var objClone = Instantiate(Resources.Load("UI_Display") as GameObject, t.position + Vector3.up * 4, Quaternion.identity);
        objClone.GetComponent<SpriteRenderer>().sprite = sprite;
        Destroy(objClone, displayTime);
    }
    
    private static Transform t;
    public void Start() => t = transform;
}
