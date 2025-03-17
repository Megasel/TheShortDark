using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public interface IInteractable
{
    void Interact();
   
}
public class ItemPicker : MonoBehaviour
{
    [SerializeField] private float raycastDistance = 3f;    
    [SerializeField] private LayerMask itemLayer;           
    [SerializeField] private Inventory inventory;
    [SerializeField] private PlayerAnimationsController playerAnimationsController;
    [SerializeField] private AnimationEvent animationEvent;
    [SerializeField] private Item pickedItem;
    [SerializeField] private PlayerUi playerUi;
    bool isPickingAvailable = true;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip lootClip;
    private Coroutine searchCoroutine;
    [Header("Search")]
    [SerializeField] private GameObject searchUi;
    [SerializeField] private Image searchSlider;
    [SerializeField] private GameObject foundedItemObject;
    [SerializeField] private Image foundedItemImage;
    [SerializeField] private TMP_Text foundedItemName;
    [SerializeField] private TMP_Text foundedItemDescription;
    [SerializeField] private AudioClip searchInClip;
    [SerializeField] private AudioClip searchOutClip;
    [SerializeField] private AudioClip getItemClip;
    private LootContainer curLootContainer;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryPickupItem();
        }
    }
    private void OnEnable()
    {
        animationEvent.OnItemCollected += DestroyPickedItem;
    }
    private void TryPickupItem()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, raycastDistance, itemLayer))
        {
            print(hit.collider.name);
            IInteractable interactable;
            if(hit.collider.gameObject.TryGetComponent(out interactable)){
                interactable.Interact();
                return;
            }
            LootContainer lootContainer;
            if(hit.collider.gameObject.TryGetComponent(out lootContainer) && searchCoroutine == null)
            {
                searchCoroutine = StartCoroutine(Search(lootContainer));
            }
            if (hit.collider.CompareTag("Item") && Input.GetKeyDown(KeyCode.E) && isPickingAvailable)
            {
                isPickingAvailable = false;
                
                pickedItem = hit.collider.gameObject.GetComponent<Item>();
                inventory.AddItem(pickedItem.itemData, 1);
                if (inventory.inHandPrefab == null)
                    playerAnimationsController.notSkeletPlayerAnimator.SetTrigger("Loot");
                else
                    DestroyPickedItem();
            }
        }
    }
    IEnumerator Search(LootContainer lootContainer)
    {
        playerUi.Toggle(searchUi);
        curLootContainer = lootContainer;
        float elapsedTime = 0f;
        lootContainer.Loot(inventory);
        audioSource.PlayOneShot(searchInClip);
        
        while (elapsedTime < 10)
        {
            elapsedTime += Time.deltaTime;
            searchSlider.fillAmount = elapsedTime / searchInClip.length;
            yield return null;
        }   
        searchSlider.fillAmount = 0;
        searchUi.SetActive(false);
        searchCoroutine = null;
        playerUi.Toggle(searchUi);
        playerUi.Toggle(foundedItemObject);
        ShowNextFoundedItem();
        yield return null;
    }
    public void ShowNextFoundedItem()
    {
        if (curLootContainer.items.Count != 0)
        {
            foundedItemImage.sprite = curLootContainer.items[0].itemData.itemIcon;
            foundedItemName.text = curLootContainer.items[0].itemData.itemName;
            foundedItemDescription.text = curLootContainer.items[0].itemData.itemDescription;
            audioSource.PlayOneShot(lootClip);
            curLootContainer.NextItem();
        }
        else
        {
            playerUi.Toggle(foundedItemObject);
            audioSource.PlayOneShot(searchOutClip);
            Destroy(curLootContainer);
        }
        
    }
    private void DestroyPickedItem()
    {
        audioSource.PlayOneShot(lootClip);
        Destroy(pickedItem.gameObject);
        pickedItem = null;
        isPickingAvailable = true;
    }
    private void OnDrawGizmos()
    {
        if (Camera.main == null) return;

        Gizmos.color = Color.green;

        Vector3 rayOrigin = Camera.main.transform.position;
        Vector3 rayDirection = Camera.main.transform.forward * raycastDistance;

        Gizmos.DrawLine(rayOrigin, rayOrigin + rayDirection);

        Gizmos.DrawSphere(rayOrigin + rayDirection, 0.1f);
    }
}
