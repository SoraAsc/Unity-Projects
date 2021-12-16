using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remover membros privados não utilizados", Justification = "To avoid warnings in private methods provided by Unity.")]
public class DefaultBullet : Ammo
{
    [SerializeField] int directionSignal = -1;
    void Start()
    {
        Direction(directionSignal);
        ChangeMovementCondition();
    }

}
