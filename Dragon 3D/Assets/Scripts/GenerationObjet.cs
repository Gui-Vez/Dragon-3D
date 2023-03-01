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

    public static List<GameObject> listeNuages = new List<GameObject>();
    private GameObject[] nuages;
    private int nombreNuagesActifs = 0;
    private int nombreNuagesPrecedent;

    public static int fruitsObtenus = 0;
    public int fruitsParMouettes = 2;
    public int nombreFruitsRequisInitial = 1;

    public float delaiActivationMouettes = 0.5f;
    public float delaiActivationFruits = 1f;

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

                nombreFruitsPrecedent = nombreNuagesActifs;

                nuages = GameObject.FindGameObjectsWithTag("Nuage");

                foreach (GameObject nuage in nuages)
                {
                    listeNuages.Add(nuage.transform.parent.gameObject);
                }

                ActiverNuages();

                break;
        }
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

    void ActiverNuages()
    {
        for (int i = listeNuages.Count - 1; i >= nombreNuagesActifs; i--)
            listeNuages[i].SetActive(false);

        int indexNuage = Random.Range(0, listeNuages.Count);

        listeNuages[indexNuage].SetActive(true);
    }
}
