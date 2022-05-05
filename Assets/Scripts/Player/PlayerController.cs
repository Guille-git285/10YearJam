using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{

    [Header("Input")]
    [SerializeField] private string horizontalAxis = "Horizontal";
    [SerializeField] private string verticalAxis = "Vertical";

    [Space]
    [SerializeField] private string jumpButton = "Jump";
    [SerializeField] private string runButton = "Run";
    [SerializeField] private string crouchButton = "Crouch";

    [Header("Movement")]
    [SerializeField] private float walkSpeed = 5.5f;
    [SerializeField] private float runSpeed = 8.5f;
    [SerializeField] private float crouchSpeed = 2.5f;
    [SerializeField] private float movementAccelOnGround = 0.1f;
    [SerializeField] private float movementAccelOffGround = 0.005f;

    [Space]
    [SerializeField] private bool canJumpWhenCrouched = false;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float coyoteTime = 0.12f;
    [SerializeField] private float bufferTime = 0.12f;

    [Space]
    [SerializeField] private float gravityScale = 3.0f;
    [SerializeField] private float terminalVelocity = -80.0f;

    [Header("Tweening")]
    [SerializeField] private float standingHeight = 2.0f;
    [SerializeField] private float crouchedHeight = 1.0f;

    [Space]
    [SerializeField] private float crouchTime = 0.3f;
    [SerializeField] private float uncrouchTime = 0.3f;

    [Space]
    [SerializeField] private LeanTweenType crouchType = LeanTweenType.easeOutQuart;
    [SerializeField] private LeanTweenType uncrouchType = LeanTweenType.easeOutQuart;

    [Header("Permissions")]
    public bool canWalk = true;
    public bool canRun = true;
    public bool canCrouch = true;
    public bool canJump = true;

    private CharacterController controller;
    private CameraController camController;
    private Vector3 targetDirection;
    private Vector3 currentDirection;
    private Vector3 currentVelocity;
    private float lastTimeOnGround;
    private float jumpPressedTime;
    private float controllerGravity;
    private float controllerSpeed;
    private float currentAcceleration;
    private bool isRunning;
    private bool isCrouching;
    private bool isOnGround;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        camController = GetComponentInChildren<CameraController>();

        if (!controller || !camController)
        {
            enabled = false;
            return;
        }

        controllerSpeed = walkSpeed;

        canWalk = true;
        canRun = true;
        canCrouch = true;
        canJump = true;
    }

    void Update()
    {
        isOnGround = controller.isGrounded;

        currentAcceleration = isOnGround ? movementAccelOnGround : movementAccelOffGround;
        targetDirection.x = Mathf.MoveTowards(targetDirection.x, Input.GetAxisRaw(horizontalAxis), currentAcceleration);
        targetDirection.z = Mathf.MoveTowards(targetDirection.z, Input.GetAxisRaw(verticalAxis), currentAcceleration);

        currentDirection = transform.right * targetDirection.x + transform.forward * targetDirection.z;
        currentDirection = Vector3.ClampMagnitude(currentDirection, 1.0f);

        controllerGravity = Physics.gravity.y * gravityScale;

        if (isOnGround)
        {
            currentVelocity.y = -2.0f;
            controller.slopeLimit = 45.0f;
            controller.stepOffset = 0.5f;

            lastTimeOnGround = coyoteTime;
        }
        else
        {
            controller.slopeLimit = 90.0f;
            controller.stepOffset = 0.0f;

            currentVelocity.y += controllerGravity * Time.deltaTime;
            currentVelocity.y = Mathf.Clamp(currentVelocity.y, terminalVelocity, -terminalVelocity);

            lastTimeOnGround -= Time.deltaTime;
        }

        if (Physics.Raycast(transform.position, transform.up, (controller.height / 2.0f) + 0.1f) && currentVelocity.y > 0.0f)
        {
            currentVelocity.y = -2.0f;
        }

        if (Input.GetButtonDown(jumpButton))
        {
            jumpPressedTime = bufferTime;
        }
        else
        {
            jumpPressedTime -= Time.deltaTime;
        }

        if (Input.GetButton(runButton))
        {
            StartRunning();
        }

        if (Input.GetButtonUp(runButton))
        {
            StopRunning();
        }

        if (Input.GetButtonDown(crouchButton))
        {
            StartCrouching();
        }

        if (Input.GetButtonUp(crouchButton))
        {
            StopCrouching();
        }

        if (jumpPressedTime > 0.0f && lastTimeOnGround > 0.0f)
        {
            Jump();
        }

        if (currentDirection.magnitude > 0.0f && canWalk)
        {
            controller.Move(currentDirection * controllerSpeed * Time.deltaTime);
        }

        controller.Move(currentVelocity * Time.deltaTime);
    }

    void Jump()
    {
        if (isCrouching && !canJumpWhenCrouched)
        {
            StopCrouching();
            jumpPressedTime = 0.0f;
            lastTimeOnGround = 0.0f;
            return;
        }

        if (canJump)
        {
            lastTimeOnGround = 0.0f;
            jumpPressedTime = 0.0f;
            currentVelocity.y = Mathf.Sqrt(jumpHeight * Mathf.Abs(controllerGravity * 2.0f));
        }
    }

    void StartRunning()
    {
        if (isRunning || isCrouching || !canRun ||
            currentDirection.magnitude == 0.0f || targetDirection.z <= 0.0f)
        {
            return;
        }

        isRunning = true;
        controllerSpeed = runSpeed;
    }

    void StopRunning()
    {
        if (isRunning)
        {
            isRunning = false;
            controllerSpeed = walkSpeed;
        }
    }

    void StartCrouching()
    {
        if (!canCrouch)
        {
            return;
        }

        if (isRunning)
        {
            StopRunning();
        }

        isCrouching = true;
        controllerSpeed = crouchSpeed;

        LeanTween.value(gameObject, controller.height, crouchedHeight, crouchTime)
            .setEase(crouchType).setOnUpdate((float height) =>
            {
                camController.transform.localPosition = new Vector3(0.0f, height * 0.4f, 0.0f);
                controller.height = height;
            });
    }

    void StopCrouching()
    {
        if (Physics.Raycast(transform.position, transform.up, controller.height * 1.5f))
        {
            return;
        }

        if (isCrouching)
        {
            isCrouching = false;
            controllerSpeed = walkSpeed;

            LeanTween.value(gameObject, controller.height, standingHeight, uncrouchTime)
                .setEase(uncrouchType).setOnUpdate((float height) =>
                {
                    camController.transform.localPosition = new Vector3(0.0f, height * 0.4f, 0.0f);
                    controller.height = height;
                });
        }
    }
}
