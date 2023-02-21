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


    void Start()
    {
        // Trouver la scène actuelle
        sceneActuelle = SceneManager.GetActiveScene();

        // Donner des instructions aux scènes respectives
        switch (sceneActuelle.name)
        {
            case "EcranTitre":

                //StartCoroutine(ChargerScene("Jeu", 3f));

                break;


            case "Galerie":

                boutonsAnimationsContenant = GameObject.Find("Animations");

                RenderSettings.skybox = skyboxes[indexSkyboxActuel];

                break;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitterJeu();
        }

        // Donner des instructions aux scènes respectives
        switch (sceneActuelle.name)
        {
            case "EcranTitre":

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

                StartCoroutine(ChargerScene("Jeu", tempsChargement));

                break;

            case "Gallery":
            case "Galerie":

                StartCoroutine(ChargerScene("Galerie", tempsChargement));

                break;
        }
    }

    IEnumerator ChargerScene(string nomScene, float delaiSecondes)
    {
        yield return new WaitForSeconds(delaiSecondes);

        // Ouvrir la scène de jeu
        SceneManager.LoadScene(nomScene);

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
