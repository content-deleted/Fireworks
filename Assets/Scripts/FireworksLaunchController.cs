using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FireworksLaunchController : MonoBehaviour
{
     [System.Serializable]
    public class fireworkMapping {
        public List<int> chemNumbers;
        public GameObject firework;
    }
    public List<fireworkMapping> mappings = new List<fireworkMapping>();
    private bool launched = false;
    void OnTriggerEnter()
    {
        if(!launched){
            launched = true;
            foreach(var f in mappings) {
                f.firework.SetActive(GameStateManager.singleton.chemicals.Where((x,i)=> f.chemNumbers.Contains(i)).Select(c => c.crafted).Aggregate((a,b) => a&&b));
            }
            StartCoroutine(movebutton());
        }
    }

    IEnumerator movebutton () {
        for(int i = 0; i<10; i++){
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.01f, transform.position.z);
            yield return new WaitForEndOfFrame();
        }
    }
}
