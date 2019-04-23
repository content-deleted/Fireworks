using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireworksTVControl : MonoBehaviour
{
    [System.Serializable]
    public class tvLine {
        public GameObject blank;
        public GameObject checkmark;
        public void enable() {
            blank.SetActive(false);
            checkmark.SetActive(true);
        }
    }
    public List<tvLine> setChem; 
    private static List<tvLine> chem;   
    public void Start () => chem = setChem;
    public static void updateTV ( int chemNum ) => chem[chemNum].enable();
}
