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
        // Le score précédent est initialisé avec le score actuel
        scorePrecedent = scoreActuel;

        // S'il y a un contenant de gestion d'audio,
        if (contenantGererAudio != null)
            // Obtenir la composante de script de cet objet
            GererAudio = contenantGererAudio.GetComponent<GererAudio>();

        GererAudio.JouerMusiqueAleatoire(GererAudio.exceptionsChansons);
    }

    void Update()
    {
        // Si le score a changé depuis la dernière frame,
        if (scorePrecedent != scoreActuel)
        {
            // On affiche le score
            AfficherScore();

            // On met à jour le score précédent
            scorePrecedent = scoreActuel;
        }

        // On ajoute le temps écoulé depuis la dernière frame au temps écoulé depuis la dernière mise à jour
        tempsDepuisDernierUpdate += Time.deltaTime;

        // Si le temps écoulé depuis la dernière mise à jour est supérieur ou égal au temps de chargement pour une nouvelle mise à jour,
        if (tempsDepuisDernierUpdate >= tempsChargementUpdate)
        {
            // On lance la coroutine EtablirScoreFruits()
            StartCoroutine(EtablirScoreFruits());

            // On réinitialise le temps écoulé depuis la dernière mise à jour
            tempsDepuisDernierUpdate = 0f;
        }
    }

    void AfficherScore()
    {
        // Si le score actuel est suppérieur à sa valeur maximale,
        if (scoreActuel > scoreMaximal)
            // Attribuer le score actuel à sa valeur maximale 
            scoreActuel = scoreMaximal;

        // Si le score actuel est inférieur à sa valeur minimale,
        if (scoreActuel < scoreMinimal)
            // Attribuer le score actuel à sa valeur minimale
            scoreActuel = scoreMinimal;

        // Appliquer le texte UI du score en cours
        EntreeScore.GetComponent<TMP_Text>().text = scoreActuel.ToString();

        // Changer le nom de l'objet contenant le texte de score
        EntreeScore.name = scoreActuel.ToString();
    }

    IEnumerator EtablirScoreFruits()
    {
        // Si le tableau de valeurs de pointage des fruits est plus petit que le nombre de fruits dans la liste de fruits générés,
        if (tableauValeursPointageFruits.Length < GenerationObjet.listeFruits.Count)
        {
            // On crée un nouveau tableau avec la taille correspondante
            tableauValeursPointageFruits = new int[GenerationObjet.listeFruits.Count];

            // Pour chaque fruit dans la liste de fruits générés,
            for (int i = 0; i < GenerationObjet.listeFruits.Count; i++)
            {
                // On récupère son nom et sa valeur de pointage
                string nomFruit = GenerationObjet.listeFruits[i].transform.GetChild(0).name;

                // Si le fruit n'a pas encore de valeur de pointage attribuée,
                if (!pointagesFruits.TryGetValue(nomFruit, out int pointage))
                {
                    // On lui attribue la valeur par défaut
                    pointagesFruits.Add(nomFruit, valeurFruitDefaut);
                    pointage = valeurFruitDefaut;
                }

                // On ajoute la valeur de pointage du fruit au tableau de valeurs de pointage des fruits
                tableauValeursPointageFruits[i] = pointage;
            }
        }

        // Pour chaque fruit dans la liste de fruits générés,
        for (int i = 0; i < GenerationObjet.listeFruits.Count; i++)
        {
            // On récupère son nom
            string nomFruit = GenerationObjet.listeFruits[i].transform.GetChild(0).name;

            // Si le fruit n'a pas encore de valeur de pointage attribuée,
            if (!pointagesFruits.TryGetValue(nomFruit, out int pointage))
            {
                // On lui attribue la valeur par défaut
                pointagesFruits.Add(nomFruit, valeurFruitDefaut);
                pointage = valeurFruitDefaut;
            }

            // On met à jour la valeur de pointage du fruit dans le dictionnaire de pointages des fruits
            pointagesFruits[nomFruit] = tableauValeursPointageFruits[i];
        }

        yield return null;
    }
}
