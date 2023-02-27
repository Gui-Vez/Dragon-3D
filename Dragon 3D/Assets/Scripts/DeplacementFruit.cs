using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeplacementFruit : MonoBehaviour
{
    public Transform positionCible;

    public float vitesseDeplacement = 10f;
    
    void Start()
    {
        // Générer une vitesse de déplacement aléatoire
        //vitesseDeplacement = Random.Range(vitesseDeplacement / 2, vitesseDeplacement * 2);
    }

    void Update()
    {
        // Calculer la direction de la cible
        Vector3 direction = positionCible.position - transform.position;

        // Cacluler la distance de la cible
        float distance = direction.magnitude;

        // Si l'objet est suffisament près de la cible,
        if (distance < 0.1f)
            // Arrêter de bouger
            return;

        // Calculer la distance maximale de déplacement
        float distanceMax = vitesseDeplacement * Time.deltaTime;

        // Calculer le mouvement à parcourir
        Vector3 mouvement = Vector3.ClampMagnitude(direction, distanceMax);

        // Bouger vers la cible
        transform.position += mouvement;
    }
}
