using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeplacementDecor : MonoBehaviour
{
    public float vitesseDeplacement = 2f;

    void Start()
    {
        if (gameObject.CompareTag("Nuage"))
            vitesseDeplacement = Random.Range(vitesseDeplacement / 2, vitesseDeplacement * 2);
    }

    void Update()
    {
        switch (gameObject.tag)
        {
            case "Nuage":

                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - vitesseDeplacement);

                break;

            case "Terrain":

                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - vitesseDeplacement);

                break;
        }
    }
}
