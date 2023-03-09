using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GererVies : MonoBehaviour
{
    public GameObject[] iconesVies;
    public static int nombreVies;

    void Start()
    {
        // Trouver l'objet comportant les icônes des vies
        iconesVies = GameObject.FindGameObjectsWithTag("Vie");

        // Affecter le nombre de vies selon le tableau
        nombreVies = iconesVies.Length;
    }

    public void AjouterVie()
    {
        // Si le nombre de vies est inférieur à la longueur du tableau,
        if (nombreVies < iconesVies.Length)
            // Incrémenter le nombre de vies
            nombreVies++;

        // Mettre à jour les vies
        MiseAJourVies();
    }

    public void ReduireVie()
    {
        // Si le nombre de vies est suppérieur à 0,
        if (nombreVies > 0)
            // Décrémenter le nombre de vies
            nombreVies--;

        // Mettre à jour les vies
        MiseAJourVies();
    }

    void MiseAJourVies()
    {
        // Pour toutes les icônes de vie,
        for (int i = 0; i < iconesVies.Length; i++)
        {
            // Si l'index est inférieur au nombre de vies,
            if (i < nombreVies)
                // Activer cette vie
                iconesVies[i].SetActive(true);

            // Sinon,
            else
                // Désactiver cette vie
                iconesVies[i].SetActive(false);
        }

        // S'il ne reste plus de vies,
        if (nombreVies <= 0)
        {
            // Émettre un message dans la console pour déclarer que la partie est terminée
            //Debug.Log("Partie terminée");

            //// Peut potentiellement inclure une variable qui détecte si la partie est terminée ou non ////
        }
    }
}
