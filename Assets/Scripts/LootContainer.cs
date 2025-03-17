using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootContainer : MonoBehaviour
{ 
    public List<InventoryItem> items;
    public void Loot(Inventory inventory)
    {
        foreach (InventoryItem item in items) 
        {
            inventory.AddItem(item.itemData, item.amount);   
        }
    }
    public void NextItem()
    {
        items.RemoveAt(0);
    }
   
}
