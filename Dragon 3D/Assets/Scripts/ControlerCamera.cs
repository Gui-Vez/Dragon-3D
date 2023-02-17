using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlerCamera : MonoBehaviour
{
    public Transform _cible;
    private Transform _transformEnfant;

    public float rotationSpeed = 1.5f;
    public float vitesseZoom = 2f;
    public float distanceMinZoom = 3f;
    public float distanceMaxZoom = 5f;
    public float sensitiviteInputZoom = 2f;
    public float sensitiviteScroll = 0.5f;
    public float tempsZoom = 0.1f;

    private float _yaw = 20f;
    private float _pitch = 0f;
    private float _zoom = 5.0f;
    private float velociteZoom;

    private void Start()
    {
        transform.LookAt(_cible);
        _transformEnfant = transform.GetChild(0);
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        _yaw += horizontal * rotationSpeed * -1;
        _pitch += vertical * rotationSpeed * -1;
        _pitch = Mathf.Clamp(_pitch, -40f, 10f);

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