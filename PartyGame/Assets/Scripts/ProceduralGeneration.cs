using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UIElements;

public class ProceduralGeneration : MonoBehaviour
{
    public Vector3 roomSize = new Vector2(10, 10);
    public Vector3 wallSize = new Vector2(1, 3);
    public GameObject wallPrefab;
    public List<Matrix4x4> wallMatricesN;
    public List<Matrix4x4> wallMatricesNB;
    public List<Matrix4x4> wallMatricesNC;
    public Mesh wallMesh;
    public Material wallMaterial10;
    public Material wallMaterial11;

    public int seed = 42;

    void Start()
    {
        // create a simple two-submesh wall mesh if none assigned
        if (wallMesh == null)
            wallMesh = CreateWallMesh();

        creationwalls();
    }
    void Update()
    {
        rendeWalls();
    }

    void rendeWalls()
    {
        // draw a list of instances if present
        void DrawList(List<Matrix4x4> list)
        {
            if (list != null && list.Count > 0)
            {
                var arr = list.ToArray();
                // draw submesh 0 with material 10 and submesh 1 with material 11
                if (wallMaterial10 != null)
                    Graphics.DrawMeshInstanced(wallMesh, 0, wallMaterial10, arr, arr.Length);
                if (wallMaterial11 != null)
                    Graphics.DrawMeshInstanced(wallMesh, 1, wallMaterial11, arr, arr.Length);
            }
        }

        DrawList(wallMatricesN);
        DrawList(wallMatricesNB);
        DrawList(wallMatricesNC);
    }

    void createWalls()
    {
        int wallCount = Mathf.Max(1, (int)(roomSize.x / wallSize.x));
        float scale = (roomSize.x / wallCount) / wallSize.x;
        for (int i = 0; i < wallCount; i++)
        {
            var position = transform.position + new Vector3(-roomSize.x / 2 + wallSize.x / scale / 2 + i * wallSize.x, 0, -roomSize.y / 2);
            var s = new Vector3(scale, 1, 1);

            Instantiate(wallPrefab, position, Quaternion.identity);
        }
    }

    void creationwalls()
    {
        Random.InitState(seed);

        wallMatricesN = new List<Matrix4x4>();
        wallMatricesNB = new List<Matrix4x4>();
        wallMatricesNC = new List<Matrix4x4>();

        int WallsCount = Mathf.Max(1, (int)(roomSize.x / wallSize.x));
        float SCAle = (roomSize.x / WallsCount) / wallSize.x;

        for (int i = 0; i < WallsCount; i++)
        {
            // position along X, place wall so its bottom sits at y=0
            float panelWidth = SCAle * wallSize.x;
            var t = transform.position + new Vector3(-roomSize.x / 2 + panelWidth / 2 + i * panelWidth, wallSize.y / 2f, -roomSize.y / 2);
            var r = transform.rotation;
            // scale the unit mesh to desired wall width and height
            var s = new Vector3(panelWidth, wallSize.y, 1f);
            var mat = Matrix4x4.TRS(t, r, s);

            var rand = Random.Range(0f, 3f);
            if (rand < 1f)
                wallMatricesN.Add(mat);
            else if (rand < 2f)
                wallMatricesNB.Add(mat);
            else
                wallMatricesNC.Add(mat);
        }
    }

    // create a simple two-sided quad mesh with two submeshes so it can be drawn with two materials
    Mesh CreateWallMesh()
    {
        var mesh = new Mesh();
        mesh.name = "ProceduralWallUnit";

        // unit quad centered at origin, size 1x1 in XY plane (we scale via matrix)
        var verts = new Vector3[]
        {
            new Vector3(-0.5f, -0.5f, 0f), // 0
            new Vector3( 0.5f, -0.5f, 0f), // 1
            new Vector3( 0.5f,  0.5f, 0f), // 2
            new Vector3(-0.5f,  0.5f, 0f)  // 3
        };

        var uvs = new Vector2[]
        {
            new Vector2(0f, 0f),
            new Vector2(1f, 0f),
            new Vector2(1f, 1f),
            new Vector2(0f, 1f)
        };

        // front-facing triangles (submesh 0)
        var tris0 = new int[] { 0, 1, 2, 2, 3, 0 };
        // back-facing triangles (submesh 1) - reversed winding so a separate material can show on the other side
        var tris1 = new int[] { 2, 1, 0, 0, 3, 2 };

        mesh.vertices = verts;
        mesh.uv = uvs;
        mesh.subMeshCount = 2;
        mesh.SetTriangles(tris0, 0);
        mesh.SetTriangles(tris1, 1);
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        return mesh;
    }
}
