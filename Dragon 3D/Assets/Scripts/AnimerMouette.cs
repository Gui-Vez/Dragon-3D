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
        // Obtenir la composante Animator attachée à cet objet
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
            // Démarrer la coroutine GestionAnimations avec l'animation "Déplacement"
            StartCoroutine(GestionAnimations("Déplacement"));
        }
    }

    public IEnumerator GestionAnimations(string animation)
    {
        // Activer la coroutine
        coroutineActivee = true;

        // Sélectionner l'animation à jouer en fonction du paramètre
        switch (animation)
        {
            // Animation de déplacement
            case "Déplacement":

                // Sélectionner un index aléatoire pour la séquence de déplacement
                indexAleatoireDeplacement = Random.Range(0, 10);

                // Obtenir la valeur actuelle de i_deplacement dans l'Animator et la remplacer par l'index aléatoire
                i_deplacement = Animator.GetInteger("i_deplacement");
                Animator.SetInteger("i_deplacement", indexAleatoireDeplacement);

                break;

            // Animation d'attaque
            case "Attaque":

                // Sélectionner un index aléatoire pour la séquence d'attaque
                indexAleatoireAttaque = Random.Range(0, 2);

                // Obtenir la valeur actuelle de i_attaque dans l'Animator et la remplacer par l'index aléatoire
                i_attaque = Animator.GetInteger("i_attaque");
                Animator.SetInteger("i_attaque", indexAleatoireAttaque);

                // Déclencher le déclencheur de l'animation d'attaque
                Animator.SetTrigger("t_attaque");

                // Jouer l'effet sonore correspondant à l'index d'attaque
                GererAudio.JouerEffetSonore("Seagull attack 0" + (indexAleatoireAttaque + 1).ToString());

                break;
        }

        // Attendre la durée de rembobinage d'animations avant de désactiver la coroutine
        yield return new WaitForSeconds(dureeRembobinageAnimations);

        // Désactiver la coroutine
        coroutineActivee = false;
    }
}
