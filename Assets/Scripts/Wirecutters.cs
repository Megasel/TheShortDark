using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wirecutters : MonoBehaviour
{
    PlayerAnimationsController playerAnimationsController;
    Animator handsAnimator;
    PlayerMoveController moveController;
   [SerializeField] bool cooldown;
    private void OnEnable()
    {
        moveController = FindObjectOfType<PlayerMoveController>();
        playerAnimationsController = FindObjectOfType<PlayerAnimationsController>();
        handsAnimator = playerAnimationsController.notSkeletPlayerAnimator;
        playerAnimationsController.animationEvents.OnAnimationEnded += AnimEvent;
    }
    private void OnDisable()
    {
        playerAnimationsController.animationEvents.OnAnimationEnded -= AnimEvent;

    }
    void AnimEvent() {
        cooldown = false;

    }
    private void Update()
    {
        if (playerAnimationsController.isInHandItemBlocked) return;
        if (Input.GetMouseButtonDown(0) && !cooldown) 
        {
            cooldown = true;
            handsAnimator.SetTrigger("WirecuttersCut");
            ShotRay();
        
        }
    }
    void ShotRay()
    {
        RaycastHit hit;
        Vector3 direction = (transform.position - Camera.main.transform.position).normalized;


        Wire wire;
        if (Physics.Raycast(transform.position,direction, out hit) && !hit.collider.CompareTag("Item") && hit.collider.TryGetComponent(out wire))
        {
            print(hit.collider.name);
            wire.Cut();
        }
    }
}
