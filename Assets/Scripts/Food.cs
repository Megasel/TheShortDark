using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Food", menuName = "Inventory/Food")]
[System.Serializable]
public class Food : ItemData
{
    public AudioClip usageSound;
    public float timeToUse;
    [Header("Характеристики еды")]
    public float hungerRestoreAmount;
    public float amount;

    public override IEnumerator Use(Inventory inventory, PlayerAnimationsController playerAnimationsController,
        Metrics metrics, AudioSource usageAudioSource, Image usingCircleSlider, ItemData selecteditemData)
    {
        float elapsedTime = 0f;
        if (metrics.Hunger < 0.99f)
        {
            inventory.isUsing = true;
            metrics.UpdateTriangles(MetricType.hunger, true);

            if (selecteditemData.animationName != "")
            {
                playerAnimationsController.PlayUsingAnimation(selecteditemData);
            }

            usageAudioSource.PlayOneShot(usageSound);

            float startHunger = metrics.Hunger;     
            float targetHunger = startHunger + hungerRestoreAmount;    

            while (elapsedTime < timeToUse)
            {
                elapsedTime += Time.deltaTime;

                usingCircleSlider.fillAmount = elapsedTime / timeToUse;

                metrics.Hunger = Mathf.Lerp(startHunger, targetHunger, usingCircleSlider.fillAmount);

                Debug.Log($"Hunger: {metrics.Hunger} | Target: {targetHunger} | Progress: {usingCircleSlider.fillAmount}");

                yield return null;
            }

            metrics.Hunger = targetHunger;
            metrics.UpdateTriangles(MetricType.water, false);

            usingCircleSlider.fillAmount = 1f;
            inventory.RemoveItem(inventory.selectedCell, 1);
            inventory.DeselectItem();
            inventory.isUsing = false;
            inventory.usingUi.SetActive(false);
        }
    }
}