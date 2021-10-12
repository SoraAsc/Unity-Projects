using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0044:Adicionar modificador somente leitura", Justification = "<Pendente>")]
public class Player : Actor
{
    private InputControl control;
    //SpriteRenderer sr;
    BoxCollider2D box;
    Rigidbody2D rd2;

    [SerializeField]
    LayerMask layerMask;
    [SerializeField]
    private float extraHeight;
    [SerializeField]
    private float jumpPower;
    [SerializeField] bool isGround;

    private Transform shotZone;
    [SerializeField]
    private GameObject[] bullets;
    [SerializeField]
    float shotDelay;
    float shotCurrentDelay;



    #pragma warning disable IDE0051 // Remover membros privados não utilizados
    private void Awake()
    {
        isGround = false;
        control = new InputControl();
        shotZone = transform.GetChild(0);
        //sr = GetComponent<SpriteRenderer>();
        box = GetComponent<BoxCollider2D>();
        rd2 = GetComponent<Rigidbody2D>();
        control.Player.Shot.performed += _ => ActiveShot();
        shotCurrentDelay = 0;
    }
    private void Update()
    {
        MoveHandler();
        shotCurrentDelay += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        isGround = IsGrounded();
    }

    public override void Movement(Vector2 dir)
    {
        if (dir.x > 0)
            transform.localScale = new Vector2(7, transform.localScale.y);
            //sr.flipX = false;
        else if (dir.x < 0)
            transform.localScale = new Vector2(-7, transform.localScale.y);
            //sr.flipX = true;

        transform.Translate(dir.x * Time.deltaTime * speed * Vector2.right);
        if (dir.y > 0 && isGround)
        {
            rd2.AddForce(Vector2.up * Time.deltaTime * jumpPower * dir.y, ForceMode2D.Impulse);
        }
        ani.SetFloat("verticalMove", dir.y);
        ani.SetFloat("horizontalMove", Mathf.Abs(dir.x));
        ani.SetBool("isGround", isGround);

    }


    private void OnEnable() => control.Enable();

    private void OnDisable() => control.Disable();

    private void MoveHandler()
    {
        switch (control.Player.Movement.phase)
        {
            case InputActionPhase.Performed:
                Movement(control.Player.Movement.ReadValue<Vector2>());
                break;
            default:
                Movement(Vector2.zero);
                break;
        }
    }

    private bool IsGrounded()
    {
        RaycastHit2D rayH2D = Physics2D.BoxCast(box.bounds.center, box.bounds.size - new Vector3(0.1f, 0f, 0f), 0f, Vector2.down, extraHeight, layerMask);

        return rayH2D.collider != null;
    }

    private void ActiveShot()
    {
        if (shotCurrentDelay >= shotDelay)
        {
            ani.SetBool("shot", true);
            shotCurrentDelay = 0;
        }
    }

    private void Shot()
    {

        GameObject bullet = Instantiate(bullets[0], new Vector2(shotZone.position.x, shotZone.position.y), Quaternion.identity);
        bullet.transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y);
        bullet.transform.GetChild(0).GetComponent<Ammo>().Direction(transform.localScale.x > 0 ? 1 : -1);

        ani.SetBool("shot", false);
    }
    #pragma warning restore IDE0051 // Remover membros privados não utilizados
}
