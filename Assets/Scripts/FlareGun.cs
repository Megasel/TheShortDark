using System.Collections;
using UnityEngine;

public class FlareGun : MonoBehaviour
{
    [Header("Animators")]
    [SerializeField] private Animator handsAnimator;

    [Header("Player Controllers")]
    [SerializeField] private PlayerMoveController moveController;
    [SerializeField] private PlayerAnimationsController playerAnimationsController;

    [Header("Audio and Visual Effects")]
    [SerializeField] private AudioSource aud;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private AudioClip shotSound;
    [SerializeField] private GameObject projectile;
    [SerializeField] private float bulletForce;
    [Header("Weapon Characteristics")]
    [SerializeField] private float aimSpeed = 2f;
    [SerializeField] private float aimingRange = 30f;

    private bool isAiming = false;
    private bool isShooting = false;
    private Camera cam;

    private void OnEnable()
    {
        cam = Camera.main;
        moveController = FindObjectOfType<PlayerMoveController>();
        playerAnimationsController = FindObjectOfType<PlayerAnimationsController>();
        handsAnimator = playerAnimationsController.notSkeletPlayerAnimator;
    }

    private void Update()
    {
        if (playerAnimationsController.isInHandItemBlocked) return;
        HandleAiming();
        HandleShooting();
    }

 
    private void HandleAiming()
    {
        if (Input.GetMouseButtonDown(1))
        {
            StartAiming();
        }
        if (Input.GetMouseButtonUp(1))
        {
            StopAiming();
        }

        cam.fieldOfView = Mathf.MoveTowards(cam.fieldOfView, isAiming ? aimingRange : 60f, aimSpeed * Time.deltaTime);
    }

    private void StartAiming()
    {
        handsAnimator.SetBool("FlareAim", true);
        isAiming = true;
        moveController.sensitivity = 1;
        playerAnimationsController.aimCircle.enabled = false;
    }

    private void StopAiming()
    {
        playerAnimationsController.aimCircle.enabled = true;
        handsAnimator.SetBool("FlareAim", false);
        moveController.sensitivity = 2;
        isAiming = false;
    }

    private void HandleShooting()
    {
        if (Input.GetMouseButtonDown(0) && !isShooting && isAiming)
        {
            Shot();
        }
    }

    private void Shot()
    {
        Projectile();
        aud.PlayOneShot(shotSound);
        isShooting = true;
        handsAnimator.SetTrigger("FlareShot");



    }

    private void Projectile()
    {
        Vector3 direction = (bulletSpawnPoint.position - cam.transform.position).normalized;

        GameObject bullet = Instantiate(projectile, bulletSpawnPoint.position, Quaternion.identity);

        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
        if (bulletRigidbody != null)
        {
            bulletRigidbody.AddForce(direction * bulletForce, ForceMode.Impulse);
        }
        
    }
}
