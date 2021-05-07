using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject flame;
    [SerializeField] float wait = 2.0f;
    int bombPowerSpread=1;

    string bomberTag="Player";
    GameObject[] allPowerUp;
    public void Explosion()
    {
        GameObject centerFlame = Instantiate(flame, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
        centerFlame.GetComponent<Flame>().PopulateFlame(allPowerUp, GameConstant.GridDirection.Line, 0, 0, flame, wait);


        GameObject flameLine1 = Instantiate(flame, new Vector2(transform.position.x + 1, transform.position.y), Quaternion.identity);
        flameLine1.GetComponent<Flame>().PopulateFlame(allPowerUp,GameConstant.GridDirection.Line, 1, bombPowerSpread,flame,wait);
        GameObject flameLine2 = Instantiate(flame, new Vector2(transform.position.x - 1, transform.position.y), Quaternion.identity);
        flameLine2.GetComponent<Flame>().PopulateFlame(allPowerUp, GameConstant.GridDirection.Line, -1, bombPowerSpread,flame, wait);

        GameObject flameCol1 = Instantiate(flame, new Vector2(transform.position.x, transform.position.y+1), Quaternion.identity);
        flameCol1.GetComponent<Flame>().PopulateFlame(allPowerUp,GameConstant.GridDirection.Col, 1, bombPowerSpread, flame, wait);
        GameObject flameCol2 = Instantiate(flame, new Vector2(transform.position.x, transform.position.y - 1), Quaternion.identity);
        flameCol2.GetComponent<Flame>().PopulateFlame(allPowerUp,GameConstant.GridDirection.Col, -1, bombPowerSpread, flame, wait);

        Destroy(gameObject);
    }

    public void BomberManDetails(GameObject[] allNewPowerUp,string newBomberTag ="Player", int newBombPowerSpread=1)
    {
        bomberTag = newBomberTag;
        bombPowerSpread = newBombPowerSpread;
        allPowerUp = allNewPowerUp;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(bomberTag))
        {
            gameObject.GetComponent<CircleCollider2D>().isTrigger = false;
        }
    }
}
