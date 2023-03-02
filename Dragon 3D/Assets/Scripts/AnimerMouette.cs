using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimerMouette : MonoBehaviour
{
    public int indexAleatoireDeplacement;
    public int indexAleatoireAttaque;
    public bool attaque;

    private int i_deplacement;
    private int i_attaque;
    private bool t_attaque;

    private Animator Animator;

    public float dureeRembobinageAnimations = 0.25f;
    private bool coroutineActivee = false;


    void Start()
    {
        Animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!coroutineActivee)
        {
            StartCoroutine(GestionAnimations("Déplacement"));
        }
    }

    public IEnumerator GestionAnimations(string animation)
    {
        coroutineActivee = true;

        switch (animation)
        {
            case "Déplacement":

                indexAleatoireDeplacement = Random.Range(0, 10);

                i_deplacement = Animator.GetInteger("i_deplacement");
                Animator.SetInteger("i_deplacement", indexAleatoireDeplacement);

                break;

            case "Attaque":

                indexAleatoireAttaque = Random.Range(0, 2);

                i_attaque = Animator.GetInteger("i_attaque");
                Animator.SetInteger("i_attaque", indexAleatoireAttaque);
                Animator.SetTrigger("t_attaque");

                break;
        }

        yield return new WaitForSeconds(dureeRembobinageAnimations);

        coroutineActivee = false;
    }
}
