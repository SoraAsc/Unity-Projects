using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remover membros privados não utilizados", Justification = "To avoid warnings in private methods provided by Unity.")]
public class Spikes : Enemy
{
    [SerializeField]
    private Crusher crusher;
    DestroyedTile destroyedTile;

    private void Start()
    {
        InitializeComponent();
        InitializeHurthVar();
        crusher = transform.parent.GetComponent<Crusher>();
        destroyedTile = GameObject.FindGameObjectWithTag("TileDestroyed").GetComponent<DestroyedTile>();
    }

    protected override void LoseHealth(int damage = 1)
    {
        base.LoseHealth(damage);
        if(crusher) crusher.CallLoseHealth(damage);
    }

    private void Update()
    {
        bool touchedGround = destroyedTile.ChangeTile(transform.GetChild(0).position, transform.GetChild(1).position, crusher);
        CheckIfHasCrusher(touchedGround);
    }


    private void CheckIfHasCrusher(bool touchedTheGround)
    {
        if(crusher==null && touchedTheGround)
        {
            LoseHealth(maxHp + 1);
        }
    }

    

}
