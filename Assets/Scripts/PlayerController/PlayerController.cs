using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject playerNode;
    public GameObject playerNode_Preview;
    public float captureTime = 1.5f, captureTimePerNode = 0.25f;
    private GameObject[] playerNodePlaced;
    private int numberOfValidNodesToUse = 1;
    private int numberOfActicveNodes = 0;

    public Color playerCaptureColor_baseTile;
    public Color playerCaptureColor_resourceTile;
    public Color playerCaptureColor_neutralColony;
    private Dictionary<Vector2, Tile> owenedTiles, possibleNodesToCapture;
    private Dictionary<Vector2, Tile> mapDictionary;
    private GameObject previewInstance;
    private Map mapInstance;
    private Tile selectedTile;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mapInstance = GetComponent<Map>();
        owenedTiles = new Dictionary<Vector2, Tile>();
        possibleNodesToCapture = new Dictionary<Vector2, Tile>();
        mapDictionary = mapInstance.map;
        playerNodePlaced = new GameObject[mapInstance.numberOfColonies];
        owenedTiles.Add(new Vector2(-4, -4), mapDictionary[new Vector2(-4, -4)]);
    }

    // Update is called once per frame
    void Update()
    {
        CheckPosibleNodes();
        manageSelectedTile();
    }

    void manageSelectedTile()
    {
        
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
        if (hit.collider != null && hit.collider.gameObject.CompareTag("Tile"))
        {
            Vector2 tilePosition = hit.collider.gameObject.transform.position;
            Vector2 key = new Vector2(Mathf.Round(tilePosition.x), Mathf.Round(tilePosition.y));
            if(previewInstance!= null)
                Destroy(previewInstance);
            previewInstance = Instantiate(playerNode_Preview, tilePosition, Quaternion.identity);
            
            if (Input.GetMouseButtonDown(0) && 
                possibleNodesToCapture.ContainsKey(tilePosition) && 
                numberOfValidNodesToUse > 0) 
            {
                playerNodePlaced[numberOfValidNodesToUse-1] = Instantiate(playerNode, tilePosition, Quaternion.identity);
                selectedTile = mapDictionary[key];
                StartCoroutine(CaptureTile(selectedTile, numberOfValidNodesToUse-1));
                Debug.Log(selectedTile);
            }
        }
        else
        {
            Destroy(previewInstance);
            previewInstance = null;
        }
        
    }

    void CheckPosibleNodes()
    {
        foreach (KeyValuePair<Vector2, Tile> entry in owenedTiles)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    Vector2 position = new Vector2(entry.Key.x + i, entry.Key.y + j);
                    if(mapDictionary.ContainsKey(position) && 
                       isTileValid_ToCapture(mapDictionary[position]) && 
                       !possibleNodesToCapture.ContainsKey(position))
                    {
                        possibleNodesToCapture.Add(position, mapDictionary[position]);
                    }
                }
            }
        }
    }

    bool isTileValid_ToCapture(Tile tile)
    {
        return tile.tileType !=  TileType.playerOwner;
    }
    IEnumerator CaptureTile(Tile tile, int index)
    {
        numberOfValidNodesToUse--;
        Color startColor = tile._spriteRenderer.color, endColor = GetColorToConvert(tile.tileType);
        SpriteRenderer spriteRendererOfNode = playerNodePlaced[index].GetComponent<SpriteRenderer>();
        float elapsedTime = 0f, duration = (tile.tileType != TileType.neutralColony)? captureTime : captureTime * 2.5f;
        while (elapsedTime <= duration)
        {
            spriteRendererOfNode.color = Color.Lerp( tile._spriteRenderer.color, Color.clear, elapsedTime / duration);
            tile._spriteRenderer.color = Color.Lerp(startColor, endColor, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        if(possibleNodesToCapture.ContainsKey(tile.Position))
            possibleNodesToCapture.Remove(tile.Position);
        BoostPlayerStats(tile); 
        owenedTiles.Add(tile.Position, tile);
        spriteRendererOfNode.color = Color.clear;
        tile._spriteRenderer.color = endColor;
        Destroy(playerNodePlaced[index]);
        playerNodePlaced[index] = null;
        numberOfValidNodesToUse++;
    }

    void BoostPlayerStats(Tile tile)
    {
        switch (tile.tileType)
        {
            case (TileType.resourceTile):
                captureTime += captureTimePerNode;
                break;
            case (TileType.neutralColony):
                numberOfValidNodesToUse++;
                numberOfValidNodesToUse = Mathf.Clamp(numberOfValidNodesToUse, 1, mapInstance.numberOfColonies);
                break;
        }
    }

    Color GetColorToConvert(TileType tileType)
    {
        switch (tileType)
        {
            case (TileType.resourceTile):
                return playerCaptureColor_resourceTile;
            case (TileType.neutralColony):
                return playerCaptureColor_neutralColony;
            case (TileType.baseTile):
                return playerCaptureColor_baseTile;
            default:
                return playerCaptureColor_baseTile;
        }
    }
    
    
}
