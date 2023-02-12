using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GererBoutonsUI : MonoBehaviour
{
    GererAssetsDragon _GererAssetsDragon;

    public static List<GameObject> ecransBoutonsListe = new List<GameObject>();
    int indexEcran;

    void Start()
    {
        _GererAssetsDragon = GameObject.FindGameObjectWithTag("Player").GetComponent<GererAssetsDragon>();

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
        switch (this.name)
        {
            case "Animations"     : _GererAssetsDragon.AnimerDragon(boutonUI.name); break;
            case "Textures Corps" : _GererAssetsDragon.TexturerCorps(boutonUI.name); break;

            case "Défilement" : ChangerEcranUI(boutonUI.name); break;
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
        {
            indexEcran = 0;


        }

        if (indexEcran > ecransBoutonsListe.Count - 1)
        {
            indexEcran = ecransBoutonsListe.Count - 1;
        }

        foreach (GameObject ecranBouton in ecransBoutonsListe)
        {
            ecranBouton.SetActive(false);
        }

        ecransBoutonsListe[indexEcran].SetActive(true);
    }
}






/*
    /////////////////////////////////////////////////////////////////////
    if (GUI.Button(new Rect(20, 700, 120, 40), "RandomFace"))
    {
        faceMesh.materials[0].SetTexture("_MainTex", faceTextureArray[Random.Range(0, faceTextureArray.Length)]);
    }
    if (GUI.Button(new Rect(150, 700, 70, 40), "Face01"))
    {
        faceMesh.materials[0].SetTexture("_MainTex", faceTextureArray[0]);
    }
    if (GUI.Button(new Rect(220, 700, 70, 40), "Face02"))
    {
        faceMesh.materials[0].SetTexture("_MainTex", faceTextureArray[1]);
    }
    if (GUI.Button(new Rect(290, 700, 70, 40), "Face03"))
    {
        faceMesh.materials[0].SetTexture("_MainTex", faceTextureArray[2]);
    }
    if (GUI.Button(new Rect(360, 700, 70, 40), "Face04"))
    {
        faceMesh.materials[0].SetTexture("_MainTex", faceTextureArray[3]);
    }
    if (GUI.Button(new Rect(430, 700, 70, 40), "Face05"))
    {
        faceMesh.materials[0].SetTexture("_MainTex", faceTextureArray[4]);
    }
    if (GUI.Button(new Rect(500, 700, 70, 40), "Face06"))
    {
        faceMesh.materials[0].SetTexture("_MainTex", faceTextureArray[5]);
    }
    if (GUI.Button(new Rect(570, 700, 70, 40), "Face07"))
    {
        faceMesh.materials[0].SetTexture("_MainTex", faceTextureArray[6]);
    }
    if (GUI.Button(new Rect(640, 700, 70, 40), "Face08"))
    {
        faceMesh.materials[0].SetTexture("_MainTex", faceTextureArray[7]);
    }
    if (GUI.Button(new Rect(710, 700, 70, 40), "Face09"))
    {
        faceMesh.materials[0].SetTexture("_MainTex", faceTextureArray[8]);
    }
    //if (GUI.Button(new Rect(780, 700, 70, 40), "Face10"))
    //{
    //    faceMesh.materials[0].SetTexture("_MainTex", faceTextureArray[9]);
    //}
    /////////////////////////////////////////////////////////////////////////////////

    if (GUI.Button(new Rect(20, 740, 120, 40), "RandomBody"))
    {
        bodyMesh.materials[0].SetTexture("_MainTex", bodyTextureArray[Random.Range(0, bodyTextureArray.Length)]);
    }
    if (GUI.Button(new Rect(150, 740, 70, 40), "Body_01"))
    {
        bodyMesh.materials[0].SetTexture("_MainTex", bodyTextureArray[0]);
    }
    if (GUI.Button(new Rect(220, 740, 70, 40), "Body_02"))
    {
        bodyMesh.materials[0].SetTexture("_MainTex", bodyTextureArray[1]);
    }
    if (GUI.Button(new Rect(290, 740, 70, 40), "Body_03"))
    {
        bodyMesh.materials[0].SetTexture("_MainTex", bodyTextureArray[2]);
    }
    if (GUI.Button(new Rect(360, 740, 70, 40), "Body_04"))
    {
        bodyMesh.materials[0].SetTexture("_MainTex", bodyTextureArray[3]);
    }
    if (GUI.Button(new Rect(430, 740, 70, 40), "Body_05"))
    {
        bodyMesh.materials[0].SetTexture("_MainTex", bodyTextureArray[4]);
    }
    if (GUI.Button(new Rect(500, 740, 70, 40), "Body_06"))
    {
        bodyMesh.materials[0].SetTexture("_MainTex", bodyTextureArray[5]);
    }
    if (GUI.Button(new Rect(570, 740, 70, 40), "Body_07"))
    {
        bodyMesh.materials[0].SetTexture("_MainTex", bodyTextureArray[6]);
    }
    if (GUI.Button(new Rect(640, 740, 70, 40), "Body_08"))
    {
        bodyMesh.materials[0].SetTexture("_MainTex", bodyTextureArray[7]);
    }
    ////////////////////////////////////////////////////////////////////
    if (GUI.Button(new Rect(720, 280, 120, 40), "Fire Dragon 1"))
    {
        SceneManager.LoadScene("Dragon_11");
    }

    if (GUI.Button(new Rect(720, 320, 120, 40), "Fire Dragon 2"))
    {
        SceneManager.LoadScene("Dragon_12");
    }

    if (GUI.Button(new Rect(720, 360, 120, 40), "Fire Dragon 3"))
    {
        SceneManager.LoadScene("Dragon_13");
    }
    if (GUI.Button(new Rect(720, 400, 120, 40), "Ice Dragon 1"))
    {
        SceneManager.LoadScene("Dragon_21");
    }

    if (GUI.Button(new Rect(720, 440, 120, 40), "Ice Dragon 2"))
    {
        SceneManager.LoadScene("Dragon_22");
    }

    if (GUI.Button(new Rect(720, 480, 120, 40), "Ice Dragon 3"))
    {
        SceneManager.LoadScene("Dragon_23");
    }
    if (GUI.Button(new Rect(720, 520, 120, 40), "Thunder Dragon 1"))
    {
        SceneManager.LoadScene("Dragon_31");
    }

    if (GUI.Button(new Rect(720, 560, 120, 40), "Thunder Dragon 2"))
    {
        SceneManager.LoadScene("Dragon_32");
    }

    if (GUI.Button(new Rect(720, 600, 120, 40), "Thunder Dragon 3"))
    {
        SceneManager.LoadScene("Dragon_33");
    }
}
}
*/
