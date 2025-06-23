using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Map : MonoBehaviour
{
    public int size;
    public GameObject tilePrefab;
    public Dictionary<Vector2, Tile> map;
    private List<GameObject> grids;

    #region Setup

    [ContextMenu("Generate Nodes")]
    public void GenerateNodes()
    {
        int start = size / 2;
        int y = start / 2;
        map = new Dictionary<Vector2, Tile>();
        grids = new List<GameObject>();
        GameObject prefab = new GameObject(" --- Nodes --- ");
        for (int i = -start; i < start; i++)
        {
            for (int p = -y; p < y; p++)
            {
                Vector2 position = new Vector2(i, p);
                map[position] = new Tile(tilePrefab, position);
                //Debug.Log($"New tile created at positon{i}, {p}");
                GameObject tile = Instantiate(tilePrefab);
                grids.Add(tile);
                tile.transform.SetParent(prefab.transform, worldPositionStays: true);
                tile.transform.position = new Vector3(i, p, 10);
            }
        }

        ProcedurallyGenerateNodes(map);
    }

    [ContextMenu("Clear Nodes")]
    public void ClearNodes()
    {
        map = new Dictionary<Vector2, Tile>();
        foreach (var tile in grids)
        {
            DestroyImmediate(tile);
        }
        grids.Clear();
    }

    private void ProcedurallyGenerateNodes(Dictionary<Vector2, Tile> input)
    {
        //foreach (Tile tile in map)
        {
            //if(isOuterEdge(tile,size / 2, size / 4))
                //continue;
            
            
        }
    }

    private bool isOuterEdge(Tile tile, int x, int y)
    {
        return tile.Position.x != x || tile.Position.x != -x || tile.Position.y != -y || tile.Position.y != y;
    }

    #endregion

    #region Helper Methods

    public Dictionary<Vector2, Tile> getMap()
    {
        return map;
    }

    #endregion

}
