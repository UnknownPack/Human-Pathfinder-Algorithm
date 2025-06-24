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
                color = new Color(179f/255f, 179f/255f, 179f/255f);
                break;
            case (TileType.playerOwner):
                color = new Color(82f/255f, 82f/255f, 255f/255f);
                break;
            case TileType.enemyOwned:
                color = new Color(1, 221f/255f, 0);
                break;
            case TileType.neutralColony:
                color = new Color(250f/255f, 166f/255f, 63f/255f);
                break;
            case TileType.resourceTile:
                color = new Color(0.368f, 1f, 0.964f);
                break;
            case TileType.oblivion:
                color = new Color(46f/255f, 46f/255f, 46f/255f);
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
