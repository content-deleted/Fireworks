using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAtPlayer : MonoBehaviour
{
    public GameObject player;

    public Vector3 offset;

    public void Start () => transform.position = player.transform.position+offset;
}
