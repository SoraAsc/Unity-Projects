using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected float damage;
    [SerializeField]
    protected float waitToDestroy;

    private bool canMove=false;
    Animator ani;
    Rigidbody2D rd2;

    [SerializeField]
    protected Sprite[] particleSprites;

    public Sprite[] ParticleSprites { get => particleSprites; }


#pragma warning disable IDE0051 // Remover membros privados não utilizados
    private void Awake()
    {
        canMove = false;
        ani = GetComponent<Animator>();
        rd2 = transform.parent.GetComponent<Rigidbody2D>();
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
        Destroy(transform.parent.gameObject);
    }

}
