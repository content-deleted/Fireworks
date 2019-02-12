using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowUI : MonoBehaviour
{
    [SerializeField] public TextMesh customText;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            customText.gameObject.SetActive(true);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            customText.gameObject.SetActive(false);
        }
    }
}