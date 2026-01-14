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

    PhysicsShapeGroup2D colliders = new PhysicsShapeGroup2D();

    void Start()
    {
        TileMap.Add(new int2(0, 0), Tiles.tile);
        TileMap.Add(new int2(1, 0), Tiles.tile);

        GenerateMesh();
    }

    void GenerateMesh()
    {
        matrices.Clear();
        floats.Clear();
        combineInstances.Clear();

        colliders.Clear();

        foreach (var tile in TileMap)
        {

            int2 pos = tile.Key;

            Mesh newMesh = new Mesh();

            newMesh.vertices = mesh.vertices;
            newMesh.triangles = mesh.triangles;
            newMesh.uv = mesh.uv;

            Vector2[] uvCoords = newMesh.uv;

            for (int i = 0; i < uvCoords.Length; i++)
            {
                uvCoords[i] = new Vector2(uvCoords[i].x / 4, uvCoords[i].y);
            }

            newMesh.uv = uvCoords;

            combineInstances.Add(new CombineInstance
            {
                mesh = newMesh,
                transform = Matrix4x4.Translate(new Vector3(pos.x, pos.y, 0))
            });

            floats.Add(tile.Value.GetSprite());

            colliders.AddBox(new Vector2(pos.x, pos.y), new Vector2(1, 1));


        }

        gameObject.GetComponent<CustomCollider2D>().SetCustomShapes(colliders);

        Mesh combinedMesh = new Mesh();
        combinedMesh.CombineMeshes(combineInstances.ToArray());
        gameObject.GetComponent<MeshFilter>().sharedMesh = combinedMesh;
    }

    void Update()
    {

        
    }
}
