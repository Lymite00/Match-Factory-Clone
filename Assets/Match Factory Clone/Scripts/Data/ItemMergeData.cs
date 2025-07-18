using System.Collections.Generic;
using UnityEngine;

public struct ItemMergeData
{
    public EItemName itemName;
    public List<Item> items;

    public ItemMergeData(Item firstItem)
    {
        itemName = firstItem.ItemName;
        items = new List<Item>();
        items.Add(firstItem);
    }

    public void Add(Item item)
    {
        items.Add(item);
    }

    public bool CanMergeItems()
    {
        return items.Count >= 3;
    }
}