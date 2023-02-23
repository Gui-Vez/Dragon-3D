using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class GenerationMesh : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;
    //Vector2[] uvs;
    Color[] couleurs;

    public Gradient degrade;

    private float hauteurTerrainMin;
    private float hauteurTerrainMax;

    public int   xSize = 20;
    public int   zSize = 20;
    public float ySize = 5f;

    private int   prevXSize;
    private int   prevZSize;
    private float prevYSize;

    private int  xSizeMin = 1;
    private int  xSizeMax = 100;
    private int  zSizeMin = 1;
    private int  zSizeMax = 100;
    public float ySizeMin = -5f;
    public float ySizeMax = 10f;

    private bool dimensionsValides;

    public  bool toggleMenuGizmo = false;
    private bool activerSpheresGizmo = false;

    private static List<GameObject> gizmoSpheres = new List<GameObject>();
    private static int maxSphereCount;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateShape();
        UpdateMesh();

        prevXSize = xSize;
        prevZSize = zSize;
        prevYSize = ySize;
    }

    void Update()
    {
        VerifierDimensions();

        if (!dimensionsValides)
            return;


        if (toggleMenuGizmo)
            activerSpheresGizmo = !activerSpheresGizmo;

        if (prevXSize != xSize || prevZSize != zSize || prevYSize != ySize)
        {
            CreateShape();
            UpdateMesh();

            prevXSize = xSize;
            prevZSize = zSize;
            prevYSize = ySize;

            toggleMenuGizmo = true;
        }
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
                if   ((x == 0 && z == 0) || (x == xSize && z == 0) || (x == 0 && z == zSize) || (x == xSize && z == zSize)
                   || (x == 0 && z > 0 && z < zSize) || (x == xSize && z > 0 && z < zSize)
                   || (z == 0 && x > 0 && x < xSize) || (z == zSize && x > 0 && x < xSize))
                {
                    y = 0f;
                }

                vertices[i] = new Vector3(x, y, z);

                if (y > hauteurTerrainMax)
                    hauteurTerrainMax = y;
                if (y < hauteurTerrainMin)
                    hauteurTerrainMin = y;

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


        //uvs = new Vector2[vertices.Length];
        couleurs = new Color[vertices.Length];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                //uvs[i] = new Vector2((float)x / xSize, (float)z / zSize);
                float hauteur = Mathf.InverseLerp(hauteurTerrainMin, hauteurTerrainMax, vertices[i].y);
                couleurs[i] = degrade.Evaluate(hauteur);
                i++;
            }
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        //mesh.uv = uvs;
        mesh.colors = couleurs;

        mesh.RecalculateNormals();

        mesh.RecalculateBounds();
        MeshCollider meshCollider = gameObject.GetComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
    }

    void OnDestroy()
    {
        foreach (var sphere in gizmoSpheres)
        {
            if (sphere != null)
            {
                Destroy(sphere);
            }
        }
    }

    void OnDrawGizmos()
    {
        if (!toggleMenuGizmo)
            return;

        else if (!activerSpheresGizmo)
        {
            foreach (GameObject sphere in gizmoSpheres)
                sphere.SetActive(false);
        }

        else if (activerSpheresGizmo && vertices != null)
        {
            // Check if we need to resize the sphere pool
            int sphereCount = vertices.Length;

            if (maxSphereCount < sphereCount)
            {
                for (int i = maxSphereCount; i < sphereCount; i++)
                {
                    GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    sphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                    sphere.GetComponent<MeshRenderer>().material.color = Color.red;
                    gizmoSpheres.Add(sphere);
                }

                maxSphereCount = sphereCount;
            }

            // Set the position of each sphere
            for (int i = 0; i < sphereCount; i++)
            {
                gizmoSpheres[i].transform.position = vertices[i];
                gizmoSpheres[i].SetActive(true);
            }

            // Disable unused spheres
            for (int i = sphereCount; i < maxSphereCount; i++)
            {
                gizmoSpheres[i].SetActive(false);
            }
        }

        toggleMenuGizmo = false;
    }


    void VerifierDimensions()
    {
        if (xSize < xSizeMin || zSize < zSizeMin || ySize < ySizeMin)
        {
            Debug.LogWarning("La dimension du mesh est à sa valeur minimale.");
            dimensionsValides = false;
        }

        else if (xSize > xSizeMax || zSize > zSizeMax || ySize > ySizeMax)
        {
            Debug.LogWarning("La dimension du mesh est à sa valeur maximale.");
            dimensionsValides = false;
        }

        else
        {
            dimensionsValides = true;
        }

        xSize = Mathf.Clamp(xSize, xSizeMin, xSizeMax);
        zSize = Mathf.Clamp(zSize, zSizeMin, zSizeMax);
        ySize = Mathf.Clamp(ySize, ySizeMin, ySizeMax);
    }
}