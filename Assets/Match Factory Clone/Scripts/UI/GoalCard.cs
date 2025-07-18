using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoalCard : MonoBehaviour
{
    [Header("Elements")] 
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private Image iconImage;
    
    [SerializeField] private GameObject checkMark;
    [SerializeField] private GameObject backFace;
    [SerializeField] private Animator animator;

    private void Start()
    {
        animator.enabled = false;
    }

    private void Update()
    {
        backFace.SetActive(Vector3.Dot(Vector3.forward, transform.forward) < 0);
    }

    public void Configure(int initialAmount, Sprite icon)
    {
        amountText.text = initialAmount.ToString();
        iconImage.sprite = icon;
    }

    public void UpdateAmount(int amount)
    {
        amountText.text = amount.ToString();

        Bump();
    }

    private void Bump()
    {
        LeanTween.cancel(gameObject);

        transform.localScale = Vector3.one;
        LeanTween.scale(gameObject, Vector3.one * 1.1f,.2f).setLoopPingPong(1);
    }

    public void Complete()
    {
        animator.enabled = true;

        checkMark.SetActive(true);
        amountText.gameObject.SetActive(false);
        
        animator.Play("Complete");
        //amountText.text = "";
        //gameObject.SetActive(false);
    }
}