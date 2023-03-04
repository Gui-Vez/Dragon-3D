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

    private void Start()
    {
        precedenteAnimation = GetComponent<Animation>().clip;
        Rigidbody = GetComponent<Rigidbody>();

        positionInitiale = transform.position;
    }

    void Update()
    {
        // Si le dragon peut bouger,
        if (peutBouger)
        {
            //// Défilement Horizontal ////
            if (!estEnContactAvecUnMurHorizontal)
            {
                horizontal = Input.GetAxis("Horizontal");
                Vector3 nouvellePosition = transform.position + new Vector3(horizontal, 0, 0) * vitesseDeplacement * Time.deltaTime;
                transform.position = nouvellePosition;
            }

            else if (delaiRestantBougerHorizontal > 0)
                delaiRestantBougerHorizontal -= Time.deltaTime;

            else
                estEnContactAvecUnMurHorizontal = false;


            //// Défilement Vertical ////
            if (!estEnContactAvecUnMurVertical)
            {
                vertical = Input.GetAxis("Vertical");
                Vector3 nouvellePosition = transform.position + new Vector3(0, vertical, 0) * vitesseDeplacement * Time.deltaTime;
                transform.position = nouvellePosition;
            }

            else if (delaiRestantBougerVertical > 0)
                delaiRestantBougerVertical -= Time.deltaTime;

            else
                estEnContactAvecUnMurVertical = false;
        }

        if (peutBouger)
        {
            Vector3 velocite = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")) * vitesseDeplacement;

            if (velocite.x > valeurVelociteMinimale.x || velocite.z > valeurVelociteMinimale.z ||
                velocite.x < valeurVelociteMinimale.x || velocite.z < valeurVelociteMinimale.z)
                seDeplace = true;

            else
                seDeplace = false;


            if (seDeplace)
                gameObject.GetComponent<GererAssetsDragon>().AnimerDragon("Marcher");

            else
                gameObject.GetComponent<GererAssetsDragon>().AnimerDragon("Inactif");

            precedenteAnimation = GetComponent<Animation>().clip;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Mur")
        {
            Vector3 normal = collision.contacts[0].normal;

            if (Mathf.Abs(normal.x) > Mathf.Abs(normal.y))
            {
                horizontal = 0;
                estEnContactAvecUnMurHorizontal = true;
                delaiRestantBougerHorizontal = delaiBouger;
            }

            else
            {
                vertical = 0;
                estEnContactAvecUnMurVertical = true;
                delaiRestantBougerVertical = delaiBouger;
            }

            Vector3 nouvellePosition = transform.position + normal * reculMurs;
            transform.position = nouvellePosition;
        }
    }
    
    void OnTriggerEnter(Collider trigger)
    {
        if (trigger.gameObject.tag == "Mur")
        {
            transform.position = positionInitiale;
        }
    }
}
