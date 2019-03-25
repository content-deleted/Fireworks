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
    public static void addElement(elements e)
    {
        combined.Add(e);
        bool valid = false;
        foreach(chemical c in GameStateManager.singleton.chemicals){
            List<elements> chemComp = c.elements.ToList();
            for(int i =0; i< combined.Count; i++){
                if(!chemComp.Contains(combined[i])){
                    break;
                }
                else if(i==combined.Count-1)
                    valid=true;
                chemComp.Remove(combined[i]);
            }
            if(valid){
                if(chemComp.Any()) break;
                else {
                    craftChemical(c);
                }
            }
        }
        if(!valid){
            // clear combined
            combined = new List<elements>();
            // show feedback
            var objClone = Instantiate(Resources.Load("UI_Display") as GameObject, t.position + Vector3.up * 4, Quaternion.identity);
            objClone.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("X");
            Destroy(objClone, displayTime);
        }

        // update feedback 
        t.GetChild(0).GetComponent<Text>().text = (combined.Any()) ? toString() : "";

    }
    
    public static void craftChemical(chemical chem) {
        chem.crafted = true;
        var objClone = Instantiate(Resources.Load("UI_Display") as GameObject, t.position + Vector3.up * 4, Quaternion.identity);
        objClone.GetComponent<SpriteRenderer>().sprite = chem.chemSprite;
        Destroy(objClone, displayTime);

        // clear combined
        combined = new List<elements>();
    }

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
    
    private static Transform t;
    public void Start() => t = transform;
}
