using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remover membros privados n�o utilizados", Justification = "To avoid warnings in private methods provided by Unity.")]
public class WhellEnemy : Enemy//Actor
{
    [SerializeField]
    private Vector2 initialDir;

    private void Update()
    {
        Movement(initialDir);
    }



}