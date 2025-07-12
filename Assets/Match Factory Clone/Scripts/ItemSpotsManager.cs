using System;
using UnityEngine;

public class ItemSpotsManager : MonoBehaviour
{
    [Header("Elements")] 
    [SerializeField] private Transform itemSpotsParent;
    private ItemSpot[] _spots;

    [Header("Settings")] 
    [SerializeField] private Vector3 itemLocalPositionOnSpot;
    [SerializeField] private Vector3 itemLocalScaleOnSpot;

    private void Awake()
    {
        InputManager.itemClicked += OnItemClicked;

        StoreSpots();
    }
    
    private void OnDestroy()
    {
        InputManager.itemClicked -= OnItemClicked;
    }

    private void OnItemClicked(Item item)
    {
        if (!IsFreeSpotAvailable())
        {
            Debug.LogWarning("No free spots");
            return;
        }

        HandleItemClicked(item);
        // 1. Turn the item as a child of the item spot
        
        // 2. Scale the item down, set its local position to 0,0,0
        
        // 3. Reset the rotations
        
        // 4. Disable its shadow
        
        // 5. Disable its collider or physics
    }

    private void HandleItemClicked(Item item)
    {
        MoveItemToFirstFreeSpot(item);
    }

    private void MoveItemToFirstFreeSpot(Item item)
    {
        ItemSpot targetSpot = GetFreeSpot();
        if (targetSpot==null)
        {
            Debug.LogError("Target spot is null");
            return;
        }
        
        targetSpot.Populate(item);
        
        item.transform.SetParent(targetSpot.transform);

        item.transform.localPosition = itemLocalPositionOnSpot;
        item.transform.localScale = itemLocalScaleOnSpot;
        item.transform.localRotation = Quaternion.identity;

        item.DisableShadows();
        
        item.DisablePhysics();
    }

    private ItemSpot GetFreeSpot()
    {
        for (int i = 0; i < _spots.Length; i++)
        {
            if (_spots[i].IsEmpty())
            {
                return _spots[i];
            }
        }
        return null;
    }
    
    private void StoreSpots()
    {
        _spots = new ItemSpot[itemSpotsParent.childCount];
        
        for (int i = 0; i < itemSpotsParent.childCount; i++)
        {
            _spots[i] = itemSpotsParent.GetChild(i).GetComponent<ItemSpot>();
        }
    }
    
    private bool IsFreeSpotAvailable()
    {
        for (int i = 0; i < _spots.Length; i++)
        {
            if (_spots[i].IsEmpty())
            {
                return true;
            }
        }

        return false;
    }
}