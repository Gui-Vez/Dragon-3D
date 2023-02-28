using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeplacementFruit : MonoBehaviour
{
    public Transform positionCible;

    public float vitesseDeplacement = 10f;
    public float vitesseRotation = 100f;
    //public float amplitudeSinus = 0.1f;
    //public float frequenceSinus = 3f;
    //private float tempsDepartSinus;
    //private float initialY;

    void Start()
    {
        // G�n�rer une vitesse de d�placement al�atoire
        //vitesseDeplacement = Random.Range(vitesseDeplacement / 2, vitesseDeplacement * 2);

        // D�marrer le temps � accorder � la translation sinuso�dale
        //tempsDepartSinus = Time.time;

        // Sauvegarder la position en y initiale
        //initialY = transform.localPosition.y;
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

        // Calculer l'angle de rotation du fruit
        float angleRotation = vitesseRotation * Time.deltaTime;
        transform.Rotate(Vector3.forward, angleRotation);

        // Calculer la position en y en utilisant une fonction sinusoidale
        //float y = initialY + Mathf.Sin((Time.time - tempsDepartSinus) * frequenceSinus) * amplitudeSinus;
        //transform.localPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
    }
}
