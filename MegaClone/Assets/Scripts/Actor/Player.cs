using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0044:Adicionar modificador somente leitura", Justification = "<Pendente>")]
public class Player : Actor
{

    #region Attributes

    private InputControl control;
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
    float shotDelay, shotAniDisableDelay, shotHoldMaxDelay;
    float shotCurrentDelay, shotHoldCurrentDelay;
    bool canShoot, shotNow, isHoldShoot;
    Coroutine disableShotWait;

    private ParticleSystem pS;

    #endregion


    #pragma warning disable IDE0051 // Remover membros privados não utilizados
    private void Awake()
    {
        canShoot = true;
        shotNow = false;
        isGround = false;
        shotCurrentDelay = 0;
        shotHoldCurrentDelay = 0;

        shotZone = transform.GetChild(0);

        pS = transform.GetComponentInChildren<ParticleSystem>();
        pS.Stop();
        control = new InputControl();
        box = GetComponent<BoxCollider2D>();
        rd2 = GetComponent<Rigidbody2D>();


        control.Player.Shot.started += _ => isHoldShoot = true;
        control.Player.Shot.performed += _ => shotNow = true;
        control.Player.Shot.canceled += _ => { isHoldShoot = false; ani.SetBool("shot", shotNow); };

    }
    private void Update()
    {
        MoveHandler();
        DoDelay();
        DoHoldAnimation();
    }

    private void DoHoldAnimation()
    {
        if (isHoldShoot)
        {
            ChangeParticleAnimationSheet();
        }
    }

    private void ChangeParticleAnimationSheet()
    {
        ParticleSystem.TextureSheetAnimationModule textureSAnimation = pS.textureSheetAnimation;
        for (int i = 0; i < pS.textureSheetAnimation.spriteCount; i++)
        {
            textureSAnimation.RemoveSprite(i);           
        }
        int selecShot = ShotSelect();
        Ammo ammo = bullets[selecShot].GetComponentInChildren<Ammo>();

        if (ammo.ParticleSprites.Length > 0)
        {
            for(int i = 0; i < ammo.ParticleSprites.Length; i++)
            {
                textureSAnimation.AddSprite(ammo.ParticleSprites[i]);
            }
            pS.Play();
        }
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

    public void CallShot()
    {
        Shot();
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

    private void Shot()
    {        
        if (canShoot && shotNow && !isHoldShoot)
        {            
            pS.Stop();
            CreateShot();

            canShoot = false;
            shotNow = false;
            isHoldShoot = false;
            shotCurrentDelay = 0;
            shotHoldCurrentDelay = 0;
                        
            CallEnterDisableShotAnimation();
            ani.SetBool("shot", shotNow);            
        }
    }

    private void CreateShot()
    {
        GameObject bullet = Instantiate(bullets[ShotSelect()], new Vector2(shotZone.position.x, shotZone.position.y), Quaternion.identity);
        bullet.transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y);
        bullet.transform.GetChild(0).GetComponent<Ammo>().Direction(transform.localScale.x > 0 ? 1 : -1);
    }

    private int ShotSelect()
    {
        float holdPercent = shotHoldCurrentDelay / shotHoldMaxDelay;
        if(holdPercent >= 0.8f) { return 2; } 
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

    #pragma warning restore IDE0051 // Remover membros privados não utilizados
}
