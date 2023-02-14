using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeplacementMouette : MonoBehaviour
{
    public float vitesseDeplacement = 5.0f;

    void Update()
    {
        transform.position += transform.forward * vitesseDeplacement * Time.deltaTime;
    }
}
