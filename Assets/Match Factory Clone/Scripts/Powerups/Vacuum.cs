using System;
using UnityEngine;

public class Vacuum : MonoBehaviour
{
    [Header("Actions")] 
    public static Action started;
    private void TriggerPowerupStart()
    {
        started?.Invoke();
    }
}