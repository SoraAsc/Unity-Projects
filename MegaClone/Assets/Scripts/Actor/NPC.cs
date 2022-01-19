using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remover membros privados n�o utilizados", Justification = "To avoid warnings in private methods provided by Unity.")]
public class NPC : Actor
{
    [SerializeField]
    protected Vector2 initialDi; //Initial Direction or distance.
    //[SerializeField]
    //protected Vector2 areaOutOfBoundsBegin, areaOutOfBoundsEnd;

    private void Start()
    {
        //Getting the components
        InitializeComponent();
    }

    protected override void Hurth()
    {
        Debug.Log("Can't be hurth");
    }

    /// <summary>
    /// NPC Movement
    /// </summary>
    /// <param name="dir"></param>
    protected override void Movement(Vector2 dir)
    {
        Flip(dir);
        transform.Translate(dir.x * Time.deltaTime * speed * Vector2.right);
    }

    private void Flip(Vector2 dir)
    {
        if (dir.x > 0)
            sr.flipX = true;
        else if (dir.x < 0)
            sr.flipX = false;
    }

    private void Update()
    {
        Movement(initialDi);
        Limit();
    }

    protected void Limit()
    {
        if (transform.position.x <= -14.4f || transform.position.y <= -36.4f)
        {
            Destroy(gameObject);
        }
    }


}
