using INab.BetterFog.URP;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal; // ��������� ��� ������ � ��������� �������������

public class Indoors : MonoBehaviour, IInteractable
{
    [SerializeField] private PlayerMoveController playerMoveController;
    [SerializeField] private Metrics metrics;
    [SerializeField] private bool isInside;
    [SerializeField] private Transform targetPosition;
    [SerializeField] private AudioSource outdoorsAmbientSound;
    [SerializeField] private AudioSource doorSound;
    [SerializeField] private GameObject fadeAnimation;
    [SerializeField] private List<EventObject> objectsOn;
    [SerializeField] private Volume volume; // ��������� Volume
     
    private BetterFogVolume fog; // ������ �������������

    private void Start()
    {
        // �������� ������ Vignette �� VolumeProfile
        if (volume != null && volume.profile != null)
        {
            volume.profile.TryGet(out fog); // �������� ������ � �������
        }
    }

    public void Interact()
    {
        fadeAnimation.SetActive(true);
        if (doorSound != null)
            doorSound.Play();
        foreach (EventObject eventObject in objectsOn)
        {
            eventObject.obj.SetActive(eventObject.setActive);
        }
        if (isInside)
            StartCoroutine(GoInside());
        else
            StartCoroutine(GoOutside());
    }

    private IEnumerator GoInside()
    {
        yield return new WaitForSeconds(1.5f);
        playerMoveController.MoveInstantly(targetPosition);
        metrics.temperatureMultiplier -= 0.002f;
        outdoorsAmbientSound.Stop();

        // ��������� ������ Vignette
        if (fog != null)
        {
            fog.active = false;
        }

        yield return new WaitForSeconds(1.5f);
        fadeAnimation.SetActive(false);
    }

    private IEnumerator GoOutside()
    {
        yield return new WaitForSeconds(1.5f);
        metrics.temperatureMultiplier += 0.002f;
        playerMoveController.MoveInstantly(targetPosition);
        print("!!!");
        outdoorsAmbientSound.Play();

        // �������� ������ Vignette
        if (fog != null)
        {
            fog.active = true;
        }

        yield return new WaitForSeconds(1.5f);
        fadeAnimation.SetActive(false);
    }
}

[Serializable]
class EventObject
{
    public GameObject obj;
    public bool setActive;
}