using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class Map : MonoBehaviour
{
    public int size;
    public GameObject tilePrefab;
    public Dictionary<Vector2, Tile> map;
    private Dictionary<Vector2, GameObject> _grids;

    #region Setup

    void Awake()
    {
        GenerateNodes();  
    }
    
    [ContextMenu("Generate Nodes")]
    public void GenerateNodes()
    { 
        map = new Dictionary<Vector2, Tile>();
        _grids = new Dictionary<Vector2, GameObject>();
        GameObject prefab = new GameObject(" --- Nodes --- ");
        for (int i = -size; i <= size; i++)
        {
            for (int p = -size; p <= size; p++)
            {
                Vector2 position = new Vector2(i, p);
                GameObject tile = Instantiate(tilePrefab, prefab.transform, true);
                tile.transform.position = new Vector3(i, p, 10);
            
                _grids.Add(position, tile);
                map[position] = new Tile(tile, position);
            }
        }

        ProcedurallyGenerateNodes(map);
    }

    private void ProcedurallyGenerateNodes(Dictionary<Vector2, Tile> input)
    {
        foreach (KeyValuePair<Vector2, Tile> entry in input)
        { 
            Tile tile = entry.Value;  
            int randomIndex = Random.Range(0, Enum.GetNames(typeof(TileType)).Length-2);
            TileType randomTileType = (TileType)randomIndex;
            tile.setTileType(randomTileType);
        }
        input[new Vector2(-size,-size)].setTileType(TileType.playerOwner);
        input[new Vector2(size,size)].setTileType(TileType.enemyOwned);
    }
 

    #endregion

    #region Helper Methods

    public Dictionary<Vector2, Tile> getMap()
    {
        return map;
    }

    #endregion

}
