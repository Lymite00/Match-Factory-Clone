using UnityEngine;

public class Level : MonoBehaviour
{
    [Header("Elements")] 
    [SerializeField] private ItemPlacer itemPlacer;

    [Header("Elemets")] 
    [SerializeField] private int duration;
    public int Duration => duration;
    
    public ItemLevelData[] GetGoals() => itemPlacer.GetGoals();
}