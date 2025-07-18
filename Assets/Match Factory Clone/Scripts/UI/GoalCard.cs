using TMPro;
using UnityEngine;

public class GoalCard : MonoBehaviour
{
    [Header("Elements")] 
    [SerializeField] private TextMeshProUGUI amountText;


    public void Configure(int initialAmount)
    {
        amountText.text = initialAmount.ToString();
    }

    public void UpdateAmount(int amount)
    {
        amountText.text = amount.ToString();
    }

    public void Complete()
    {
        gameObject.SetActive(false);
    }
}