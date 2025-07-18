using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoalCard : MonoBehaviour
{
    [Header("Elements")] 
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private Image iconImage;


    public void Configure(int initialAmount, Sprite icon)
    {
        amountText.text = initialAmount.ToString();
        iconImage.sprite = icon;
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