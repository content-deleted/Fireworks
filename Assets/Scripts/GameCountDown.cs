using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameCountDown : MonoBehaviour
{
    public GameObject vrRig;
    public OVRScreenFade screenfade;
    public string fireworksWorkStationScene;
    public float minutesStart = 8;
    private Text text;
    public void Start () => text = GetComponent<Text>();
    public bool end=false;
    public void Update () {
        if(!end) {
            var t = ((minutesStart * 60) - Time.timeSinceLevelLoad);
            text.text =  $"{(int)(t/60)}:{((int)(t%60)).ToString("D2")}";
            if(t<=0) EndGame();
        }
    }

    public void EndGame() {
        end=true;

        screenfade.FadeOut(() => 
            SceneManager.LoadScene(fireworksWorkStationScene,LoadSceneMode.Single)
        );

        /*
        screenfade.StartTransition(() => {
    
            vrRig.transform.position = new Vector3(1.986026f,4.52f,8.795609f);
            vrRig.transform.rotation = Quaternion.identity;
            vrRig.transform.Rotate(0,180,0);
            vrRig.GetComponent<CameraControl>().enabled=false;
            vrRig.GetComponent<Rigidbody>().velocity = Vector3.zero;
        });*/
    }
}
