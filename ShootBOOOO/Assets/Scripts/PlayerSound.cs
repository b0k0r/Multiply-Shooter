using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSound : MonoBehaviourPunCallbacks
{
    public AudioClip walkSound;
    public float footStepDelay = 1.0f;
    
    private float nextFootstep = 0;
    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S)
            || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W))
        {
            FootSteps();
        }
    }

    public void FootSteps()
    {
        nextFootstep -= Time.deltaTime;
        if (nextFootstep <= 0)
        {
            audioSource.PlayOneShot(walkSound, 0.2f);
            nextFootstep += footStepDelay;
        }
    }
}
