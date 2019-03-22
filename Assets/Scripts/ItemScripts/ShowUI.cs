using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ShowUI : MonoBehaviour
{
    [SerializeField] 
    private GameStateManager.elements element;
    private GameObject objClone;

    Animator animator;

    private bool skip = false;

    public int FRAMESTOWAIT = 300;

    void Start() => animator = PlayerState.singleton.GetComponent<Animator>();
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            objClone = Instantiate(Resources.Load($"{GameStateManager.getElementName(element)}_UI_Display") as GameObject, other.transform.position + Vector3.up * 4, Quaternion.identity);
            animator.SetTrigger("PickUp"); 
            PlayerState.singleton.GetComponent<Rigidbody>().velocity = Vector3.zero;
            UIDisplay.singleton.Active = true;
            StartCoroutine(endPickup());

            GameStateManager.singleton.collect(element);
        }
    }

    IEnumerator endPickup()
    {
        for (int i = 0; i <= FRAMESTOWAIT; i++)
        {
            yield return new WaitForSeconds(3);
            if(skip) break;
        }

        animator.SetTrigger("PickDown");
        UIDisplay.singleton.Active = false;
        Destroy(objClone);
        Destroy(gameObject);
    }

    void Update () {
        if(OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) || Input.GetKeyDown(KeyCode.Space) ) skip = true;
    }
}