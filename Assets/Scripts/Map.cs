using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public int size;
    public GameObject tilePrefab;
    private Dictionary<Vector2, Tile> map;
    private List<GameObject> grids;

    [ContextMenu("Generate Nodes")]
    public void GenerateNodes()
    {
        int start = size / 2;
        map = new Dictionary<Vector2, Tile>();
        grids = new List<GameObject>();
        GameObject prefab = new GameObject(" --- Nodes --- ");
        for (int i = -start; i < start; i++)
        {
            for (int p = -start; p < start; p++)
            {
                Vector2 position = new Vector2(i, p);
                map[position] = new Tile(position);
                //Debug.Log($"New tile created at positon{i}, {p}");
                GameObject tile = Instantiate(tilePrefab);
                grids.Add(tile);
                tile.transform.SetParent(prefab.transform, worldPositionStays: true);
                tile.transform.position = new Vector3(i, p, 10);
            }
        }
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
}
