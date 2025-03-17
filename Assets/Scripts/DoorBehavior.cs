using System.Collections;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class DoorBehavior : MonoBehaviour, IInteractable
{
    [SerializeField] private float duration;
    [SerializeField] private float maxAngle;
    [SerializeField] private float minAngle;
    public bool isLocked;
    [SerializeField] private AudioClip openClip;
    [SerializeField] private AudioClip closeClip;
    [SerializeField] private float openDelay;
    [SerializeField] private float closeDelay;
    [SerializeField] private AudioClip lockClip;
    private bool isOpened;
    private bool opening = false;
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private AudioSource aud;
    private Coroutine openingCoroutine;
    public IEnumerator OpenDoor()
    {
        if(!isOpened && !opening)
        {
            opening = true;
            aud.clip = openClip;
            aud.PlayDelayed(openDelay);

            for (float i = 0; i < 1; i += Time.deltaTime / duration)
            {
                transform.parent.rotation = Quaternion.Lerp(
                    Quaternion.Euler(0, minAngle, 0),
                    Quaternion.Euler(0, maxAngle, 0),
                    curve.Evaluate(i));

                yield return null;
            }
            isOpened = true;
            opening = false;
        }
        else if(isOpened && !opening)
        {
            opening = true;
            aud.clip = closeClip;
            aud.PlayDelayed(closeDelay);
            for (float i = 0; i < 1; i += Time.deltaTime / duration)
            {
                transform.parent.rotation = Quaternion.Lerp(
                    Quaternion.Euler(0, maxAngle, 0),
                    Quaternion.Euler(0, minAngle, 0),
                    curve.Evaluate(i));

                yield return null;
            }
            isOpened = false;
            opening = false;
        }
        openingCoroutine = null;
    }
    public void Open()
    {
        if (isLocked)
        {
            aud.PlayOneShot(lockClip);
            return;
        }

        if (openingCoroutine == null)
            openingCoroutine = StartCoroutine(OpenDoor());
    }

    public void Interact()
    {
       Open();
    }
}
