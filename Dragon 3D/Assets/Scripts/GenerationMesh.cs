using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class GenerationMesh : MonoBehaviour
{

    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;

    public int   xSize = 20;
    public int   zSize = 20;
    public float ySize = 2f;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateShape();
        UpdateMesh();
    }

    void Update()
    {
        CreateShape();
        UpdateMesh();   
    }

    void CreateShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y = 0f;

                switch (name)
                {
                    case "Terrain Procédural":
                        y = Mathf.PerlinNoise(x * 0.3f, z * 0.3f) * ySize;
                        break;

                    case "Vagues Procédurales":
                        y = Mathf.Sin((x * 0.5f) + (z * 0.5f)) * ySize;
                        break;
                }

                // check if vertex is on a corner or edge of the plane
                if ((x == 0 && z == 0) || (x == xSize && z == 0) || (x == 0 && z == zSize) || (x == xSize && z == zSize)
                    || (x == 0 && z > 0 && z < zSize) || (x == xSize && z > 0 && z < zSize)
                    || (z == 0 && x > 0 && x < xSize) || (z == zSize && x > 0 && x < xSize))
                {
                    y = 0f; // set y position to 0
                }

                vertices[i] = new Vector3(x, y, z);
                i++;
            }
        }

        triangles = new int[xSize * zSize * 6];

        int vert = 0;
        int tris = 0;

        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }

            vert++;
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
        
        // optionally, add a mesh collider (As suggested by Franku Kek via Youtube comments).
        // To use this, your MeshGenerator GameObject needs to have a mesh collider
        // component added to it.  Then, just re-enable the code below.
        /*
        mesh.RecalculateBounds();
        MeshCollider meshCollider = gameObject.GetComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
        //*/
    }

    // Optionally, draw spheres at each vertex
    private void OnDrawGizmos()
    {
        if (vertices == null)
            return;

        for (int i=0; i<vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }
}