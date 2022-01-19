using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMissile : Ammo
{
    [SerializeField] int directionSignal = -1;
    [SerializeField] float waitForFollow = 0.2f,speedMultiply = 10;
    //[SerializeField] Sprite diagonalSprite;
    Transform player;
    bool isFollowing = false;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        isFollowing = false;
        Direction(directionSignal);
        ChangeMovementCondition();
        StartCoroutine(TimeToSeek());
    }
    protected override void Movement()
    {
        if (!isFollowing)
            base.Movement();        
    }

    IEnumerator TimeToSeek()
    {
        yield return new WaitForSeconds(waitForFollow);
        FollowPlayer();
        //if (rd2.velocity.x > 0 && rd2.velocity.y > 0) GetComponent<SpriteRenderer>().sprite = diagonalSprite;
    }

    private void FollowPlayer()
    {
        isFollowing = true;
        rd2.velocity = Vector2.zero;
        Vector3 diff = player.position - transform.parent.position;

        rd2.velocity = (transform.parent.position - player.position) * speed * speedMultiply * Time.deltaTime;
        float angle = Mathf.Atan2(diff.y,diff.x) *  Mathf.Rad2Deg;
        Quaternion target = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.parent.rotation = Quaternion.RotateTowards(transform.parent.rotation, target, speed * Time.deltaTime);
    }
}
