using UnityEngine;
public class PlayerTextUI : MonoBehaviour
{

    private GameObject bubble;
    void Start()
    {
        GameObject bubble = new GameObject();
        bubble.gameObject.SetActive(true);
        GetComponent<PushText>();
        Destroy(gameObject);
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
        bubble.gameObject.SetActive(false);
    }
}