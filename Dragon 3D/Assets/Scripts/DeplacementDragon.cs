using System.Collections;
using UnityEngine;

public class DeplacementDragon : MonoBehaviour
{
    public float vitesseDeplacement = 3.0f;

    private float horizontal = 0f;
    private float vertical = 0f;

    private bool estEnContactAvecUnMurHorizontal = false;
    private bool estEnContactAvecUnMurVertical = false;
    private float delaiRestantBougerHorizontal = 0;
    private float delaiRestantBougerVertical = 0;

    public float reculMurs = 0.05f;
    public float delaiBouger = 0.25f;

    private bool seDeplace = false;
    public bool peutBouger = true;

    private AnimationClip precedenteAnimation;
    private Rigidbody Rigidbody;
    public Vector3 valeurVelociteMinimale = new Vector3(0.5f, 0.5f, 0.5f);

    private Vector3 positionInitiale;

    void Start()
    {
        // Stocke l'animation courante
        precedenteAnimation = GetComponent<Animation>().clip;

        // Récupère la référence au composant Rigidbody du dragon
        Rigidbody = GetComponent<Rigidbody>();

        // Stocke la position initiale du dragon
        positionInitiale = transform.position;
    }

    void Update()
    {
        // Si le dragon peut bouger,
        if (peutBouger)
        {
            //// Défilement Horizontal ////
            // Si le dragon n'est pas en contact avec un mur horizontal,
            if (!estEnContactAvecUnMurHorizontal)
            {
                // Récupère la valeur de déplacement horizontal depuis les entrées de l'utilisateur
                horizontal = Input.GetAxis("Horizontal");
                Vector3 nouvellePosition = transform.position + new Vector3(horizontal, 0, 0) * vitesseDeplacement * Time.deltaTime;
                transform.position = nouvellePosition;
            }

            // Sinon, si le dragon est en contact avec un mur horizontal et qu'il doit attendre avant de pouvoir bouger à nouveau,
            else if (delaiRestantBougerHorizontal > 0)
                // Décrémente le délai restant
                delaiRestantBougerHorizontal -= Time.deltaTime;

            // Sinon,
            else
                // Le dragon n'est plus en contact avec le mur horizontal
                estEnContactAvecUnMurHorizontal = false;


            //// Défilement Vertical ////
            // Si le dragon n'est pas en contact avec un mur vertical,
            if (!estEnContactAvecUnMurVertical)
            {
                // Récupère la valeur de déplacement vertical depuis les entrées de l'utilisateur
                vertical = Input.GetAxis("Vertical");
                Vector3 nouvellePosition = transform.position + new Vector3(0, vertical, 0) * vitesseDeplacement * Time.deltaTime;
                transform.position = nouvellePosition;
            }

            // Sinon, si le dragon est en contact avec un mur vertical et qu'il doit attendre avant de pouvoir bouger à nouveau,
            else if (delaiRestantBougerVertical > 0)
                // Décrémente le délai restant
                delaiRestantBougerVertical -= Time.deltaTime;

            // Sinon,
            else
                // Le dragon n'est plus en contact avec le mur vertical
                estEnContactAvecUnMurVertical = false;
        }

        // Si le dragon peut jouer,
        if (peutBouger)
        {
            // Saisir la vélocité horizontale et verticale du déplacement 
            Vector3 velocite = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")) * vitesseDeplacement;

            // Si l'objet se déplace avec une vitesse suffisante,
            if (velocite.x > valeurVelociteMinimale.x || velocite.z > valeurVelociteMinimale.z ||
                velocite.x < valeurVelociteMinimale.x || velocite.z < valeurVelociteMinimale.z)
                // Le dragon peut se déplacer
                seDeplace = true;

            // Sinon,
            else
                // Le dragon ne peut pas se déplacer
                seDeplace = false;

            // Si le dragon se déplace,
            if (seDeplace)
                // Jouer l'animation de marche
                gameObject.GetComponent<GererAssetsDragon>().AnimerDragon("Marcher");

            // Sinon,
            else
                // Jouer l'animation d'inactivité
                gameObject.GetComponent<GererAssetsDragon>().AnimerDragon("Inactif");

            // Saisir la précédente animation
            precedenteAnimation = GetComponent<Animation>().clip;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Vérifie si l'objet est en contact avec un mur
        if (collision.gameObject.tag == "Mur")
        {
            // Collecter les normals de la collision
            Vector3 normal = collision.contacts[0].normal;

            // Si la normale est plus grande en x,
            if (Mathf.Abs(normal.x) > Mathf.Abs(normal.y))
            {
                // L'objet ne peut plus se déplacer horizontalement
                horizontal = 0;
                estEnContactAvecUnMurHorizontal = true;
                delaiRestantBougerHorizontal = delaiBouger;
            }

            // Sinon,
            else
            {
                // L'objet ne peut plus se déplacer verticalement
                vertical = 0;
                estEnContactAvecUnMurVertical = true;
                delaiRestantBougerVertical = delaiBouger;
            }

            // Repousser l'objet hors du mur
            Vector3 nouvellePosition = transform.position + normal * reculMurs;
            transform.position = nouvellePosition;
        }
    }
    
    void OnTriggerEnter(Collider trigger)
    {
        // Si l'objet entre en collision avec un mur,
        if (trigger.gameObject.tag == "Mur")
        {
            // Positionner le dragon selon sa position initiale
            transform.position = positionInitiale;
        }
    }
}
