using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movment : MonoBehaviour
{
   



    [SerializeField] Transform playerCamera;
    [SerializeField] float mouseSensitivity = 3.5f;
    [SerializeField] float walkSpeed = 6.0f;
    [SerializeField] float gravity = -13.0f;
    [SerializeField] float jumpHeight = 3f;
    [SerializeField][Range(0.01f, 0.5f)] float smoothTime = 0.03f;
    [SerializeField] bool lockCursor = true;

    float cameraPitch = 0.0f;
    CharacterController controller;
    Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Update()
    {
        UpdateMouseLook();
        UpdateMovement();
    }

    void UpdateMouseLook()
    {
        Vector2 currentMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        cameraPitch -= currentMouseDelta.y * mouseSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 0.0f);

        // 2-rotate camera up and down around x-axis and remember that camera is inside the player, so we need to rotate it locally
        playerCamera.localEulerAngles = new Vector3(cameraPitch, 0.0f, 0.0f);

        // 1-rotating the player body around Y-axis
        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
    }

    void UpdateMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (controller.isGrounded)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;

        if (controller.isGrounded && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // move the character
        Vector3 move = transform.right * horizontal + transform.forward * vertical + Vector3.up * velocity.y;
        controller.Move(move * Time.deltaTime * walkSpeed);
    }
}

