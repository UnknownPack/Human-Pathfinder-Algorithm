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
        if (Input.GetMouseButtonDown(0)) 
        {
            Debug.Log("Pressed");
        
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
        
            if (hit.collider != null && hit.collider.gameObject.CompareTag("Tile"))
            {
                Vector2 tilePosition = hit.collider.gameObject.transform.position;
                Vector2 key = new Vector2(Mathf.Round(tilePosition.x), Mathf.Round(tilePosition.y));
                selectedTile = GetComponent<Map>().map[key];
                Debug.Log($"{key} tile is assigned");
            }
            else
            {
                selectedTile = null;
            }
        }
    }


    void SpawnUiAtTile(Tile tile)
    {
        Vector2 offset = new Vector2(0.3f, 0f);
    }
    
    
}
