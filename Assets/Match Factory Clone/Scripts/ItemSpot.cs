using UnityEngine;

public class ItemSpot : MonoBehaviour
{
    [Header("Settings")] 
    private Item _item;

    public void Populate(Item item)
    {
        this._item = item;
        item.transform.SetParent(transform);
    }
    public bool IsEmpty() =>_item == null;
}