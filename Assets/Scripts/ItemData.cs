
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public abstract class ItemData : ScriptableObject
{
    public string itemName;
    [TextArea(3,6)] public string itemDescription;
    public Sprite itemIcon;
    public int maxStackSize;
    public GameObject prefab;
    public bool forHand = false;
    public bool notForUse = false;
    public string animationName;
    public abstract IEnumerator Use(Inventory inventory, PlayerAnimationsController playerAnimationsController, Metrics metrics, AudioSource usageAudioSource, Image usingCircleSlider, ItemData selectedItemData);
}




