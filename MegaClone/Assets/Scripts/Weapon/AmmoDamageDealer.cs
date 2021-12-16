using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remover membros privados não utilizados", Justification = "To avoid warnings in private methods provided by Unity.")]
public class AmmoDamageDealer : MonoBehaviour
{
    [SerializeField]
    protected Animator explosionAnimator;
    [SerializeField]
    private string yourselfTag="Player";

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealerTrigger(other);
    }
    protected virtual void DamageDealerTrigger(Collider2D other)
    {
        
        if (!other.CompareTag(yourselfTag))
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
