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

    public int FRAMESTOWAIT = 300;

    void Start() => animator = PlayerState.singleton.GetComponent<Animator>();
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            objClone = Instantiate(customSprite, other.transform.position + Vector3.up * 4, Quaternion.identity);
            animator.SetTrigger("PickUp"); 
            PlayerState.singleton.GetComponent<Rigidbody>().velocity = Vector3.zero;
            UIDisplay.singleton.Active = true;
            StartCoroutine(endPickup());
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