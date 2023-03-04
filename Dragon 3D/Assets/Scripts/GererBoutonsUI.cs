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
        GameObject BoutonsEcrans = GameObject.Find("Boutons Écrans");

        if (ecransBoutonsListe.Count == 0)
        {
            for (int i = 0; i < BoutonsEcrans.transform.childCount; i++)
            {
                Transform child = BoutonsEcrans.transform.GetChild(i);
                ecransBoutonsListe.Add(child.gameObject);
            }
        }
    }

    public void BoutonTrigger(Button boutonUI)
    {
        // Donner des instructions aux scènes respectives
        switch (GererScenes.sceneActuelle.name)
        {
            case "Galerie":

                GererAssetsDragon.TrouverAssetsDragon();

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
        switch (direction)
        {
            case "Gauche": indexEcran--; break;
            case "Droite": indexEcran++; break;
        }

        if (indexEcran < 0)
            indexEcran = 0;

        if (indexEcran > ecransBoutonsListe.Count - 1)
            indexEcran = ecransBoutonsListe.Count - 1;

        foreach (GameObject ecranBouton in ecransBoutonsListe)
            ecranBouton.SetActive(false);

        ecransBoutonsListe[indexEcran].SetActive(true);
    }
}