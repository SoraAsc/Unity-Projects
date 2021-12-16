using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remover membros privados não utilizados", Justification = "To avoid warnings in private methods provided by Unity.")]
public class Ammo : MonoBehaviour
{
    [Header("Ammo Default Attributes")]
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected int damage;
    [SerializeField]
    protected float waitToDestroy;

    private bool canMove=false;
    [SerializeField]
    private bool hasOneHit=true;
    Animator ani;
    Rigidbody2D rd2;

    [SerializeField]
    protected Sprite[] particleSprites;

    public Sprite[] ParticleSprites { get => particleSprites; }
    public int Damage { get => damage; }
    public bool HasOneHit { get => hasOneHit;  }


    #pragma warning disable IDE0051 // Remover membros privados não utilizados
    private void Awake()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        canMove = false;
        ani = GetComponent<Animator>();
        rd2 = transform.parent.GetComponent<Rigidbody2D>();
        if (waitToDestroy > 0) { StartCoroutine(DestroyAmmoAfter()); }
    }

    protected void InitializeComponentSelf()
    {
        canMove = false;
        ani = GetComponent<Animator>();
        rd2 = GetComponent<Rigidbody2D>();
        StartCoroutine(DestroyAmmoAfter());
    }

    
    private void Update()
    {
        Movement();
    }

    #pragma warning restore IDE0051 // Remover membros privados não utilizados

    public void Direction(int signal=1)
    {
        speed *= signal;
    }

    protected virtual void Movement()
    {
        if (canMove)
        {
            rd2.velocity = speed * 10 * Time.deltaTime * Vector2.right;
        }
    }

    protected void ChangeMovementCondition()
    {
        canMove = !canMove;
        if(ani) ani.SetBool("canMove", canMove);
    }

    IEnumerator DestroyAmmoAfter()
    {
        yield return new WaitForSeconds(waitToDestroy);
        if (transform.parent)
        {
            Destroy(transform.parent.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


}
