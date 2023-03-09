using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GererBoutonsUI : MonoBehaviour
{
    public static GererAssetsDragon _GererAssetsDragon;

    public static List<GameObject> ecransBoutonsListe = new List<GameObject>();
    private int indexEcran;


    void Start()
    {
        // Trouver l'objet qui comporte les boutons des écrans
        GameObject BoutonsEcrans = GameObject.Find("Boutons Écrans");

        // S'il n'y a pas d'écran à la liste,
        if (ecransBoutonsListe.Count == 0)
        {
            // Pour tous les écrans de boutons,
            for (int i = 0; i < BoutonsEcrans.transform.childCount; i++)
            {
                // Trouver l'enfant de l'objet
                Transform child = BoutonsEcrans.transform.GetChild(i);

                // Ajouter cet enfant à la liste
                ecransBoutonsListe.Add(child.gameObject);
            }
        }
    }

    public void BoutonTrigger(Button boutonUI)
    {
        // Donner des instructions aux scènes respectives
        switch (GererScenes.sceneActuelle.name)
        {
            // Pour la galerie,
            case "Galerie":

                // Trouver les assets du dragon
                GererAssetsDragon.TrouverAssetsDragon();

                // selon le nom de l'objet; animer, texturer ou changer le personnage / défiler l'écran des boutons UI
                switch (this.name)
                {
                    case "Animations"     : if (_GererAssetsDragon) _GererAssetsDragon.AnimerDragon(boutonUI.name);      break;
                    case "Textures Corps" : if (_GererAssetsDragon) _GererAssetsDragon.TexturerCorps(boutonUI.name);     break;
                    case "Personnages"    : if (_GererAssetsDragon) _GererAssetsDragon.ChangerPersonnage(boutonUI.name); break;

                    case "Défilement" : ChangerEcranUI(boutonUI.name); break;
                }

                break;
        }
    }

    void ChangerEcranUI(string direction)
    {
        // Selon la direction du bouton, changer l'écran vers la gauche ou vers la droite
        switch (direction)
        {
            case "Gauche": indexEcran--; break;
            case "Droite": indexEcran++; break;
        }

        // S'il y a moins d'un écran,
        if (indexEcran < 0)
            // Faire en sorte qu'il n'y a que 0 écrans
            indexEcran = 0;

        // Si l'index de l'écran surpasse le nombre d'écrans de boutons maximal,
        if (indexEcran > ecransBoutonsListe.Count - 1)
            // Rendre l'index à la valeur maximale de la liste des écrans
            indexEcran = ecransBoutonsListe.Count - 1;

        // Pour chaque écran de la liste des écrans de boutons,
        foreach (GameObject ecranBouton in ecransBoutonsListe)
            // Désactiver l'écran
            ecranBouton.SetActive(false);

        // Activer l'écran actif
        ecransBoutonsListe[indexEcran].SetActive(true);
    }
}