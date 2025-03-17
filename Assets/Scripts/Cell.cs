using NUnit.Framework.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Cell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Image cellImage;
    [SerializeField] public Image itemImage;
    [SerializeField] private TMP_Text itemAmount;
    [SerializeField] private Sprite emptySprite;

    [SerializeField] private Sprite defaultImage;
    [SerializeField] private Sprite outlinedImage;
    public ItemData CurrentItemData;
    public int CurrentAmount { get; private set; }
    public bool HasItem => CurrentItemData != null;

    private Inventory inventory;

    private void Awake()
    {
        inventory = FindObjectOfType<Inventory>();
    }

    public void RefreshUI()
    {
        if (itemImage == null || itemAmount == null) return;

        if (HasItem)
        {
            itemImage.sprite = CurrentItemData.itemIcon;
            itemImage.gameObject.SetActive(true);
            itemAmount.text = CurrentAmount > 1 ? CurrentAmount.ToString() : "";
        }
        else
        {
            itemImage.gameObject.SetActive(false);
            itemAmount.text = "";
        }

        inventory?.UpdateInspectorView();
    }

    public void SetItem(ItemData item, int amount)
    {
        CurrentItemData = item;
        CurrentAmount = amount;
        RefreshUI();
    }

    public void HideItem()
    {
        itemImage.sprite = emptySprite;
        itemAmount.text = "";
    }

    public void ClearItem()
    {
        if (!HasItem) return;

        Debug.Log($"Очищаем ячейку с предметом {CurrentItemData?.itemName}");

        CurrentItemData = null;
        CurrentAmount = 0;
        RefreshUI();
    }

    public void SwapItems(Cell targetCell)
    {
        ItemData tempItem = CurrentItemData;
        int tempAmount = CurrentAmount;
        if (inventory.selectedCell != null)
            inventory.selectedCell.DisableOutline();
        inventory.SelectItem(this);
        SetItem(targetCell.CurrentItemData, targetCell.CurrentAmount);
        targetCell.SetItem(tempItem, tempAmount);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
      
        if (CurrentItemData != null) {
            inventory.SelectItem(this);

            cellImage.sprite = outlinedImage;
        }
        else
        {
            inventory.DeselectItem();
        }
    }
    public void DisableOutline()
    {
        cellImage.sprite = defaultImage;
    }
    public void OnPointerEnter(PointerEventData eventData) { cellImage.color = new Color(cellImage.color.r, cellImage.color.g, cellImage.color.b, 1); }
    public void OnPointerExit(PointerEventData eventData) { cellImage.color = new Color(cellImage.color.r, cellImage.color.g, cellImage.color.b, 0.85f); }
}
