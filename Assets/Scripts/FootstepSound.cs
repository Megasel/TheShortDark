using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FootstepSound : MonoBehaviour
{
    [Header("Audio Clips for Surfaces")]
    [SerializeField] private List<GroundSound> groundSounds;   

    [SerializeField] private PlayerMoveController moveController;

    [Header("Audio Source")]
    public AudioSource audioSource;   

    private string currentSurfaceTag = "";    

    AudioClip stepClip;
    private void Update()
    {
        if (!moveController.isMoving && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
    public void PlayFootstep()
    {
        if (moveController.isMoving)
        {
            if (!audioSource.isPlaying)
            {

                audioSource.Play();


            }
        }
        else
        {
            audioSource.clip = null;
            audioSource.Stop();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (!moveController.isMoving) return;

        string surfaceTag = other.tag;
        stepClip = GetStepClip(surfaceTag);
        audioSource.clip = stepClip;
        PlayFootstep();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == currentSurfaceTag)
        {
            currentSurfaceTag = "";
        }
    }

    private AudioClip GetStepClip(string surfaceTag)
    {
        foreach (GroundSound groundSound in groundSounds)
        {
            if(groundSound.tag.Equals(surfaceTag))
                return groundSound.audioClip;
        
        
        }
        return null;
    }
}
[Serializable]
class GroundSound
{
    public string tag;
    public AudioClip audioClip;
}
