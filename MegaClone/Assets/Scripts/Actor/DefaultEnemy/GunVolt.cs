using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remover membros privados não utilizados", Justification = "To avoid warnings in private methods provided by Unity.")]
public class GunVolt : Enemy
{
    [SerializeField]
    private Transform shotZone;
    [SerializeField]
    private GameObject[] bullets;

    [SerializeField]
    float waitDelayToShot;
    float delayToShot=0;

    bool canShot = false;
    int bulletIndex = 0;

    private void Start()
    {
        InitializeComponent();
        InitializeHurthVar();
        canShot = false;
        bulletIndex = Random.Range(0, bullets.Length);
    }
    private void CanShot(bool isActive)
    {        
        ani.SetBool("canShot", isActive);
    }


    private void DesactiveShot()
    {
        CanShot(false);
        canShot = false;
        bulletIndex = Random.Range(0, bullets.Length);
    }

    private void SpawnBullet()
    {
        if (canShot&&isAlive)
        {
            Instantiate(bullets[bulletIndex], new Vector2(shotZone.position.x, shotZone.position.y), Quaternion.identity);
        }
    }

    private void Update()
    {
        if (!canShot)
        {
            delayToShot += Time.deltaTime;
        }
        if (delayToShot >= waitDelayToShot)
        {
            canShot = true;
            CanShot(true);
            delayToShot = 0;
        }
    }
}
