using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GererScenes : MonoBehaviour
{
    /* ****************** */
    /* Gestion des scènes */
    /* ****************** */

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
            case "Jeu":

                GererScore.scoreActuel = 0;
                GenerationObjet.fruitsObtenus = 0;
                GenerationObjet.listeMouettes = new List<GameObject>();
                GenerationObjet.listeFruits = new List<GameObject>();
                GenerationObjet.listeNuages = new List<GameObject>();

                break;

            case "Galerie":

                GererAssetsDragon.personnagesJoueur = GameObject.FindGameObjectsWithTag("Player");
                GererAssetsDragon.personnageJoueurActuel = GererAssetsDragon.personnagesJoueur[0];
                GererBoutonsUI.ecransBoutonsListe = new List<GameObject>();

                boutonsAnimationsContenant = GameObject.Find("Animations");

                RenderSettings.skybox = skyboxes[indexSkyboxActuel];

                break;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && sceneActuelle.name != "EcranTitre")
        {
            StartCoroutine(ChargerScene("EcranTitre", 0f));
        }

        // Donner des instructions aux scènes respectives
        switch (sceneActuelle.name)
        {
            case "EcranTitre":

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    QuitterJeu();
                }

                break;

            case "Jeu":

                if (partieTerminee)
                {
                    if (!chargeScene)
                    {
                        chargeScene = true;

                        StartCoroutine(ChargerScene("EcranTitre", 15f));
                    }
                }

                break;

            case "Galerie":

                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse2))
                {
                    indexLangueActuelle = (indexLangueActuelle + 1) % optionsLangages.Length;
                    AssignerTextes(optionsLangages[indexLangueActuelle]);
                }

                if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Mouse1))
                {
                    indexSkyboxActuel++;
                    if (indexSkyboxActuel >= skyboxes.Length)
                    {
                        indexSkyboxActuel = 0;
                    }

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

        List<GameObject> boutonAnimationsListe = new List<GameObject>();

        for (int i = 0; i < boutonsAnimationsContenant.transform.childCount; i++)
            boutonAnimationsListe.Add(boutonsAnimationsContenant.transform.GetChild(i).gameObject);


        List<string> textesBoutonsAnimFR = new List<string>();
        textesBoutonsAnimFR.AddRange(new[]
        { "Inactif", "Rester", "Marcher", "Courir", "Attaquer", "Position Attaque", "Dégâts", "Souffler", "Mort", "Aléatoire" });

        List<string> textesBoutonsAnimEN = new List<string>();
        textesBoutonsAnimEN.AddRange(new[]
        { "Idle", "Stand", "Walk", "Run", "Attack", "Attack Stand", "Damage", "Breath", "Die", "Random" });


        int j = 0;

        foreach (GameObject texteAnimation in boutonAnimationsListe)
        {
            TMP_Text tmp = texteAnimation.transform.GetChild(0).GetComponent<TMP_Text>();

            switch (langage)
            {
                case "FR":

                    tmp.text = textesBoutonsAnimFR[j];

                    j++;

                    if (j >= textesBoutonsAnimFR.Count)
                        j = 0;

                    break;


                case "EN":
                default:

                    tmp.text = textesBoutonsAnimEN[j];

                    j++;

                    if (j >= textesBoutonsAnimEN.Count)
                        j = 0;

                    break;
            }
        }
    }

    public void GererBoutons(GameObject bouton)
    {
        switch (bouton.name)
        {
            case "Play":
            case "Jouer":

                GererAudio.JouerEffetSonore("Play");

                StartCoroutine(ChargerScene("Jeu", tempsChargement));

                break;

            case "Gallery":
            case "Galerie":

                GererAudio.JouerEffetSonore("Gallery");

                StartCoroutine(ChargerScene("Galerie", tempsChargement));

                break;
        }
    }

    public IEnumerator ChargerScene(string nomScene, float delaiSecondes)
    {
        yield return new WaitForSeconds(delaiSecondes);

        SceneManager.LoadScene(nomScene, LoadSceneMode.Single);

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
