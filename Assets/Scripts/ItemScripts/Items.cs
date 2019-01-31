using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class Items : MonoBehaviour {
    List<GameObject> items = new List<GameObject>
    { prefab1, prefab2, prefab3, prefab4, prefab5};

    void Start() {
        Spawn();
    }

    void Spawn() {

        int spawnPointIndex = Random.Range(0, spawnPoints.Length);

        int objectIndex = Random.Range(0, items.Length);

        Instantiate(items[objectIndex], spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
    }
}