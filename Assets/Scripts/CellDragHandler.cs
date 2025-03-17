using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CellDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Cell originalCell;   
    private Image draggingIcon;    
    [SerializeField]private Canvas canvas;    

    private void Awake()
    {
        originalCell = GetComponent<Cell>();       
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (originalCell == null || !originalCell.HasItem) return;   

        draggingIcon = new GameObject("DraggingIcon").AddComponent<Image>();
        draggingIcon.transform.SetParent(canvas.transform, false);
        draggingIcon.sprite = originalCell.itemImage.sprite;
        draggingIcon.preserveAspect = true;
        draggingIcon.raycastTarget = false;
        draggingIcon.rectTransform.localScale = new Vector3(1.5f,1.5f,1.5f);
        originalCell.HideItem();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggingIcon != null)
        {
            draggingIcon.transform.position = Input.mousePosition;     
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (draggingIcon != null)
        {
            Destroy(draggingIcon.gameObject);         
        }

        if (originalCell == null)
        {
            Debug.LogWarning("Ошибка: originalCell не найден!");
            return;
        }

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results)
        {
            Cell targetCell = result.gameObject.GetComponent<Cell>();

            if (targetCell != null && targetCell != originalCell)
            {
                originalCell.SwapItems(targetCell);
                return;
            }
        }

        originalCell.RefreshUI();
    }
}
