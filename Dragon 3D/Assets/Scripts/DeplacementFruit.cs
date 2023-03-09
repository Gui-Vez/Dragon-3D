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

        // Le fruit n'est pas obtenu
        fruitObtenu = false;

        // Trouver le transform parent nommé "Particules"
        particules = gameObject.transform.parent.Find("Particules");

        // Trouver les enfants des "Particules" nommés "Tracée d'étoile" et "Étoile éclatante"
        traceeEtoile = particules.Find("Tracée d'étoile").gameObject;
        etoileEclatante = particules.Find("Étoile éclatante").gameObject;

        // S'il y a le particule de tracée d'étoile,
        if (traceeEtoile != null)
        {
            // Activer et positionner la trace d'étoile
            traceeEtoile.SetActive(true);
            traceeEtoile.transform.position = particules.position;
        }

        // S'il y a le particule d'étoile éclatante,
        if (etoileEclatante != null)
        {
            // Désactiver et positionner l'étoile éclatante
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

        // Réduction de l'échelle du fruit s'il faut le faire
        if (reduireEchelle)
            ReduireEchelle();

        // Positionnement des particules attachées au fruit s'il n'a pas été obtenu
        if (particules != null && !fruitObtenu)
            PositionnerParticules();
    }

    void ReduireEchelle()
    {
        // Ralentir la vitesse de réduction de la taille de l'objet en fonction du temps
        //vitesseEchelle *= Time.deltaTime;

        // Si la taille de l'objet est inférieure ou égale à 0,
        if (transform.localScale.x <= 0)
        {
            // La variable reduireEchelle est définie sur false
            reduireEchelle = false;

            // Appeler une coroutine pour instancier un nouvel objet
            gameObject.GetComponent<GererCollisions>().AppelerCoroutineInstancierObjet();
        }

        // Sinon,
        else
        {
            // Désactiver la collider sphérique attachée à l'objet de jeu
            gameObject.GetComponent<SphereCollider>().enabled = false;

            // Réduire la taille de l'objet de jeu en utilisant la variable vitesseEchelle pour déterminer la quantité de réduction de la taille
            transform.localScale -= new Vector3(vitesseEchelle, vitesseEchelle, vitesseEchelle);
        }
    }

    void PositionnerParticules()
    {
        // Positionner les particules au-dessus de l'objet de jeu en utilisant la position de l'objet.
        particules.position = gameObject.transform.position;
    }

    public void GererParticulesFruitObtenu()
    {
        // Définit la variable fruitObtenu sur true
        fruitObtenu = true;

        // Si un objet traceeEtoile est attaché à l'objet de jeu,
        if (traceeEtoile != null)
        {
            // Positionner l'objet traceeEtoile au-dessus de l'objet de jeu en utilisant sa position
            traceeEtoile.transform.position = gameObject.transform.position;

            // Désactiver l'objet traceeEtoile
            traceeEtoile.SetActive(false);
        }

        // Si un objet etoileEclatante est attaché à l'objet de jeu,
        if (etoileEclatante != null)
        {
            // Activer l'objet etoileEclatante
            etoileEclatante.SetActive(true);

            // Positionner l'objet etoileEclatante au-dessus de l'objet de jeu en utilisant sa position
            etoileEclatante.transform.position = gameObject.transform.position;
        }
    }
}
