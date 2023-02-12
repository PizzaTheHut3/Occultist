using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    CharacterController controller;
    Vector3 input, moveDirection;
    public float speed = 8f;
    public float jumpHeight = 4f;
    public float gravity = 40f;
    public float airControl = 3;
    public int numAirJumps = 1; 
    public float blinkDistance = 10f;
    private int airJumpsLeft;
    private bool isJumpPressed;
    private bool isBlinkPressed;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        airJumpsLeft = numAirJumps;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            isJumpPressed = true;
        } 
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            isBlinkPressed = true;
        }
    }

    void FixedUpdate() {
        // gets user movement input
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");
        input = speed * (transform.right * moveHorizontal + transform.forward * moveVertical).normalized;

        // handles jumping and double jumping
        if(controller.isGrounded) {
            moveDirection = input;
            airJumpsLeft = numAirJumps;
            if(isJumpPressed) {
                isJumpPressed = false;
                moveDirection.y = Mathf.Sqrt(2 * jumpHeight * gravity);
            }
            else {
                moveDirection.y = 0f;
            }
        }
        else {
            if(isJumpPressed && airJumpsLeft > 0) { //handles double jumping
                isJumpPressed = false;
                moveDirection.y = Mathf.Sqrt(2 * jumpHeight * gravity);
                airJumpsLeft--;
            }
            else {
                input.y = moveDirection.y;
                moveDirection = Vector3.Lerp(moveDirection, input, airControl * Time.deltaTime);
            }
        }

        // handles blinking. Currently player blinks in the direction they are moving and not the direction the are facing.
        // This means that blinking is always horizontal.
        if(isBlinkPressed) {
            isBlinkPressed = false;
            controller.Move(blinkDistance * (transform.right * moveHorizontal + transform.forward * moveVertical).normalized);
        }

        // applies gravity and moves player
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }
}
