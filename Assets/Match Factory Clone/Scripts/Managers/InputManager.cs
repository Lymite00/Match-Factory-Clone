using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static Action<Item> itemClicked;

    [Header("Settings")] 
    [SerializeField] private Material outlineMaterial;
    private Item currentItem;
    private void Update()
    {
        if (GameManager.instance.IsGame())
        {
            HandleControl();
        }
    }

    private void HandleControl()
    {
        if (Input.GetMouseButton(0))
            HandeDrag();
        else if (Input.GetMouseButtonUp(0))
            HandleMouseUp();
    }

    private void HandleMouseUp()
    {
        if (currentItem==null)
            return;
        
        itemClicked?.Invoke(currentItem);

        DeselectCurrentItem();
    }
    
    private void HandeDrag()
    {
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit,100);

        if (hit.collider == null)
        {
            DeselectCurrentItem();
            return;
        }

        if (hit.collider.transform.parent == null)
            return;
        
        //!!!
        if (!hit.collider.transform.parent.TryGetComponent(out Item item))
        {
            DeselectCurrentItem();
            return;
        }
      
        Debug.Log("Hit collider"+ hit.collider.name);

        DeselectCurrentItem();
        
        currentItem = item;
        currentItem.Select(outlineMaterial);
    }

    private void DeselectCurrentItem()
    {
        if (currentItem!=null)
            currentItem.Deselect();
        
        currentItem = null;
    }
}