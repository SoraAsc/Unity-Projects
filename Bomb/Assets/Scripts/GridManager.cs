using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    //public GameObject backgroundTile;
    //public GameObject solidBlockTile;
    public GameObject explodableBlockTile;
    [SerializeField] int colSize;
    [SerializeField] int lineSize;
    [SerializeField] int beginXPos;
    [SerializeField] int beginYPos;

    public Transform blockHolder;

    [SerializeField] float chanceToCreateTile=0.8f;
    private void Start()
    {
        int newBeginXPos = beginXPos;
        int newBeginYPos = beginYPos;
        GameObject newBlockHolderBackup = Instantiate(blockHolder.gameObject);
        for(int i = 0; i < lineSize; i++) //y
        {
            GameObject newBlockHolderLine = Instantiate(newBlockHolderBackup);
            newBlockHolderLine.transform.SetParent(blockHolder);
            newBlockHolderLine.name = "Line: " + (i+1);
            for(int j = 0; j < colSize; j++) //x
            {
                if (((i + 1) % 2 == 0 && (j + 1) % 2 == 0) ||
                (i == 0 && j == 0) || (i == 0 && j == 1) || (i == 1 && j == 0) || (i == 0 && j == 2) ||
                (i == 10 && j == 0) || (i == 9 && j == 0) || (i == 10 && j == 1) || (i == 10 && j == 2) ||
                (i == 10 && j == 16) || (i == 9 && j == 16) || (i == 10 && j == 15) || (i == 10 && j == 14) ||
                (i == 0 && j == 15) || (i == 0 && j == 16) || (i == 1 && j == 16) || (i == 2 && j == 16))
                    Debug.Log("Wall or Free Tile");
                else
                {
                    if (chanceToCreateTile >= Random.Range(0.00f, 1.00f))
                    {
                        GameObject newTile = Instantiate(explodableBlockTile);
                        newTile.transform.SetParent(newBlockHolderLine.transform);
                        newTile.transform.position = new Vector2(newBeginXPos, newBeginYPos);
                    }
                }
                newBeginXPos += 1;
            }
            newBeginYPos -= 1;
            newBeginXPos = beginXPos;
        }
        Destroy(newBlockHolderBackup);
    }
}
