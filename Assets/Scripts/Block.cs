using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class Block {
    public int health;
    public int points;
    
    [HideInInspector]
    public TileBase tile;

     [HideInInspector]
    public Vector3Int position;

    public void addTileData(TileBase tile, Vector3Int position) {
        this.tile = tile;
        this.position = position;
    }

    public Vector3Int getPosition() {
        return position;
    }

    public TileBase getTile() {
        return tile;
    }
}