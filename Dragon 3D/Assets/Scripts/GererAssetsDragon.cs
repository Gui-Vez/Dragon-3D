using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.SceneManagement;


public class GererAssetsDragon : MonoBehaviour
{
    public GameObject Animal;
    public GameObject ShootPoint;
    public SkinnedMeshRenderer bodyMesh;
    public SkinnedMeshRenderer faceMesh;

    public Texture[] faceTextureArray = new Texture[9];
    public Texture[] bodyTextureArray = new Texture[4];
    public GameObject[] effPrefabArray = new GameObject[9];

    void Start()
    {
        AnimerDragon("Idle");
    }

    void Update()
    {
        
    }

    public void AnimerDragon(string animation)
    {
        Vector3 playerV = Vector3.zero;

        switch (animation)
        {
            case "Idle":
            case "Inactif":
                EffectClear();
                Animal.GetComponent<Animation>().wrapMode = WrapMode.Loop;
                Animal.GetComponent<Animation>().CrossFade("Idle");
                faceMesh.materials[0].SetTexture("_MainTex", faceTextureArray[0]);
                break;

            case "Stand":
            case "Rester":
                EffectClear();
                Animal.GetComponent<Animation>().wrapMode = WrapMode.Loop;
                Animal.GetComponent<Animation>().CrossFade("Stand");
                faceMesh.materials[0].SetTexture("_MainTex", faceTextureArray[5]);
                if (GameObject.FindGameObjectWithTag("Effect") == null) GameObject.Instantiate(effPrefabArray[6]);
                break;

            case "Walk":
            case "Marcher":
                EffectClear();
                Animal.GetComponent<Animation>().wrapMode = WrapMode.Loop;
                Animal.GetComponent<Animation>().CrossFade("Walk");
                faceMesh.materials[0].SetTexture("_MainTex", faceTextureArray[0]);
                break;

            case "Run":
            case "Courir":
                EffectClear();
                Animal.GetComponent<Animation>().wrapMode = WrapMode.Loop;
                Animal.GetComponent<Animation>().CrossFade("Run");
                faceMesh.materials[0].SetTexture("_MainTex", faceTextureArray[6]);
                if (GameObject.FindGameObjectWithTag("Effect") == null) GameObject.Instantiate(effPrefabArray[5]);
                break;

            case "Attack":
            case "Attaquer":
                EffectClear();
                Animal.GetComponent<Animation>().wrapMode = WrapMode.Once;
                Animal.GetComponent<Animation>().CrossFade("Attack");
                faceMesh.materials[0].SetTexture("_MainTex", faceTextureArray[3]);

                playerV = new Vector3(ShootPoint.transform.position.x, ShootPoint.transform.position.y, ShootPoint.transform.position.z);
                Instantiate(effPrefabArray[0], new Vector3(playerV.x, playerV.y, playerV.z), Animal.transform.rotation);
                break;

            case "Attack Stand":
            case "Position d'attaque":
                EffectClear();
                Animal.GetComponent<Animation>().wrapMode = WrapMode.Loop;
                Animal.GetComponent<Animation>().CrossFade("AttackStand");
                faceMesh.materials[0].SetTexture("_MainTex", faceTextureArray[7]);
                if (GameObject.FindGameObjectWithTag("Effect") == null) GameObject.Instantiate(effPrefabArray[2]);
                break;

            case "Damage":
            case "Dégâts":
                EffectClear();
                Animal.GetComponent<Animation>().wrapMode = WrapMode.Once;
                Animal.GetComponent<Animation>().CrossFade("Damage");
                faceMesh.materials[0].SetTexture("_MainTex", faceTextureArray[8]);
                if (GameObject.FindGameObjectWithTag("Effect") == null) GameObject.Instantiate(effPrefabArray[3]);
                break;

            case "Eat":
            case "Manger":
                EffectClear();
                Animal.GetComponent<Animation>().wrapMode = WrapMode.Loop;
                Animal.GetComponent<Animation>().CrossFade("Eat");
                faceMesh.materials[0].SetTexture("_MainTex", faceTextureArray[5]);
                if (GameObject.FindGameObjectWithTag("Effect") == null) GameObject.Instantiate(effPrefabArray[7]);
                break;

            case "Sleep":
            case "Dormir":
                EffectClear();
                Animal.GetComponent<Animation>().wrapMode = WrapMode.Loop;
                Animal.GetComponent<Animation>().CrossFade("Sleep");
                faceMesh.materials[0].SetTexture("_MainTex", faceTextureArray[4]);
                if (GameObject.FindGameObjectWithTag("Effect") == null) GameObject.Instantiate(effPrefabArray[1]);
                break;

            case "Breath":
            case "Souffler":
                EffectClear();
                Animal.GetComponent<Animation>().wrapMode = WrapMode.Once;
                Animal.GetComponent<Animation>().CrossFade("Breath");
                faceMesh.materials[0].SetTexture("_MainTex", faceTextureArray[1]);
                if (GameObject.FindGameObjectWithTag("Effect") == null) GameObject.Instantiate(effPrefabArray[8]);
                playerV = new Vector3(ShootPoint.transform.position.x, ShootPoint.transform.position.y, ShootPoint.transform.position.z);
                Instantiate(effPrefabArray[8], new Vector3(playerV.x, playerV.y, playerV.z), Animal.transform.rotation);
                break;

            case "Die":
            case "Mort":
                EffectClear();
                Animal.GetComponent<Animation>().wrapMode = WrapMode.Once;
                Animal.GetComponent<Animation>().CrossFade("Die");
                faceMesh.materials[0].SetTexture("_MainTex", faceTextureArray[2]);
                if (GameObject.FindGameObjectWithTag("Effect") == null) GameObject.Instantiate(effPrefabArray[4]);
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
    }

    public void TexturerCorps(string texture)
    {
        switch (texture)
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

            case "Black":
            case "Noir" :  bodyMesh.materials[0].SetTexture("_MainTex", bodyTextureArray[5]); break;

            case "Grey" :
            case "Gris" :  bodyMesh.materials[0].SetTexture("_MainTex", bodyTextureArray[6]); break;

            case "White":
            case "Blanc":  bodyMesh.materials[0].SetTexture("_MainTex", bodyTextureArray[7]); break;

            case "Random":
            case "Aléatoire":
                GameObject BoutonsTexturesCorps = GameObject.Find("Textures Corps");
                Button[] boutonsEnfants = BoutonsTexturesCorps.GetComponentsInChildren<Button>();
                Button boutonAleatoire = boutonsEnfants[Random.Range(0, boutonsEnfants.Length)].GetComponent<Button>();
                BoutonsTexturesCorps.GetComponent<GererBoutonsUI>().BoutonTrigger(boutonAleatoire);
                break;
        }
    }

    void EffectClear()
    {
        GameObject tFindObj = GameObject.FindGameObjectWithTag("Effect");
        if (tFindObj != null)
        {
            DestroyImmediate(tFindObj);
        }
    }
}
