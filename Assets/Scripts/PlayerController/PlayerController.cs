using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Tile selectedTile;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        manageSelectedTile();
    }

    void manageSelectedTile()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetMouseButton(0))
        {
            Debug.Log("Pressed");
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.CompareTag("Tile")){
                    Vector2 tilePosition = hit.collider.gameObject.transform.position;
                    Vector2 key = new Vector2(Mathf.Round(tilePosition.x), Mathf.Round(tilePosition.y));
                    selectedTile = GameManager.Instance.map[key];
                    //SpawnUiAtTile(selectedTile);
                    Debug.Log($"{key} tile is assigned");
                }
                else
                    selectedTile = null;
            }
            
        } 
    }

    void SpawnUiAtTile(Tile tile)
    {
        Vector2 offset = new Vector2(0.3f, 0f);
    }
    
    
}
