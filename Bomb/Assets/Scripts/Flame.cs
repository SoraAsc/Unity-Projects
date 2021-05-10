using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame : MonoBehaviour
{
    [SerializeField] float appearPowerUpRate=0.1f;
    bool limit = false;
    public void PopulateFlame(GameConstant.GridDirection dire, int i, int cont,GameObject flamePrefab, float wait)
    {
        Destroy(gameObject, wait);
        if (cont > 1)
        {
            Vector2 pos;
            switch (dire)
            {
                case GameConstant.GridDirection.Line:
                    pos = new Vector2(transform.position.x + i, transform.position.y);
                    break;
                default:
                    pos = new Vector2(transform.position.x, transform.position.y + i);
                    break;
            }
            StartCoroutine(WaitToExecute(flamePrefab,pos,i,cont,wait));
        }
    }

    IEnumerator WaitToExecute(GameObject flamePrefab,Vector2 pos,int i, int cont, float wait)
    {
        yield return new WaitForSeconds(0.1f); //Wait to detect the collision
        if (!limit)
        {
            GameObject flame1 = Instantiate(flamePrefab, pos, Quaternion.identity);
            flame1.GetComponent<Flame>().PopulateFlame(GameConstant.GridDirection.Line, i, cont - 1, flame1, wait-0.1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            limit = true;
            Destroy(gameObject);
        }
        else if (other.CompareTag("canExplode"))
        {
            limit = true;
            PowerUp powerUp = other.GetComponent<ExplodableBlock>().PowerUp;
            if(powerUp)
                Instantiate(powerUp.gameObject, other.transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Bomb"))
        {
            other.GetComponent<Bomb>().Explosion();
        }
    }

}
