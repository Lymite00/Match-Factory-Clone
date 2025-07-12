using System;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Rigidbody))]
public class Item : MonoBehaviour
{
    [Header("Elements")] 
    public Renderer _renderer;
    public Collider _collider;

    private Material baseMaterial;

    private void Awake()
    {
        baseMaterial = _renderer.material;
        _collider = GetComponentInChildren<Collider>();
    }

    public void DisableShadows()
    {
        _renderer.shadowCastingMode = ShadowCastingMode.Off;
    }
    
    public void DisablePhysics()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        _collider.enabled = true;
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