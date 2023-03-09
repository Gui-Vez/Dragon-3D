using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GererAssetsDragon : MonoBehaviour
{
    public GameObject Animal;
    public GameObject ShootPoint;
    public SkinnedMeshRenderer bodyMesh;
    public SkinnedMeshRenderer faceMesh;

    public Texture[] faceTextureArray = new Texture[9];
    public Texture[] bodyTextureArray = new Texture[4];
    public GameObject[] effPrefabArray = new GameObject[9];

    public static GameObject personnageJoueurActuel;
    public static GameObject[] personnagesJoueur;
    public static string animationActuelle;
    public static string textureCorpsActuelle;

    public Vector3 positionApercuPersonnages = new Vector3(0, 0, 0);
    private Vector3 positionInitialePersonnage;

    private Texture textureCorpsInitiale;

    private Animation Animation;


    void Start()
    {
        // Récupérer la référence au composant Animation
        Animation = GetComponent<Animation>();

        // Si la variable personnagesJoueur est nulle,
        if (personnagesJoueur == null)
        {
            // Initialiser la liste de personnages
            personnagesJoueur = GameObject.FindGameObjectsWithTag("Player");

            // Choisir le personnage joueur actuel
            personnageJoueurActuel = personnagesJoueur[0];
        }

        // Stocker la position initiale de l'objet Dragon
        positionInitialePersonnage = Animal.transform.position;

        // Stocker la texture du corps initiale pour une utilisation ultérieure
        textureCorpsInitiale = bodyMesh.materials[0].GetTexture("_MainTex");

        // Lancer l'animation Idle sur l'objet Dragon
        AnimerDragon("Idle");

        // Si la scène actuelle est "Galerie",
        if (GererScenes.sceneActuelle.name == "Galerie")
            // supprimer tous les objets de type Dragon
            EnleverDragons();
    }

    public void AnimerDragon(string animation)
    {
        Vector3 playerV = Vector3.zero;

        // Lancer les animations sur l'objet Dragon
        switch (animation)
        {
            case "Idle":
            case "Inactif":
                EffectClear();
                Animal.GetComponent<Animation>().wrapMode = WrapMode.Loop;
                Animal.GetComponent<Animation>().CrossFade("Idle");
                Animation.clip = Animation.GetClip("Idle");
                TexturerYeux(0);
                break;

            case "Stand":
            case "Rester":
                EffectClear();
                Animal.GetComponent<Animation>().wrapMode = WrapMode.Loop;
                Animal.GetComponent<Animation>().CrossFade("Stand");
                Animation.clip = Animation.GetClip("Stand");
                TexturerYeux(5);
                if (GameObject.FindGameObjectWithTag("Effect") == null) GameObject.Instantiate(effPrefabArray[6]);
                break;

            case "Walk":
            case "Marcher":
                EffectClear();
                Animal.GetComponent<Animation>().wrapMode = WrapMode.Loop;
                Animal.GetComponent<Animation>().CrossFade("Walk");
                Animation.clip = Animation.GetClip("Walk");
                TexturerYeux(0);
                break;

            case "Run":
            case "Courir":
                EffectClear();
                Animal.GetComponent<Animation>().wrapMode = WrapMode.Loop;
                Animal.GetComponent<Animation>().CrossFade("Run");
                Animation.clip = Animation.GetClip("Run");
                TexturerYeux(6);
                if (GameObject.FindGameObjectWithTag("Effect") == null) GameObject.Instantiate(effPrefabArray[5]);
                break;

            case "Attack":
            case "Attaquer":
                EffectClear();
                Animal.GetComponent<Animation>().wrapMode = WrapMode.Once;
                Animal.GetComponent<Animation>().CrossFade("Attack");
                Animation.clip = Animation.GetClip("Attack");
                TexturerYeux(3);

                playerV = new Vector3(ShootPoint.transform.position.x, ShootPoint.transform.position.y, ShootPoint.transform.position.z);
                Instantiate(effPrefabArray[0], new Vector3(playerV.x, playerV.y, playerV.z), Animal.transform.rotation);
                break;

            case "AttackStand":
            case "PositionAttaque":
                EffectClear();
                Animal.GetComponent<Animation>().wrapMode = WrapMode.Loop;
                Animal.GetComponent<Animation>().CrossFade("AttackStand");
                Animation.clip = Animation.GetClip("AttackStand");
                TexturerYeux(7);
                if (GameObject.FindGameObjectWithTag("Effect") == null) GameObject.Instantiate(effPrefabArray[2]);
                break;

            case "Damage":
            case "Dégâts":
                EffectClear();
                Animal.GetComponent<Animation>().wrapMode = WrapMode.Once;
                Animal.GetComponent<Animation>().CrossFade("Damage");
                Animation.clip = Animation.GetClip("Damage");
                faceMesh.materials[0].SetTexture("_MainTex", faceTextureArray[8]);
                TexturerYeux(8);
                if (GameObject.FindGameObjectWithTag("Effect") == null) GameObject.Instantiate(effPrefabArray[3], transform.position, Quaternion.identity);
                break;

            case "Eat":
            case "Manger":
                EffectClear();
                Animal.GetComponent<Animation>().wrapMode = WrapMode.Loop;
                Animal.GetComponent<Animation>().CrossFade("Eat");
                Animation.clip = Animation.GetClip("Eat");
                TexturerYeux(5);
                if (GameObject.FindGameObjectWithTag("Effect") == null) GameObject.Instantiate(effPrefabArray[7]);
                break;

            case "Sleep":
            case "Dormir":
                EffectClear();
                Animal.GetComponent<Animation>().wrapMode = WrapMode.Loop;
                Animal.GetComponent<Animation>().CrossFade("Sleep");
                Animation.clip = Animation.GetClip("Sleep");
                TexturerYeux(4);
                if (GameObject.FindGameObjectWithTag("Effect") == null) GameObject.Instantiate(effPrefabArray[1]);
                break;

            case "Breath":
            case "Souffler":
                EffectClear();
                Animal.GetComponent<Animation>().wrapMode = WrapMode.Once;
                Animal.GetComponent<Animation>().CrossFade("Breath");
                Animation.clip = Animation.GetClip("Breath");
                TexturerYeux(1);
                if (GameObject.FindGameObjectWithTag("Effect") == null) GameObject.Instantiate(effPrefabArray[8]);
                playerV = new Vector3(ShootPoint.transform.position.x, ShootPoint.transform.position.y, ShootPoint.transform.position.z);
                Instantiate(effPrefabArray[8], new Vector3(playerV.x, playerV.y, playerV.z), Animal.transform.rotation);
                break;

            case "Die":
            case "Mort":
                EffectClear();
                Animal.GetComponent<Animation>().wrapMode = WrapMode.Once;
                Animal.GetComponent<Animation>().CrossFade("Die");
                Animation.clip = Animation.GetClip("Die");
                TexturerYeux(2);
                if (GameObject.FindGameObjectWithTag("Effect") == null) GameObject.Instantiate(effPrefabArray[4], transform.position, Quaternion.identity);
                break;

            case "Random":
            case "Aléatoire":
                GameObject BoutonsAnimations = GameObject.Find("Animations");
                Button[] boutonsEnfants = BoutonsAnimations.GetComponentsInChildren<Button>();
                Button boutonAleatoire = boutonsEnfants[Random.Range(0, boutonsEnfants.Length)].GetComponent<Button>();
                BoutonsAnimations.GetComponent<GererBoutonsUI>().BoutonTrigger(boutonAleatoire);
                break;

            default:
                Debug.LogWarning("Pas d'animation de définie");
                break;
        }

        if (animation == "Random" || animation == "Aléatoire")
            return;

        else
            animationActuelle = animation;
    }

    public void TexturerCorps(string textureCorps)
    {
        // Choisir la texture du corps du dragon
        switch (textureCorps)
        {
            case "Red"   :
            case "Rouge" : bodyMesh.materials[0].SetTexture("_MainTex", bodyTextureArray[0]); break;

            case "Green" :
            case "Vert"  : bodyMesh.materials[0].SetTexture("_MainTex", bodyTextureArray[1]); break;

            case "Blue"  :
            case "Bleu"  : bodyMesh.materials[0].SetTexture("_MainTex", bodyTextureArray[2]); break;

            case "Purple":
            case "Mauve" : bodyMesh.materials[0].SetTexture("_MainTex", bodyTextureArray[3]); break;

            case "Yellow":
            case "Jaune" : bodyMesh.materials[0].SetTexture("_MainTex", bodyTextureArray[4]); break;

            case "Black" :
            case "Noir"  : bodyMesh.materials[0].SetTexture("_MainTex", bodyTextureArray[5]); break;

            case "Grey"  :
            case "Gris"  : bodyMesh.materials[0].SetTexture("_MainTex", bodyTextureArray[6]); break;

            case "White" :
            case "Blanc" : bodyMesh.materials[0].SetTexture("_MainTex", bodyTextureArray[7]); break;

            case "Random":
            case "Aléatoire":
                GameObject BoutonsTexturesCorps = GameObject.Find("Textures Corps");
                Button[] boutonsEnfants = BoutonsTexturesCorps.GetComponentsInChildren<Button>();
                Button boutonAleatoire = boutonsEnfants[Random.Range(0, boutonsEnfants.Length)].GetComponent<Button>();
                BoutonsTexturesCorps.GetComponent<GererBoutonsUI>().BoutonTrigger(boutonAleatoire);
                break;
        }

        if (textureCorps == "Random" || textureCorps == "Aléatoire")
            return;

        else
            textureCorpsActuelle = textureCorps;
    }

    void TexturerYeux(int textureYeux)
    {
        // Choisir la texture des yeux du dragon
        faceMesh.materials[0].SetTexture("_MainTex", faceTextureArray[textureYeux]);
    }

    public void ChangerPersonnage(string personnage)
    {
        // Appeler la méthode qui active les dragons
        ActiverDragons();

        // Selon le personnage en question, choisir ce personnage à afficher
        switch (personnage)
        {
            case "Baby 1" :
            case "Bébé 1" : personnageJoueurActuel = GameObject.Find("Dragon_11"); break;
            case "Young 1":
            case "Jeune 1": personnageJoueurActuel = GameObject.Find("Dragon_12"); break;
            case "Old 1"  :
            case "Vieux 1": personnageJoueurActuel = GameObject.Find("Dragon_13"); break;

            case "Baby 2" :
            case "Bébé 2" : personnageJoueurActuel = GameObject.Find("Dragon_21"); break;
            case "Young 2":
            case "Jeune 2": personnageJoueurActuel = GameObject.Find("Dragon_22"); break;
            case "Old 2"  :
            case "Vieux 2": personnageJoueurActuel = GameObject.Find("Dragon_23"); break;

            case "Baby 3" :
            case "Bébé 3" : personnageJoueurActuel = GameObject.Find("Dragon_31"); break;
            case "Young 3":
            case "Jeune 3": personnageJoueurActuel = GameObject.Find("Dragon_32"); break;
            case "Old 3"  :
            case "Vieux 3": personnageJoueurActuel = GameObject.Find("Dragon_33"); break;

            case "Random"   :
            case "Aléatoire":
                GameObject BoutonsPersonnages = GameObject.Find("Personnages");
                Button[] boutonsEnfants = BoutonsPersonnages.GetComponentsInChildren<Button>();
                Button boutonAleatoire = boutonsEnfants[Random.Range(0, boutonsEnfants.Length)].GetComponent<Button>();
                BoutonsPersonnages.GetComponent<GererBoutonsUI>().BoutonTrigger(boutonAleatoire);
                break;
        }

        // Enlever les dragons
        EnleverDragons();
    }

    void ActiverDragons()
    {
        // Pour tous les personnages joueurs,
        for (int i = 0; i < personnagesJoueur.Length; i++)
            // Activer le dragon
            personnagesJoueur[i].SetActive(true);

        // Affecter la texture du corps
        bodyMesh.materials[0].SetTexture("_MainTex", textureCorpsInitiale);
    }

    void EnleverDragons()
    {
        // Pour tous les personnages dragon,
        for (int i = 0; i < personnagesJoueur.Length; i++)
        {
            // Si le dragon correspond à celui sélectionné,
            if (personnagesJoueur[i] == personnageJoueurActuel)
            {
                // Affecter les composantes du dragon
                personnagesJoueur[i].transform.position = positionApercuPersonnages;
                personnageJoueurActuel.GetComponent<Animation>().CrossFade(animationActuelle);
                faceMesh.materials[0].SetTexture("_MainTex", faceTextureArray[0]);
                TexturerCorps(textureCorpsActuelle);
            }

            // Sinon,
            else
            {
                // Transformer la osition du dragon pour le mettre ailleurs que la zone d'affichage
                personnagesJoueur[i].transform.position = positionInitialePersonnage;

                // Désactiver le dragon
                personnagesJoueur[i].SetActive(false);
            }
        }
    }

    public static void TrouverAssetsDragon()
    {
        // Si le dragon existe,
        if (personnageJoueurActuel != null)
            // Prendre le composant qui gère les boutons UI du dragon
            GererBoutonsUI._GererAssetsDragon = personnageJoueurActuel.GetComponent<GererAssetsDragon>();

        // Sinon, retourner la méthode
        else
            return;
    }

    void EffectClear()
    {
        // Trouver l'objet d'effet spécial
        GameObject tFindObj = GameObject.FindGameObjectWithTag("Effect");

        // S'il existe un effet,
        if (tFindObj != null)
            // Détruire cet objet
            Destroy(tFindObj);
    }
}
