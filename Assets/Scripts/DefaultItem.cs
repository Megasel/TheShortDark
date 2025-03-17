using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "Default", menuName = "Inventory/Default")]
public class DefaultItem : ItemData
{
    public override IEnumerator Use(Inventory inventory, PlayerAnimationsController playerAnimationsController, Metrics metrics,
        AudioSource usageAudioSource, Image usingCircleSlider, ItemData selectedItemData)
    {
        throw new System.NotImplementedException();
    }
}
