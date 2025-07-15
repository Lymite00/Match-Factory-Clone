using UnityEngine;

[System.Serializable]
public struct ItemLevelData
{
    public Item itemPrefab;
    
    [NaughtyAttributes.ValidateInput("ValidateAmount", "Amount must be a multiple of 3")]
    [NaughtyAttributes.AllowNesting]
    [Range(0,100)]
    public int amount;

    private bool ValidateAmount()
    {
        return amount % 3 == 0;
    }
}
