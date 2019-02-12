using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowUI : MonoBehaviour
{
    [SerializeField] private Sprite customSprite;
    void OnTriggerEnter(Collider other)
    {
        Instantiate (customSprite, other.transform.position, other.transform.rotation);
    }
    void OnTriggerExit(Collider other)
    {
        Destroy(gameObject);
    }
}