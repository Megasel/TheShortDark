using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperRifle : MonoBehaviour
{
    [SerializeField] Animator handsAnimator;
    [SerializeField] Animator gunAnimator;
    [SerializeField] PlayerMoveController moveController;
    [SerializeField] PlayerAnimationsController playerAnimationsController;
    private PlayerUi playerUi;
    private float sniperBlend = 0f;
    private float breathBlend = 0f;

    public float sniperBlendSpeed = 2f;
    public float breathBlendSpeed = 2f;
    public float aimSpeed = 2f;
    public float aimingRange = 2f;
    bool isAiming;
    bool isBreathHolding;
    private Camera cam;
    private ItemData itemData;
    [SerializeField] private AudioSource aud;
    [SerializeField] private ParticleSystem shotParticle;
    [SerializeField] private Transform bulletSpawnPoint;
    [Header("?????????????? ??????")]
    [SerializeField] private AudioClip emptyReload;
    public AudioClip reloadSound;
    public AudioClip chargingSound;
    public AudioClip shotSound;
    public AudioClip breathIn;
    public AudioClip breathOut;
    [SerializeField] private float reloadTime;
    private bool isShooting;
    private bool isReloaded = true;
    private bool isReloading;
    private Coroutine reloadingCoroutine;
    private Coroutine breathCoroutine;
    private void OnEnable()
    {
        itemData = GetComponent<Item>().itemData;
        playerUi = FindAnyObjectByType<PlayerUi>();
        moveController = FindObjectOfType<PlayerMoveController>();
        cam = moveController.cam;
        playerAnimationsController = FindObjectOfType<PlayerAnimationsController>();
        handsAnimator = playerAnimationsController.notSkeletPlayerAnimator;
        playerAnimationsController.animationEvents.OnGunReload += ReloadEvent;
    }
    private void OnDisable()
    {
        playerAnimationsController.animationEvents.OnGunReload -= ReloadEvent;
    }
    void ReloadEvent() => isReloading = false;
    void Update()
    {
        if (playerAnimationsController.isInHandItemBlocked || isReloading) return;
            if (moveController.isMoving)
            {
                sniperBlend = Mathf.MoveTowards(sniperBlend, 1f, sniperBlendSpeed * Time.deltaTime);
            }
            else
            {
                sniperBlend = Mathf.MoveTowards(sniperBlend, 0f, sniperBlendSpeed * Time.deltaTime);
            }        
            handsAnimator.SetFloat("SniperBlend", sniperBlend);
        if (Input.GetKeyDown(KeyCode.R) && !isAiming && reloadingCoroutine == null  && cam.fieldOfView ==60) 
        {
            reloadingCoroutine = StartCoroutine(Reload());
            
        }
       
        if (Input.GetMouseButtonDown(0) && !isShooting) {
            if (isReloaded && isAiming && cam.fieldOfView == aimingRange)
                StartCoroutine(Shot());
            else
                aud.PlayOneShot(emptyReload);
        
        }
        if (Input.GetMouseButtonDown(1))
        {
            playerUi.tutorial.ShowTutorialStep(TutorialStepNames.breath, true);

            handsAnimator.SetBool("SniperAiming", true);
            isAiming = true;
            moveController.sensitivity = 1;
            playerAnimationsController.aimCircle.enabled = false;
            playerUi.isUiBlocked = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            playerAnimationsController.aimCircle.enabled = true;
            handsAnimator.SetBool("SniperAiming", false);
            moveController.sensitivity = 2;
            playerUi.isUiBlocked = false;
            isAiming = false;
        }
        if (isAiming)
        {
            cam.fieldOfView = Mathf.MoveTowards(cam.fieldOfView, aimingRange, aimSpeed * Time.deltaTime);
        }
        else
        {
            cam.fieldOfView = Mathf.MoveTowards(cam.fieldOfView, 60, aimSpeed * Time.deltaTime);
        }

       
        
        if (Input.GetKeyDown(KeyCode.LeftAlt)) {
            isBreathHolding = true;
            playerUi.tutorial.ShowTutorialStep(TutorialStepNames.breath, false);
            if(breathCoroutine == null)
            {
                breathCoroutine = StartCoroutine(BreathIn());
            }
                
        }
        if (isBreathHolding) {

            breathBlend = Mathf.MoveTowards(breathBlend, 1f, breathBlendSpeed * Time.deltaTime);
        }
        else
        {
            breathBlend = Mathf.MoveTowards(breathBlend, 0f, breathBlendSpeed * Time.deltaTime);

        }
        handsAnimator.SetFloat("SniperBreath", breathBlend);
     
    }
    IEnumerator BreathIn()
    {
        aud.PlayOneShot(breathIn);
        yield return new WaitForSeconds(5);
        aud.PlayOneShot(breathOut);
        isBreathHolding = false;
        breathCoroutine = null;
    }
    IEnumerator Shot()
    {
        
        ShotRay();
       // print(transform.eulerAngles);
        aud.PlayOneShot(shotSound);
        isShooting = true;
        isReloaded = false;
        shotParticle.Play();
        handsAnimator.SetTrigger("SniperShot");
        yield return new WaitForSeconds(reloadTime);
        isShooting = false;
    }
    IEnumerator Reload()
    {
        playerUi.isUiBlocked = true;
        handsAnimator.SetTrigger("SniperReload");
        isReloading = true;
        aud.PlayOneShot(reloadSound);
        gunAnimator.SetTrigger("gun_reload");
        yield return new WaitForSeconds(2);
        isReloaded = true;
        playerUi.isUiBlocked = false;
        reloadingCoroutine = null;
    }
    void ShotRay()
    {
        RaycastHit hit;
        Vector3 direction = (bulletSpawnPoint.position - Camera.main.transform.position).normalized;
        Ray ray = new Ray(cam.transform.position, direction);
        //Birds 
        LandingSpotController birdsController = FindFirstObjectByType<LandingSpotController>();
        birdsController.ScareAll();
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100f, Color.red);
        IDamagable damagable;
       
        if (Physics.Raycast(bulletSpawnPoint.position,direction, out hit) && hit.collider.TryGetComponent(out damagable))
        {
            
            damagable.TakeDamage(30);
        }
    }

}
