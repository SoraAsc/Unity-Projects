using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remover membros privados não utilizados", Justification = "To avoid warnings in private methods provided by Unity.")]
[RequireComponent(typeof(Tilemap))]
public class DestroyedTile : MonoBehaviour
{
    Tilemap colTileMap, mainTileMap, destroyedTileMap;
    [SerializeField] private List<string> ignoreTilesName;
    [SerializeField] RuleTile destroyedRuleTile;
    private void Start()
    {
        mainTileMap = GameObject.FindGameObjectWithTag("TileMain").GetComponent<Tilemap>();
        colTileMap = GameObject.FindGameObjectWithTag("TileCol").GetComponent<Tilemap>();
        destroyedTileMap = GetComponent<Tilemap>();
    }

    public bool ChangeTile(Vector3 leftLocation, Vector3 rightLocation, Crusher crusher)
    {
        return ChangeTile(mainTileMap.WorldToCell(leftLocation), mainTileMap.WorldToCell(rightLocation), crusher);
    }

    private bool ChangeTile(Vector3Int leftLocation, Vector3Int rightLocation, Crusher crusher)
    {
        TileBase leftColTileBase = colTileMap.GetTile(leftLocation);
        TileBase rightColTileBase = colTileMap.GetTile(rightLocation);
        bool canChange = false;
        if (leftColTileBase || rightColTileBase)
        {
            canChange = true;
            bool placedAtLeft = CheckIfCanBePlaced(mainTileMap.GetTile(leftLocation));
            bool placedAtRight = CheckIfCanBePlaced(mainTileMap.GetTile(rightLocation));

            if (placedAtLeft && placedAtRight)
            {
                colTileMap.SetTile(leftLocation, null); destroyedTileMap.SetTile(leftLocation, destroyedRuleTile);
                colTileMap.SetTile(rightLocation, null); destroyedTileMap.SetTile(rightLocation, destroyedRuleTile);
            }

           crusher.HitFloor = true;
        }
        return canChange;
    }

    private bool CheckIfCanBePlaced(TileBase mainTileBase)
    {
        if (ignoreTilesName.Contains(mainTileBase.name.Substring(0, mainTileBase.name.LastIndexOf("_"))))
        {
            return false;
        }
        return true;
    }

}
