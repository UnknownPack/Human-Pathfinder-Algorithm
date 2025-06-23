using UnityEngine;

public class Tile
{
    public BaseFaction owner;
    public Resource resource;
    public Vector2 Position;
    public TileType tileType;
    public GameObject tile;
    public SpriteRenderer _spriteRenderer;
    

    public Tile(GameObject gameObject, Vector2 position)
    {
        owner = null;
        resource = null;
        tile = gameObject;
        _spriteRenderer = tile.GetComponent<SpriteRenderer>();
        tileType = TileType.baseTile;
        Position = position;
    }

    public void SetOwener(BaseFaction baseFaction)
    {
        owner = baseFaction;
        if(baseFaction is EnemyFaction )
            tileType = TileType.enemyOwned;
        else if (baseFaction is AlliedFaction)
            tileType = TileType.playerOwner;
    }
    

    public void setTileType(TileType type)
    {
        tileType = type;
        setTileColour();
    }

    private void setTileColour()
    {
        Color color = Color.white;
        switch (tileType)
        {
            case (TileType.baseTile):
                color = Color.gray;
                break;
            case (TileType.playerOwner):
                color = Color.blue;
                break;
            case TileType.enemyOwned:
                color = Color.red;
                break;
            case TileType.neutralColony:
                color = Color.yellow;
                break;
            case TileType.resourceTile:
                color = Color.green;
                break;
            case TileType.oblivion:
                color = Color.black;
                break;
            default:
                break;
        }
        _spriteRenderer.color = color;
    }
}

public enum TileType
{
    baseTile,
    resourceTile,
    neutralColony,
    oblivion,
    increaseTimer,
    playerOwner,
    enemyOwned
}
