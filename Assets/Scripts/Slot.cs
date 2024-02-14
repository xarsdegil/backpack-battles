using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    InventoryManager inventoryManager;

    private void Awake()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
    }

    int x;
    int y;

    public void SetSlot(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public int GetX()
    {
        return x;
    }
    public int GetY()
    {
        return y;
    }

    public (int, int) GetSlot()
    {
        return (x, y);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Mouse over slot" + x + " " + y);

        if (!DragManager.instance.isDragging) return;

        Debug.Log("Drag mouse over slot" + x + " " + y);

        float rand = Random.Range(0, 1000f);

        if(inventoryManager.CanPlaceItem(this, DragManager.instance.draggingObject))
        {
            DragManager.instance.isSnapped = true;
            var draggedItemRect = DragManager.instance.draggingObject.GetComponent<RectTransform>();
            var rect = GetComponent<RectTransform>();
            draggedItemRect.transform.SetParent(transform.parent);
            draggedItemRect.anchoredPosition = rect.anchoredPosition;
        }
        else
        {
            Debug.Log("Can't place item" + rand);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!DragManager.instance.isDragging) return;
        if (!DragManager.instance.isSnapped) return;

        var draggedItemRect = DragManager.instance.draggingObject.GetComponent<RectTransform>();
        draggedItemRect.transform.SetParent(transform.parent.parent);
        DragManager.instance.isSnapped = false;
    }

    public void OnMouseExit()
    {
        if (!DragManager.instance.isDragging) return;
    }

    
}
