using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected float damage;

    private bool canMove=false;
    Animator ani;
    Rigidbody2D rd2;


    private void Awake()
    {
        canMove = false;
        ani = GetComponent<Animator>();
        rd2 = transform.parent.GetComponent<Rigidbody2D>();
    }


    public virtual void Movement()
    {
        if (canMove)
        {
            rd2.velocity = speed * 10 * Time.deltaTime * Vector2.right;
        }
    }

    #pragma warning disable IDE0051 // Remover membros privados não utilizados
    private void ChangeMovementCondition()
    {
        canMove = !canMove;
        ani.SetBool("canMove", canMove);
    }


}
