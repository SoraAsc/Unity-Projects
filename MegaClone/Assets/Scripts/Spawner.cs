using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remover membros privados n�o utilizados", Justification = "To avoid warnings in private methods provided by Unity.")]
public class Spawner : MonoBehaviour
{
    [SerializeField]
    NPC npc;
    [SerializeField]
    Vector2 offset;
    [SerializeField]
    NPC spawnedEnemy;
    [SerializeField]
    bool uniqueSpawn;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && spawnedEnemy == null && other.transform.localScale.x > 0)
        {
            spawnedEnemy = Instantiate(npc, new Vector2(transform.position.x, npc.transform.position.y) + offset, Quaternion.identity);
            if (uniqueSpawn)
            {
                Destroy(gameObject);
            }
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawCube(new Vector2(transform.position.x, npc.transform.position.y) + offset, Vector3.one);
        //Gizmos.DrawCube(transform.position + new Vector3(offset.x, npc.transform.position.y + offset.y, 0), Vector3.one);
    }

}
