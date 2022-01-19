using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionHandler : MonoBehaviour
{
    #pragma warning disable IDE0051 // Remover membros privados não utilizados
    private void DestroyOnFinish()
    {
        Destroy(gameObject);
    }
    #pragma warning restore IDE0051 // Remover membros privados não utilizados

}
