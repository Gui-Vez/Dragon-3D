using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class GenerationMesh : MonoBehaviour
{
    Mesh mesh;

    Vector3[] sommets;
    int[] triangles;
    //Vector2[] uvs;
    Color[] couleurs;

    public Gradient degrade;

    private float hauteurTerrainMin;
    private float hauteurTerrainMax;

    public int   tailleX = 20;
    public int   tailleZ = 20;
    public float tailleY = 5f;

    private int   tailleXPrecedente;
    private int   tailleZPrecedente;
    private float tailleYPrecedente;

    private int  tailleXMin = 1;
    private int  tailleXMax = 100;
    private int  tailleZMin = 1;
    private int  tailleZMax = 100;
    public float tailleYMin = -5f;
    public float tailleYMax = 10f;

    public float bruit = 0.2f;

    public float hauteurVaguesMin = 0.25f;
    public float hauteurVaguesMax = 0.75f;
    private float delaiVagues = 0.5f;

    private bool dimensionsValides;

    public  bool basculerMenuGizmo = false;
    private bool activerSpheresGizmo = false;

    private static List<GameObject> spheresGizmo = new List<GameObject>();
    private static int maxNombreSpheres;

    void Start()
    {
        // Créer un nouveau mesh
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        // Appeler les méthodes pour la création du mesh
        CreerMesh();
        MettreAJourMesh();

        // Actualiser la taille précédente du mesh
        tailleXPrecedente = tailleX;
        tailleZPrecedente = tailleZ;
        tailleYPrecedente = tailleY;
    }

    void Update()
    {
        // Vérifier si les dimensions du mesh fonctionnent
        VerifierDimensions();

        // Si elles ne le sont pas, annuler les prochaines commandes
        if (!dimensionsValides)
            return;


        // Si l'objet scripté est une terrain de vagues procédurales,
        if (gameObject.name == "Vagues Procédurales")
            // Bouger les vagues
            BougerVagues();


        // Si l'on active le menu Gizmo, activer les spheres
        if (basculerMenuGizmo)
            activerSpheresGizmo = !activerSpheresGizmo;

        // Si la taille du mesh n'est pas à jour,
        if (tailleXPrecedente != tailleX || tailleZPrecedente != tailleZ || tailleYPrecedente != tailleY)
        {
            // Appeler les méthodes pour mettre à jour le mesh
            CreerMesh();
            MettreAJourMesh();

            // Assigner les variables de taille pour les mettre à jour
            tailleXPrecedente = tailleX;
            tailleZPrecedente = tailleZ;
            tailleYPrecedente = tailleY;

            // Activer le menu Gizmo
            //basculerMenuGizmo = true;
        }
    }

    void VerifierDimensions()
    {
        // Si la taille désirée est inférieure à la valeur minimale,
        if (tailleX < tailleXMin || tailleZ < tailleZMin || tailleY < tailleYMin)
        {
            // Rendre les dimensions invalides
            //Debug.LogWarning("La dimension du mesh est à sa valeur minimale.");
            dimensionsValides = false;
        }

        // Si la taille désirée est suppérieure à la valeur maximale,
        else if (tailleX > tailleXMax || tailleZ > tailleZMax || tailleY > tailleYMax)
        {
            // Rendre les dimensions invalides
            //Debug.LogWarning("La dimension du mesh est à sa valeur maximale.");
            dimensionsValides = false;
        }

        // Sinon, rendre les dimensions valides
        else
            dimensionsValides = true;

        // Écrêter la taille du mesh entre les valeurs minimales et maximales
        tailleX = Mathf.Clamp(tailleX, tailleXMin, tailleXMax);
        tailleZ = Mathf.Clamp(tailleZ, tailleZMin, tailleZMax);
        tailleY = Mathf.Clamp(tailleY, tailleYMin, tailleYMax);
    }

    void CreerMesh()
    {
        // Contenir les sommet du mesh selon le nombre de tuiles
        sommets = new Vector3[(tailleX + 1) * (tailleZ + 1)];

        // Pour tous les points sur l'axe Z du mesh,
        for (int i = 0, z = 0; z <= tailleZ; z++)
        {
            // Pour tous les points sur l'axe X du mesh,
            for (int x = 0; x <= tailleX; x++)
            {
                // Initialiser un point sur l'axe Y
                float y = 0f;

                // Selon le nom de l'objet scripté,
                switch (gameObject.name)
                {
                    case "Terrain Procédural":

                        // Assigner la valeur Y à une valeur aléatoire à l'aide du générateur de bruit de Perlin
                        y = Mathf.PerlinNoise(x * bruit, z * bruit) * tailleY;

                        // Ajouter une valeur aléatoire (afin de rendre les pointes du terrain plus naturelles)
                        y += Random.Range(-0.5f, 0.5f);

                        // Si le sommet se situe sur un coin ou sur le côté de la surface du mesh,
                        if ((x == 0 && z == 0) || (x == tailleX && z == 0) || (x == 0 && z == tailleZ) || (x == tailleX && z == tailleZ)
                           || (x == 0 && z > 0 && z < tailleZ) || (x == tailleX && z > 0 && z < tailleZ)
                           || (z == 0 && x > 0 && x < tailleX) || (z == tailleZ && x > 0 && x < tailleX))
                            // Nullifier la valeur Y
                            y = 0f;

                        break;

                    case "Vagues Procédurales":

                        // Assigner la valeur Y à une valeur aléatoire à l'aide d'un générateur sinusoïdale
                        y = Mathf.Sin((x * hauteurVaguesMin) + (z * hauteurVaguesMax)) * tailleY;

                        // Assigner la valeur Y à une valeur aléatoire à l'aide d'un générateur de bruit de Perlin
                        //y = Mathf.PerlinNoise(x * bruit, z * bruit) * tailleY - tailleY / 2f;

                        break;
                }

                // Assigner les valeurs de position à chaque sommet
                sommets[i] = new Vector3(x, y, z);


                // Si la valeur Y est suppérieure à la valeur maximale du terrain,
                if (y > hauteurTerrainMax)
                    // Concaténer la valeur maximale du terrain par la valeur Y
                    hauteurTerrainMax = y;

                // Si la valeur Y est inférieure à la valeur minimale du terrain,
                if (y < hauteurTerrainMin)
                    // Concaténer la valeur minimale du terrain par la valeur Y
                    hauteurTerrainMin = y;

                // Incrémenter l'index de la boucle
                i++;
            }
        }

        // Contenir les triangles du mesh selon le nombre de tuiles
        triangles = new int[tailleX * tailleZ * 6];

        // Initialiser les vertex et les tris
        int vertex = 0;
        int tris = 0;

        // Pour tous les points sur l'axe Z du mesh,
        for (int z = 0; z < tailleZ; z++)
        {
            // Pour tous les points sur l'axe X du mesh,
            for (int x = 0; x < tailleX; x++)
            {
                // Assigner la face des triangle selon la position de six vertex
                triangles[tris + 0] = vertex + 0;
                triangles[tris + 1] = vertex + tailleX + 1;
                triangles[tris + 2] = vertex + 1;
                triangles[tris + 3] = vertex + 1;
                triangles[tris + 4] = vertex + tailleX + 1;
                triangles[tris + 5] = vertex + tailleX + 2;

                // Incrémenter le nombre de vertex sur l'axe X
                vertex++;

                // Incrémenter le nombre de tris par 6 (pour chaque vertex de deux triangles juxtaposés)
                tris += 6;
            }

            // Incrémenter le nombre de vertex sur l'axe Z
            vertex++;
        }


        // Contenir les UVs du mesh selon le nombre de sommets
        //uvs = new Vector2[sommets.Length];

        // Contenir les couleurs selon le nombre de sommets
        couleurs = new Color[sommets.Length];

        // Pour tous les points sur l'axe Z du mesh,
        for (int i = 0, z = 0; z <= tailleZ; z++)
        {
            // Pour tous les points sur l'axe X du mesh,
            for (int x = 0; x <= tailleX; x++)
            {
                // Assigner les UVs en fonction de la dimension du mesh
                //uvs[i] = new Vector2((float)x / tailleX, (float)z / tailleZ);

                // Assigner la hauteur du mesh en inversant la fonction qui permet d'obtenir la hauteur de tous les sommets
                float hauteur = Mathf.InverseLerp(hauteurTerrainMin, hauteurTerrainMax, sommets[i].y);

                // Assigner la couleur du dégradé en fonction de la hauteur du mesh
                couleurs[i] = degrade.Evaluate(hauteur);

                // Incrémenter l'index de la boucle
                i++;
            }
        }
    }

    void BougerVagues()
    {
        /*
        // Calculer le temps écoulé depuis le début du jeu
        float time = Time.time;

        // Initialiser la valeur Y
        float y;

        // Calculer la valeur Y en fonction du temps écoulé
        y = Mathf.Sin(time) * hauteurVaguesMin + delaiVagues;

        // Mapper la valeur Y à l'intervalle des deux pôles
        y = Mathf.Lerp(hauteurVaguesMin, hauteurVaguesMax, y);

        // Assigner la valeur Y à la variable tailleY de l'objet
        GetComponent<GenerationMesh>().tailleY = y;
        */
    }

    void MettreAJourMesh()
    {
        // Retirer le mesh déjà existant
        mesh.Clear();

        // Assigner les sommets, triangles, UVs et couleurs du mesh
        mesh.vertices = sommets;
        mesh.triangles = triangles;
        //mesh.uv = uvs;
        mesh.colors = couleurs;

        // Recalculer l'éclairage et le shading de la surface du mesh
        mesh.RecalculateNormals();
        // Recalculer la boîte englobante du mesh (afin de faire du "culling" en enlevant la partie inférieure pour sauver des ressources)
        mesh.RecalculateBounds();

        // Créer la collision du mesh et l'appliquer
        MeshCollider meshCollider = gameObject.GetComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
    }

    void OnDrawGizmos()
    {
        // Si le menu Gizmo n'est pas actif, annuler le reste des commandes
        if (!basculerMenuGizmo)
            return;

        // Sinon, si l'on ne peut activer les spheres Gizmo,
        else if (!activerSpheresGizmo)
        {
            // Pour chaque sphere Gizmo,
            foreach (GameObject sphere in spheresGizmo)
                // Désactiver la sphère
                sphere.SetActive(false);
        }

        // Sinon, si l'on peut activer les spheres Gizmo et qu'il y a des sommets,
        else if (activerSpheresGizmo && sommets != null)
        {
            // Stocker le nombre de spheres
            int nombreSpheres = sommets.Length;

            // Si le nombre de sphères est suppérieure à sa valeur maximale,
            if (nombreSpheres > maxNombreSpheres)
            {
                // Réitérer pour chaque sphère du nombre de sphères les commandes suivantes
                for (int i = maxNombreSpheres; i < nombreSpheres; i++)
                {
                    // Créer un objet sphère
                    GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

                    // Assigner la position de la sphère
                    sphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

                    // Assigner la couleur rouge au rendu du mesh de la sphère
                    sphere.GetComponent<MeshRenderer>().material.color = Color.red;

                    // Ajouter la sphère à la liste
                    spheresGizmo.Add(sphere);
                }

                // Concaténer le nombre maximal de sphères par son nombre actuel
                maxNombreSpheres = nombreSpheres;
            }

            // Réitérer pour chaque sphère utilisée les commandes suivantes
            for (int i = 0; i < nombreSpheres; i++)
            {
                // Assigner la position de la sphère au sommet respectif du mesh
                spheresGizmo[i].transform.position = sommets[i];

                // Activer la sphère
                spheresGizmo[i].SetActive(true);
            }

            // Réitérer pour chaque sphère inutilisée les commandes suivantes
            for (int i = nombreSpheres; i < maxNombreSpheres; i++)
                // Désactiver la sphère
                spheresGizmo[i].SetActive(false);
        }

        // Désactiver le menu Gizmo
        basculerMenuGizmo = false;
    }

    void OnDestroy()
    {
        // Pour chaque sphère Gizmo,
        foreach (var sphere in spheresGizmo)
            // S'il y a une sphère,
            if (sphere != null)
                // Détruire celle-ci
                Destroy(sphere);
    }
}