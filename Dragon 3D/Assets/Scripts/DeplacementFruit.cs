using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeplacementFruit : MonoBehaviour
{
    public Transform positionCible;

    public float vitesseDeplacement = 10f;
    
    void Start()
    {
        // G�n�rer une vitesse de d�placement al�atoire
        //vitesseDeplacement = Random.Range(vitesseDeplacement / 2, vitesseDeplacement * 2);
    }

    void Update()
    {
        // Calculer la direction de la cible
        Vector3 direction = positionCible.position - transform.position;

        // Cacluler la distance de la cible
        float distance = direction.magnitude;

        // Si l'objet est suffisament pr�s de la cible,
        if (distance < 0.1f)
            // Arr�ter de bouger
            return;

        // Calculer la distance maximale de d�placement
        float distanceMax = vitesseDeplacement * Time.deltaTime;

        // Calculer le mouvement � parcourir
        Vector3 mouvement = Vector3.ClampMagnitude(direction, distanceMax);

        // Bouger vers la cible
        transform.position += mouvement;
    }
}
