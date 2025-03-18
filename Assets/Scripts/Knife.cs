using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    PlayerAnimationsController playerAnimationsController;
    Animator handsAnimator;
    PlayerMoveController moveController;

    [SerializeField] private float rayDistance = 10f; // Добавляем переменную для регулировки дальности луча

    private void OnEnable()
    {
        moveController = FindObjectOfType<PlayerMoveController>();
        playerAnimationsController = FindObjectOfType<PlayerAnimationsController>();
        playerAnimationsController.animationEvents.OnAnimationEnded += ShotRay;

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
        Vector3 direction = moveController.cam.transform.forward; // Используем направление вперед от камеры
        Ray ray = new Ray(moveController.cam.transform.position, direction);
        IDamagable Idamagable;

        // Используем rayDistance для ограничения дальности луча
        if (Physics.Raycast(ray, out hit, rayDistance) && hit.collider.TryGetComponent(out Idamagable))
        {
            print(hit.collider.name);
            Idamagable.TakeDamage(40);
        }
    }

    // Метод для отображения луча в редакторе Unity
    private void OnDrawGizmos()
    {
        if (moveController != null && moveController.cam != null)
        {
            Gizmos.color = Color.red;
            Vector3 direction = moveController.cam.transform.forward;
            // Используем rayDistance для визуализации луча
            Gizmos.DrawLine(moveController.cam.transform.position, moveController.cam.transform.position + direction * rayDistance);
        }
    }
}