using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Lamp : MonoBehaviour
{
    [SerializeField] private PlayerAnimationsController playerAnimationsController;
    [SerializeField] private Animator handsAnimator;
    [SerializeField] private Light light;
    [SerializeField] private bool isEnabled;
    [SerializeField] private bool isProcessing;
    [SerializeField] private AudioSource aud;
    [SerializeField] private AudioClip toggleClip;
    private void OnEnable()
    {
        playerAnimationsController = FindObjectOfType<PlayerAnimationsController>();
        handsAnimator = playerAnimationsController.notSkeletPlayerAnimator;
        playerAnimationsController.animationEvents.OnAnimationEnded += ToggleLight;
        playerAnimationsController.animationEvents.OnLanternToggled += PlaySound;
    }
    private void OnDisable()
    {
        playerAnimationsController.animationEvents.OnAnimationEnded -= ToggleLight;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isProcessing && !playerAnimationsController.isInHandItemBlocked)
        {
            isProcessing = true;
            handsAnimator.SetTrigger("LampToggle");
            
        }
    }
    private void PlaySound()
    {
        aud.PlayOneShot(toggleClip);
    }
    private void ToggleLight()
    {
        if (!isEnabled)
        {
            isEnabled = true;
            light.enabled = true;
            
        }
        else
        {
            isEnabled = false;
            light.enabled = false;

        }
        isProcessing = false;
    }
}
