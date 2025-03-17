using UnityEngine;

public class FootstepSound : MonoBehaviour
{
    [Header("Audio Clips for Surfaces")]
    public AudioClip snowStep;     
    public AudioClip iceStep;      
    public AudioClip woodStep;      

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
        switch (surfaceTag)
        {
            case "SnowGround":
                return snowStep;
            case "Ice":
                return iceStep;
            case "Wood":
                return woodStep;
            default:
                return null;
        }
    }
}
