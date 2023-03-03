using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationObjet : MonoBehaviour
{
    public static List<GameObject> listeMouettes = new List<GameObject>();
    private GameObject[] mouettes;
    private int nombreMouettesActives = 0;
    private int nombreMouettesPrecedent;

    public static List<GameObject> listeFruits = new List<GameObject>();
    private GameObject[] fruits;
    private int nombreFruitsActifs = 0;
    private int nombreFruitsPrecedent;

    public static int fruitsObtenus = 0;
    public int fruitsParMouettes = 2;
    public int nombreFruitsRequisInitial = 1;

    public float delaiActivationMouettes = 0.5f;
    public float delaiActivationFruits = 1f;

    private Transform playerTransform;
    public static List<GameObject> listeNuages = new List<GameObject>();
    public GameObject[] nuages;

    public int nombreNuages = 20; // Number of clouds to generate
    public float distanceMin = 0f; // Minimum distance from player to generate clouds
    public float distanceMax = 200f; // Maximum distance from player to generate clouds
    public float altitudeMin = 5f; // Minimum altitude of clouds
    public float altitudeMax = 100f; // Maximum altitude of clouds
    public float margeCoteMin = 200f; // Minimum offset from terrain center for cloud position
    public float margeCoteMax = 500f; // Maximum offset from terrain center for cloud position



    void Start()
    {
        switch (gameObject.name)
        {
            case "Mouettes":

                nombreMouettesPrecedent = nombreMouettesActives;

                mouettes = GameObject.FindGameObjectsWithTag("Mouette");

                foreach (GameObject mouette in mouettes)
                {
                    listeMouettes.Add(mouette.transform.parent.gameObject);

                    mouette.transform.parent.gameObject.SetActive(false);
                }

                Invoke("ActiverMouettes", delaiActivationMouettes);

                break;

            case "Fruits":

                nombreFruitsPrecedent = nombreFruitsActifs;

                fruits = GameObject.FindGameObjectsWithTag("Fruit");

                foreach (GameObject fruit in fruits)
                {
                    listeFruits.Add(fruit.transform.parent.gameObject);

                    fruit.transform.parent.gameObject.SetActive(false);
                }

                Invoke("ActiverFruit", delaiActivationFruits);

                break;

            case "Nuages":

                playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

                nuages = GameObject.FindGameObjectsWithTag("Nuage");

                foreach (GameObject nuage in nuages)
                {
                    listeNuages.Add(nuage.transform.parent.gameObject);
                    nuage.transform.parent.gameObject.SetActive(false);
                }

                for (int i = 0; i < nombreNuages; i++)
                    CreerNuage();

                break;
        }
    }

    public void CreerNuage()
    {
        // Choose random cloud prefab from array
        GameObject cloudPrefab = listeNuages[Random.Range(0, listeNuages.Count)];

        cloudPrefab.GetComponentInChildren<GererCollisions>().enabled = true;
        cloudPrefab.GetComponentInChildren<DeplacementDecor>().enabled = true;

        // Choose random side to generate cloud on (left or right)
        bool generateOnLeft = Random.value < 0.5f;

        // Calculate position of cloud
        float xPos = generateOnLeft ?
            Random.Range(-margeCoteMax, -margeCoteMin) :
            Random.Range(margeCoteMin, margeCoteMax);

        float zPos = Random.Range(transform.position.z + distanceMin, transform.position.z + distanceMax);
        float yPos = Random.Range(transform.position.y + altitudeMin, transform.position.y + altitudeMax);
        Vector3 cloudPosition = new Vector3(xPos, yPos, zPos);

        // Instantiate cloud prefab at position
        GameObject cloudInstance = Instantiate(cloudPrefab, cloudPosition, Quaternion.identity, transform);

        cloudInstance.name = cloudPrefab.name;

        cloudInstance.SetActive(true);
    }

    void Update()
    {
        switch (gameObject.name)
        {
            case "Mouettes":

                if (nombreMouettesPrecedent != nombreMouettesActives)
                    nombreMouettesPrecedent = nombreMouettesActives;

                break;

            case "Fruits":

                if (nombreFruitsPrecedent != nombreFruitsActifs)
                    nombreFruitsPrecedent = nombreFruitsActifs;

                break;

            case "Nuages":

                // Destroy clouds that are too far behind the player
                //for (int i = listeNuages.Count - 1; i >= 0; i--)
                //{
                //    if (playerTransform.position.z - listeNuages[i].transform.position.z > maxDistance)
                //    {
                //        Destroy(listeNuages[i]);
                //        listeNuages.RemoveAt(i);
                //    }
                //}

                break;
        }
    }

    public void IncrementerNombreMouettes(int nombreIncrementationMouettes)
    {
        nombreMouettesActives += nombreIncrementationMouettes;

        if (nombreMouettesActives > listeMouettes.Count)
            nombreMouettesActives = listeMouettes.Count;

        if (nombreMouettesActives < 0)
            nombreMouettesActives = 0;

        Invoke("ActiverMouettes", delaiActivationMouettes);
    }

    public void IncrementerNombreFruits()
    {
        fruitsObtenus++;

        // Check if the number of fruits collected is divisible by the specified value
        if (fruitsObtenus % fruitsParMouettes - nombreFruitsRequisInitial == 0)
        {
            IncrementerNombreMouettes(1);
        }
    }

    void ActiverMouettes()
    {
        for (int i = 0; i < nombreMouettesActives; i++)
            listeMouettes[i].SetActive(true);

        for (int i = listeMouettes.Count - 1; i >= nombreMouettesActives; i--)
            listeMouettes[i].SetActive(false);
    }

    public void ActiverFruit()
    {
        for (int i = listeFruits.Count - 1; i >= nombreFruitsActifs; i--)
            listeFruits[i].SetActive(false);

        int indexFruit = Random.Range(0, listeFruits.Count);

        listeFruits[indexFruit].SetActive(true);
    }
}
