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
        // G�n�rer une vitesse de d�placement al�atoire
        //vitesseDeplacement = Random.Range(vitesseDeplacement / 2, vitesseDeplacement * 2);

        // D�marrer le temps � accorder � la translation sinuso�dale
        //tempsDepartSinus = Time.time;

        // Sauvegarder la position en y initiale
        //initialY = transform.localPosition.y;

        // Le fruit n'est pas obtenu
        fruitObtenu = false;

        // Trouver le transform parent nomm� "Particules"
        particules = gameObject.transform.parent.Find("Particules");

        // Trouver les enfants des "Particules" nomm�s "Trac�e d'�toile" et "�toile �clatante"
        traceeEtoile = particules.Find("Trac�e d'�toile").gameObject;
        etoileEclatante = particules.Find("�toile �clatante").gameObject;

        // S'il y a le particule de trac�e d'�toile,
        if (traceeEtoile != null)
        {
            // Activer et positionner la trace d'�toile
            traceeEtoile.SetActive(true);
            traceeEtoile.transform.position = particules.position;
        }

        // S'il y a le particule d'�toile �clatante,
        if (etoileEclatante != null)
        {
            // D�sactiver et positionner l'�toile �clatante
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

        // Si l'objet N'EST PAS suffisament pr�s de la cible OU que l'�chelle de l'objet N'EST PAS en train de se r�duire,
        if (!(distance < 0.1f || reduireEchelle))
        {
            // Calculer la distance maximale de d�placement
            float distanceMax = vitesseDeplacement * Time.deltaTime;

            // Calculer le mouvement � parcourir
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

        // R�duction de l'�chelle du fruit s'il faut le faire
        if (reduireEchelle)
            ReduireEchelle();

        // Positionnement des particules attach�es au fruit s'il n'a pas �t� obtenu
        if (particules != null && !fruitObtenu)
            PositionnerParticules();
    }

    void ReduireEchelle()
    {
        // Ralentir la vitesse de r�duction de la taille de l'objet en fonction du temps
        //vitesseEchelle *= Time.deltaTime;

        // Si la taille de l'objet est inf�rieure ou �gale � 0,
        if (transform.localScale.x <= 0)
        {
            // La variable reduireEchelle est d�finie sur false
            reduireEchelle = false;

            // Appeler une coroutine pour instancier un nouvel objet
            gameObject.GetComponent<GererCollisions>().AppelerCoroutineInstancierObjet();
        }

        // Sinon,
        else
        {
            // D�sactiver la collider sph�rique attach�e � l'objet de jeu
            gameObject.GetComponent<SphereCollider>().enabled = false;

            // R�duire la taille de l'objet de jeu en utilisant la variable vitesseEchelle pour d�terminer la quantit� de r�duction de la taille
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
        // D�finit la variable fruitObtenu sur true
        fruitObtenu = true;

        // Si un objet traceeEtoile est attach� � l'objet de jeu,
        if (traceeEtoile != null)
        {
            // Positionner l'objet traceeEtoile au-dessus de l'objet de jeu en utilisant sa position
            traceeEtoile.transform.position = gameObject.transform.position;

            // D�sactiver l'objet traceeEtoile
            traceeEtoile.SetActive(false);
        }

        // Si un objet etoileEclatante est attach� � l'objet de jeu,
        if (etoileEclatante != null)
        {
            // Activer l'objet etoileEclatante
            etoileEclatante.SetActive(true);

            // Positionner l'objet etoileEclatante au-dessus de l'objet de jeu en utilisant sa position
            etoileEclatante.transform.position = gameObject.transform.position;
        }
    }
}
