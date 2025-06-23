using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class Map : MonoBehaviour
{
    public int size;
    public int numberOfColonies = 4;
    public int numberOfResourceNodes = 9;
    public GameObject tilePrefab;
    public Dictionary<Vector2, Tile> map;
    private Dictionary<Vector2, GameObject> _grids;

    #region Setup

    void Awake()
    {
        GenerateNodes();  
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
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
        // Get all eligible positions (excluding player/enemy positions)
        List<Vector2> positions = new List<Vector2>(input.Keys);
        positions.Remove(new Vector2(-size, -size)); // player position
        positions.Remove(new Vector2(size, size));   // enemy position

        // Shuffle the list
        for (int i = 0; i < positions.Count; i++)
        {
            Vector2 temp = positions[i];
            int randomIndex = Random.Range(i, positions.Count);
            positions[i] = positions[randomIndex];
            positions[randomIndex] = temp;
        }

        int index = 0;

        // Assign resource tiles
        for (int i = 0; i < numberOfResourceNodes && index < positions.Count; i++, index++)
        {
            input[positions[index]].setTileType(TileType.resourceTile);
        }

        // Assign neutral colonies
        for (int i = 0; i < numberOfColonies && index < positions.Count; i++, index++)
        {
            input[positions[index]].setTileType(TileType.neutralColony);
        }

        // Assign the rest as base tiles
        for (; index < positions.Count; index++)
        {
            input[positions[index]].setTileType(TileType.baseTile);
        }

        // Manually assign player and enemy tiles
        input[new Vector2(-size, -size)].setTileType(TileType.playerOwner);
        input[new Vector2(size, size)].setTileType(TileType.enemyOwned);
    }
 

    #endregion

    #region Helper Methods

    public Dictionary<Vector2, Tile> getMap()
    {
        return map;
    }

    #endregion

}
