using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class Inventory : MonoBehaviour
{
    public int InventorySize = 16;

    [SerializeField] private PlayerAnimationsController playerAnimationsController;
    [SerializeField] private PlayerUi playerUi;
    [SerializeField] private CameraShake camShake;
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private Transform cellParent;
    [SerializeField] private AudioSource usageAudioSource;
    [SerializeField] private AudioClip openClip;
    [SerializeField] private AudioClip closeClip;
    [SerializeField] private Metrics metrics;
    public GameObject usingUi;
    [SerializeField] private Image usingCircleSlider;
    [SerializeField] private TMP_Text needsNotification;
    public GameObject inHandPrefab;
    public bool isUsing;
    private Coroutine takeInHandCoroutine;
    [Header("Отладка инвентаря")]
    public List<InventoryItem> inventoryItems = new List<InventoryItem>();
    [SerializeField]private List<Cell> cells;
    [Header("Выбранный предмет")]
    [SerializeField] private ItemData selecteditemData;
    public Cell selectedCell;
    public TMP_Text selectedItemName;
    public Image selectedCellImage;
    [SerializeField] private Sprite emptySprite;
    public TMP_Text selectedItemDescription;
    public Button selectedItemUseButton;
    public Button selectedItemTakeInHandButton;
    public Button selectedItemDropButton;
    [SerializeField]private Image line;
    private void Start()
    {
        cells = new List<Cell>(GetComponentsInChildren<Cell>());

        if (cells.Count != InventorySize)
        {
            Debug.LogWarning("Количество ячеек не совпадает с размером инвентаря!");
        }
    }
    private void OnEnable()
    {
        usageAudioSource.PlayOneShot(openClip);
        playerAnimationsController.isInHandItemBlocked = true;
    }
    private void OnDisable()
    {
        usageAudioSource.PlayOneShot(closeClip);
        playerAnimationsController.isInHandItemBlocked = false;

    }
    public bool AddItem(ItemData item, int amount)
    {

        RecreateItem(item);

        foreach (var cell in cells)
        {
            if (cell.HasItem && cell.CurrentItemData.itemName == item.itemName)
            {
                int newAmount = cell.CurrentAmount + amount;

                if (newAmount <= item.maxStackSize)
                {
                    cell.SetItem(item, newAmount);
                    UpdateInspectorView();
                    return true;
                }
                else
                {
                    amount = newAmount - item.maxStackSize;
                    cell.SetItem(item, item.maxStackSize);
                }
            }
        }

        foreach (var cell in cells)
        {
            if (!cell.HasItem)
            {
                cell.SetItem(item, amount);
                UpdateInspectorView();
                return true;
            }
        }

        Debug.Log("Инвентарь заполнен!");
        return false;
    }


    public void UseButton()=>StartCoroutine(UseItem());
    public void TakeInHandButton()
    {
        if(takeInHandCoroutine == null)
            takeInHandCoroutine = StartCoroutine(TakeInHand());
       
       

    }
    IEnumerator TakeInHand()
    {
        playerUi.isUiBlocked = true;
        print("startCoroutine");
        if (inHandPrefab != null && inHandPrefab != selecteditemData.prefab)
        {
            playerAnimationsController.SwitchAnimation();
            yield return new WaitForSeconds(0.5f);
            playerAnimationsController.StopAnimation();

            print("SWITCH 1 ");
            
            inHandPrefab = selecteditemData.prefab;
            playerAnimationsController.PlayUsingAnimation(selecteditemData);
        }

        if (inHandPrefab == null && inHandPrefab != selecteditemData.prefab)
        {
            print("SWITCH 2 ");
            playerAnimationsController.PlayUsingAnimation(selecteditemData);
            inHandPrefab = selecteditemData.prefab;
        }
        playerUi.isUiBlocked = false;
        takeInHandCoroutine = null;
    }
    IEnumerator UseItem()
    {
        if (selecteditemData == null) yield break;
        usingUi.SetActive(true);
        StartCoroutine(selecteditemData.Use(this, playerAnimationsController, metrics, usageAudioSource, usingCircleSlider, selecteditemData));
    }
    public void RemoveItem(Cell cell, int amount)
    {
        if (cell.HasItem)
        {
            int remaining = cell.CurrentAmount - amount;
            if (remaining <= 0) cell.ClearItem();
            else cell.SetItem(cell.CurrentItemData, remaining);

            UpdateInspectorView();
        }
    }
    public void SelectItem(Cell cell)
    {
        InventoryItem itemData = inventoryItems[cells.IndexOf(cell)];
        if (selectedCell != null) {
        selectedCell.DisableOutline();        
        }
        selecteditemData = itemData.itemData;
        selectedCellImage.sprite = itemData.itemData.itemIcon;
        selectedItemName.text = itemData.itemData.itemName;
        selectedItemDescription.text = itemData.itemData.itemDescription;
        selectedItemTakeInHandButton.gameObject.SetActive(false);
        selectedItemUseButton.gameObject.SetActive(false);
        if (itemData.itemData.forHand)
        {
            selectedItemTakeInHandButton.gameObject.SetActive(true);
        }
        else if (!itemData.itemData.notForUse)
        {
            selectedItemUseButton.gameObject.SetActive(true);
        }
        
        selectedItemDropButton.gameObject.SetActive(true);
        
        line.enabled = true;
        selectedCell = cell;
    }
    public void DeselectItem()
    {
        if (selectedCell != null)
        {
            selectedCell.DisableOutline();

        }
        selectedCellImage.sprite = emptySprite;
        selectedItemName.text = "";
        selectedItemDescription.text = "";
        selectedItemDropButton.gameObject.SetActive(false);
        selectedItemTakeInHandButton.gameObject.SetActive(false);
        selectedItemUseButton.gameObject.SetActive(false);
        selectedCell = null;
        line.enabled = false; 
    }
    public void UpdateInspectorView()
    {
        if (inventoryItems.Count != 24)
        {
            inventoryItems = new List<InventoryItem>(new InventoryItem[24]);     
        }


        for (int i = 0; i < cells.Count; i++)
        {
            if (cells[i].CurrentItemData != null)
            {
                inventoryItems[i].itemData = Instantiate(cells[i].CurrentItemData);    
                inventoryItems[i].amount = cells[i].CurrentAmount;
            }
            else
            {
                inventoryItems[i] = new InventoryItem     
                {
                    itemData = null,
                    amount = 0
                };
            }
        }

    }


    private void RecreateItem(ItemData itemData)
    {
        

    }
}
[Serializable]
public class InventoryItem
{
    public ItemData itemData;
    public int amount;  
}
