using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remover membros privados não utilizados", Justification = "To avoid warnings in private methods provided by Unity.")]
public abstract class Actor : MonoBehaviour
{
    [Header("Actor Default Attributes")]
    [SerializeField] 
    protected int maxHp, hp, damage;
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected Transform deathParticle;
    protected bool isAlive = true;
    protected bool isInvincible = false;
    protected bool canMove;

    protected Animator ani;
    protected SpriteRenderer sr;
    protected Rigidbody2D rd2;


    public int Damage { get => damage; }

    protected void InitializeComponent()
    {
        isAlive = true;
        isInvincible = false;
        canMove = true;
        ani = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        rd2 = GetComponent<Rigidbody2D>();
    }

    protected abstract void Hurth();

    protected virtual void LoseHealth(int damage = 1)
    {
        hp -= damage;
        CheckIfIsDeath(damage);
    }

    protected virtual void CheckIfIsDeath(int damage)
    {
        if (hp <= 0)
        {
            isAlive = false;
            ani.Play("death", 0, 0.0f);            
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("InstantDeathZone"))
        {
            isAlive = false;
            ani.Play("death", 0, 0.0f);
        }
    }

    protected abstract void Movement(Vector2 dir);

}
