using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class Items : MonoBehaviour {
        public List<GameObject> items = new List<GameObject>();
        public List<Transform> spawnPoints = new List<Transform>();
        void Start() => Spawn();
        
        void Spawn() {

            int spawnPointIndex = Random.Range(0, spawnPoints.Count);

            int objectIndex = Random.Range(0, items.Count);

            Instantiate(items[objectIndex], spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
        }
}