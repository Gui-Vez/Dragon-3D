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

    public static List<GameObject> listeNuages = new List<GameObject>();
    private GameObject[] nuages;

    public int nombreNuages = 20;
    public float distanceMin = 0f;
    public float distanceMax = 200f;
    public float altitudeMin = 5f;
    public float altitudeMax = 100f;
    public float margeCoteMin = 200f;
    public float margeCoteMax = 500f;

    void Start()
    {
        // S�lectionner la section en fonction du nom de l'objet
        switch (gameObject.name)
        {
            // Si le nom est "Mouettes",
            case "Mouettes":

                // Enregistrer le nombre de mouettes actives avant de les d�sactiver
                nombreMouettesPrecedent = nombreMouettesActives;

                // Trouver tous les objets avec le tag "Mouette"
                mouettes = GameObject.FindGameObjectsWithTag("Mouette");

                // Pour chaque mouette,
                foreach (GameObject mouette in mouettes)
                {
                    // Ajouter l'objet parent � la liste
                    listeMouettes.Add(mouette.transform.parent.gameObject);

                    // Puis le d�sactiver
                    mouette.transform.parent.gameObject.SetActive(false);
                }

                // Activer les mouettes apr�s un certain d�lai
                Invoke("ActiverMouettes", delaiActivationMouettes);

                break;

            // Si le nom est "Fruits",
            case "Fruits":

                // Enregistrer le nombre de fruits actifs avant de les d�sactiver
                nombreFruitsPrecedent = nombreFruitsActifs;

                // Trouver tous les objets avec le tag "Fruit"
                fruits = GameObject.FindGameObjectsWithTag("Fruit");

                // Pour chaque fruit,
                foreach (GameObject fruit in fruits)
                {
                    // Ajouter l'objet parent � la liste
                    listeFruits.Add(fruit.transform.parent.gameObject);

                    // Puis le d�sactiver
                    fruit.transform.parent.gameObject.SetActive(false);
                }

                // Activer les fruits apr�s un certain d�lai
                Invoke("ActiverFruit", delaiActivationFruits);

                break;

            // Si le nom est "Nuages",
            case "Nuages":

                // Trouver tous les objets avec le tag "Nuage"
                nuages = GameObject.FindGameObjectsWithTag("Nuage");

                // Pour chaque nuage,
                foreach (GameObject nuage in nuages)
                {
                    // Ajouter l'objet parent � la liste
                    listeNuages.Add(nuage.transform.parent.gameObject);

                    // Puis le d�sactiver
                    nuage.transform.parent.gameObject.SetActive(false);
                }

                // Cr�er des nuages initiaux
                for (int i = 0; i < nombreNuages; i++)
                    CreerNuage();

                break;
        }
    }

    public void CreerNuage()
    {
        // S�lectionner un nuage al�atoire dans la liste de nuages
        GameObject nuagePrefab = listeNuages[Random.Range(0, listeNuages.Count)];

        // Activer les scripts de collision et de d�placement
        nuagePrefab.GetComponentInChildren<GererCollisions>().enabled = true;
        nuagePrefab.GetComponentInChildren<DeplacementDecor>().enabled = true;

        // Choisir une position al�atoire entre l'emplacement de gauche ou de droite
        bool generateAGauche = Random.value < 0.5f;

        // Calculer la position X du nuage
        float posX = generateAGauche ?
            Random.Range(-margeCoteMax, -margeCoteMin) :
            Random.Range(margeCoteMin, margeCoteMax);

        // Calculer la position Y et Z du nuage
        float posY = Random.Range(transform.position.y + altitudeMin, transform.position.y + altitudeMax);
        float posZ = Random.Range(transform.position.z + distanceMin, transform.position.z + distanceMax);

        // Attribuer la position du nuage selon les axes X, Y et Z
        Vector3 positionNuage = new Vector3(posX, posY, posZ);

        // Instancier le nuage
        GameObject instanceNuage = Instantiate(nuagePrefab, positionNuage, Quaternion.identity, transform);

        // Affecter le nom du clone
        instanceNuage.name = nuagePrefab.name;

        // Activer le clone
        instanceNuage.SetActive(true);
    }

    void Update()
    {
        // Selon le nom de l'objet,
        switch (gameObject.name)
        {
            // S'il s'agit de mouettes,
            case "Mouettes":
                // Si le nombre de mouettes est diff�rent,
                if (nombreMouettesPrecedent != nombreMouettesActives)
                    // Affecter le nombre de mouettes pr�c�dent
                    nombreMouettesPrecedent = nombreMouettesActives;
                break;

            // S'il s'agit de fruits,
            case "Fruits":
                // Si le nombre de fruits est diff�rent,
                if (nombreFruitsPrecedent != nombreFruitsActifs)
                    // Affecter le nombre de fruits pr�c�dent
                    nombreFruitsPrecedent = nombreFruitsActifs;
                break;
        }
    }

    public void IncrementerNombreMouettes(int nombreIncrementationMouettes)
    {
        // Incr�menter le nombre de mouettes
        nombreMouettesActives += nombreIncrementationMouettes;

        // Si le nombre de mouettes est supp�rieur � la liste de mouettes,
        if (nombreMouettesActives > listeMouettes.Count)
            // Correspondre le nombre de mouettes selon le nombre de mouettes sur la liste 
            nombreMouettesActives = listeMouettes.Count;

        // Si le nombre de mouettes actives est inf�rieur � 0,
        if (nombreMouettesActives < 0)
            // �tablir la variable � 0
            nombreMouettesActives = 0;

        // Appeler la m�thode qui active les mouettes apr�s un certain d�lai
        Invoke("ActiverMouettes", delaiActivationMouettes);
    }

    public void IncrementerNombreFruits()
    {
        // Incr�menter le nombre de fruits obtenus
        fruitsObtenus++;

        // Si le nombre de fruits obtenus est divisible par le nombre de fruits par mouette et sa valeur requise initialement,
        if (fruitsObtenus % fruitsParMouettes - nombreFruitsRequisInitial == 0)
            // Incr�menter le nombre de mouettes
            IncrementerNombreMouettes(1);
    }

    void ActiverMouettes()
    {
        // Activer les mouettes actives de la liste
        for (int i = 0; i < nombreMouettesActives; i++)
            listeMouettes[i].SetActive(true);

        // D�sactiver les mouettes non-actives de la liste
        for (int i = listeMouettes.Count - 1; i >= nombreMouettesActives; i--)
            listeMouettes[i].SetActive(false);
    }

    public void ActiverFruit()
    {
        // D�sactiver les fruits non-actifs de la liste
        for (int i = listeFruits.Count - 1; i >= nombreFruitsActifs; i--)
            listeFruits[i].SetActive(false);

        // Trouver l'index du fruit � choisir de fa�on al�atoire
        int indexFruit = Random.Range(0, listeFruits.Count);

        // Activer le fruit al�atoire
        listeFruits[indexFruit].SetActive(true);
    }
}
