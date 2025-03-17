using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarCrash : MonoBehaviour
{
    [SerializeField] PlayerMoveController player;
    [SerializeField] GameObject car;
    [SerializeField] GameObject crashedCar;
    [SerializeField] GameObject fullScreenAnimation;
    [SerializeField] private GameObject metrics;
    [SerializeField] private Canvas carUi;
    [SerializeField] private AudioSource aud;
    [SerializeField] private AudioClip carCrashClip;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            GetComponent<BoxCollider>().enabled = false;
            
            StartCoroutine(Crash());
        }
    }
    IEnumerator Crash()
    {
        aud.PlayOneShot(carCrashClip);
        fullScreenAnimation.SetActive(true);
        yield return new WaitForSeconds(3);
        crashedCar.SetActive(true);
        car.SetActive(false);
        player.gameObject.SetActive(true);
        player.enabled = false;
        Debug.Log("!");
        yield return new WaitForSeconds(2);
        metrics.SetActive(true);
        Destroy(carUi);
        player.enabled = true;
        gameObject.SetActive(false);
    }
}
