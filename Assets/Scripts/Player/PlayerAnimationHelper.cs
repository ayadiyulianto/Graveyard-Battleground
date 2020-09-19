using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHelper : MonoBehaviour
{
    PlayerMovement playerMovement;
    AudioSource audioSource;
    public AudioClip[] footstepSound;

    void Start()
    {
        playerMovement = transform.parent.GetComponent<PlayerMovement>();
        audioSource = GetComponent<AudioSource>();
    }

    void PlayFootStepAudio()
    {
        int n = Random.Range(1, footstepSound.Length);
        audioSource.clip = footstepSound[n];
        audioSource.PlayOneShot(audioSource.clip);
        footstepSound[n] = footstepSound[0];
        footstepSound[0] = audioSource.clip;
    }
}
