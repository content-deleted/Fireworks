using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowUI : MonoBehaviour
{
    [SerializeField] private GameObject customSprite;
    private GameObject objClone;

    Animator animator;

    private bool skip = false;

    private int FRAMESTOWAIT = 30;

    void Start() => animator = PlayerState.singleton.GetComponent<Animator>();
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            objClone = Instantiate(customSprite, other.transform.position + Vector3.up * 2, Quaternion.identity);
            animator.SetTrigger("PickUp");
            UIDisplay.singleton.Active = true;
            StartCoroutine(endPickup());
        }
    }

    IEnumerator endPickup()
    {
        if(skip)
        for (int i = 0; i <= FRAMESTOWAIT; i++)
        {
            yield return new WaitForEndOfFrame();
            animator.SetTrigger("PickDown");
           break;
        }

        UIDisplay.singleton.Active = false;
        Destroy(objClone);
    }

    void Update () {
        if(OVRInput.GetDown(OVRInput.Button.Any) || Input.GetKeyDown(KeyCode.Space) ) skip = true;
    }

    void OnTriggerExit(Collider other)
    {
        Destroy(gameObject);
    }
}