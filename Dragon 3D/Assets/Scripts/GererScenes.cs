using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GererScenes : MonoBehaviour
{
    public static Scene sceneActuelle;

    GameObject boutonsAnimationsContenant;

    public float tempsChargement = 1.0f;

    string[] optionsLangages = { "EN", "FR" };
    int indexLangueActuelle = 0;

    public Material[] skyboxes;
    private int indexSkyboxActuel = 0;

    public static bool partieTerminee;
    private bool chargeScene;

    private GererAudio GererAudio;
    public GameObject contenantGererAudio;

    void Start()
    {
        // La partie n'est pas terminée
        partieTerminee = false;

        // Trouver la scène actuelle
        sceneActuelle = SceneManager.GetActiveScene();

        // S'il y a un contenant de gestion d'audio,
        if (contenantGererAudio != null)
            // Obtenir la composante de script de cet objet
            GererAudio = contenantGererAudio.GetComponent<GererAudio>();

        // Donner des instructions aux scènes respectives
        switch (sceneActuelle.name)
        {
            // D'après la scène "Jeu",
            case "Jeu":

                // Réinitialiser la valeur des variables statiques
                GererScore.scoreActuel = 0;
                GenerationObjet.fruitsObtenus = 0;
                GenerationObjet.listeMouettes = new List<GameObject>();
                GenerationObjet.listeFruits = new List<GameObject>();
                GenerationObjet.listeNuages = new List<GameObject>();

                break;

            // D'après la scène "Galerie",
            case "Galerie":

                // Réinitialiser la valeur des variables statiques
                GererAssetsDragon.personnagesJoueur = GameObject.FindGameObjectsWithTag("Player");
                GererAssetsDragon.personnageJoueurActuel = GererAssetsDragon.personnagesJoueur[0];
                GererBoutonsUI.ecransBoutonsListe = new List<GameObject>();

                // Trouver les boutons d'animation
                boutonsAnimationsContenant = GameObject.Find("Animations");

                // Trouver la liste de skybox pour le décor de la scène
                RenderSettings.skybox = skyboxes[indexSkyboxActuel];

                break;
        }
    }

    void Update()
    {
        // Si l'on appuie sur la touche "Escape" et que nous ne sommes pas dans l'écran titre,
        if (Input.GetKeyDown(KeyCode.Escape) && sceneActuelle.name != "EcranTitre")
        {
            // Appeler la coroutine qui ramène le joueur à l'écran titre
            StartCoroutine(ChargerScene("EcranTitre", 0f));
        }

        // Donner des instructions aux scènes respectives
        switch (sceneActuelle.name)
        {
            // D'après la scène "Écran titre",
            case "EcranTitre":

                // Si l'on appuie sur la touche "Escape",
                if (Input.GetKeyDown(KeyCode.Escape))
                    // Faire quitter le jeu
                    QuitterJeu();

                break;

            // D'après la scène "Jeu",
            case "Jeu":

                // Si la partie est terminée,
                if (partieTerminee)
                {
                    // Si la scène ne charge pas,
                    if (!chargeScene)
                    {
                        // Charger la scène
                        chargeScene = true;

                        // Commencer la coroutine qui ramène le joueur à l'écran titre
                        StartCoroutine(ChargerScene("EcranTitre", 15f));
                    }
                }

                break;

            // D'après la scène "Galerie",
            case "Galerie":

                // Si l'on appuie sur la touche d'espace ou la molette de la souris,
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse2))
                {
                    // Alterner la langue actuelle
                    indexLangueActuelle = (indexLangueActuelle + 1) % optionsLangages.Length;
                    AssignerTextes(optionsLangages[indexLangueActuelle]);
                }

                // Si l'on appuie sur la touche de retour ou le clic droit de la souris,
                if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Mouse1))
                {
                    // Changer la skybox en cours
                    indexSkyboxActuel++;

                    if (indexSkyboxActuel >= skyboxes.Length)
                        indexSkyboxActuel = 0;

                    RenderSettings.skybox = skyboxes[indexSkyboxActuel];
                    RenderSettings.skybox.SetFloat("_Rotation", transform.eulerAngles.y);
                }

                break;
        }
    }

    void AssignerTextes(string langage)
    {
        /*
         * Note pour moi-même :
         * Pour gérer les textes, il serait préférable d'utiliser des enums ou des dictionnaires plutôt que des listes.
         * Ce serait à implémenter plus tard lorsque j'en aurais le temps pour m'y focaliser davantage.
         */

        // Créer une liste des boutons d'animation
        List<GameObject> boutonAnimationsListe = new List<GameObject>();

        // Pour tous les enfants des boutons d'animation,
        for (int i = 0; i < boutonsAnimationsContenant.transform.childCount; i++)
            // Ajouter à la liste des boutons d'animations cet objet
            boutonAnimationsListe.Add(boutonsAnimationsContenant.transform.GetChild(i).gameObject);

        // Liste des mots français
        List<string> textesBoutonsAnimFR = new List<string>();
        textesBoutonsAnimFR.AddRange(new[]
        { "Inactif", "Rester", "Marcher", "Courir", "Attaquer", "Position Attaque", "Dégâts", "Souffler", "Mort", "Aléatoire" });

        // Liste des mots anglais
        List<string> textesBoutonsAnimEN = new List<string>();
        textesBoutonsAnimEN.AddRange(new[]
        { "Idle", "Stand", "Walk", "Run", "Attack", "Attack Stand", "Damage", "Breath", "Die", "Random" });

        // Initialiser la variable qui incrémente le nombre de mots de la liste
        int j = 0;

        // Pour chaque texte de la liste des boutons d'animation,
        foreach (GameObject texteAnimation in boutonAnimationsListe)
        {
            // Trouver le composant de Text Mesh Pro de l'objet enfant
            TMP_Text tmp = texteAnimation.transform.GetChild(0).GetComponent<TMP_Text>();

            // Par rapport à la langue,
            switch (langage)
            {
                // Si la langue est française,
                case "FR":

                    // Affecter le texte du bouton selon la langue française
                    tmp.text = textesBoutonsAnimFR[j];

                    // Incrémenter la variable d'index J
                    j++;

                    // Si l'index J est supérieur ou égal au nombre de textes de la langue française,
                    if (j >= textesBoutonsAnimFR.Count)
                        // Établir l'index J à 0
                        j = 0;

                    break;

                // Si la langue est anglaise,
                case "EN":
                default:

                    // Affecter le texte du bouton selon la langue anglaise
                    tmp.text = textesBoutonsAnimEN[j];

                    // Incrémenter la variable d'index J
                    j++;

                    // Si l'index J est supérieur ou égal au nombre de textes de la langue anglaise,
                    if (j >= textesBoutonsAnimEN.Count)
                        // Établir l'index J à 0
                        j = 0;

                    break;
            }
        }
    }

    public void GererBoutons(GameObject bouton)
    {
        // Selon le nom du bouton activé,
        switch (bouton.name)
        {
            // S'il s'agit du bouton pour jouer,
            case "Play":
            case "Jouer":

                // Jouer l'effet sonore "Play"
                GererAudio.JouerEffetSonore("Play");

                // Appeler la coroutine qui lance le jeu
                StartCoroutine(ChargerScene("Jeu", tempsChargement));

                break;

            // S'il s'agit du bouton pour la galerie,
            case "Gallery":
            case "Galerie":

                // Jouer l'effet sonore "Gallery"
                GererAudio.JouerEffetSonore("Gallery");

                // Appeler la coroutine qui lance la galerie
                StartCoroutine(ChargerScene("Galerie", tempsChargement));

                break;
        }
    }

    public IEnumerator ChargerScene(string nomScene, float delaiSecondes)
    {
        // Attendre un délai de quelques secondes
        yield return new WaitForSeconds(delaiSecondes);

        // Charger la scène
        SceneManager.LoadScene(nomScene, LoadSceneMode.Single);

        // Retoruner la coroutine
        yield return null;
    }

    public void QuitterJeu()
    {
        // Si cela joue dans l'éditeur d'Unity,
        if (Application.isEditor)
        {
            // Arrêter la scène de jouer
            //UnityEditor.EditorApplication.isPlaying = false;
        }

        // Sinon,
        else
        {
            // Quitter l'application directement
            Application.Quit();
        }
    }
}
