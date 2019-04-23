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
        foreach((chemical c, int j) in GameStateManager.singleton.chemicals.Select((x, y)=> (x,y))){
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
                if(!chemComp.Any()) craftChemical(c,j);
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
    
    public static void craftChemical(chemical chem, int i){
        chem.crafted = true;
        showFeedback(chem.chemSprite);
        AudioManager.singleton.audioSource.PlayOneShot(AudioManager.singleton.correct, 0.7F);

        //update tv
        FireworksTVControl.updateTV(i);

        // clear combined
        combined = new List<elements>();

        // check if this was enough to make red and purple
        if(!GameStateManager.singleton.purpleCrafted 
        && GameStateManager.singleton.chemicals[2].crafted 
        && GameStateManager.singleton.chemicals[3].crafted)  {
            // craft purple 
            GameStateManager.singleton.purpleCrafted = true;
            var purple = ElementCombine.showFeedback(GameStateManager.singleton.purpleSprite, 8);
            purple.GetComponent<Billboard>().enabled = false;
            purple.transform.LookAt(purple.transform.position + Vector3.forward);

            //update tv
            FireworksTVControl.updateTV(5);
            
            // explanation
            PlayerTextUI.singleton.helpMessages.Add("Purple is created with a combination of red and blue light!");
            PlayerTextUI.singleton.helpMessages.Add("By mixing your strontium and copper componds you can have purple as well.");
            PlayerTextUI.singleton.startPush();
        }

        // defer to our next frame to check for all chemicals crafted
        GameStateManager.singleton.StartCoroutine(GameStateManager.singleton.checkForAllChemicals());
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
    public static GameObject showFeedback(Sprite sprite, float height = 4){
        var objClone = Instantiate(Resources.Load("UI_Display") as GameObject, t.position + Vector3.up * height, Quaternion.identity);
        objClone.GetComponent<SpriteRenderer>().sprite = sprite;
        Destroy(objClone, displayTime);
        return objClone;
    }
    
    private static Transform t;
    public void Start() => t = transform;
    
}
