using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]
public class Item : MonoBehaviour
{
    [Header("Elements")] 
    public Renderer _renderer;

    private Material baseMaterial;

    private void Awake()
    {
        baseMaterial = _renderer.material;
    }

    public void DisableShadows()
    {
        
    }
    
    public void DisablePhysics()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Collider>().enabled = true;
    }

    public void Select(Material outlineMaterial)
    {
        _renderer.materials = new Material[] { baseMaterial, outlineMaterial };
    }
    
    public void Deselect()
    {
        _renderer.materials = new Material[] { baseMaterial };
    }
}