using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Heal", menuName = "Inventory/Heal")]
[Serializable]
public class Heal : ItemData
{
    public AudioClip usageSound;
    public float timeToUse;
    [Header("Характеристики лечения")]
    public float healRestoreAmount;

    public override IEnumerator Use(Inventory inventory, PlayerAnimationsController playerAnimationsController,
        Metrics metrics, AudioSource usageAudioSource, Image usingCircleSlider, ItemData selecteditemData)
    {
        float elapsedTime = 0f;
        
            inventory.isUsing = true;

            if (selecteditemData.animationName != "")
            {
                playerAnimationsController.PlayUsingAnimation(selecteditemData);
            }

            usageAudioSource.PlayOneShot(usageSound);

            float startHealth = metrics.Health;     
            float targetHealth = startHealth + healRestoreAmount;    

            while (elapsedTime < timeToUse)
            {
                elapsedTime += Time.deltaTime;

                usingCircleSlider.fillAmount = elapsedTime / timeToUse;

                metrics.Health = Mathf.Lerp(startHealth, targetHealth, usingCircleSlider.fillAmount);

                Debug.Log($"Health: {metrics.Health} | Target: {targetHealth} | Progress: {usingCircleSlider.fillAmount}");

                yield return null;
            }

            metrics.Health = targetHealth;

            usingCircleSlider.fillAmount = 1f;
            inventory.RemoveItem(inventory.selectedCell, 1);
            inventory.DeselectItem();
            inventory.isUsing = false;
            inventory.usingUi.SetActive(false);
        
    }
}