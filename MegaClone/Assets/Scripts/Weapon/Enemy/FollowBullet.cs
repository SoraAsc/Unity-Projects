using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBullet : Ammo
{
    [SerializeField] int directionSignal = -1;
    [SerializeField] Transform target;
    public Transform Target { get => target; set => target = value; }
    [SerializeField] bool isFollowing = true;
    private void Awake()
    {
        InitializeComponentSelf();
        target = null;
        isFollowing = true;
        Direction(directionSignal);
        ChangeMovementCondition();
    }


    protected override void Movement()
    {
        if (target && isFollowing && canMove) rd2.AddForce((transform.position - target.position) * speed * Time.deltaTime, ForceMode2D.Impulse); isFollowing = false;  //{ rd2.velocity = (transform.position - target.position) * speed * Time.deltaTime; isFollowing = false; }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground")) ani.SetBool("hit", true);
    }

    private void SelfDestruction()
    {
        canMove = false;
        rd2.velocity = Vector2.zero;
        Destroy(gameObject, 0.07f);
    }
}
