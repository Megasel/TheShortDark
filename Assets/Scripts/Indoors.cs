using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indoors : MonoBehaviour, IInteractable
{
    [SerializeField] private PlayerMoveController playerMoveController;
    [SerializeField] private bool isInside;
    [SerializeField] private Transform targetPosition;
    [SerializeField] private AudioSource outdoorsAmbientSound;
    [SerializeField] private AudioSource doorSound;
    [SerializeField] private GameObject fadeAnimation;
   
    public void Interact()
    {
        fadeAnimation.SetActive(true);
        doorSound.Play();
        if (isInside)
            StartCoroutine(GoInside());
        else
            StartCoroutine(GoOutside());
    }
    private IEnumerator GoInside()
    {
        yield return new WaitForSeconds(1.5f);
        playerMoveController.MoveInstantly(targetPosition);

        outdoorsAmbientSound.Stop();
        yield return new WaitForSeconds(1.5f);
        fadeAnimation.SetActive(false);

    }
    private IEnumerator GoOutside()
    {
        yield return new WaitForSeconds(1.5f);
        playerMoveController.MoveInstantly(targetPosition);
        print("!!!");
        outdoorsAmbientSound.Play();
        yield return new WaitForSeconds(1.5f);
        fadeAnimation.SetActive(false);
    }
}
