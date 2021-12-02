using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remover membros privados não utilizados", Justification = "<Pendente>")]
public class AmmoDamageDealer : MonoBehaviour
{
    [SerializeField]
    protected Animator explosionAnimator;

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealerTrigger(other);
    }
    protected virtual void DamageDealerTrigger(Collider2D other)
    {
        
        if (!other.CompareTag("Player"))
        {
            Actor target = other.GetComponent<Actor>() ? other.GetComponent<Actor>() : null;
            
            if (target)
            {                
                Animator newExplosion =  Instantiate(explosionAnimator, new Vector2(transform.position.x,transform.position.y), Quaternion.identity);
                newExplosion.Play("Buster_Explosion", 0, 0.0f);
                Destroy(gameObject);
            }
        }
    }
}
