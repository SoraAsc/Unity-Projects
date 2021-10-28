using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remover membros privados n�o utilizados", Justification = "<Pendente>")]
public abstract class Actor : MonoBehaviour
{
    [SerializeField]
    protected int maxHp,hp,damage;
    [SerializeField]
    protected float speed;


    protected Animator ani;

    private void Start()
    {
        ani = GetComponent<Animator>();
    }

    public abstract void Movement(Vector2 dir);
}
