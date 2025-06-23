using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class Map : MonoBehaviour
{
    public GameObject camera;
    public float timer = 125f;
    public Vector2 goalVector2;
    public int size;
    public int numberOfColonies = 4;
    public int numberOfResourceNodes = 9;
    public int numberOfWall = 4;
    public GameObject tilePrefab;
    public Dictionary<Vector2, Tile> map;
    private Dictionary<Vector2, GameObject> _grids;
    private GameObject nodeParent;
    private float currentTime;
    private Camera _camera;
    private UIDocument uiDocument;
    private ProgressBar timerProgressBar;
    private Label currentLevel, capSpeed, availableNodes, totalNodes;
    private int currentLevelCount = 1;
    private PlayerController player;
    


    #region Setup

    void Awake()
    {
        GenerateNodes(size);  
    }

    private void Start()
    {
        uiDocument = GetComponent<UIDocument>();
        timerProgressBar = uiDocument.rootVisualElement.Q<ProgressBar>("timer");
        currentTime = timer;
        timerProgressBar.highValue = timer;
        timerProgressBar.value = currentTime;
        currentLevel = uiDocument.rootVisualElement.Q<Label>("levelDisplay");
        capSpeed = uiDocument.rootVisualElement.Q<Label>("CapSpeed");
        totalNodes = uiDocument.rootVisualElement.Q<Label>("totalNodes");
        availableNodes = uiDocument.rootVisualElement.Q<Label>("nodeCount");
        _camera = camera.GetComponent<Camera>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            Destroy(nodeParent);
            GenerateNodes(size);
        }
        
        currentLevel.text = "Level: " + currentLevelCount;
        if(player== null)
        {
            Debug.LogError("PlayerController not set in Map script.");
            return;
        } 
        capSpeed.text = "Capture Speed: " + player.captureTime; 
        availableNodes.text = $"Cap Nodes: {player.numberOfValidNodesToUse}/{player.maxValidNodesToUse}";
        totalNodes.text = $"Total nodes captured:\n " +
                          $"{player.totalNodesCaptured}";
        currentTime -= Time.deltaTime;
        timerProgressBar.value = currentTime;
        //Debug.Log($"Progression: {timerProgressBar.value}");
        if(currentTime <= 0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    [ContextMenu("Generate Nodes")]
    public void GenerateNodes(int size)
    { 
        map = new Dictionary<Vector2, Tile>();
        _grids = new Dictionary<Vector2, GameObject>();
        if(nodeParent!= null)
        {
            Destroy(nodeParent);
        }
        nodeParent = new GameObject(" --- Nodes --- ");
        for (int i = -size; i <= size; i++)
        {
            for (int p = -size; p <= size; p++)
            {
                Vector2 position = new Vector2(i, p);
                GameObject tile = Instantiate(tilePrefab, nodeParent.transform, true);
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
        
        for (int i = 0; i < numberOfWall && index < positions.Count; i++, index++)
        {
            input[positions[index]].setTileType(TileType.oblivion);
        }

        // Assign the rest as base tiles
        for (; index < positions.Count; index++)
        {
            input[positions[index]].setTileType(TileType.baseTile);
        }

        // Manually assign player and enemy tiles
        input[new Vector2(-size, -size)].setTileType(TileType.playerOwner);
        goalVector2 = new Vector2(size, size);
        input[new Vector2(size, size)].setTileType(TileType.enemyOwned);
    }
 

    #endregion

    #region Helper Methods

    public Dictionary<Vector2, Tile> getMap()
    {
        return map;
    }
    
    public void SetPlayerController(PlayerController playerController)
    {
        player = playerController;
    }

    public void IncreaseMapDifficulty()
    {
        size ++;
        numberOfWall += 2;
        GenerateNodes(size);
        _camera.orthographicSize += 0.5f;
        timer-=0.25f;
        currentTime = timer;
        timerProgressBar.highValue = timer;
        timerProgressBar.lowValue = currentTime;
        currentLevelCount++;
    }

    #endregion

}
