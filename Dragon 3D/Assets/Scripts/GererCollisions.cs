using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GererCollisions : MonoBehaviour
{
    private GenerationObjet GenerationObjet;
    public GameObject contenantGenerationObjet;

    private GererVies GererVies;
    public GameObject contenantGererVies;

    private GameObject objetACloner;

    public Transform positionEntree;

    public float largeurBoite;
    public float hauteurBoite;
    public float profondeurBoite;

    private Vector3 positionAleatoire;

    private Vector3 echelleCloneInitialle;

    public GameObject objetCorps;
    private Material materielCorps;

    public GameObject objetYeux;
    private Material materielYeux;

    private bool estTouche  = false;
    private bool clignote   = false;
    private bool peutBouger = true;

    void Start()
    {
        // S'il y a un contenant de g�n�ration d'objets,
        if (contenantGenerationObjet != null)
            // Obtenir la composante de script de cet objet
            GenerationObjet = contenantGenerationObjet.GetComponent<GenerationObjet>();

        // S'il y a un contenant de gestion de vies,
        if (contenantGererVies != null)
            // Obtenir la composante de script de cet objet
            GererVies = contenantGererVies.GetComponent<GererVies>();

        if (objetCorps != null)
            materielCorps = objetCorps.GetComponent<SkinnedMeshRenderer>().material;

        if (objetYeux != null)
            materielYeux = objetYeux.GetComponent<SkinnedMeshRenderer>().material;

        // D�clarer l'objet � cl�ner
        objetACloner = this.gameObject;

        // Appliquer la valeur initiale de l'�chelle de l'objet
        echelleCloneInitialle = transform.localScale;

        // Selon le tag de l'objet,
        switch (gameObject.tag)
        {
            // S'il s'agit de ces tags;
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
        // Selon le tag de l'objet � cl�ner,
        switch (objetACloner.tag)
        {
            // S'il s'agit d'une mouette;
            case "Mouette":
                if (trigger.name == "Trigger Mouettes")
                    AppelerCoroutineInstancierObjet();
                break;

            // S'il s'agit d'un fruit;
            case "Fruit":
                if (trigger.name == "Trigger Fruits")
                    AppelerCoroutineInstancierObjet();

                if (trigger.CompareTag("Player") || trigger.CompareTag("Dragon"))
                    objetACloner.GetComponent<DeplacementFruit>().reduireEchelle = true;
                break;

            // S'il s'agit d'un nuage;
            case "Nuage":
                if (trigger.name == "Trigger Nuages")
                {

                }
                break;

            // S'il s'agit d'un terrain;
            case "Terrain":
                if (trigger.name == "Trigger Terrain")
                {

                }
                break;

            default:
                break;
        }

        // Si le joueur / le dragon...
        if (gameObject.CompareTag("Player") || gameObject.CompareTag("Dragon"))
        {
            // interragit avec...
            switch (trigger.tag)
            {
                // Une mouette;
                case "Mouette":

                    // Appeler la fonction qui r�duit le nombre de vies
                    GererVies.ReduireVie();

                    // Ex�cuter la coroutine qui fait donner des d�g�ts au dragon
                    StartCoroutine(DegatsDragon());

                    break;

                // Un fruit;
                case "Fruit":
                    
                    // Appeler la fonction qui incr�mente le nombre de fruits
                    GenerationObjet.IncrementerNombreFruits();

                    // Incr�menter le score actuel par rapport au fruit obtenu
                    int fruitScore = GererScore.pointagesFruits[trigger.gameObject.name];
                    GererScore.scoreActuel += fruitScore;

                    break;

                default:
                    break;
            }
        }
    }

    public void AppelerCoroutineInstancierObjet()
    {
        // Rendre la position du cl�ne de fa�on al�atoire
        RandomiserPositionClone();

        // Initialiser la valeur de d�lai par d�faut
        float delai = 0;

        // Si l'objet � cl�ner est un fruit,
        if (objetACloner.CompareTag("Fruit"))
        {
            // Cr�er le d�lai de l'activation de la coroutine
            delai = Random.Range(1f, 1f * GenerationObjet.fruitsObtenus);
            delai = Mathf.Clamp(delai, 1f, 10f);
        }

        // D�marrer la coroutine qui instancie le cl�ne
        StartCoroutine("InstancierObjet", delai);
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
        // Faire patienter pendant un d�lai
        yield return new WaitForSeconds(delai);

        // Instancier un objet
        GameObject instanceObjet = Instantiate(objetACloner, positionAleatoire, Quaternion.identity, objetACloner.transform.parent);

        // Transformer l'�chelle locale du cl�ne par sa valeur initialle
        instanceObjet.transform.localScale = echelleCloneInitialle;

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

                // Activer la collision du cl�ne
                instanceObjet.GetComponent<SphereCollider>().enabled = true;

                // Appeler la fonction qui active le fruit
                GenerationObjet.ActiverFruit();

                break;
        }

        // D�truire l'objet � cl�ner
        Destroy(objetACloner);

        yield return null;
    }

    IEnumerator DegatsDragon()
    {
        // Si le dragon n'est pas touch�,
        if (!estTouche)
        {
            // Le dragon est touch�
            estTouche = true;

            // Le dragon ne peut plus bouger
            peutBouger = false;

            // Jouer l'animation de d�g�ts
            //animation.Play();

            // Patienter la fin de l'animation
            //yield return new WaitForSeconds(animation.clip.length);
        }

        // Si le dragon ne clignotte pas,
        if (!clignote)
        {
            // Le dragon clignote
            clignote = true;

            // Le dragon peut bouger
            peutBouger = true;

            // Faire clignoter le dragon 5 fois
            for (int i = 0; i < 5; i++)
            {
                // D�sactiver le mat�riel du dragon
                materielCorps.color = new Color(materielCorps.color.r, materielCorps.color.g, materielCorps.color.b, 0f);
                materielYeux.color = new Color(materielYeux.color.r, materielYeux.color.g, materielYeux.color.b, 0f);
                yield return new WaitForSeconds(0.1f);

                // Activer le mat�riel du dragon
                materielCorps.color = new Color(materielCorps.color.r, materielCorps.color.g, materielCorps.color.b, 1f);
                materielYeux.color = new Color(materielYeux.color.r, materielYeux.color.g, materielYeux.color.b, 1f);
                yield return new WaitForSeconds(0.1f);
            }

            // Le dragon n'est plus en train de clignoter
            clignote = false;

            // Le dragon n'est plus touch�
            estTouche = false;
        }
    }
}
