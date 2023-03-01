using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GererVies : MonoBehaviour
{
    public GameObject[] iconesVies;
    private int nombreVies;

    void Start()
    {
        iconesVies = GameObject.FindGameObjectsWithTag("Vie");
        nombreVies = iconesVies.Length;
    }

    public void AjouterVie()
    {
        if (nombreVies < iconesVies.Length)
            nombreVies++;

        MiseAJourVies();
    }

    public void ReduireVie()
    {
        if (nombreVies > 0)
            nombreVies--;

        MiseAJourVies();
    }

    void MiseAJourVies()
    {
        for (int i = 0; i < iconesVies.Length; i++)
        {
            if (i < nombreVies)
                iconesVies[i].SetActive(true);

            else
                iconesVies[i].SetActive(false);
        }

        if (nombreVies <= 0)
        {
            //Debug.Log("Partie terminée");
        }
    }
}
