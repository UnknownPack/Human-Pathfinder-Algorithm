using UnityEngine;

public class Tile
{
    public BaseFaction owner;
    public Resource resource;
    public Vector2 Position;
    public TileType tileType;
    

    public Tile(Vector2 position)
    {
        owner = null;
        resource = null;
        tileType = TileType.baseTile;
        Position = position;
    }

    public void SetOwener(BaseFaction baseFaction)
    {
        owner = baseFaction;
        tileType = TileType.colonizedTile;
    }
}

public enum TileType
{
    baseTile,
    colonizedTile,
    resourceTile
}
