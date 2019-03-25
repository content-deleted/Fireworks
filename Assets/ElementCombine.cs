using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameStateManager;
using System.Linq;
public class ElementCombine : MonoBehaviour
{
    public static List<elements> combined = new List<elements>();
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
            //clear combined
            combined = new List<elements>();
        }
    }
    
    public static void craftChemical(chemical chem) {
        chem.crafted = true;
        var objClone = Instantiate(Resources.Load($"UI_Display") as GameObject, t.position + Vector3.up * 4, Quaternion.identity);
        objClone.GetComponent<SpriteRenderer>().sprite = chem.chemSprite;
        Destroy(objClone, 3);
    }
    
    private static Transform t;
    public void Start() => t = transform;
}
