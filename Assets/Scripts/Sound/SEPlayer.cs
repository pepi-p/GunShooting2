using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip[] clips;
    private AudioSource audioSource;
    
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(int idx)
    {
        Debug.Log(clips[idx].name);
        audioSource.PlayOneShot(clips[idx]);
    }
}
