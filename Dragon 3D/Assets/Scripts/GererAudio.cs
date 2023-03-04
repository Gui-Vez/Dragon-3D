using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GererAudio : MonoBehaviour
{
    [System.Serializable]
    public struct AudioData
    {
        public string name;
        public AudioClip clip;
        public float volume;
    }

    public AudioData[] effetsSonores;
    public AudioData[] pistesMusicales;

    private Dictionary<string, AudioClip> effetsSonoresDict = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> pistesMusicalesDict = new Dictionary<string, AudioClip>();

    private AudioSource audioSourceEffetsSonores;
    private AudioSource audioSourceMusiques;

    public static string[] exceptionsChansons = { "Une piste musicale d'exception", "Game Over" };
    private string lastPlayedSong;

    void Start()
    {
        audioSourceMusiques = Camera.main.transform.GetChild(0).Find("AudioSource Musiques").GetComponent<AudioSource>();
        audioSourceEffetsSonores = Camera.main.transform.GetChild(0).Find("AudioSource SFX").GetComponent<AudioSource>();

        // Ajouter les effets sonores au dictionnaire
        foreach (AudioData audioData in effetsSonores)
            effetsSonoresDict[audioData.name] = audioData.clip;

        // Ajouter les pistes musicales au dictionnaire
        foreach (AudioData audioData in pistesMusicales)
            pistesMusicalesDict[audioData.name] = audioData.clip;
    }

    void Update()
    {
        // Si le clip audio a fini d'�tre jou� et qu'il ne joue pas,
        if ((audioSourceMusiques.clip != null && !audioSourceMusiques.isPlaying && audioSourceMusiques.time >= audioSourceMusiques.clip.length) ||
            audioSourceMusiques.clip == null || !audioSourceMusiques.isPlaying)
        {
            // Joue une musique al�atoire
            JouerMusiqueAleatoire(exceptionsChansons);
        }
    }

    public void JouerEffetSonore(string name)
    {
        // Si le nom de l'effet sonore existe dans le dictionnaire,
        if (effetsSonoresDict.ContainsKey(name))
        {
            // R�cup�rer le clip de l'effet sonore depuis le dictionnaire
            AudioClip clip = effetsSonoresDict[name];

            // Trouver le volume de l'effet sonore correspondant dans le tableau
            float volume = 0f;
            foreach (AudioData audioData in effetsSonores)
            {
                if (audioData.name == name)
                {
                    volume = audioData.volume;
                    break;
                }
            }

            // Jouer le clip de l'effet sonore avec le volume sp�cifi�
            audioSourceEffetsSonores.PlayOneShot(clip, volume);
        }

        // Sinon,
        else
            // Afficher un message d'avertissement dans la console
            Debug.LogWarning("Nom de l'effet sonore invalide : " + name);
    }

    public void JouerPisteMusicale(string name)
    {
        // Si le nom de la piste de musique existe dans le dictionnaire,
        if (pistesMusicalesDict.ContainsKey(name))
        {
            // R�cup�rer le clip de la piste de musique depuis le dictionnaire
            AudioClip clip = pistesMusicalesDict[name];

            // Trouver le volume de la piste de musique correspondante dans le tableau
            float volume = 0f;
            foreach (AudioData audioData in pistesMusicales)
            {
                if (audioData.name == name)
                {
                    volume = audioData.volume;
                    break;
                }
            }

            // Si la partie n'est pas termin�e,
            if (!GererScenes.partieTerminee)
            {
                // D�finir le clip de la piste de musique avec le volume sp�cifi� comme clip de l'audio source
                audioSourceMusiques.clip = clip;
                audioSourceMusiques.volume = volume;

                // Jouer la piste de musique
                audioSourceMusiques.Play();
            }
        }

        // Sinon,
        else
            // Afficher un message d'avertissement dans la console
            Debug.LogWarning("Nom de la piste de musique invalide : " + name);
    }

    public void JouerMusiqueAleatoire(string[] exceptions)
    {
        // Cr�er une liste de toutes les cl�s du dictionnaire de pistes musicales
        List<string> keys = new List<string>(pistesMusicalesDict.Keys);

        // Supprimer toutes les cl�s des pistes sp�cifi�es dans la liste d'exceptions
        for (int i = 0; i < exceptions.Length; i++)
        {
            if (pistesMusicalesDict.ContainsKey(exceptions[i]))
            {
                keys.Remove(exceptions[i]);
            }
        }

        // Retirer la derni�re chanson jou�e de la liste des cl�s
        if (!string.IsNullOrEmpty(lastPlayedSong))
        {
            keys.Remove(lastPlayedSong);
        }

        // Choisir al�atoirement une cl� parmi les cl�s restantes
        int index = Random.Range(0, keys.Count);
        string randomKey = keys[index];

        // Jouer la piste musicale correspondant � la cl� choisie
        JouerPisteMusicale(randomKey);

        // Sauvegarder la derni�re chanson jou�e
        lastPlayedSong = randomKey;
    }

    public IEnumerator GameOverRoutine()
    {
        // Gradually lower the music volume
        float startingVolume = audioSourceMusiques.volume;
        float fadeDuration = 2f;
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);
            audioSourceMusiques.volume = Mathf.Lerp(startingVolume, 0f, t);
            yield return null;
        }

        // Play game over sound effect
        JouerEffetSonore("Failure 02");
    }
}
