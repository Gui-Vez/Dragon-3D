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

    public float delaiClignotement = 0.05f;
    public int nombreClignotements = 10;
    public float vitesseClignotement = 0.1f;

    private GameObject dragonJoueur;

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

        if (gameObject.name == "Collision Mouettes")
        {
            dragonJoueur = GameObject.FindGameObjectWithTag("Dragon");

            if (dragonJoueur == null)
                dragonJoueur = GameObject.FindGameObjectWithTag("Player");
        }
    }

    void Update()
    {
        // S'il y a un objet contenant le dragon,
        if (dragonJoueur != null)
            // Si l'objet contenant le script s'agit de la collision des mouettes,
            if (gameObject.name == "Collision Mouettes")
                // Modifier la position de cet �l�ment pour correspondre � celui du dragon
                transform.position = dragonJoueur.transform.position;
    }

    void OnTriggerEnter(Collider trigger)
    {
        // Selon le tag de l'objet � cl�ner,
        switch (objetACloner.tag)
        {
            // S'il s'agit d'une mouette;
            case "Mouette":
                // Si l'interraction se fait avec le mur qui fait t�l�porter la mouette,
                if (trigger.name == "Trigger Mouettes")
                    // Appeler la m�thode qui cl�ne l'objet
                    AppelerCoroutineInstancierObjet();

                // Si l'interraction se fait avec l'objet contenant la collision des mouettes,
                if (trigger.name == "Collision Mouettes")
                    // Appeler la coroutine qui permet de g�rer l'animation d'attaque de la mouette
                    objetACloner.GetComponent<AnimerMouette>().StartCoroutine("GestionAnimations", "Attaque");

                break;

            // S'il s'agit d'un fruit;
            case "Fruit":
                // Si l'interraction se fait avec le mur qui fait t�l�porter le fruit,
                if (trigger.name == "Trigger Fruits")
                    // Appeler la m�thode qui cl�ne l'objet
                    AppelerCoroutineInstancierObjet();

                // Si l'interraction se fait avec le joueur OU le dragon,
                if (trigger.CompareTag("Player") || trigger.CompareTag("Dragon"))
                    // Appeler la m�thode qui fait r�duire l'�chelle du fruit
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

                    // Si le dragon n'est pas touch�,
                    if (!estTouche)
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
        // Le dragon est touch�
        estTouche = false;

        // Le dragon ne peut plus bouger
        GetComponent<DeplacementDragon>().peutBouger = false;

        if (GererVies.nombreVies > 0)
        {
            // Jouer l'animation de d�g�ts
            gameObject.GetComponent<GererAssetsDragon>().AnimerDragon("D�g�ts");

            // Patienter la fin de l'animation
            yield return new WaitForSeconds(gameObject.GetComponent<Animation>().clip.length + delaiClignotement);

            // Jouer l'animation initiale
            gameObject.GetComponent<GererAssetsDragon>().AnimerDragon("Inactif");

            // Si le dragon ne clignotte pas,
            if (!clignote)
            {
                // Le dragon clignote
                clignote = true;

                // Le dragon peut bouger
                GetComponent<DeplacementDragon>().peutBouger = true;

                // Faire clignoter le dragon 10 fois
                for (int i = 0; i < nombreClignotements; i++)
                {
                    ///////////////////////////////////////////////
                    /* Cr�er un mat�riel Universal Render Pipeline
                     * pour chaque dragon dans la sc�ne Jeu,
                     * afin qu'il diff�re de celui de la Galerie */
                    ///////////////////////////////////////////////

                    // D�sactiver le mat�riel du dragon
                    materielCorps.color = new Color(materielCorps.color.r, materielCorps.color.g, materielCorps.color.b, 0f);
                    materielYeux.color = new Color(materielYeux.color.r, materielYeux.color.g, materielYeux.color.b, 0f);
                    yield return new WaitForSeconds(vitesseClignotement);

                    // Activer le mat�riel du dragon
                    materielCorps.color = new Color(materielCorps.color.r, materielCorps.color.g, materielCorps.color.b, 1f);
                    materielYeux.color = new Color(materielYeux.color.r, materielYeux.color.g, materielYeux.color.b, 1f);
                    yield return new WaitForSeconds(vitesseClignotement);
                }
            }
        }

        else
        {
            gameObject.GetComponent<GererAssetsDragon>().AnimerDragon("Mort");

            gameObject.GetComponent<Rigidbody>().useGravity = true;

            gameObject.GetComponent<BoxCollider>().enabled = false;

            Destroy(gameObject, 5f);

            Debug.Log("Partie termin�e");
        }

        // Le dragon n'est plus en train de clignoter
        clignote = false;

        // Le dragon n'est plus touch�
        estTouche = false;
    }
}
