using UnityEngine;

public class Level : MonoBehaviour
{
    [Header("Elemets")] 
    [SerializeField] private ItemPlacer itemPlacer;


    public ItemLevelData[] GetGoals() => itemPlacer.GetGoals();
}
