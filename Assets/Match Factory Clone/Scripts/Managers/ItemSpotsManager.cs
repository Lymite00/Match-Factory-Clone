using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpotsManager : MonoBehaviour
{
    [Header("Elements")] [SerializeField] private Transform itemSpotsParent;
    private ItemSpot[] _spots;

    [Header("Settings")] [SerializeField] private Vector3 itemLocalPositionOnSpot;
    [SerializeField] private Vector3 itemLocalScaleOnSpot;
    private bool isBusy;

    [Header("Data")]
    private Dictionary<EItemName, ItemMergeData> itemMergeDataDictionary = new Dictionary<EItemName, ItemMergeData>();

    [Header("Animation Settings")] 
    [SerializeField] private float animationDuration;
    [SerializeField] private LeanTweenType animationEasing;

    [Header("Actions")] 
    public static Action<List<Item>> mergerStarted;
    public static Action<Item> itemPickedUp;
    
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
        if (isBusy)
        {
            Debug.LogWarning("Item spots manager is busy");
            return;
        }

        if (!IsFreeSpotAvailable())
        {
            Debug.LogWarning("No free spots");
            return;
        }

        isBusy = true;

        itemPickedUp?.Invoke(item);
        
        HandleItemClicked(item);

        // 1. Turn the item as a child of the item spot

        // 2. Scale the item down, set its local position to 0,0,0

        // 3. Reset the rotations

        // 4. Disable its shadow

        // 5. Disable its collider or physics
    }

    private void HandleItemClicked(Item item)
    {
        if (itemMergeDataDictionary.ContainsKey(item.ItemName))
            HandeItemMergeDataFound(item);
        else
            MoveItemToFirstFreeSpot(item);
    }

    private void HandeItemMergeDataFound(Item item)
    {
        ItemSpot idealSpot = GetIdealSpot(item);
        itemMergeDataDictionary[item.ItemName].Add(item);

        TryMoveItemToIdealSpot(item,idealSpot);
    }
    
    private ItemSpot GetIdealSpot(Item item)
    {
        List<Item> items = itemMergeDataDictionary[item.ItemName].items;
        List<ItemSpot> itemSpots = new List<ItemSpot>();

        for (int i = 0; i < items.Count; i++)
        {
            itemSpots.Add(items[i].Spot);
        }

        if (itemSpots.Count >= 2)
        {
            itemSpots.Sort((a ,b) => b.transform.GetSiblingIndex().CompareTo(a.transform.GetSiblingIndex()));
        }

        int idealSpotIndex = itemSpots[0].transform.GetSiblingIndex() + 1;

        return _spots[idealSpotIndex];
    }
    
    private void TryMoveItemToIdealSpot(Item item, ItemSpot targetSpot)
    {
        if (!targetSpot.IsEmpty())
        {
            HandleIdealSpotFull(item, targetSpot);
            return;
        }

        MoveItemToSpot(item, targetSpot, () => HandleItemReachedSpot(item));
    }

    private void MoveItemToSpot(Item item, ItemSpot idealSpot, Action completeCallback)
    {
        idealSpot.Populate(item);

        //item.transform.SetParent(idealSpot.transform);

        //item.transform.localPosition = itemLocalPositionOnSpot;
        //item.transform.localScale = itemLocalScaleOnSpot;
        //item.transform.localRotation = Quaternion.identity;

        LeanTween.moveLocal(item.gameObject, itemLocalPositionOnSpot, animationDuration).setEase(animationEasing);
        LeanTween.scale(item.gameObject, itemLocalScaleOnSpot, animationDuration).setEase(animationEasing);
        LeanTween.rotateLocal(item.gameObject, Vector3.zero, animationDuration)
            .setOnComplete(completeCallback);

        item.DisableShadows();

        
        item.DisablePhysics();
        

        //HandleItemReachedSpot(item, checkForMerge);
    }

    private void HandleItemReachedSpot(Item item, bool checkForMerge = true)
    {
        item.Spot.BumpDown();
        
        if (!checkForMerge)
        {
            return;
        }
        
        if (itemMergeDataDictionary[item.ItemName].CanMergeItems())
        {
            MergeItems(itemMergeDataDictionary[item.ItemName]);
        }
        else
        {
            CheckForGameOver();
        }
    }

    private void MergeItems(ItemMergeData itemMergeData)
    {
        List<Item> items = itemMergeData.items;
        itemMergeDataDictionary.Remove(itemMergeData.itemName);

        for (int i = 0; i < items.Count; i++)
        {
            items[i].Spot.Clear();
            //Destroy(items[i].gameObject);
        }

        if (itemMergeDataDictionary.Count<=0)
        {
            isBusy = false;
        }
        else
        {
            MoveAllItemsToTheLeft(HandleAllItemsMovedToTheLeft);
        }
        
        mergerStarted?.Invoke(items);
    }

    private void MoveAllItemsToTheLeft(Action completeCallback)
    {
        bool callbackTriggered = false;
 
        for (int i = 3; i < _spots.Length; i++)
        {
            ItemSpot spot = _spots[i];
 
            if (spot.IsEmpty())
                continue;
 
            Item item = spot.Item;
            ItemSpot targetSpot = _spots[i - 3];
 
            if(!targetSpot.IsEmpty())
            {
                Debug.LogWarning($"{targetSpot.name} is full");
                isBusy = false;
                return;
            }
 
            spot.Clear();
 
            completeCallback += () => HandleItemReachedSpot(item, false);
            MoveItemToSpot(item, targetSpot, completeCallback);
 
            callbackTriggered = true;
        }
 
        
        if (!callbackTriggered)
        {
            completeCallback?.Invoke();
        }
    }

    private void HandleAllItemsMovedToTheLeft()
    {
        isBusy = false;
    }

    private void HandleIdealSpotFull(Item item, ItemSpot idealSpot)
    {
        MoveAllItemsToTheRightFrom(idealSpot, item);
    }

    private void MoveAllItemsToTheRightFrom(ItemSpot idealSpot, Item itemToPlace)
    {
        int spotIndex = idealSpot.transform.GetSiblingIndex();

        for (int i = _spots.Length - 2; i >= spotIndex; i--)
        {
            ItemSpot spot = _spots[i];
            if (spot.IsEmpty())
            {
                continue;
            }
            
            if (_spots[i].IsEmpty())
            {
                continue;
            }
            
            Item item = _spots[i].Item;
            
            spot.Clear();

            ItemSpot targetSpot = _spots[i + 1];

            if (!targetSpot.IsEmpty())
            {
                Debug.LogError("Error");
                isBusy = false;
                return;
            }
            
            MoveItemToSpot(item, targetSpot, () => HandleItemReachedSpot(item, false));
        }
        
        MoveItemToSpot(itemToPlace, idealSpot, () => HandleItemReachedSpot(itemToPlace));
    }

    private void MoveItemToFirstFreeSpot(Item item)
    {
        ItemSpot targetSpot = GetFreeSpot();
        if (targetSpot == null)
        {
            Debug.LogError("Target spot is null");
            return;
        }

        CreateItemMergeData(item);
        
        MoveItemToSpot(item, targetSpot, () => HandleFirstItemReachedSpot(item));
        
        //targetSpot.Populate(item);
  
        //item.transform.SetParent(targetSpot.transform);
  
        //item.transform.localPosition = itemLocalPositionOnSpot;
        //item.transform.localScale = itemLocalScaleOnSpot;
        //item.transform.localRotation = Quaternion.identity;
  
        //item.DisableShadows();
  
        //item.DisablePhysics();
  
        //HandleFirstItemReachedSpot(item);
    }

    private void HandleFirstItemReachedSpot(Item item)
    {
        item.Spot.BumpDown();
        
        CheckForGameOver();
    }

    private void CheckForGameOver()
    {
        if (GetFreeSpot()==null)
        {
            GameManager.instance.SetGameState(EGameState.GAMEOVER);
        }
        else
        {
            isBusy = false;
        }
    }

    private void CreateItemMergeData(Item item)
    {
        itemMergeDataDictionary.Add(item.ItemName, new ItemMergeData(item));
        
        Debug.Log("Added" + item.name + "key to the dictionary");
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