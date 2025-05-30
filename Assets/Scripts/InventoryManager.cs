using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] int inventoryWidth = 10;
    [SerializeField] int inventoryHeight = 6;
    [SerializeField] int slotSize = 50;
    [SerializeField] int slotPadding = 10;
    [SerializeField] GameObject slotPrefab;
    [SerializeField] RectTransform firstSlotPos;

    public Canvas canvas;

    HashSet<(int, int, bool)> grids = new HashSet<(int, int, bool)>();

    void Start()
    {
        CreateInventory();
    }

    private void CreateInventory()
    {
        for (int i = 0; i < inventoryWidth; i++)
        {
            for (int j = 0; j < inventoryHeight; j++)
            {
                GameObject slot = Instantiate(slotPrefab, firstSlotPos);
                RectTransform slotRect = slot.GetComponent<RectTransform>();
                slotRect.anchoredPosition = new Vector2(firstSlotPos.position.x + i * (slotSize + slotPadding), firstSlotPos.position.y + j * (slotSize + slotPadding));
                slotRect.sizeDelta = new Vector2(slotSize, slotSize);
                slot.name = $"Slot_{i}_{j}";
                slot.GetComponent<Slot>().SetSlot(i, j);
                grids.Add((i, j, false));
                slot.transform.SetParent(firstSlotPos.parent);
            }
        }

        FindCenterSlots();
        SetSlotOpacities();
    }

    private void FindCenterSlots()
    {
        int x = inventoryWidth / 2;
        int y = inventoryHeight / 2;
        for (int i = x - 2; i < x + 2; i++)
        {
            for (int j = y - 1; j < y + 1; j++)
            {
                grids.Remove((i, j, false));
                grids.Add((i, j, true));
            }
        }
    }

    private void SetSlotOpacities()
    {
        foreach (var grid in grids)
        {
            GameObject slot = GameObject.Find($"Slot_{grid.Item1}_{grid.Item2}");
            if (slot != null)
            {
                slot.GetComponent<Image>().color = new Color(1, 1, 1, grid.Item3 ? 1 : 0.3f);
            }
        }
    }

    public bool CanPlaceItem(Slot slot, GameObject draggingObject)
    {
        if (slot == null) return false;
        if(draggingObject == null) return false;
        if(grids.Contains((slot.GetX(), slot.GetY(), false))) return false;


        Item item = draggingObject.GetComponent<Item>();
        var (itemWidth, itemHeight, isVertical) = item.GetItemInfo();

        if (isVertical)
        {
            float temp = itemWidth;
            itemWidth = itemHeight;
            itemHeight = temp;
        }

        (int startX, int startY) = slot.GetSlot();

        for (int x = startX; x < startX + itemWidth; x++)
        {
            for (int y = startY; y < startY + itemHeight; y++)
            {
                if (x >= inventoryWidth || y >= inventoryHeight || y < 0 || x < 0 || grids.Contains((x, y, false)))
                {
                    return false;
                }
            }
        }

        return true;
    }

}