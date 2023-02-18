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
        transform.LookAt(_cible);
        _transformEnfant = transform.GetChild(0);
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float scroll = Input.GetAxis("Mouse ScrollWheel");


        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            sourisParDessusUI = true;
        }

        else
        {
            sourisParDessusUI = false;
        }

        if (Input.GetMouseButtonDown(0) && !sourisParDessusUI)
        {
            enRotation = true;
            dernierePositionSouris = Input.mousePosition;
            _pitchPrec = _pitch;
        }

        if (Input.GetMouseButtonUp(0))
        {
            enRotation = false;
        }

        if (enRotation && !sourisParDessusUI)
        {
            Vector3 nouvellePositionSouris = Input.mousePosition;
            Vector3 deplacementSouris = nouvellePositionSouris - dernierePositionSouris;
            dernierePositionSouris = nouvellePositionSouris;

            float rotationY =  deplacementSouris.x * sensitiviteDrag;
            float rotationX = -deplacementSouris.y * sensitiviteDrag;
            _yaw += rotationY;
            _pitch = Mathf.Clamp(_pitchPrec + rotationX, -maxPitchDrag, maxPitchDrag);
        }

        else
        {
            _yaw += horizontal * vitesseRotation * -1;
            _pitch += vertical * vitesseRotation * -1;
            _pitch = Mathf.Clamp(_pitch, minPitch, maxPitch);
        }


        _zoom += scroll * vitesseZoom * sensitiviteScroll * -1;
        _zoom = Mathf.Clamp(_zoom, distanceMinZoom, distanceMaxZoom);

        if (Input.GetKey(KeyCode.Q))
        {
            _zoom += vitesseZoom * Time.deltaTime * sensitiviteInputZoom;
            _zoom = Mathf.Clamp(_zoom, distanceMinZoom, distanceMaxZoom);
        }

        else if (Input.GetKey(KeyCode.E))
        {
            _zoom -= vitesseZoom * Time.deltaTime * sensitiviteInputZoom;
            _zoom = Mathf.Clamp(_zoom, distanceMinZoom, distanceMaxZoom);
        }

        transform.position = _cible.position;
        transform.rotation = Quaternion.Euler(_pitch, _yaw, 0f);


        Vector3 positionLocaleEnfant = _transformEnfant.localPosition;
        
        float smoothZoom = Mathf.SmoothDamp(_zoom, _zoom + scroll * vitesseZoom, ref velociteZoom, tempsZoom);
        _zoom = Mathf.Clamp(smoothZoom, distanceMinZoom, distanceMaxZoom);

        positionLocaleEnfant.z = Mathf.Clamp(_zoom, distanceMinZoom, distanceMaxZoom);
        _transformEnfant.localPosition = positionLocaleEnfant;
    }
}