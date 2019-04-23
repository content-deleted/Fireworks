using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager singleton;
    public AudioSource audioSource;
    public AudioClip wrong;
    public AudioClip correct;

    public void Start () {
        singleton = this;
        audioSource = GetComponent<AudioSource>();
        wrong = Resources.Load("wrong") as AudioClip;
        correct = Resources.Load("correct") as AudioClip;
    }
}