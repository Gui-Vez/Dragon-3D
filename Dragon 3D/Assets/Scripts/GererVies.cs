using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GererVies : MonoBehaviour
{
    public GameObject[] iconesVies;
    public static int nombreVies;

    void Start()
    {
        // Trouver l'objet comportant les ic�nes des vies
        iconesVies = GameObject.FindGameObjectsWithTag("Vie");

        // Affecter le nombre de vies selon le tableau
        nombreVies = iconesVies.Length;
    }

    public void AjouterVie()
    {
        // Si le nombre de vies est inf�rieur � la longueur du tableau,
        if (nombreVies < iconesVies.Length)
            // Incr�menter le nombre de vies
            nombreVies++;

        // Mettre � jour les vies
        MiseAJourVies();
    }

    public void ReduireVie()
    {
        // Si le nombre de vies est supp�rieur � 0,
        if (nombreVies > 0)
            // D�cr�menter le nombre de vies
            nombreVies--;

        // Mettre � jour les vies
        MiseAJourVies();
    }

    void MiseAJourVies()
    {
        // Pour toutes les ic�nes de vie,
        for (int i = 0; i < iconesVies.Length; i++)
        {
            // Si l'index est inf�rieur au nombre de vies,
            if (i < nombreVies)
                // Activer cette vie
                iconesVies[i].SetActive(true);

            // Sinon,
            else
                // D�sactiver cette vie
                iconesVies[i].SetActive(false);
        }

        // S'il ne reste plus de vies,
        if (nombreVies <= 0)
        {
            // �mettre un message dans la console pour d�clarer que la partie est termin�e
            //Debug.Log("Partie termin�e");

            //// Peut potentiellement inclure une variable qui d�tecte si la partie est termin�e ou non ////
        }
    }
}
