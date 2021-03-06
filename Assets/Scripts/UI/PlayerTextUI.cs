using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public class PlayerTextUI : MonoBehaviour
{
    public static PlayerTextUI singleton;
    private Text textRender;

    private List<Image> images;
    public List<string> helpMessages = new List<string>();

    public float textSpeed;
    void Awake()
    {
        textRender = GetComponent<Text>();
        images = transform.parent.GetComponentsInChildren<Image>().ToList();
        singleton = this;
        var tempColor = Color.white;
        tempColor.a = 0f;
        foreach(Image i in images) i.color = tempColor;
        transform.parent.gameObject.SetActive(false);
    }
    public bool running=false;
    void OnEnable() {
        if(running) {
            textRender.text = "";
            StartCoroutine(push());
        }
    }
    public void startPush()
    {
        if (!gameObject.activeInHierarchy)
        {
            transform.parent.gameObject.SetActive(true);
            StartCoroutine(push());
        }
    }
    public int waitBetweenMessages = 200;
    IEnumerator push()
    {
        running = true;
        while (images[0].color.a < 1)
        {

            var temp = images[0].color;
            temp.a += 0.03f;
            foreach(Image i in images) i.color = temp;
            yield return new WaitForEndOfFrame();
        }
        
        while (helpMessages.Any())
            foreach (string text in helpMessages.ToList())
            {
                foreach (char c in text.ToCharArray())
                {
                    textRender.text = textRender.text + c;
                    yield return new WaitForSeconds(textSpeed);
                }

                yield return new WaitForSeconds(textSpeed * 50);
                helpMessages.Remove(text);
                textRender.text = "";
            }

        while (images[0].color.a > 0)
        {

            var temp = images[0].color;
            temp.a -= 0.03f;
            foreach(Image i in images) i.color = temp;
            yield return new WaitForEndOfFrame();
        }
        running = false;

        transform.parent.gameObject.SetActive(false);
    }
}