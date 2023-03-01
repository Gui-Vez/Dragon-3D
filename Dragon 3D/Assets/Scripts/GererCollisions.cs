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
        // S'il y a un contenant de génération d'objets,
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

        // Déclarer l'objet à clôner
        objetACloner = this.gameObject;

        // Appliquer la valeur initiale de l'échelle de l'objet
        echelleCloneInitialle = transform.localScale;

        // Selon le tag de l'objet,
        switch (gameObject.tag)
        {
            // S'il s'agit de ces tags;
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
        // Selon le tag de l'objet à clôner,
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

                    // Appeler la fonction qui réduit le nombre de vies
                    GererVies.ReduireVie();

                    // Exécuter la coroutine qui fait donner des dégâts au dragon
                    StartCoroutine(DegatsDragon());

                    break;

                // Un fruit;
                case "Fruit":
                    
                    // Appeler la fonction qui incrémente le nombre de fruits
                    GenerationObjet.IncrementerNombreFruits();

                    // Incrémenter le score actuel par rapport au fruit obtenu
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
        // Rendre la position du clône de façon aléatoire
        RandomiserPositionClone();

        // Initialiser la valeur de délai par défaut
        float delai = 0;

        // Si l'objet à clôner est un fruit,
        if (objetACloner.CompareTag("Fruit"))
        {
            // Créer le délai de l'activation de la coroutine
            delai = Random.Range(1f, 1f * GenerationObjet.fruitsObtenus);
            delai = Mathf.Clamp(delai, 1f, 10f);
        }

        // Démarrer la coroutine qui instancie le clône
        StartCoroutine("InstancierObjet", delai);
    }

    void RandomiserPositionClone()
    {
        // Déclarer une position aléatoire d'emplacement d'entrée
        positionAleatoire = new Vector3(Random.Range(positionEntree.position.x - largeurBoite / 2f, positionEntree.position.x + largeurBoite / 2f),
                                        Random.Range(positionEntree.position.y - hauteurBoite / 2f, positionEntree.position.y + hauteurBoite / 2f),
                                        Random.Range(positionEntree.position.z - profondeurBoite / 2f, positionEntree.position.z + profondeurBoite / 2f));
    }

    IEnumerator InstancierObjet(float delai)
    {
        // Faire patienter pendant un délai
        yield return new WaitForSeconds(delai);

        // Instancier un objet
        GameObject instanceObjet = Instantiate(objetACloner, positionAleatoire, Quaternion.identity, objetACloner.transform.parent);

        // Transformer l'échelle locale du clône par sa valeur initialle
        instanceObjet.transform.localScale = echelleCloneInitialle;

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

                // Activer la collision du clône
                instanceObjet.GetComponent<SphereCollider>().enabled = true;

                // Appeler la fonction qui active le fruit
                GenerationObjet.ActiverFruit();

                break;
        }

        // Détruire l'objet à clôner
        Destroy(objetACloner);

        yield return null;
    }

    IEnumerator DegatsDragon()
    {
        // Si le dragon n'est pas touché,
        if (!estTouche)
        {
            // Le dragon est touché
            estTouche = true;

            // Le dragon ne peut plus bouger
            peutBouger = false;

            // Jouer l'animation de dégâts
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
                // Désactiver le matériel du dragon
                materielCorps.color = new Color(materielCorps.color.r, materielCorps.color.g, materielCorps.color.b, 0f);
                materielYeux.color = new Color(materielYeux.color.r, materielYeux.color.g, materielYeux.color.b, 0f);
                yield return new WaitForSeconds(0.1f);

                // Activer le matériel du dragon
                materielCorps.color = new Color(materielCorps.color.r, materielCorps.color.g, materielCorps.color.b, 1f);
                materielYeux.color = new Color(materielYeux.color.r, materielYeux.color.g, materielYeux.color.b, 1f);
                yield return new WaitForSeconds(0.1f);
            }

            // Le dragon n'est plus en train de clignoter
            clignote = false;

            // Le dragon n'est plus touché
            estTouche = false;
        }
    }
}
