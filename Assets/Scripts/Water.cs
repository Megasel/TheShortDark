using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Water", menuName = "Inventory/Water")]
[System.Serializable]
public class Water : ItemData
{
    public AudioClip usageSound;
    public float timeToUse;
    [Header("Характеристики воды")]
    public float thirstRestoreAmount;
    public float amount;

    public override IEnumerator Use(Inventory inventory, PlayerAnimationsController playerAnimationsController,
        Metrics metrics, AudioSource usageAudioSource, Image usingCircleSlider, ItemData selecteditemData)
    {
        float elapsedTime = 0f;
        if (metrics.Water < 0.99f)
        {
            inventory.isUsing = true;
            metrics.UpdateTriangles(MetricType.water, true);
            if (selecteditemData.animationName != "")
            {
                playerAnimationsController.PlayUsingAnimation(selecteditemData);
            }

            usageAudioSource.PlayOneShot(usageSound);

            float startWater = metrics.Water;     
            float targetWater = startWater + thirstRestoreAmount;    

            while (elapsedTime < timeToUse)
            {
                elapsedTime += Time.deltaTime;

                usingCircleSlider.fillAmount = elapsedTime / timeToUse;

                metrics.Water = Mathf.Lerp(startWater, targetWater, usingCircleSlider.fillAmount);

                Debug.Log($"Water: {metrics.Water} | Target: {targetWater} | Progress: {usingCircleSlider.fillAmount}");

                yield return null;
            }

            metrics.Water = targetWater;
            metrics.UpdateTriangles(MetricType.water, false);

            usingCircleSlider.fillAmount = 1f;
            inventory.RemoveItem(inventory.selectedCell, 1);
            inventory.DeselectItem();
            inventory.isUsing = false;
            inventory.usingUi.SetActive(false);
        }
    }
}