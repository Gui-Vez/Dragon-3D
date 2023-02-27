using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationObjet : MonoBehaviour
{
    private List<GameObject> listeMouettes = new List<GameObject>();
    private GameObject[] mouettes;
    public int nombreMouettesActives = 1;
    private int nombreMouettesPrecedent;

    private List<GameObject> listeFruits = new List<GameObject>();
    private GameObject[] fruits;
    public int nombreFruitsActifs = 1;
    private int nombreFruitsPrecedent;
    


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

                ActiverMouettes();

                break;

            case "Fruits":

                fruits = GameObject.FindGameObjectsWithTag("Fruit");

                foreach (GameObject fruit in fruits)
                {
                    listeMouettes.Add(fruit.transform.parent.gameObject);

                    fruit.transform.parent.gameObject.SetActive(false);
                }

                ActiverFruits();

                break;
        }
    }

    void Update()
    {
        switch (gameObject.name)
        {
            case "Mouettes":

                if (nombreMouettesPrecedent != nombreMouettesActives)
                {
                    IncrementerNombreMouettes(nombreMouettesActives - nombreMouettesPrecedent);

                    nombreMouettesPrecedent = nombreMouettesActives;
                }

                break;

            case "Fruits":

                if (nombreFruitsPrecedent != nombreFruitsActifs)
                {
                    IncrementerNombreMouettes(nombreFruitsActifs - nombreFruitsPrecedent);

                    nombreFruitsPrecedent = nombreFruitsActifs;
                }

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

        ActiverMouettes();
    }

    public void IncrementerNombreFruits(int nombreIncrementationFruits)
    {
        nombreFruitsActifs += nombreIncrementationFruits;

        if (nombreFruitsActifs > listeFruits.Count)
            nombreFruitsActifs = listeFruits.Count;

        if (nombreFruitsActifs < 0)
            nombreFruitsActifs = 0;

        ActiverFruits();
    }

    void ActiverMouettes()
    {
        for (int i = 0; i < nombreMouettesActives; i++)
            listeMouettes[i].SetActive(true);

        for (int i = listeMouettes.Count - 1; i >= nombreMouettesActives; i--)
            listeMouettes[i].SetActive(false);
    }

    void ActiverFruits()
    {
        for (int i = 0; i < nombreFruitsActifs; i++)
            listeFruits[i].SetActive(true);

        for (int i = listeFruits.Count - 1; i >= nombreFruitsActifs; i--)
            listeFruits[i].SetActive(false);
    }
}
