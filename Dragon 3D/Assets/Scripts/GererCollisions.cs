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

        // D�clarer l'objet � cl�ner
        objetACloner = this.gameObject;

        switch (gameObject.tag)
        {
            case "Mouette":
            case "Nuage"  :
            case "Fruit"  :
                // Rendre la position du cl�ne de fa�on al�atoire
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
                    // Rendre la position du cl�ne de fa�on al�atoire
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
                    // Rendre la position du cl�ne de fa�on al�atoire
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
        // D�clarer une position al�atoire d'emplacement d'entr�e
        positionAleatoire = new Vector3(Random.Range(positionEntree.position.x - largeurBoite / 2f, positionEntree.position.x + largeurBoite / 2f),
                                        Random.Range(positionEntree.position.y - hauteurBoite / 2f, positionEntree.position.y + hauteurBoite / 2f),
                                        Random.Range(positionEntree.position.z - profondeurBoite / 2f, positionEntree.position.z + profondeurBoite / 2f));

        // Positionner l'objet � l'entr�e
        objetACloner.transform.position = positionAleatoire;
    }

    void InstancierObjet()
    {
        // Instancier un objet
        GameObject instanceObjet = Instantiate(objetACloner, positionAleatoire, Quaternion.identity, objetACloner.transform.parent);

        // Associer le nom du cl�ne
        instanceObjet.name = objetACloner.name;

        // Selon le tag du cl�ne,
        switch (instanceObjet.tag)
        {
            case "Mouette":
                // Faire regarder le cl�ne vers l'avant
                instanceObjet.transform.LookAt(transform.forward);
                break;

            case "Fruit":
                // Faire regarder le cl�ne selon la rotation du mod�le initial
                instanceObjet.transform.rotation = objetACloner.transform.rotation;
                break;
        }

        // D�truire l'objet � cl�ner
        Destroy(objetACloner);
    }
}
