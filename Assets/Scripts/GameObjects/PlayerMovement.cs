using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController CharacterController;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public Transform GroundCheck;
    public float GroundDistance = 0.4f;

    private Vector3 velocity;
    private bool isGrounded;

    void Update()
    {
        if (!GameManager.Instance.IsWaitingForInputForText)
        {
            RaycastHit hit = new RaycastHit();
            isGrounded = Physics.SphereCast(GroundCheck.position, GroundDistance, Vector3.down, out hit, GroundDistance * 3, 9);

            if (isGrounded && velocity.y < 0)
                velocity.y = -2f;

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;

            CharacterController.Move(move * speed * Time.deltaTime);

            if (Input.GetButtonDown("Jump") && isGrounded)
                velocity.y = Mathf.Sqrt(-2f * gravity * jumpHeight);

            velocity.y += gravity * Time.deltaTime;

            CharacterController.Move(velocity * Time.deltaTime);
        }
    }
}
