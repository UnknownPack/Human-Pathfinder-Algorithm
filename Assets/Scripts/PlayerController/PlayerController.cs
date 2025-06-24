using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject playerNode;
    public GameObject playerNode_Preview;
    public float captureTime = 1.5f, captureTimePerNode = 0.025f;
    private List<GameObject> playerNodePlaced;
    public int numberOfValidNodesToUse = 1, maxValidNodesToUse;

    public int totalNodesCaptured = 0;
    public Color playerCaptureColor_baseTile;
    public Color playerCaptureColor_resourceTile;
    public Color playerCaptureColor_neutralColony;

    public List<AudioClip> notification;
    
    private Dictionary<Vector2, Tile> owenedTiles, possibleNodesToCapture;
    private Dictionary<Vector2, Tile> mapDictionary;
    private GameObject previewInstance;
    private Map mapInstance;
    private Tile selectedTile;
    private AudioSource _audioSource;

    private List<GameObject> garbage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxValidNodesToUse = 1;
        mapInstance = GetComponent<Map>();
        mapInstance.SetPlayerController(this);
        owenedTiles = new Dictionary<Vector2, Tile>();
        possibleNodesToCapture = new Dictionary<Vector2, Tile>();
        mapDictionary = mapInstance.map;
        playerNodePlaced = new List<GameObject>();
        garbage = new List<GameObject>();
        _audioSource = GetComponent<AudioSource>();
        owenedTiles.Add(mapInstance.startVector, mapDictionary[mapInstance.startVector]);
    }

    // Update is called once per frame
    void Update()
    {
        CheckPosibleNodes();
        manageSelectedTile();

        if (owenedTiles.ContainsKey(mapInstance.goalVector2))
        {
            mapInstance.IncreaseMapDifficulty();
            StopAllCoroutines();
            foreach (GameObject obj in garbage)
            {
                if(obj != null)
                    Destroy(obj);
            } 
            garbage.Clear();
            possibleNodesToCapture.Clear();
            owenedTiles.Clear();
            mapDictionary = mapInstance.map;
            owenedTiles.Add(mapInstance.startVector, mapDictionary[mapInstance.startVector]);
        }
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
            if(possibleNodesToCapture.ContainsKey(tilePosition) 
               && !owenedTiles.ContainsKey(tilePosition)
                && numberOfValidNodesToUse >= 1
               && possibleNodesToCapture[tilePosition].tileType != TileType.oblivion)
            {
                previewInstance = Instantiate(playerNode_Preview, tilePosition, Quaternion.identity);
                if (Input.GetMouseButtonDown(0))
                {
                    
                    numberOfValidNodesToUse--;
                    numberOfValidNodesToUse = Mathf.Max(0, numberOfValidNodesToUse);
                    int index = numberOfValidNodesToUse;
                    
                    GameObject newNode = Instantiate(playerNode, tilePosition, Quaternion.identity);
                    playerNodePlaced.Add(newNode);
                    garbage.Add(newNode); 
                    selectedTile = mapDictionary[key];
                    StartCoroutine(CaptureTile(selectedTile, newNode)); 
                }
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
                    if (Mathf.Abs(i) == 1 && Mathf.Abs(j) == 1)
                        continue;
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
    IEnumerator CaptureTile(Tile tile, GameObject node)
    { 
        Color startColor = tile._spriteRenderer.color, endColor = GetColorToConvert(tile.tileType);
        SpriteRenderer spriteRendererOfNode = node.GetComponent<SpriteRenderer>();
        Color SC = spriteRendererOfNode.color;
        float elapsedTime = 0f, duration = (tile.tileType != TileType.neutralColony)? captureTime : captureTime + 1.5f;
        while (elapsedTime <= duration)
        { 
            if(spriteRendererOfNode!=null)
                spriteRendererOfNode.color = Color.Lerp(SC, Color.clear, elapsedTime / duration);
            if(tile._spriteRenderer!=null)
                tile._spriteRenderer.color = Color.Lerp(startColor, endColor, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        Destroy(node);
        playerNodePlaced.Remove(node) ;
        yield return null;
        
        if(spriteRendererOfNode!=null)
            spriteRendererOfNode.color = Color.clear;
        if(tile._spriteRenderer!=null)
            tile._spriteRenderer.color = endColor; 
        
        if(possibleNodesToCapture.ContainsKey(tile.Position))
            possibleNodesToCapture.Remove(tile.Position);
        BoostPlayerStats(tile);
        owenedTiles.TryAdd(tile.Position, tile); 
        numberOfValidNodesToUse++; 
        totalNodesCaptured++;
    }

    void BoostPlayerStats(Tile tile)
    {
        switch (tile.tileType)
        {
            case (TileType.resourceTile):
                captureTime -= captureTimePerNode;
                break;
            case (TileType.neutralColony):
                numberOfValidNodesToUse++;
                maxValidNodesToUse++;
                numberOfValidNodesToUse = Mathf.Clamp(numberOfValidNodesToUse, 1, mapInstance.numberOfColonies);
                break;
            case TileType.enemyOwned:
                _audioSource.clip = notification[1];
                _audioSource.Play();
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
