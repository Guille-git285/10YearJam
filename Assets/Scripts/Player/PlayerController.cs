using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{

    [Header("Input")]
    [SerializeField] private string horizontalAxis = "Horizontal";
    [SerializeField] private string verticalAxis = "Vertical";
    [SerializeField] private string jumpButton = "Jump";

    [Header("Movement")]
    [SerializeField] private float walkSpeed = 5.8f;
    [SerializeField] private float groundAccel = 0.1f;
    [SerializeField] private float airAccel = 0.02f;

    [Header("Jumping")]
    [SerializeField] private float jumpHeight = 1.8f;
    [SerializeField] private float coyoteTime = 0.12f;
    [SerializeField] private float bufferTime = 0.1f;

    [Header("Gravity")]
    [SerializeField] private float gravityScale = 3.0f;
    [SerializeField] private float terminalVelocity = -120.0f;

    private CharacterController controller;
    private Vector3 targetMovement;
    private Vector3 actualMovement;
    private Vector3 velocity;
    private float acceleration;
    private float groundTime;
    private float jumpTime;
    private bool onGround;

    void Start()
    {
        enabled = TryGetComponent(out controller);
    }

    void Update()
    {
        onGround = controller.isGrounded;
        acceleration = onGround ? groundAccel : airAccel;

        float x = Input.GetAxisRaw(horizontalAxis);
        float z = Input.GetAxisRaw(verticalAxis);

        targetMovement = transform.right * x + transform.forward * z;
        actualMovement = Vector3.MoveTowards(actualMovement, targetMovement, acceleration);
        actualMovement = Vector3.ClampMagnitude(actualMovement, 1.0f);

        if (onGround)
        {
            velocity.y = -1.0f;
            controller.slopeLimit = 45.0f;
            controller.stepOffset = 0.3f;
            groundTime = coyoteTime;
        }
        else
        {
            if (velocity.y > terminalVelocity)
                velocity.y += Physics.gravity.y * gravityScale * Time.deltaTime;

            controller.slopeLimit = 90.0f;
            controller.stepOffset = 0.0f;
            groundTime -= Time.deltaTime;
        }

        if (Input.GetButtonDown(jumpButton))
            jumpTime = bufferTime;
        else
            jumpTime -= Time.deltaTime;

        if (groundTime > 0.0f && jumpTime > 0.0f)
        {
            groundTime = 0.0f;
            jumpTime = 0.0f;
            velocity.y = Mathf.Sqrt(jumpHeight * Mathf.Abs(Physics.gravity.y * gravityScale * 2.0f));
        }

        if (Physics.Raycast(transform.position, Vector3.up, 1.1f))
            velocity.y = -1.0f;

        if (actualMovement.magnitude > 0.0f)
            controller.Move(actualMovement * walkSpeed * Time.deltaTime);

        if (!onGround || (onGround && velocity.y != -1.0f))
            controller.Move(velocity * Time.deltaTime);
    }
}
