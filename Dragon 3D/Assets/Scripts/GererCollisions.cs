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

                    // Cr�er le d�lai de l'activation de la coroutine
                    float delai = 0f;

                    // D�marrer la coroutine qui instancie le cl�ne
                    StartCoroutine("InstancierObjet", delai);
                }
                break;

            case "Nuage":
                if (trigger.name == "Trigger Nuages")
                {

                }
                break;

            case "Terrain":
                if (trigger.name == "Trigger Terrain")
                {

                }
                break;

            case "Fruit":
                if (trigger.name == "Trigger Fruits")
                {
                    // Rendre la position du cl�ne de fa�on al�atoire
                    RandomiserPositionClone();

                    // Cr�er le d�lai de l'activation de la coroutine
                    float delai = Random.Range(1f, 1f * GenerationObjet.fruitsObtenus);
                    delai = Mathf.Clamp(delai, 1f, 10f);

                    // D�marrer la coroutine qui instancie le cl�ne
                    StartCoroutine("InstancierObjet", delai);
                }
                break;

            default:
                break;
        }

        // Si le joueur / le dragon
        if (gameObject.CompareTag("Player") || gameObject.CompareTag("Dragon"))
        {
            // interragit avec
            switch (trigger.tag)
            {
                // Une mouette,
                case "Mouette":
                    break;

                // Un fruit,
                case "Fruit":
                    GenerationObjet.IncrementerNombreFruits();

                    int fruitScore = GererScore.pointagesFruits[trigger.gameObject.name];
                    GererScore.scoreActuel += fruitScore;

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
    }

    IEnumerator InstancierObjet(float delai)
    {
        yield return new WaitForSeconds(delai);

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

                GenerationObjet.ActiverFruit();

                break;
        }

        // D�truire l'objet � cl�ner
        Destroy(objetACloner);

        yield return null;
    }
}
