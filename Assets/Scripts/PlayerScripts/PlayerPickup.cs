using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerPickup : MonoBehaviour {

    private Rigidbody rb;

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag ("Pick Up")) {
            other.gameObject.SetActive(false);
        }
    }
}