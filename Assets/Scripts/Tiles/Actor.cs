using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Actor
{
    public float2 Position;

    public Dictionary<int2, Tile> Tiles;

    public Actor(float2 position, ref Dictionary<int2, Tile> tiles)
    {
        Position = position;
        Tiles = tiles;
    }

    public void OnUpdate() 
    {
        if (!IsColliding())
        {
            this.Position.x += 0.4f * Time.deltaTime;
        }
    }

    private bool IsColliding()
    {
        int2[] oTiles = new int2[4];

        int cx = Mathf.CeilToInt(this.Position.x);
        int cy = Mathf.CeilToInt(this.Position.y);
        int fx = Mathf.FloorToInt(this.Position.x);
        int fy = Mathf.FloorToInt(this.Position.y);

        oTiles[0] = new int2(cx, cy);
        oTiles[1] = new int2(fx, fy);
        oTiles[2] = new int2(cx, fy);
        oTiles[3] = new int2(fx, cy);

        foreach (int2 pos in oTiles)
        {
            Tile tile;
            Tiles.TryGetValue(pos, out tile);

            if (tile != null) return true;
        }

        return false;
    }
    
}
