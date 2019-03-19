using UnityEngine;
public class PlayerTextUI : MonoBehaviour
{

   public GameObject HelpfulText;
    void Start()
    {
        GetComponent<PushText>();
        HelpfulText.SetActive(true);
        Destroy(HelpfulText);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<PlayerTextUI>().Start();
        }
    }

    void OnTriggerStay()
    {
        //TBD
    }

    void OnTriggerExit()
    {
        HelpfulText.SetActive(false);
    }
}