using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeplacementFruit : MonoBehaviour
{
    public Transform positionCible;

    public float vitesseDeplacement = 10f;
    public float vitesseRotation = 100f;
    //public float amplitudeSinus = 0.1f;
    //public float frequenceSinus = 3f;
    //private float tempsDepartSinus;
    //private float initialY;

    public bool reduireEchelle;
    public float vitesseEchelle = 1f;

    private Transform particules;
    private GameObject traceeEtoile;
    private GameObject etoileEclatante;

    private bool fruitObtenu;

    void Start()
    {
        // Générer une vitesse de déplacement aléatoire
        //vitesseDeplacement = Random.Range(vitesseDeplacement / 2, vitesseDeplacement * 2);

        // Démarrer le temps à accorder à la translation sinusoïdale
        //tempsDepartSinus = Time.time;

        // Sauvegarder la position en y initiale
        //initialY = transform.localPosition.y;

        fruitObtenu = false;

        particules = gameObject.transform.parent.Find("Particules");

        traceeEtoile = particules.Find("Tracée d'étoile").gameObject;
        etoileEclatante = particules.Find("Étoile éclatante").gameObject;

        if (traceeEtoile != null)
        {
            traceeEtoile.SetActive(true);
            traceeEtoile.transform.position = particules.position;
        }

        if (etoileEclatante != null)
        {
            etoileEclatante.transform.position = particules.position;
            etoileEclatante.SetActive(false);
        }
    }

    void Update()
    {
        // Calculer la direction de la cible
        Vector3 direction = positionCible.position - transform.position;

        // Cacluler la distance de la cible
        float distance = direction.magnitude;

        // Si l'objet N'EST PAS suffisament près de la cible OU que l'échelle de l'objet N'EST PAS en train de se réduire,
        if (!(distance < 0.1f || reduireEchelle))
        {
            // Calculer la distance maximale de déplacement
            float distanceMax = vitesseDeplacement * Time.deltaTime;

            // Calculer le mouvement à parcourir
            Vector3 mouvement = Vector3.ClampMagnitude(direction, distanceMax);

            // Bouger vers la cible
            transform.position += mouvement;
        }

        // Calculer l'angle de rotation du fruit
        float angleRotation = vitesseRotation * Time.deltaTime;
        transform.Rotate(Vector3.forward, angleRotation);

        // Calculer la position en y en utilisant une fonction sinusoidale
        //float y = initialY + Mathf.Sin((Time.time - tempsDepartSinus) * frequenceSinus) * amplitudeSinus;
        //transform.localPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);

        if (reduireEchelle)
            ReduireEchelle();

        if (particules != null && !fruitObtenu)
            PositionnerParticules();
    }

    void ReduireEchelle()
    {
        //vitesseEchelle *= Time.deltaTime;
        
        if (transform.localScale.x <= 0)
        {
            reduireEchelle = false;

            gameObject.GetComponent<GererCollisions>().AppelerCoroutineInstancierObjet();
        }

        else
        {
            gameObject.GetComponent<SphereCollider>().enabled = false;

            transform.localScale -= new Vector3(vitesseEchelle, vitesseEchelle, vitesseEchelle);
        }
    }

    void PositionnerParticules()
    {
        particules.position = gameObject.transform.position;
    }

    public void GererParticulesFruitObtenu()
    {
        fruitObtenu = true;

        if (traceeEtoile != null)
        {
            traceeEtoile.transform.position = gameObject.transform.position;
            traceeEtoile.SetActive(false);
        }

        if (etoileEclatante != null)
        {
            etoileEclatante.SetActive(true);
            etoileEclatante.transform.position = gameObject.transform.position;
        }
    }
}
