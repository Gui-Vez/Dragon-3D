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

    private GererAudio GererAudio;
    public GameObject contenantGererAudio;

    void Start()
    {
        // Obtenir la composante Animator attach�e � cet objet
        Animator = GetComponent<Animator>();

        // S'il y a un contenant de gestion d'audio,
        if (contenantGererAudio != null)
            // Obtenir la composante de script de cet objet
            GererAudio = contenantGererAudio.GetComponent<GererAudio>();
    }

    void Update()
    {
        // Si la coroutine n'est pas active,
        if (!coroutineActivee)
        {
            // D�marrer la coroutine GestionAnimations avec l'animation "D�placement"
            StartCoroutine(GestionAnimations("D�placement"));
        }
    }

    public IEnumerator GestionAnimations(string animation)
    {
        // Activer la coroutine
        coroutineActivee = true;

        // S�lectionner l'animation � jouer en fonction du param�tre
        switch (animation)
        {
            // Animation de d�placement
            case "D�placement":

                // S�lectionner un index al�atoire pour la s�quence de d�placement
                indexAleatoireDeplacement = Random.Range(0, 10);

                // Obtenir la valeur actuelle de i_deplacement dans l'Animator et la remplacer par l'index al�atoire
                i_deplacement = Animator.GetInteger("i_deplacement");
                Animator.SetInteger("i_deplacement", indexAleatoireDeplacement);

                break;

            // Animation d'attaque
            case "Attaque":

                // S�lectionner un index al�atoire pour la s�quence d'attaque
                indexAleatoireAttaque = Random.Range(0, 2);

                // Obtenir la valeur actuelle de i_attaque dans l'Animator et la remplacer par l'index al�atoire
                i_attaque = Animator.GetInteger("i_attaque");
                Animator.SetInteger("i_attaque", indexAleatoireAttaque);

                // D�clencher le d�clencheur de l'animation d'attaque
                Animator.SetTrigger("t_attaque");

                // Jouer l'effet sonore correspondant � l'index d'attaque
                GererAudio.JouerEffetSonore("Seagull attack 0" + (indexAleatoireAttaque + 1).ToString());

                break;
        }

        // Attendre la dur�e de rembobinage d'animations avant de d�sactiver la coroutine
        yield return new WaitForSeconds(dureeRembobinageAnimations);

        // D�sactiver la coroutine
        coroutineActivee = false;
    }
}
