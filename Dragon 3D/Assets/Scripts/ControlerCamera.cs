using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ControlerCamera : MonoBehaviour
{
    public Transform _cible;
    private Transform _transformEnfant;

    public float vitesseRotation = 1.5f;
    public float vitesseZoom = 2f;
    public float vitesseDrag = 1f;
    public float distanceMinZoom = 3f;
    public float distanceMaxZoom = 5f;
    public float sensitiviteInputZoom = 2f;
    public float sensitiviteScroll = 0.5f;
    public float sensitiviteDrag = 0.1f;
    public float tempsZoom = 0.1f;
    public float minPitch = -40f;
    public float maxPitch = 10f;
    public float maxPitchDrag = 70f;

    private float _yaw = 20f;
    private float _pitch = 0f;
    private float _zoom = 5.0f;
    private float velociteZoom = 0f;
    private float _pitchPrec = 0f;
    private bool enRotation = false;
    private bool sourisParDessusUI = false;

    private Vector3 dernierePositionSouris;

    void Start()
    {
        // Récupère le transform de l'enfant de la caméra
        _transformEnfant = transform.GetChild(0);
    }

    void Update()
    {
        // Récupère la cible de la caméra à partir du personnage actuel
        _cible = GererAssetsDragon.personnageJoueurActuel.transform;

        // Oriente la caméra vers la cible
        transform.LookAt(_cible);

        // Récupère les entrées de l'utilisateur pour l'axe horizontal, vertical et le défilement de la souris
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        // Vérifie si la souris est sur l'interface utilisateur
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            sourisParDessusUI = true;

        // Sinon, la souris n'est pas par-dessus l'interface utilisateur
        else
            sourisParDessusUI = false;

        // Vérifie si l'utilisateur a appuyé sur le bouton gauche de la souris
        // et que la souris n'est pas sur l'interface utilisateur,
        if (Input.GetMouseButtonDown(0) && !sourisParDessusUI)
        {
            // Activer la rotation de la caméra
            enRotation = true;
            dernierePositionSouris = Input.mousePosition;
            _pitchPrec = _pitch;
        }

        // Si le bouton de la souris est relâché,
        if (Input.GetMouseButtonUp(0))
        {
            // arrête la rotation de la caméra
            enRotation = false;
        }

        // Si la caméra est en rotation
        // et que la souris n'est pas sur un élément IU,
        if (enRotation && !sourisParDessusUI)
        {

            // Déterminer la position de la souris
            Vector3 nouvellePositionSouris = Input.mousePosition;
            Vector3 deplacementSouris = nouvellePositionSouris - dernierePositionSouris;
            dernierePositionSouris = nouvellePositionSouris;

            // Déterminer la rotation de la souris
            float rotationY =  deplacementSouris.x * sensitiviteDrag;
            float rotationX = -deplacementSouris.y * sensitiviteDrag;
            _yaw += rotationY;
            _pitch = Mathf.Clamp(_pitchPrec + rotationX, -maxPitchDrag, maxPitchDrag);
        }

        // Sinon,
        else
        {
            // Attribuer les valeurs du lacet et du tangage par la vitesse rotationnelle
            _yaw += horizontal * vitesseRotation * -1;
            _pitch += vertical * vitesseRotation * -1;
            _pitch = Mathf.Clamp(_pitch, minPitch, maxPitch);
        }

        // Attribuer la valeur du zoom selon l'interaction avec le scroll
        _zoom += scroll * vitesseZoom * sensitiviteScroll * -1;
        _zoom = Mathf.Clamp(_zoom, distanceMinZoom, distanceMaxZoom);

        // Si l'on appuie sur Q,
        if (Input.GetKey(KeyCode.Q))
        {
            // Augmenter le zoom de la caméra
            _zoom += vitesseZoom * Time.deltaTime * sensitiviteInputZoom;
            _zoom = Mathf.Clamp(_zoom, distanceMinZoom, distanceMaxZoom);
        }
        
        // Si l'on appuie sur E,
        else if (Input.GetKey(KeyCode.E))
        {
            // Diminuer le zoom de la caméra
            _zoom -= vitesseZoom * Time.deltaTime * sensitiviteInputZoom;
            _zoom = Mathf.Clamp(_zoom, distanceMinZoom, distanceMaxZoom);
        }

        // Transformer les valeurs de position et de rotation èa celle de la cible
        transform.position = _cible.position;
        transform.rotation = Quaternion.Euler(_pitch, _yaw, 0f);

        // Trouver la position locale de l'enfant de l'objet
        Vector3 positionLocaleEnfant = _transformEnfant.localPosition;
        
        // Faire en sorte de rendre le zoom plus fluide
        float smoothZoom = Mathf.SmoothDamp(_zoom, _zoom + scroll * vitesseZoom, ref velociteZoom, tempsZoom);
        _zoom = Mathf.Clamp(smoothZoom, distanceMinZoom, distanceMaxZoom);

        // Changer la position Z locale de l'enfant objet
        positionLocaleEnfant.z = Mathf.Clamp(_zoom, distanceMinZoom, distanceMaxZoom);
        _transformEnfant.localPosition = positionLocaleEnfant;
    }
}