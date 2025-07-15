using UnityEngine;

public class ItemSpot : MonoBehaviour
{
    [Header("Settings")] 
    private Item _item;
    public Item Item => _item;

    public void Populate(Item item)
    {
        this._item = item;
        item.transform.SetParent(transform);
        item.AssignSpot(this);
    }
    public bool IsEmpty() =>_item == null;

    public void Clear()
    {
        _item = null;
    }
}