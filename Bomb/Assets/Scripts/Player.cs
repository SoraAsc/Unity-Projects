using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float speed=5f;
    Animator ani;
    SpriteRenderer sr;
    Rigidbody2D rd2;
    public bool canSpawnBomb;

    public GameObject bombPrefab;
    int speedMultiply = 1;
    [SerializeField] int bombPower = 1;

    [SerializeField] int currentBombSpawned=0;
    public int CurrentBombSpawned { get { return currentBombSpawned; } set { currentBombSpawned = value; } }
    [SerializeField] int maxBomb=1;

    //GameManager manager;

    private void Start()
    {
        canSpawnBomb = true;
        ani = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        rd2 = GetComponent<Rigidbody2D>();
        //manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canSpawnBomb && currentBombSpawned < maxBomb)
        {
            currentBombSpawned += 1;
            //Debug.Log((transform.position.x - (int)transform.position.x));
            float posY;
            if ((transform.position.y - (int)transform.position.y) >= 0.29f)
            {
                posY = (int)transform.position.y;
            }
            else posY = (int)transform.position.y - 1f;

            Instantiate(bombPrefab, new Vector3(Mathf.RoundToInt(transform.position.x), posY, -9), Quaternion.identity).GetComponent<Bomb>().BomberManDetails(this,newBombPowerSpread: bombPower);
        }
    }

    private void FixedUpdate()
    {
        Vector2 mov = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        ani.SetFloat("verticalMove", mov.y);
        ani.SetFloat("horizontalMove", Mathf.Abs(mov.x));
        if (mov.x > 0) sr.flipX = false;
        else if (mov.x < 0) sr.flipX = true;

        rd2.velocity = mov * Time.deltaTime * (speed+(10*speedMultiply));
        //transform.Translate(mov);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PowerUp"))
        {
            UsePowerUp(other.GetComponent<PowerUp>().powerUp);
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Bomb"))
        {
            canSpawnBomb = false;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        canSpawnBomb = true;
    }
    public void UsePowerUp(GameConstant.AllPowerUps powerUp)
    {
        switch (powerUp)
        {
            case GameConstant.AllPowerUps.BombNum:
                maxBomb += 1;
                break;
            case GameConstant.AllPowerUps.WalkSpeed:
                speedMultiply += 1;
                break;
            default:
                bombPower += 1;
                break;
        }
    }


}
