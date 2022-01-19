using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0044:Adicionar modificador somente leitura", Justification = "<Pendente>")]
//[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remover membros privados n�o utilizados", Justification = "To avoid warnings in private methods provided by Unity.")]
public class Player : Actor
{

    [Header("Movement Attributes")]
    [SerializeField]
    LayerMask layerMask;
    [SerializeField] bool isGround;
    [SerializeField] bool isWallSliding;
    [SerializeField]
    private float extraHeight, jumpPower, extraDisWall;
    [SerializeField] private Vector2 wallJumpPower;
    InputControl control;
    BoxCollider2D box;
    BoxCollider2D wallBox;

    [Header("Shot Attributes")]
    [SerializeField]
    float shotDelay, shotAniDisableDelay, shotHoldMaxDelay;
    [SerializeField]
    private GameObject[] bullets;
    [SerializeField]
    private Transform shotZone;
    float shotCurrentDelay, shotHoldCurrentDelay;
    bool canShoot, shotNow, isHoldShoot;
    Coroutine disableShotWait;

    private readonly float deathParticleRotateVelocity = 80;

    [Header("SliderBar")]
    [SerializeField]
    private SliderBar lifeBar;

    float initialXScale = 6;
    private void Awake()
    {
        //Reset
        canShoot = true;
        shotNow = isGround = isWallSliding = false;
        shotCurrentDelay = shotHoldCurrentDelay = 0;
        initialXScale = Mathf.Abs(transform.localScale.x);
        transform.localScale = new Vector2(initialXScale, initialXScale);

        //Getting the components
        InitializeComponent();

        control = new InputControl();
        box = GetComponent<BoxCollider2D>();
        wallBox = transform.GetChild(3).GetComponent<BoxCollider2D>();

        //Add Functions to Player Input.
        control.Player.Shot.started += _ => { if (isAlive) isHoldShoot = true; };
        control.Player.Shot.performed += _ => { if (isAlive) { shotNow = true; } };
        control.Player.Shot.canceled += _ => { if (isAlive) { isHoldShoot = false; ani.SetBool("shot", shotNow); } };

        LoadMaxStatus();
    }

    private void LoadMaxStatus()
    {
        lifeBar.RunMaxCapacity(maxHp);
    }

    private void Update()
    {
        if (isAlive)
        {
            MoveHandler();
            DoDelay();
            DoHoldAnimation();
        }
    }
    private void FixedUpdate()
    {
        isGround = IsGrounded();
        isWallSliding = IsWallSliding();
    }

    protected override void Movement(Vector2 dir)
    {
        if (!canMove) return;

        Flip(dir.x);

        rd2.velocity = new Vector2(dir.x * Time.deltaTime * speed, rd2.velocity.y);

        if (isWallSliding && !isGround && rd2.velocity.y < -1.5f)
        {
            rd2.velocity = new Vector2(rd2.velocity.x, -1.5f);
        }

        Jump(dir, transform.localScale.x);
        ani.SetFloat("verticalMove", dir.y);
        ani.SetFloat("horizontalMove", Mathf.Abs(dir.x));
        ani.SetBool("isGround", isGround);
        ani.SetBool("isWallSliding", isWallSliding);
    }

    private void Jump(Vector2 dir, float xDir) //xDir > 0 ? right : left
    {
        if (dir.y > 0 && isGround)
        {
            rd2.velocity = Vector2.zero;
            rd2.AddForce(Vector2.up * Time.deltaTime * jumpPower * dir.y, ForceMode2D.Impulse);
        }
        else if (dir.y > 0 && !isGround && isWallSliding)
        {
            canMove = false;
            rd2.velocity = Vector2.zero;
            rd2.AddForce(new Vector2(xDir > 0 ? -wallJumpPower.x : wallJumpPower.x, wallJumpPower.y * dir.y) * Time.deltaTime, ForceMode2D.Impulse);
            Invoke(nameof(TurnOnMovement), 0.09f);
        }
    }

    private void Flip(float x)
    {
        if (x > 0)
            transform.localScale = new Vector2(initialXScale, transform.localScale.y);
        else if (x < 0)
            transform.localScale = new Vector2(-initialXScale, transform.localScale.y);
    }

    public void CallShot(int signal = 1)
    {
        Shot(signal);
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
        RaycastHit2D rayH2D = Physics2D.BoxCast(box.bounds.center, box.bounds.size - new Vector3(0.0f, 0f, 0f), 0f, Vector2.down, extraHeight, layerMask);

        return rayH2D.collider != null;
    }

    private bool IsWallSliding()
    {
        RaycastHit2D rayH2D = Physics2D.BoxCast(wallBox.bounds.center, wallBox.bounds.size, 0f,
            transform.localScale.x > 0 ? Vector2.right : Vector2.left, extraDisWall, layerMask);

        return rayH2D.collider != null;
    }

    private void DoDelay()
    {
        shotCurrentDelay += Time.deltaTime;
        if (isHoldShoot) { shotHoldCurrentDelay += Time.deltaTime; }
        if (shotCurrentDelay >= shotDelay)
        {
            shotCurrentDelay = 0;
            canShoot = true;
        }
    }

    private void Shot(int signal)
    {
        if (canShoot && shotNow && !isHoldShoot)
        {
            CreateShot(signal);

            canShoot = false;
            shotNow = false;
            isHoldShoot = false;
            shotCurrentDelay = 0;
            shotHoldCurrentDelay = 0;

            CallEnterDisableShotAnimation();
            ani.SetBool("shot", shotNow);
        }
    }

    private void CreateShot(int signal)
    {
        GameObject bullet = Instantiate(bullets[ShotSelect()], new Vector2(shotZone.position.x, shotZone.position.y), Quaternion.identity);
        bullet.transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y);
        bullet.transform.GetChild(0).GetComponent<Ammo>().Direction((transform.localScale.x > 0 ? 1 : -1) * signal);
    }

    private int ShotSelect()
    {
        float holdPercent = shotHoldCurrentDelay / shotHoldMaxDelay;
        if (holdPercent >= 0.8f) { return 2; }
        else if (holdPercent >= 0.4f) { return 1; }
        return 0;
    }

    private void CallEnterDisableShotAnimation()
    {
        ani.SetBool("shootingExitDelay", true);
        if (disableShotWait != null) { StopCoroutine(disableShotWait); }
        disableShotWait = StartCoroutine(WaitForDisable());
    }

    IEnumerator WaitForDisable()
    {
        yield return new WaitForSeconds(shotAniDisableDelay);
        ani.SetBool("shootingExitDelay", false);
    }

    private void DoHoldAnimation()
    {
        if (isHoldShoot)
        {
            int selectedShot = ShotSelect();
            if (selectedShot > 0) ani.Play(selectedShot >= 2 ? "ChargingLv2" : selectedShot >= 1 ? "ChargingLv1" : "Default");
        }
    }

    private void RunDeathAnimation()
    {
        Transform deathParTransf = Instantiate(deathParticle);
        deathParTransf.position = transform.position;
        StartCoroutine(DeathRotationParticleRun(deathParTransf));
        transform.GetChild(2).gameObject.SetActive(false);
        sr.enabled = false;
    }

    IEnumerator DeathRotationParticleRun(Transform deathParticleTransform)
    {
        yield return new WaitForSeconds(0.001f);
        deathParticleTransform.transform.Rotate(Vector3.forward * Time.deltaTime * deathParticleRotateVelocity);
        StartCoroutine(DeathRotationParticleRun(deathParticleTransform));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && !isInvincible)
        {
            Hurth();
            int damage = other.GetComponent<Actor>().Damage;
            LoseHealth(damage);
            lifeBar.RunLifeChange(damage, -1);
        }
        if (other.CompareTag("EAmmo") && isAlive)
        {
            ReceiveDamage(other.gameObject.GetComponentInChildren<Ammo>());
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("EAmmo") && isAlive)
        {
            ReceiveDamage(other.gameObject.GetComponentInChildren<Ammo>());
        }
    }

    private void ReceiveDamage(Ammo other)
    {
        Hurth();
        int damage = other.Damage;
        LoseHealth(damage);
        lifeBar.RunLifeChange(damage, -1);

        if (other.HasOneHit)
        {
            if (other.transform.parent) Destroy(other.transform.parent.gameObject);
            else Destroy(other.gameObject);
        }

    }

    protected override void Hurth()
    {
        isInvincible = true;
        ani.Play("hurth", 0, 0.0f);
    }

    IEnumerator ProcInvincible()
    {
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.1f);
            ChangeTransparency(new Color32(255, 255, 255, 127));
            yield return new WaitForSeconds(0.1f);
            ChangeTransparency(new Color32(255, 255, 255, 255));

        }
        yield return new WaitForSeconds(0.2f);
        isInvincible = false;
    }

    void ChangeTransparency(Color32 color)
    {
        foreach (SpriteRenderer srC in GetComponentsInChildren<SpriteRenderer>())
        {
            srC.color = color;
        }
    }
    private void TurnOnMovement()
    {
        canMove = true;
    }

    private void GlowChar(int isActive)
    {
        sr.material.SetInt("_isGlowing", isActive);
    }
}
