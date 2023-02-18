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

    void Update()
    {
        //// Défilement Horizontal ////
        if (!estEnContactAvecUnMurHorizontal)
        {
            horizontal = Input.GetAxis("Horizontal");
            Vector3 nouvellePosition = transform.position + new Vector3(horizontal, 0, 0) * vitesseDeplacement * Time.deltaTime;
            transform.position = nouvellePosition;
        }

        else if (delaiRestantBougerHorizontal > 0)
        {
            delaiRestantBougerHorizontal -= Time.deltaTime;
        }

        else
        {
            estEnContactAvecUnMurHorizontal = false;
        }


        //// Défilement Vertical ////
        if (!estEnContactAvecUnMurVertical)
        {
            vertical = Input.GetAxis("Vertical");
            Vector3 nouvellePosition = transform.position + new Vector3(0, vertical, 0) * vitesseDeplacement * Time.deltaTime;
            transform.position = nouvellePosition;
        }

        else if (delaiRestantBougerVertical > 0)
        {
            delaiRestantBougerVertical -= Time.deltaTime;
        }

        else
        {
            estEnContactAvecUnMurVertical = false;
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
}
