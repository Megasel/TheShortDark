using SCPE;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public interface IDamagable
{
    void TakeDamage(float damage);
}
public enum MetricType
{
    hunger,
    water,
    temperature
}
public class Metrics : MonoBehaviour, IDamagable
{
    [Range(0,1)]public float _health = 1;
    [SerializeField][Range(0, 1)]private float _hunger = 1;
    [SerializeField][Range(0, 1)]private float _water = 1;
    [SerializeField][Range(0, 1)]private float _temperature = 1;

    public float hungerMultiplier;
    public float waterMultiplier;
    public float temperatureMultiplier;
    public float healthDecreaseRate = 0.1f; // Базовая скорость уменьшения здоровья

    [SerializeField] private PlayerMoveController moveController;

    [SerializeField] private Image hungerBar;
    [SerializeField] private Image waterBar;
    [SerializeField] private Image temperatureBar;

    [SerializeField] private Image hungerTriangle;
    [SerializeField] private Image waterTriangle;
    [SerializeField] private Image temperatureTriangle;

    [SerializeField] private Sprite triangleUp;
    [SerializeField] private Sprite triangleDown;

    [SerializeField] private Volume volume;
    private Danger dangerEffect;
    private LiftGammaGain liftGammaGain;
    [SerializeField] private AudioSource aud;
    [SerializeField] private AudioClip takeDamageClip;
    [SerializeField] private GameObject deathAnimation;
    private Coroutine deathCoroutine;
    private void Awake()
    {
        volume.profile.TryGet(out dangerEffect);
        volume.profile.TryGet(out liftGammaGain);
    }
    public void TakeDamage(float damage)
    {
        aud.PlayOneShot(takeDamageClip);
        Health -= damage;
    }
    public float Health
    {
        get => _health;
        set
        {
            _health = Mathf.Clamp(value, 0f, 1f);
            SetBars();
        }
    }

    public float Hunger
    {
        get => _hunger;
        set
        {
            _hunger = Mathf.Clamp(value, 0f, 1f);
            SetBars();
        }
    }

    public float Water
    {
        get => _water;
        set
        {
            _water = Mathf.Clamp(value, 0f, 1f);
            SetBars();
        }
    }

    public float Temperature
    {
        get => _temperature;
        set
        {
            _temperature = Mathf.Clamp(value, 0f, 1f);
            SetBars();
        }
    }
    
    private void Update()
    {
        Hunger -= Time.deltaTime * hungerMultiplier;
        Water -= Time.deltaTime * waterMultiplier;
        Temperature -= Time.deltaTime * temperatureMultiplier;

        int lowMetricsCount = 0;
        if (Hunger <= 0) lowMetricsCount++;
        if (Water <= 0) lowMetricsCount++;
        if (Temperature <= 0) lowMetricsCount++;

        if (Health <= 0 && deathCoroutine== null)
        {
            deathCoroutine = StartCoroutine(Death());
        }
        
            Health -= Time.deltaTime * healthDecreaseRate * lowMetricsCount;
            dangerEffect.size.value = 1 - Health;
            liftGammaGain.lift.value = new Vector4(1, Health, Health);
        
    }

    void SetBars()
    {
        hungerBar.fillAmount = Hunger;
        waterBar.fillAmount = Water;
        temperatureBar.fillAmount = Temperature;
    }

    public void Eat(float multiplier)
    {
        Hunger += multiplier;
    }

    public void Drink(float multiplier)
    {
        Water += multiplier;
    }

    public void WarmUp(float multiplier)
    {
        Temperature += multiplier;
    }
    public void UpdateTriangles(MetricType type, bool isUp)
    {
        switch (type) 
        {
            case MetricType.hunger:
                hungerTriangle.sprite = SwapTriangleSprite(isUp);
                break;
            case MetricType.water:
                waterTriangle.sprite = SwapTriangleSprite(isUp);
                break;
            case MetricType.temperature:
                temperatureTriangle.sprite = SwapTriangleSprite(isUp);
                break;


        }
    }
    private Sprite SwapTriangleSprite(bool isUp)
    {
        if (isUp)
            return triangleUp;
        else
            return triangleDown;
        
        
    }
    private IEnumerator Death()
    {
        moveController.camShake.enabled = false;

        moveController.enabled= false;
        deathAnimation.SetActive(true);
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(1);
    }
}
