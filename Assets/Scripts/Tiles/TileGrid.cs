using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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

    // Set Editing to true to place allow placing tiles in play mode
    public bool Editing;

    //set filename to the name of the file you're editing
    public string Filename;

    void Start()
    {
        Load();

        GenerateMesh();
    }

    // Saves tilemap to Assets/Tilemaps/Filename.dat (might not be visible in unity editor)
    void Save() 
    {
        TileMapFile tileMapFile = new TileMapFile();
        tileMapFile.TileMap = TileMap;
        FileStream fileStream = File.Create(Application.dataPath + "/tilemaps/" + Filename + ".dat");
        BinaryFormatter formatter = new BinaryFormatter();

        formatter.Serialize(fileStream, tileMapFile);
        fileStream.Close();
    }

    void Load()
    {
        if (File.Exists(Application.dataPath + "/tilemaps/" + Filename + ".dat"))
        {
            FileStream fileStream = File.Open(Application.dataPath + "/tilemaps/" + Filename + ".dat", FileMode.Open);
            BinaryFormatter formatter = new BinaryFormatter();
            fileStream.Position = 0;
            TileMapFile tileMapFile = (TileMapFile)formatter.Deserialize(fileStream);
            fileStream.Close();
            TileMap = tileMapFile.TileMap;
        }
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
        if (Editing)
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetMouseButton(1))
            { 
                TileMap.TryAdd(new int2(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y)), Tiles.tile);
                GenerateMesh();
            }
            if (Input.GetKeyDown("space"))
            {
                Save();
            }
        }
    }
}

[Serializable]
public struct TileMapFile
{
    public Dictionary<int2, Tile> TileMap;
}
