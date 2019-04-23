using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireworkMaterial : MonoBehaviour
{
    public Transform spawnLocation;
    public GameStateManager.elements element;
    private Rigidbody rigidbody;

    public AudioClip poof;
    AudioSource audioSource;
    void Start()
    {
        // disable if we dont have this element
        if(!GameStateManager.singleton.hasCollected(element)) {
            var label = GameObject.Find($"UI_Display_{GameStateManager.getElementName(element)}");
            gameObject.SetActive(false);
            label.SetActive(false);
        }

        rigidbody = GetComponent<Rigidbody>();

        audioSource = GetComponent<AudioSource>();
        poof = Resources.Load("poof") as AudioClip;
    }

    private bool inArea = true;

    void OnTriggerEnter (Collider other) {
        switch(other.tag){
            case "TableArea":
                inArea = true;
                break;
            case "Burner":
                if(PointerController.singleton.heldObject !=null) PointerController.singleton.disableLine();
                ElementCombine.addElement(element);
                Respawn();
                break;
            case "Adjuster":
                adjust(other);
                break;
        }
    }

    public void adjust(Collider other) {
        Vector3 offset = 0.05f * PointerController.singleton.transform.forward;
        if(transform.position.z + 0.1f < other.transform.position.z) PointerController.singleton.grabPoint.transform.localPosition += offset;
        else if(transform.position.z - 0.1f > other.transform.position.z) PointerController.singleton.grabPoint.transform.localPosition -= offset;
    }

    void OnTriggerStay (Collider other) {
        if(other.tag == "TableArea") inArea = true;
        switch(other.tag){
            case "TableArea":
                inArea = true;
                break;
            case "Burner":
                break;
            case "Adjuster":
                adjust(other);
                break;
        }
    }

    void OnCollisionEnter(Collision c) {
        if(!inArea) Respawn();
    }
    void OnCollisionStay(Collision c) {
        if(!inArea) Respawn();
    }

    void OnTriggerExit (Collider other) {
        if(other.tag == "TableArea") inArea = false;
        audioSource.PlayOneShot(poof, 0.7F);
    }

    public void Respawn() {
        inArea = true;
        Instantiate(Resources.Load("ParticleBurst"),transform.position,Quaternion.identity);
        transform.position = spawnLocation.position + Vector3.up*0.01f;
        transform.rotation = Quaternion.identity;
        transform.Rotate(new Vector3(-90,0,-90));
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        Instantiate(Resources.Load("ParticleBurst"),transform.position,Quaternion.identity);
    }
}
