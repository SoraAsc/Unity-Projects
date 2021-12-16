using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remover membros privados não utilizados", Justification = "To avoid warnings in private methods provided by Unity.")]
public class Spikes : Enemy
{
    private Crusher crusher;
    Tilemap colTileMap, mainTileMap, destroyedTileMap;
    [SerializeField]
    private List<string> ignoreTilesName;
    [SerializeField] List<TileBase> destroyedTiles; // 0-2:TopBottom | 4-6:Top | 8-9:Middle | 12-14:Bottom  = 0,4,8,12:Left - 1,5,9,13:Middle - 2,6,10,14:Right

    List<string> destroyedTilesNames;
    private void Start()
    {
        InitializeComponent();
        InitializeHurthVar();
        crusher = transform.parent.GetComponent<Crusher>();
        mainTileMap = GameObject.FindGameObjectWithTag("TileMain").GetComponent<Tilemap>();
        colTileMap = GameObject.FindGameObjectWithTag("TileCol").GetComponent<Tilemap>();
        destroyedTileMap = GameObject.FindGameObjectWithTag("TileDestroyed").GetComponent<Tilemap>();
        destroyedTilesNames = new List<string>();
        foreach (TileBase currentTileBase in destroyedTiles)
        {
            if(currentTileBase)
                destroyedTilesNames.Add(currentTileBase.name);
        }        
    }

    protected override void LoseHealth(int damage = 1)
    {
        base.LoseHealth(damage);
        crusher.CallLoseHealth(damage);
    }
    private void FixedUpdate()
    {
        ChangeTile(mainTileMap.WorldToCell(transform.GetChild(0).position),
            mainTileMap.WorldToCell(transform.GetChild(1).position));
    }

    private void ChangeTile(Vector3Int leftLocation, Vector3Int rightLocation)
    {
        TileBase leftColTileBase = colTileMap.GetTile(leftLocation);
        TileBase rightColTileBase = colTileMap.GetTile(rightLocation);

        if (leftColTileBase || rightColTileBase)
        {
            bool canPlaceLeft = CheckIfCanBePlaced(mainTileMap.GetTile(leftLocation));
            bool canPlaceRight = CheckIfCanBePlaced(mainTileMap.GetTile(rightLocation));

            if (canPlaceLeft) colTileMap.SetTile(leftLocation, null);
            if (canPlaceRight) colTileMap.SetTile(rightLocation, null);

            OrganizeTiles(leftLocation, rightLocation, canPlaceLeft, canPlaceRight);
            if (canPlaceLeft || canPlaceRight) { crusher.HitFloor = true; }
        }
    }

    private bool CheckIfCanBePlaced(TileBase mainTileBase)
    {
        if (ignoreTilesName.Contains(mainTileBase.name.Substring(0, mainTileBase.name.LastIndexOf("_")) ))
        {
            crusher.HitFloor = true;
            return false;
        }
        return true;
    }

    private bool AlreadyDestroyed(TileBase destroyedTileBase)
    {
        if (destroyedTileBase != null)
        {
            return true;
        }
        return false;
    }

    private bool OrganizeTiles(Vector3Int leftLocation, Vector3Int rightLocation, bool canLeft, bool canRight)
    {
        TileBase rightTileBase = mainTileMap.GetTile(rightLocation);
        TileBase rightDTileBase = destroyedTileMap.GetTile(rightLocation);
        TileBase leftTileBase = mainTileMap.GetTile(leftLocation);
        TileBase leftDTileBase = destroyedTileMap.GetTile(leftLocation);

        string leftTileName = "Tile_streetDestroyedLeftTopBottom";
        string middleTileName = "Tile_streetDestroyedMiddleTopBottom";
        string rightTileName = "Tile_streetDestroyedRightTopBottom";

        TileBase leftTile = destroyedTiles[0];
        TileBase middleTile = destroyedTiles[1];
        TileBase rightTile = destroyedTiles[2];
        // 0-2:TopBottom | 4-6:Top | 8-9:Middle | 12-14:Bottom  = 0,4,8,12:Left - 1,5,9,13:Middle - 2,6,10,14:Right
        if((AlreadyDestroyed(destroyedTileMap.GetTile(leftLocation + Vector3Int.up)) || AlreadyDestroyed(destroyedTileMap.GetTile(rightLocation + Vector3Int.up))) &&
            (!AlreadyDestroyed(mainTileMap.GetTile(leftLocation + Vector3Int.down)) || !AlreadyDestroyed(mainTileMap.GetTile(rightLocation + Vector3Int.down)) ) &&
            (canLeft || canRight))
        {
            if (canLeft)
            {
                destroyedTileMap.SetTile(leftLocation + Vector3Int.up, destroyedTiles.Find(x => x &&
                    x.name.Equals(destroyedTileMap.GetTile(leftLocation + Vector3Int.up).name.Replace("Bottom", "Middle"))));
            }
            if (canRight)
            {
                destroyedTileMap.SetTile(rightLocation + Vector3Int.up, destroyedTiles.Find(x => x &&
                    x.name.Equals(destroyedTileMap.GetTile(rightLocation + Vector3Int.up).name.Replace("Bottom", "Middle"))));
            }
            leftTile = destroyedTiles.Find(x => x && x.name.Equals(leftTileName.Replace("TopBottom", "Middle")));
            middleTile = destroyedTiles.Find(x => x && x.name.Equals(middleTileName.Replace("TopBottom", "Middle")));
            rightTile = destroyedTiles.Find(x => x && x.name.Equals(rightTileName.Replace("TopBottom", "Middle")));
        }
        else if ( (AlreadyDestroyed(destroyedTileMap.GetTile(leftLocation+Vector3Int.up) ) || AlreadyDestroyed(destroyedTileMap.GetTile(rightLocation + Vector3Int.up)) ) &&
            (canLeft || canRight))
        {
            if (canLeft)
            {
                destroyedTileMap.SetTile(leftLocation + Vector3Int.up, destroyedTiles.Find(x => x &&
                    x.name.Equals(destroyedTileMap.GetTile(leftLocation + Vector3Int.up).name.Replace("Bottom", ""))));
            }
            if(canRight)
            {
                destroyedTileMap.SetTile(rightLocation + Vector3Int.up, destroyedTiles.Find(x => x &&
                    x.name.Equals(destroyedTileMap.GetTile(rightLocation + Vector3Int.up).name.Replace("Bottom", ""))));
            }
            leftTile = destroyedTiles.Find(x => x && x.name.Equals(leftTileName.Replace("Top", "")));
            middleTile = destroyedTiles.Find(x => x && x.name.Equals(middleTileName.Replace("Top", "")));
            rightTile = destroyedTiles.Find(x => x && x.name.Equals(rightTileName.Replace("Top", "")));
        }

        if (!AlreadyDestroyed(leftDTileBase) && canLeft)
        {
            TileBase leftOfLeftDTileBase = destroyedTileMap.GetTile(leftLocation + Vector3Int.left);                

            if (rightDTileBase && rightDTileBase.name.Contains("Left"))
            {
                destroyedTileMap.SetTile(rightLocation, middleTile);
            }

            if(leftOfLeftDTileBase && leftOfLeftDTileBase.name.Contains("Right"))
            {
                destroyedTileMap.SetTile(leftLocation, !CheckIfCanBePlaced(rightTileBase) ? rightTile : middleTile);
                destroyedTileMap.SetTile(leftLocation + Vector3Int.left, middleTile);
            }
            else destroyedTileMap.SetTile(leftLocation, CheckIfCanBePlaced(rightTileBase) ? leftTile : rightTile);
        }
        if (!AlreadyDestroyed(rightDTileBase) && canRight)
        {
            TileBase rightOfRightDTileBase = destroyedTileMap.GetTile(rightLocation + Vector3Int.right);

            if (leftDTileBase && leftDTileBase.name.Contains("Right"))
            {
                destroyedTileMap.SetTile(leftLocation, middleTile);
            }
            else if (rightOfRightDTileBase && rightOfRightDTileBase.name.Contains("Left"))
            {
                destroyedTileMap.SetTile(rightLocation + Vector3Int.right, middleTile);
            }

            leftDTileBase = destroyedTileMap.GetTile(leftLocation);
            rightOfRightDTileBase = destroyedTileMap.GetTile(rightLocation + Vector3Int.right);

            if ( ( rightOfRightDTileBase && rightOfRightDTileBase.name.Contains("Right") ) || 
                (rightOfRightDTileBase && rightOfRightDTileBase.name.Contains("Middle") &&
                leftDTileBase && leftDTileBase.name.Contains("Left")))
            {
                destroyedTileMap.SetTile(rightLocation, middleTile);
            }
            else
            {
                destroyedTileMap.SetTile(rightLocation, CheckIfCanBePlaced(leftTileBase) ? rightTile : leftTile);
            }
        }

        return true;
    }



}
