using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioSlot : MonoBehaviour, IInteractable
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private List<ItemData> details;
    [SerializeField] private GameObject radioPrefab;
    public void Interact()
    {
        int foundedItems = 0;
        foreach(ItemData detail in details)
        {
            foreach(InventoryItem item in inventory.inventoryItems)
            {
                if (item != null)
                {

                    if (detail.prefab.Equals(item.itemData.prefab))
                    {
                        foundedItems++;
                        break;
                    }
                }
            }
        }
        if(foundedItems == details.Count)
        {
            Instantiate(radioPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }

    }

}
