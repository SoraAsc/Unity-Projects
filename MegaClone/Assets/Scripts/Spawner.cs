using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remover membros privados não utilizados", Justification = "To avoid warnings in private methods provided by Unity.")]
public class Spawner : MonoBehaviour
{
    [SerializeField]
    Enemy enemy;
    [SerializeField]
    Vector2 offset;
    [SerializeField]
    Enemy spawnedEnemy;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && spawnedEnemy == null && other.transform.localScale.x > 0)
        {
            spawnedEnemy = Instantiate(enemy, new Vector2(transform.position.x,transform.position.y)+offset,Quaternion.identity);
        }   
    }

}
