using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CampFire : MonoBehaviour, IInteractable
{
    [SerializeField] private VisualEffect fireEffect;
    [SerializeField] private Light fireLight;
    [SerializeField] private int burningTime;
    [SerializeField] private float temperatureMultiplier = 0.007f;
    [SerializeField] private Collider warmCollider;
    [SerializeField] private AudioSource aud;
    private Coroutine burningCoroutine;
    private bool isBurning;
    private Metrics metrics;
    private void Start()
    {
        metrics = FindAnyObjectByType<Metrics>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            metrics.temperatureMultiplier -= temperatureMultiplier;
            metrics.UpdateTriangles(MetricType.temperature,true);
        }
            
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            metrics.temperatureMultiplier += temperatureMultiplier;
            metrics.UpdateTriangles(MetricType.temperature, false);
        }
      

    }
    IEnumerator MakeFire()
    {
        ToggleVisuals(true);
        yield return new WaitForSeconds(burningTime);
        ToggleVisuals(false);
        burningCoroutine = null;
    }
    private void ToggleVisuals(bool isActive)
    {
        if (isActive)
            aud.Play();
        else
            aud.Stop();
        warmCollider.enabled = isActive;
        fireEffect.enabled = isActive;
        isBurning = isActive;
        fireLight.enabled = isActive;
    }
    public void Interact()
    {
        burningCoroutine ??= StartCoroutine(MakeFire());
    }
}
