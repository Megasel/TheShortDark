using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    private PlayerAnimationsController playerAnimationsController;
    private Animator handsAnimator;
    private PlayerMoveController moveController;
    private CharacterController characterController;    
    [SerializeField] private bool isActivated;
    [SerializeField] private Transform trapPart1;
    [SerializeField] private Transform trapPart2;
    [SerializeField] private float offsetY;
    [SerializeField] private ParticleSystem bloodParticle;
    [SerializeField] private GameObject bloodTrack;
    [SerializeField] private Collider trigger;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip placeClip;
    [SerializeField] private AudioClip catchClip;
    private void OnEnable()
    {
        moveController = FindAnyObjectByType<PlayerMoveController>();
        characterController = FindAnyObjectByType<CharacterController>();
        playerAnimationsController = FindAnyObjectByType<PlayerAnimationsController>();
        handsAnimator = playerAnimationsController.notSkeletPlayerAnimator;
    }

    private void Update()
    {
        if (playerAnimationsController.isInHandItemBlocked) return;
        if (Input.GetMouseButtonDown(0) && !isActivated)
        {
            isActivated = true;
            StartCoroutine(Place());
            handsAnimator.SetTrigger("TrapPlace");
            print(characterController.height);

        }
    }
    IEnumerator Place()
    {
        yield return new WaitForSeconds(3);
        audioSource.PlayOneShot(placeClip);
        transform.position = new Vector3(characterController.transform.position.x, characterController.transform.position.y + offsetY, characterController.transform.position.z);
        transform.eulerAngles = new Vector3(0, 0, 0); 
        trapPart1.localEulerAngles = new Vector3(0, 0, 0);
        trapPart2.localEulerAngles = new Vector3(0, 0, 0);
        transform.SetParent(null);
        yield return new WaitForSeconds(2);
        trigger.enabled = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        print(other.name);
        IDamagable damagable;
        if(isActivated && other.TryGetComponent(out damagable))
        {
            damagable.TakeDamage(80);
            Catch();
        }
    }
    void Catch()
    {
        audioSource.PlayOneShot(catchClip);
        bloodParticle.Play();
        Instantiate(bloodTrack,transform.position, Quaternion.identity);
        trapPart1.localEulerAngles = new Vector3(0, 0, 75);
        trapPart2.localEulerAngles = new Vector3(0, 0, -75);
    }
}
