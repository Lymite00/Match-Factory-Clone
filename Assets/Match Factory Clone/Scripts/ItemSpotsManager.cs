using System;
using UnityEngine;

public class ItemSpotsManager : MonoBehaviour
{
    [Header("Elements")] 
    [SerializeField] private Transform itemSpot;

    [Header("Settings")] 
    [SerializeField] private Vector3 itemLocalPositionOnSpot;
    [SerializeField] private Vector3 itemLocalScaleOnSpot;

    private void Awake()
    {
        InputManager.itemClicked += OnItemClicked;
    }

    private void OnDestroy()
    {
        InputManager.itemClicked -= OnItemClicked;
    }

    private void OnItemClicked(Item item)
    {
        // 1. Turn the item as a child of the item spot
        
        // 2. Scale the item down, set its local position to 0,0,0
        
        // 3. Disable its shadow
        
        // 4. Disable its collider or physics
        
        item.transform.SetParent(itemSpot);

        item.transform.localPosition = itemLocalPositionOnSpot;
        item.transform.localScale = itemLocalScaleOnSpot;

        item.DisableShadows();
        
        item.DisablePhysics();
    }
}