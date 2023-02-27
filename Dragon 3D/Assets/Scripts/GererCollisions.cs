using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GererCollisions : MonoBehaviour
{
    private GenerationObjet GenerationObjet;
    public GameObject contenantGenerationObjet;

    private GameObject objetACloner;

    public Transform positionEntree;

    public float largeurBoite;
    public float hauteurBoite;
    public float profondeurBoite;

    private Vector3 positionAleatoire;

    void Start()
    {
        if (contenantGenerationObjet != null)
            GenerationObjet = contenantGenerationObjet.GetComponent<GenerationObjet>();

        // Déclarer l'objet à clôner
        objetACloner = this.gameObject;

        switch (gameObject.tag)
        {
            case "Mouette":
            case "Nuage"  :
            case "Fruit"  :
                // Rendre la position du clône de façon aléatoire
                RandomiserPositionClone();
                break;

            default:
                break;
        }
    }

    void OnTriggerEnter(Collider trigger)
    {
        switch (objetACloner.tag)
        {
            case "Mouette":
                if (trigger.name == "Trigger Mouettes")
                {
                    // Rendre la position du clône de façon aléatoire
                    RandomiserPositionClone();

                    InstancierObjet();
                }
                break;

            case "Nuage":
                if (trigger.name == "Trigger Nuages")
                {
                    // Do something
                }
                break;

            case "Terrain":
                if (trigger.name == "Trigger Terrain")
                {
                    // Do something
                }
                break;

            case "Fruit":
                if (trigger.name == "Trigger Fruits")
                {
                    // Rendre la position du clône de façon aléatoire
                    RandomiserPositionClone();

                    InstancierObjet();
                }
                break;

            default:
                break;
        }

        if (gameObject.CompareTag("Player") || gameObject.CompareTag("Dragon"))
        {
            switch (trigger.tag)
            {
                case "Mouette":
                    break;

                case "Fruit":
                    print("fruit obtenu");
                    GenerationObjet.IncrementerNombreMouettes(1);
                    break;

                default:
                    break;
            }
        }
    }

    void RandomiserPositionClone()
    {
        // Déclarer une position aléatoire d'emplacement d'entrée
        positionAleatoire = new Vector3(Random.Range(positionEntree.position.x - largeurBoite / 2f, positionEntree.position.x + largeurBoite / 2f),
                                        Random.Range(positionEntree.position.y - hauteurBoite / 2f, positionEntree.position.y + hauteurBoite / 2f),
                                        Random.Range(positionEntree.position.z - profondeurBoite / 2f, positionEntree.position.z + profondeurBoite / 2f));

        // Positionner l'objet à l'entrée
        objetACloner.transform.position = positionAleatoire;
    }

    void InstancierObjet()
    {
        // Instancier un objet
        GameObject instanceObjet = Instantiate(objetACloner, positionAleatoire, Quaternion.identity, objetACloner.transform.parent);

        // Associer le nom du clône
        instanceObjet.name = objetACloner.name;

        // Selon le tag du clône,
        switch (instanceObjet.tag)
        {
            case "Mouette":
                // Faire regarder le clône vers l'avant
                instanceObjet.transform.LookAt(transform.forward);
                break;

            case "Fruit":
                // Faire regarder le clône selon la rotation du modèle initial
                instanceObjet.transform.rotation = objetACloner.transform.rotation;
                break;
        }

        // Détruire l'objet à clôner
        Destroy(objetACloner);
    }
}
