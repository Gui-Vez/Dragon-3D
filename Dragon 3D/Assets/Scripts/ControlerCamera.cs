using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlerCamera : MonoBehaviour
{
    public Transform cible;
    public float rotationSpeed = 1.5f;

    private float _yaw = 20f;
    private float _pitch = 0f;

    private void Start()
    {
        transform.LookAt(cible);
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        _yaw += horizontal * rotationSpeed * -1;
        _pitch += vertical * rotationSpeed * -1;
        _pitch = Mathf.Clamp(_pitch, -40f, 10f);

        transform.position = cible.position;
        transform.rotation = Quaternion.Euler(_pitch, _yaw, 0f);
    }
}
