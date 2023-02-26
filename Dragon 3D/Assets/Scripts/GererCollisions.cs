using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GererCollisions : MonoBehaviour
{
    private GameObject objetACloner;

    public Transform positionEntree;

    public float largeurBoite = 10f;
    public float hauteurBoite = 10f;
    public float profondeurBoite = 10f;

    private Vector3 positionAleatoire;

    void Start()
    {
        // Déclarer l'objet à clôner
        objetACloner = this.gameObject;

        positionAleatoire = new Vector3(Random.Range(positionEntree.position.x - largeurBoite / 2f,    positionEntree.position.x + largeurBoite / 2f),
                                        Random.Range(positionEntree.position.y - hauteurBoite / 2f,    positionEntree.position.y + hauteurBoite / 2f),
                                        Random.Range(positionEntree.position.z - profondeurBoite / 2f, positionEntree.position.z + profondeurBoite / 2f));

        // Positionner l'objet à l'entrée
        objetACloner.transform.position = positionAleatoire;
    }

    void OnTriggerEnter(Collider trigger)
    {
        // Si l'objet à cloner est une mouette,
        if (objetACloner.tag == "Mouette")
        {
            // Si l'objet déclenche une collision,
            if (trigger.name == "Trigger Mouettes")
            {
                // Cloner l'objet
                InstancierObjet();
            }
        }

        if (objetACloner.tag == "Nuage")
        {
            if (trigger.name == "Trigger Nuages")
            {

            }
        }

        if (objetACloner.tag == "Terrain")
        {
            if (trigger.name == "Trigger Terrain")
            {

            }
        }
    }

    void InstancierObjet()
    {
        // Instancier un objet
        GameObject instanceObjet = Instantiate(objetACloner, positionAleatoire, Quaternion.identity, objetACloner.transform.parent);

        // Faire regarder le clone vers l'avant
        instanceObjet.transform.LookAt(transform.forward);

        // Associer le nom du clone
        instanceObjet.name = objetACloner.name;

        // Détruire l'objet à cloner
        Destroy(objetACloner);
    }
}
