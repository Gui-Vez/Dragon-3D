using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeplacementDecor : MonoBehaviour
{
    public float vitesseDeplacement = 2f;

    void Start()
    {
        // Si l'objet est un nuage,
        if (gameObject.CompareTag("Nuage"))
            // Donner une valeur aléatoire entre la moitié et le double de la valeur de déplacement actuelle
            vitesseDeplacement = Random.Range(vitesseDeplacement / 2, vitesseDeplacement * 2);
    }

    void Update()
    {
        // Selon le nom de l'objet,
        switch (gameObject.tag)
        {
            // S'il s'agit d'un nuage ou d'un terrain,
            case "Nuage"   :
            case "Terrain" :
                // Modifier la position de cet objet selon la vitesse de déplacement
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - vitesseDeplacement);
                break;
        }
    }
}
