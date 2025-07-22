using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    [Header("Actions")] 
    public static Action<Item> itemPickedUp;
    
    [Header("Settings")] 
    private bool isBusy;
    private int vacuumItemsToCollect;
    private int vacuumCounter;
    
    [Header("Elements")] 
    [SerializeField] private Transform vacuumPosition;
    
    private void Awake()
    {
        Vacuum.started += OnVacuumStarted;
    }

    private void OnDestroy()
    {
        Vacuum.started -= OnVacuumStarted;
    }

    private void OnVacuumStarted()
    {
        VacuumPower();
    }

    [Button]
    private void VacuumPower()
    {
        // Collect 3 target or goal items
        
        // Grab the items
        
        // Grab the goal items
        
        // Grab the goal that greatest amount
        
        // Grab 3 items

        Item[] items = LevelManager.instance.Items;
        ItemLevelData[] goals = GoalManager.instance.Goals;

        ItemLevelData? greatestGoal = GetGreatestGoal(goals);

        if (greatestGoal == null)
            return;

        ItemLevelData goal = (ItemLevelData)greatestGoal;

        isBusy = true;
        vacuumCounter = 0;
        
        List<Item> itemsToCollect = new List<Item>();
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].ItemName == goal.itemPrefab.ItemName)
            {
                itemsToCollect.Add(items[i]);

                if (itemsToCollect.Count >=3)
                {
                    break;
                }
            }
        }

        vacuumItemsToCollect = itemsToCollect.Count;
        
        for (int i = 0; i < itemsToCollect.Count; i++)
        {
            itemsToCollect[i].DisablePhysics();

            Item itemToCollect = itemsToCollect[i];
            
            LeanTween.move(itemsToCollect[i].gameObject, vacuumPosition.position, .5f)
                .setEase(LeanTweenType.easeInCubic)
                .setOnComplete(() => ItemReachedVacuum(itemToCollect));

            LeanTween.scale(itemsToCollect[i].gameObject, Vector3.zero, .5f);
        }
        
        for (int i = itemsToCollect.Count-1; i >= 0; i--)
        {
            itemPickedUp?.Invoke(itemsToCollect[i]);
            //Destroy(itemsToCollect[i].gameObject);
        }
    }

    private void ItemReachedVacuum(Item item)
    {
        vacuumCounter++;

        if (vacuumCounter >= vacuumItemsToCollect)
        {
            isBusy = false;
        }
        
        Destroy(item.gameObject);
    }
    
    private ItemLevelData? GetGreatestGoal(ItemLevelData[] goals)
    {
        int max = 0;
        int goalIndex = -1;
        
        for (int i = 0; i < goals.Length; i++)
        {
            if (goals[i].amount >= max)
            {
                max = goals[i].amount;
                goalIndex = i;
            }
        }
        
        if (goalIndex <= -1)
        {
            return null;
        }

        return goals[goalIndex];
    }
}
