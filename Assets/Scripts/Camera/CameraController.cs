using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private string mouseXAxis = "Mouse X";
    [SerializeField] private string mouseYAxis = "Mouse Y";

    [Header("Sensitivity")]
    [SerializeField] private float yawSensitivity = 1.0f;
    [SerializeField] private float pitchSensitivity = 0.8f;

    private Transform player;
    private float pitch;

    void Start()
    {
        player = transform.root;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float x = Input.GetAxisRaw(mouseXAxis) * yawSensitivity;
        float y = Input.GetAxisRaw(mouseYAxis) * pitchSensitivity;

        pitch -= y;
        pitch = Mathf.Clamp(pitch, -90.0f, 90.0f);

        transform.localRotation = Quaternion.Euler(pitch, 0.0f, 0.0f);
        player.Rotate(Vector3.up * x);
    }
}
