using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lighter : MonoBehaviour
{
    [SerializeField] private PlayerAnimationsController playerAnimationsController;
    [SerializeField] private Animator handsAnimator;
    [SerializeField] private ParticleSystem fireParticle;
    private bool isProcessing;
    private void OnEnable()
    {
        fireParticle.Play();
        playerAnimationsController = FindObjectOfType<PlayerAnimationsController>();
        handsAnimator = playerAnimationsController.notSkeletPlayerAnimator;
        playerAnimationsController.animationEvents.OnAnimationEnded += Fire;
    }
    private void OnDisable()
    {
        playerAnimationsController.animationEvents.OnAnimationEnded -= Fire;

    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isProcessing && !playerAnimationsController.isInHandItemBlocked)
        {
            isProcessing = true;
            handsAnimator.SetTrigger("LighterFire");
        }
    }
    void Fire()
    {
        RaycastHit hit;
        Vector3 direction = Camera.main.transform.forward; // Используем направление вперед от камеры
        Ray ray = new Ray(Camera.main.transform.position, direction);
        isProcessing = false;
        IInteractable interactable;

        if (Physics.Raycast(ray, out hit, 10) && hit.collider.gameObject.TryGetComponent(out interactable))
        {
            interactable.Interact();
        }
    }
}
