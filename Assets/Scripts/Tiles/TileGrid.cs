using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class TileGrid : MonoBehaviour
{
    public Dictionary<int2, Tile> TileMap = new Dictionary<int2, Tile>();
    [SerializeField] private Mesh mesh;
    [SerializeField] private Material material;
    private List<float> floats = new List<float>();
    private List<Matrix4x4> matrices = new List<Matrix4x4>();

    private List<CombineInstance> combineInstances = new List<CombineInstance>();

    void Start()
    {
        TileMap.Add(new int2(0, 0), Tiles.tile);
        TileMap.Add(new int2(1, 0), Tiles.tile);
    }

    void Update()
    {

        matrices.Clear();
        floats.Clear();
        foreach (var tile in TileMap)
        {

            int2 pos = tile.Key;

            Mesh mesh1 = this.mesh;

            Vector2[] uvs = mesh1.uv;

            for (int i = 0; i < uvs.Length; i++)
            {
                uvs[i].x = uvs[i].x + 0;
            }

            mesh1.uv = uvs;

            combineInstances.Add(new CombineInstance
            {
                mesh = mesh1,
                transform = Matrix4x4.Translate(new Vector3(pos.x, pos.y, 0))
            });

            floats.Add(tile.Value.GetSprite());

            Mesh combinedMesh = new Mesh();
            combinedMesh.CombineMeshes(combineInstances.ToArray());
            gameObject.GetComponent<MeshFilter>().sharedMesh = combinedMesh;



        }

    }
}
