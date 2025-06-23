using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Dictionary<Vector2, Tile> map;
    private Map mapComponent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static GameManager Instance;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    
    void Start()
    {
        mapComponent = GetComponent<Map>();
        map = mapComponent.map;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Map

    

    #endregion
}
