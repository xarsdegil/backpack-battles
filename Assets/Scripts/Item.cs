using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    private RectTransform rectTransform;
    private InventoryManager inventoryManager;
    private CanvasGroup canvasGroup;
    [SerializeField] Vector2Int size = Vector2Int.one;

    bool isVertical = true;

    Vector3 originalPos;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        inventoryManager = FindObjectOfType<InventoryManager>();
        canvasGroup = GetComponent<CanvasGroup>();
        originalPos = rectTransform.position;
        Debug.Log(originalPos);
    }

    public void SetOrientation(bool isVertical)
    {
        this.isVertical = isVertical;
        rectTransform.rotation = Quaternion.Euler(0, 0, isVertical ? 0 : 90);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(DragManager.instance.isDragging) return;


        canvasGroup.blocksRaycasts = false;
        DragManager.instance.StartDrag(gameObject);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(!DragManager.instance.isDragging) return;
        if(DragManager.instance.isSnapped) return;

        rectTransform.anchoredPosition += eventData.delta / inventoryManager.canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(!DragManager.instance.isDragging) return;
        

        DragManager.instance.EndDrag();
        canvasGroup.blocksRaycasts = true;

        if (DragManager.instance.isSnapped) return;
        rectTransform.anchoredPosition = originalPos;
    }

    public (float, float, bool) GetItemInfo()
    {
        return (size.x, size.y, isVertical);
    }

}
