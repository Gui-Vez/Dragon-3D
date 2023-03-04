using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GererScore : MonoBehaviour
{
    [SerializeField]
    public static int scoreActuel = 0;
    private int scorePrecedent;
    private int scoreMinimal = 0;
    private int scoreMaximal = 999999;

    public int[] tableauValeursPointageFruits;
    public static Dictionary<string, int> pointagesFruits = new Dictionary<string, int>()
    {
        {"apple"     , 20},
        {"pear"      , 40},
        {"banana"    , 60},
        {"cherries"  , 80},
        {"lemon"     , 100},
        {"peach"     , 80},
        {"strawberry", 60},
        {"watermelon", 40},
        {"avocado"   , 20}
    };

    public int valeurFruitDefaut = 10;

    private float tempsDepuisDernierUpdate = 0f;
    private float tempsChargementUpdate = 1.5f;

    public GameObject EntreeScore;

    private GererAudio GererAudio;
    public GameObject contenantGererAudio;


    void Start()
    {
        // Le score pr�c�dent est initialis� avec le score actuel
        scorePrecedent = scoreActuel;

        // S'il y a un contenant de gestion d'audio,
        if (contenantGererAudio != null)
            // Obtenir la composante de script de cet objet
            GererAudio = contenantGererAudio.GetComponent<GererAudio>();

        GererAudio.JouerMusiqueAleatoire(GererAudio.exceptionsChansons);
    }

    void Update()
    {
        // Si le score a chang� depuis la derni�re frame,
        if (scorePrecedent != scoreActuel)
        {
            // On affiche le score
            AfficherScore();

            // On met � jour le score pr�c�dent
            scorePrecedent = scoreActuel;
        }

        // On ajoute le temps �coul� depuis la derni�re frame au temps �coul� depuis la derni�re mise � jour
        tempsDepuisDernierUpdate += Time.deltaTime;

        // Si le temps �coul� depuis la derni�re mise � jour est sup�rieur ou �gal au temps de chargement pour une nouvelle mise � jour,
        if (tempsDepuisDernierUpdate >= tempsChargementUpdate)
        {
            // On lance la coroutine EtablirScoreFruits()
            StartCoroutine(EtablirScoreFruits());

            // On r�initialise le temps �coul� depuis la derni�re mise � jour
            tempsDepuisDernierUpdate = 0f;
        }
    }

    void AfficherScore()
    {
        // Si le score actuel est supp�rieur � sa valeur maximale,
        if (scoreActuel > scoreMaximal)
            // Attribuer le score actuel � sa valeur maximale 
            scoreActuel = scoreMaximal;

        // Si le score actuel est inf�rieur � sa valeur minimale,
        if (scoreActuel < scoreMinimal)
            // Attribuer le score actuel � sa valeur minimale
            scoreActuel = scoreMinimal;

        // Appliquer le texte UI du score en cours
        EntreeScore.GetComponent<TMP_Text>().text = scoreActuel.ToString();

        // Changer le nom de l'objet contenant le texte de score
        EntreeScore.name = scoreActuel.ToString();
    }

    IEnumerator EtablirScoreFruits()
    {
        // Si le tableau de valeurs de pointage des fruits est plus petit que le nombre de fruits dans la liste de fruits g�n�r�s,
        if (tableauValeursPointageFruits.Length < GenerationObjet.listeFruits.Count)
        {
            // On cr�e un nouveau tableau avec la taille correspondante
            tableauValeursPointageFruits = new int[GenerationObjet.listeFruits.Count];

            // Pour chaque fruit dans la liste de fruits g�n�r�s,
            for (int i = 0; i < GenerationObjet.listeFruits.Count; i++)
            {
                // On r�cup�re son nom et sa valeur de pointage
                string nomFruit = GenerationObjet.listeFruits[i].transform.GetChild(0).name;

                // Si le fruit n'a pas encore de valeur de pointage attribu�e,
                if (!pointagesFruits.TryGetValue(nomFruit, out int pointage))
                {
                    // On lui attribue la valeur par d�faut
                    pointagesFruits.Add(nomFruit, valeurFruitDefaut);
                    pointage = valeurFruitDefaut;
                }

                // On ajoute la valeur de pointage du fruit au tableau de valeurs de pointage des fruits
                tableauValeursPointageFruits[i] = pointage;
            }
        }

        // Pour chaque fruit dans la liste de fruits g�n�r�s,
        for (int i = 0; i < GenerationObjet.listeFruits.Count; i++)
        {
            // On r�cup�re son nom
            string nomFruit = GenerationObjet.listeFruits[i].transform.GetChild(0).name;

            // Si le fruit n'a pas encore de valeur de pointage attribu�e,
            if (!pointagesFruits.TryGetValue(nomFruit, out int pointage))
            {
                // On lui attribue la valeur par d�faut
                pointagesFruits.Add(nomFruit, valeurFruitDefaut);
                pointage = valeurFruitDefaut;
            }

            // On met � jour la valeur de pointage du fruit dans le dictionnaire de pointages des fruits
            pointagesFruits[nomFruit] = tableauValeursPointageFruits[i];
        }

        yield return null;
    }
}
