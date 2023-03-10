using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GererCollisions : MonoBehaviour
{
    private GenerationObjet GenerationObjet;
    public GameObject contenantGenerationObjet;

    private GererVies GererVies;
    public GameObject contenantGererVies;

    private GererAudio GererAudio;
    public GameObject contenantGererAudio;


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
    public float delaiRemiseANiveau = 0.5f;

    private GameObject dragonJoueur;

    public static GameObject[] terrains;
    public static GameObject dernierTerrain;
    public static int indexDernierTerrain;

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

        // S'il y a un contenant de gestion d'audio,
        if (contenantGererAudio != null)
            // Obtenir la composante de script de cet objet
            GererAudio = contenantGererAudio.GetComponent<GererAudio>();

        if (objetCorps != null)
            materielCorps = objetCorps.GetComponent<SkinnedMeshRenderer>().material;

        if (objetYeux != null)
            materielYeux = objetYeux.GetComponent<SkinnedMeshRenderer>().material;

        // Déclarer l'objet ŕ clôner
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

        // Si le nom de l'objet s'agit de la collision des mouettes,
        if (gameObject.name == "Collision Mouettes")
        {
            // Trouver l'objet dragon
            dragonJoueur = GameObject.FindGameObjectWithTag("Dragon");

            // S'il n'y a pas d'objet dragon,
            if (dragonJoueur == null)
                // Trouver l'objet joueur
                dragonJoueur = GameObject.FindGameObjectWithTag("Player");
        }

        // Trouver les terrains
        terrains = GameObject.FindGameObjectsWithTag("Terrain");
    }

    void Update()
    {
        // S'il y a un objet contenant le dragon,
        if (dragonJoueur != null)
            // Si l'objet contenant le script s'agit de la collision des mouettes,
            if (gameObject.name == "Collision Mouettes")
                // Modifier la position de cet élément pour correspondre ŕ celui du dragon
                transform.position = dragonJoueur.transform.position;
    }

    void OnTriggerEnter(Collider trigger)
    {
        // Selon le tag de l'objet ŕ clôner,
        switch (objetACloner.tag)
        {
            // S'il s'agit d'une mouette;
            case "Mouette":
                // Si l'interraction se fait avec le mur qui fait téléporter la mouette,
                if (trigger.name == "Trigger Mouettes")
                    // Appeler la méthode qui clône l'objet
                    AppelerCoroutineInstancierObjet();

                // Si l'interraction se fait avec l'objet contenant la collision des mouettes,
                if (trigger.name == "Collision Mouettes")
                    // Appeler la coroutine qui permet de gérer l'animation d'attaque de la mouette
                    objetACloner.GetComponent<AnimerMouette>().StartCoroutine("GestionAnimations", "Attaque");

                break;

            // S'il s'agit d'un fruit;
            case "Fruit":
                // Si l'interraction se fait avec le mur qui fait téléporter le fruit,
                if (trigger.name == "Trigger Fruits")
                    // Appeler la méthode qui clône l'objet
                    AppelerCoroutineInstancierObjet();

                // Si l'interaction se fait avec le joueur OU le dragon,
                if (trigger.CompareTag("Player") || trigger.CompareTag("Dragon"))
                {
                    // Appeler la méthode qui fait réduire l'échelle du fruit
                    objetACloner.GetComponent<DeplacementFruit>().reduireEchelle = true;

                    // Gérer les particules du fruit obtenu
                    objetACloner.GetComponent<DeplacementFruit>().GererParticulesFruitObtenu();
                }
                    
                break;

            // S'il s'agit d'un nuage;
            case "Nuage":

                // Si l'interaction se fait avec le mur qui fait téléporter le nuage,
                if (trigger.name == "Trigger Nuages")
                {
                    // Créer un nuage
                    GenerationObjet.CreerNuage();

                    // Détruire l'objet ŕ cloner
                    Destroy(objetACloner.transform.parent.gameObject);
                }

                break;

            // S'il s'agit d'un terrain;
            case "Terrain":

                // Si l'interaction se fait avec le mur qui fait téléporter le terrain,
                if (trigger.name == "Trigger Terrain")
                {
                    // S'il n'y a pas de dernier terrain,
                    if (dernierTerrain == null)
                    {
                        // Affecter le dernier terrain ŕ celui qui correspond au dernier du tableau
                        dernierTerrain = terrains[terrains.Length - 1];

                        // Affecter l'index du dernier terrain également
                        indexDernierTerrain = terrains.Length - 1;
                    }

                    // Sinon,
                    else
                    {
                        // Modifier l'index du dernier terrain en fonction de la taille du tableau
                        indexDernierTerrain = (indexDernierTerrain + 1) % terrains.Length;

                        // Obtenir l'objet du dernier terrain (?) --> Peut potentiellement causer des erreurs
                        dernierTerrain = dernierTerrain.transform.parent.GetChild(indexDernierTerrain).gameObject;
                    }

                    // Get the position and size of the last object
                    Vector3 lastObjectPosition = dernierTerrain.transform.position;

                    // Set the position of the new object next to the last object
                    Vector3 newPosition = new Vector3(lastObjectPosition.x, lastObjectPosition.y, lastObjectPosition.z + dernierTerrain.GetComponent<BoxCollider>().size.z);
                    objetACloner.transform.position = newPosition;
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

                    // Si le dragon n'est pas touché,
                    if (!estTouche)
                    {
                        // Le dragon est touché
                        estTouche = true;

                        // Appeler la fonction qui réduit le nombre de vies
                        GererVies.ReduireVie();

                        StartCoroutine(AffligerDegatsDragon());
                    }

                    break;

                // Un fruit;
                case "Fruit":

                    GererAudio.JouerEffetSonore("Slurp");

                    Invoke("JouerSonFruitObtenu", 0.25f);

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

        // Si l'objet s'agit de la boîte qui interragit avec la mouette,
        if (gameObject.name == "Collision Mouettes")
        {
            // Si l'objet d'interaction est une mouette,
            if (trigger.CompareTag("Mouette"))
            {
                // Enlever la boite de collision de l'objet
                gameObject.GetComponent<BoxCollider>().enabled = false;

                // Réactiver la boite de collision aprčs un certain délai
                Invoke("ReactiverBoiteCollisionMouettes", 0.5f);
            }
        }
    }

    public void AppelerCoroutineInstancierObjet()
    {
        // Rendre la position du clône de façon aléatoire
        RandomiserPositionClone();

        // Initialiser la valeur de délai par défaut
        float delai = 0;

        // Si l'objet ŕ clôner est un fruit,
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

        // Créer un objet d'instance servant de clône
        GameObject instanceObjet;

        // Instancier un objet
        instanceObjet = Instantiate(objetACloner, positionAleatoire, Quaternion.identity, objetACloner.transform.parent);

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
                // Faire regarder le clône selon la rotation du modčle initial
                instanceObjet.transform.rotation = objetACloner.transform.rotation;

                // Activer la collision du clône
                instanceObjet.GetComponent<SphereCollider>().enabled = true;

                // Appeler la fonction qui active le fruit
                GenerationObjet.ActiverFruit();

                break;
        }

        // Détruire l'objet ŕ clôner
        Destroy(objetACloner);

        yield return null;
    }

    IEnumerator AffligerDegatsDragon()
    {
        // Le dragon ne peut plus bouger
        GetComponent<DeplacementDragon>().peutBouger = false;

        // La collision du dragon est enlevée
        GetComponent<BoxCollider>().enabled = false;

        // S'il reste encore des vies,
        if (GererVies.nombreVies > 0)
        {
            GererAudio.JouerEffetSonore("Slap");

            // Jouer l'animation de dégâts
            gameObject.GetComponent<GererAssetsDragon>().AnimerDragon("Dégâts");

            // Patienter la fin de l'animation
            yield return new WaitForSeconds(gameObject.GetComponent<Animation>().clip.length + delaiClignotement);

            // Jouer l'animation initiale
            gameObject.GetComponent<GererAssetsDragon>().AnimerDragon("Inactif");

            // Activer la boîte de collision du dragon
            GetComponent<BoxCollider>().enabled = true;

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
                    /* Créer un matériel Universal Render Pipeline
                     * pour chaque dragon dans la scčne Jeu,
                     * afin qu'il diffčre de celui de la Galerie */
                    ///////////////////////////////////////////////

                    // Désactiver le matériel du dragon
                    materielCorps.color = new Color(materielCorps.color.r, materielCorps.color.g, materielCorps.color.b, 0f);
                    materielYeux.color = new Color(materielYeux.color.r, materielYeux.color.g, materielYeux.color.b, 0f);
                    yield return new WaitForSeconds(vitesseClignotement);

                    // Activer le matériel du dragon
                    materielCorps.color = new Color(materielCorps.color.r, materielCorps.color.g, materielCorps.color.b, 1f);
                    materielYeux.color = new Color(materielYeux.color.r, materielYeux.color.g, materielYeux.color.b, 1f);
                    yield return new WaitForSeconds(vitesseClignotement);
                }

                // Le dragon n'est plus en train de clignoter
                clignote = false;

                // Attendre pendant un certain délai supplémentaire
                yield return new WaitForSeconds(delaiRemiseANiveau);

                // Le dragon n'est plus touché
                estTouche = false;
            }
        }

        // Sinon,
        else
        {
            GererAudio.JouerEffetSonore("Explosion");

            GererAudio.StartCoroutine("GameOverRoutine");

            // Exécuter l'animation de mort
            gameObject.GetComponent<GererAssetsDragon>().AnimerDragon("Mort");

            // Activer la gravité du dragon
            gameObject.GetComponent<Rigidbody>().useGravity = true;

            // Le dragon n'est plus touché
            estTouche = false;

            // Détruire l'objet aprčs 5 secondes
            Destroy(gameObject, 5f);

            // La partie est terminée
            GererScenes.partieTerminee = true;

            // Émettre un commentaire dans la console
            //Debug.Log("Partie terminée");
        }
    }

    void JouerSonFruitObtenu()
    {
        // S'il y a un script associé ŕ la gestion de l'audio,
        if (GererAudio != null)
        {
            // Jouer l'effet sonore d'étincellement
            GererAudio.JouerEffetSonore("Sparkle 01");
        }
    }

    void ReactiverBoiteCollisionMouettes()
    {
        // Réactiver la boite de collision de l'objet
        gameObject.GetComponent<BoxCollider>().enabled = true;
    }
}
