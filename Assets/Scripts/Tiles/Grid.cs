using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Dictionary<int2, Tile> TileMap = new Dictionary<int2, Tile>();
    [SerializeField] private Mesh mesh;
    [SerializeField] private Material material;
    private List<Sprite> sprites = new List<Sprite>();
    private List<Matrix4x4> matrices = new List<Matrix4x4>();

    void Start()
    {
        TileMap.Add(new int2(0, 0), Tiles.tile);
        TileMap.Add(new int2(1, 0), Tiles.tile);
    }

    void Update()
    {
        matrices.Clear();
        sprites.Clear();
        foreach (var tile in TileMap)
        {
            tile.Value.GetSprite();

            int2 pos = tile.Key;

            matrices.Add(Matrix4x4.Translate(new Vector3(pos.x, pos.y, 0)));
        }

        Graphics.DrawMeshInstanced(mesh, 0, material, matrices);
    }
}
