using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    CharacterController controller;
    Vector3 input, moveDirection;
    public GameObject fireball;
    public float speed = 8f;
    private float finalSpeed;
    public float jumpHeight = 4f;
    public float gravity = 40f;
    public float airControl = 3;
    public int numAirJumps = 1; 
    public float blinkDistance = 10f;
    private int airJumpsLeft;
    private bool isJumpPressed;
    private bool isBlinkPressed;
    private bool isFireballPressed;
    public float timeScaleSpeed = 0.001f;
    public float minTimeScale = 0.2f;
    private float fixedDeltaTime;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        airJumpsLeft = numAirJumps;
        fixedDeltaTime = Time.fixedDeltaTime;
        finalSpeed = speed;
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
        if (Input.GetKeyDown(KeyCode.E)) {
            isFireballPressed = true;
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (Time.timeScale > minTimeScale) {
                Time.timeScale -= timeScaleSpeed;
            } else {
                Time.timeScale = minTimeScale;
            }
        } else {
            if (Time.timeScale < 1f) {
                Time.timeScale += timeScaleSpeed;
            } else {
                Time.timeScale = 1f;
            }
        }
        // Adjust fixed delta time according to timescale
        Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
        finalSpeed = speed * (1f / Time.timeScale);
    }

    void FixedUpdate() {
        // gets user movement input
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");
        input = finalSpeed * (transform.right * moveHorizontal + transform.forward * moveVertical).normalized;

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

        // handles shooting fireball. 
        if(isFireballPressed) {
            isFireballPressed = false;
            // camera = transform.GetComponent<Camera>();
            // GameObject cam = FindGameObjectWithTag("MainCamera");
            Instantiate(fireball, transform.position + new Vector3(0f, 1.2f, 0f), Camera.main.transform.rotation);
        }

        // applies gravity and moves player
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }
}
