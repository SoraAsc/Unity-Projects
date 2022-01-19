using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remover membros privados n�o utilizados", Justification = "To avoid warnings in private methods provided by Unity.")]
public class Enemy : NPC
{
    private Shader hurthShader;
    private Shader defaultShader;


    Coroutine hurthCoroutine;

    [SerializeField]
    Animator aniExplosionDeath;


    [SerializeField]
    float hurthWait;
    private void Start()
    {
        InitializeComponent();
        InitializeHurthVar();
    }

    protected void InitializeHurthVar()
    {
        hurthShader = Shader.Find("GUI/Text Shader");
        defaultShader = Shader.Find("Sprites/Default");
        hurthCoroutine = null;
    }


    IEnumerator CastExplosion(int runs = 1, int currentRun = 1)
    {
        float posX, posY;
        if (runs > 0)
        {
            if (currentRun == 1) { posX = transform.position.x; posY = transform.position.y; sr.enabled = false; }
            else { posX = Random.Range(transform.position.x - 0.2f, transform.position.x + 0.2f); posY = Random.Range(transform.position.y, transform.position.y + 0.3f); }
            Animator aniExplosion = Instantiate(aniExplosionDeath, new Vector2(posX, posY), Quaternion.identity);
            runs--;
            currentRun++;
            //yield return new WaitForSeconds(0.55f);
            StartCoroutine(CastExplosion(runs, currentRun));
        }
        else { SelfDestruction(); }
        yield return null;
    }

    protected override void CheckIfIsDeath(int damage)
    {
        {
            if (hp <= 0)
            {
                if (rd2) rd2.simulated = false;
                List<AnimationClip> aniClipList = new List<AnimationClip>();
                if (ani)
                {
                    AnimationClip[] aniClip = ani.runtimeAnimatorController.animationClips;
                    aniClipList.AddRange(aniClip);
                }
                if (damage >= maxHp || !ani || (ani && !aniClipList.Find(x => x.name.Equals("death"))))
                {
                    CallExplosion(1);
                }
                else { ani.Play("death", 0, 0.0f); }
                isAlive = false;
            }
        }
    }

    private void CallExplosion(int runs = 1)
    {
        StartCoroutine(CastExplosion(runs));
    }

    protected override void Hurth()
    {
        if (hurthCoroutine != null) { StopCoroutine(hurthCoroutine); hurthCoroutine = null; }
        hurthCoroutine = StartCoroutine(HurthProc());
    }

    IEnumerator HurthProc(int currentLoop = 1, int limit = 1)
    {
        if (currentLoop <= limit)
        {
            sr.material.shader = hurthShader;
            yield return new WaitForSeconds(hurthWait);
            sr.material.shader = defaultShader;

            hurthCoroutine = StartCoroutine(HurthProc(currentLoop + 1));
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        AmmoDetected(other);
    }

    protected void AmmoDetected(Collider2D other)
    {
        if (other.CompareTag("PAmmo") && isAlive)
        {
            Hurth();
            LoseHealth(other.GetComponentInChildren<Ammo>().Damage);
        }
    }

    protected virtual void SelfDestruction()
    {
        Destroy(gameObject);
    }
}
