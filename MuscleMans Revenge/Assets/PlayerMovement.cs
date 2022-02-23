using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController controller;
    public Transform cam;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public float speed = 6f;

    public float gravity = -10f;
    public float jumpHeight = 2f;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    Vector3 fallvelocity;
    bool isGrounded;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && fallvelocity.y < 0)
        {
            fallvelocity.y = -2f;
        }

        

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        fallvelocity.y += gravity * Time.deltaTime;

        controller.Move(fallvelocity * Time.deltaTime);

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            controller.Move(moveDir.normalized * speed * Time.deltaTime); 
            
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            fallvelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        if (Input.GetButtonDown("Sprint"))
        {
            speed = 9f;
        }
        if (Input.GetButtonUp("Sprint"))
        {
            speed = 6f;
        }

        
    }
}
