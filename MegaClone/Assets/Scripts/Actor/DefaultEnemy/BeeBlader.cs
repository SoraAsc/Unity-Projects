using System.Collections;
using UnityEngine;

public class BeeBlader : Enemy
{
    [SerializeField] Vector3 target;
    [SerializeField] Transform machineGunZoneSpawner, missileSpawner, machineGunMark;
    [SerializeField] GameObject[] bullets;
    int bulletIndex = 0;
    [SerializeField] int maxMachineAmmo = 20, maxMissile = 2;
    [SerializeField] float missileDelay = 1f, machineGunDelay = 0.2f, delayToChangeAttackPattern = 2f;
    [SerializeField] LayerMask detectLayer;
    float currentDelayToChangeAttackPattern = 0;
    bool canAttack = true;
    Vector2 initialPos;
    [SerializeField] float maxWalkDistance;
    Coroutine currentCoroutine;
    private void Start()
    {
        canAttack = true;
        machineGunMark.gameObject.SetActive(false);
        currentDelayToChangeAttackPattern = 0;
        initialPos = transform.position;
        InitializeComponent();
        InitializeHurthVar();
    }


    protected override void Movement(Vector2 dir)
    {
        float y = Mathf.Sin(Time.time * speed) * dir.y;
        transform.position = initialPos + new Vector2((maxHp - hp) / maxWalkDistance, y);
    }

    /*     private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(machineGunZoneSpawner.position, machineGunZoneSpawner.position + target);
        } */

    private void FixedUpdate()
    {


        RaycastHit2D hit = Physics2D.Raycast(machineGunZoneSpawner.position, machineGunZoneSpawner.position + target, 40, detectLayer);
        if (hit.collider != null)
        {
            machineGunMark.position = new Vector2(hit.point.x, hit.point.y);
        }
    }

    private void Update()
    {
        Movement(initialDi);
        ShotControl();
        Limit();
    }

    private void ShotControl()
    {
        currentDelayToChangeAttackPattern += Time.deltaTime;
        if (currentDelayToChangeAttackPattern >= delayToChangeAttackPattern && canAttack)
        {
            currentDelayToChangeAttackPattern = 0;
            bulletIndex = Random.Range(0, bullets.Length);
            CallRightBullet();
        }
    }
    private void CallRightBullet()
    {
        if (isAlive)
        {
            if (currentCoroutine != null) StopCoroutine(currentCoroutine);
            canAttack = false;
            switch (bulletIndex)
            {
                case 0:
                    machineGunMark.gameObject.SetActive(false);
                    currentCoroutine = StartCoroutine(SpawnBullet(missileSpawner, missileDelay, maxLoop: maxMissile));
                    break;
                default:
                    machineGunMark.gameObject.SetActive(true);
                    currentCoroutine = StartCoroutine(SpawnBullet(machineGunZoneSpawner, machineGunDelay, maxLoop: maxMachineAmmo, isMissile: false));
                    break;
            }

        }
    }

    IEnumerator SpawnBullet(Transform spawner, float delay, int currentLoop = 1, int maxLoop = 2, bool isMissile = true)
    {
        if (currentLoop <= maxLoop)
        {
            GameObject bullet = Instantiate(bullets[bulletIndex], spawner.position, Quaternion.identity);
            if (!isMissile) bullet.GetComponent<FollowBullet>().Target = machineGunMark;
            yield return new WaitForSeconds(delay);
            currentCoroutine = StartCoroutine(SpawnBullet(spawner, delay, currentLoop + 1, maxLoop, isMissile));
        }
        else { yield return null; canAttack = true; }

    }
}
