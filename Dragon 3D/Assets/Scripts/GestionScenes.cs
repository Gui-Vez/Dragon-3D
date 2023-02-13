using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using TMPro;

public class GestionScenes : MonoBehaviour
{
    /* ****************** */
    /* Gestion des scènes */
    /* ****************** */

    Scene sceneActuelle;

    GameObject boutonsAnimationsContenant;

    void Start()
    {
        // Trouver la scène actuelle
        sceneActuelle = SceneManager.GetActiveScene();

        // Donner des instructions aux scènes respectives
        switch (sceneActuelle.name)
        {
            case "EcranTitre":

                StartCoroutine(ChargerScene("Jeu", 3f));

                break;


            case "Galerie":

                boutonsAnimationsContenant = GameObject.Find("Animations");

                AssignerTextes("FR");

                break;
        }
    }

    void Update()
    {
        /* (À optimiser) */

        // Donner des instructions aux scènes respectives
        switch (sceneActuelle.name)
        {
            case "EcranTitre":

                break;


            case "Galerie":

                if (Input.GetKey(KeyCode.Space))
                {
                    AssignerTextes("FR");
                }

                else
                {
                    AssignerTextes("EN");
                }

                break;
        }
    }

    void AssignerTextes(string langage)
    {
        List<GameObject> boutonAnimationsListe = new List<GameObject>();

        for (int i = 0; i < boutonsAnimationsContenant.transform.childCount; i++)
        {
            boutonAnimationsListe.Add(boutonsAnimationsContenant.transform.GetChild(i).gameObject);
        }


        List<string> textesBoutonsAnimFR = new List<string>();
        textesBoutonsAnimFR.AddRange(new[]
        { "Inactif", "Rester", "Marcher", "Courir", "Attaquer", "Position d'attaque", "Dégâts", "Souffler", "Mort", "Aléatoire" });

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
                    {
                        j = 0;
                    }

                    break;


                case "EN":
                default:

                    tmp.text = textesBoutonsAnimEN[j];

                    j++;

                    if (j >= textesBoutonsAnimEN.Count)
                    {
                        j = 0;
                    }

                    break;
            }
        }
    }

    IEnumerator ChargerScene(string nomScene, float delaiSecondes)
    {
        yield return new WaitForSeconds(delaiSecondes);

        // Ouvrir la scène de jeu
        SceneManager.LoadScene(nomScene);

        yield return null;
    }
}
