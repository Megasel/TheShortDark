using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    PlayerAnimationsController playerAnimationsController;
    Animator handsAnimator;
    PlayerMoveController moveController;

    private void OnEnable()
    {
        moveController = FindObjectOfType<PlayerMoveController>();

        playerAnimationsController = FindObjectOfType<PlayerAnimationsController>();
        handsAnimator = playerAnimationsController.notSkeletPlayerAnimator;
    }

    private void Update()
    {
        if (playerAnimationsController.isInHandItemBlocked) return;
        if (Input.GetMouseButtonDown(0))
        {
            handsAnimator.SetTrigger("KnifeHit");
            ShotRay();

        }
    }
    void ShotRay()
    {
        RaycastHit hit;
        Vector3 direction = (Camera.main.transform.position - Camera.main.transform.position).normalized;
        Ray ray = new Ray(Camera.main.transform.position, direction);
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100f, Color.red);


        if (Physics.Raycast(ray, out hit) && !hit.collider.CompareTag("Item"))
        {
            Debug.Log("Попадание в объект: " + hit.collider.name);
        }
    }
}
