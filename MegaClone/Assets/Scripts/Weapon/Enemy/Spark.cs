using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remover membros privados não utilizados", Justification = "To avoid warnings in private methods provided by Unity.")]
public class Spark : Ammo
{
    [SerializeField]
    private int directionSignal=-1;

    private void Awake()
    {
        InitializeComponentSelf();
        Direction(directionSignal);
    }

    private void OnCollisionEnter2D()
    {
        ChangeMovementCondition();
    }
}
