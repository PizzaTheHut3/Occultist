using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    CharacterController controller;
    Vector3 input, moveDirection;
    public GameObject fireball;
    public float speed = 8f;
    private float finalSpeed;
    public int health = 100;
    public float jumpHeight = 4f;
    public float knockback = 2f;
    public float gravity = 40f;
    public float airControl = 3;
    public int numAirJumps = 1;
    public float blinkDistance = 10f;
    public AudioClip blinkSFX;
    public AudioClip jumpSFX;
    public AudioClip walkSFX;
    public AudioClip timeSlowSFX;
    public AudioClip deathSFX;
    public AudioClip hitSFX;
    private int airJumpsLeft;
    private bool isJumpPressed;
    private bool isBlinkPressed;
    private bool isBlinkActive = false;
    private bool isFireballPressed;
    public float timeScaleSpeed = 0.001f;
    public float minTimeScale = 0.2f;
    private float fixedDeltaTime;
    private float blinkStep = 1.3f;
    private int numBlinkSteps;
    private int currentBlinkStep = 0;
    private Vector3 blinkDirection = Vector3.forward;
    private Slider slider;
    private Image crosshairImage;
    private Color crosshairColor;
    private bool grounded;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        airJumpsLeft = numAirJumps;
        fixedDeltaTime = Time.fixedDeltaTime;
        finalSpeed = speed;
        crosshairImage = GameObject.Find("Crosshair").GetComponent<Image>();
        crosshairColor = crosshairImage.color;
        grounded = false;
        numBlinkSteps = (int) (blinkDistance/blinkStep);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJumpPressed = true;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumpPressed = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isBlinkPressed = true;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            isFireballPressed = true;
        }
        if (controller.isGrounded)
        {
            grounded = true;
        }
        else 
        {
            grounded = false;
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            AudioSource.PlayClipAtPoint(timeSlowSFX, transform.position);
            if (Time.timeScale > minTimeScale)
            {
                Time.timeScale -= timeScaleSpeed;
            }
            else
            {
                Time.timeScale = minTimeScale;
            }
        }
        else
        {
            if (Time.timeScale < 1f)
            {
                Time.timeScale += timeScaleSpeed;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }
        // Adjust fixed delta time according to timescale
        Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
        finalSpeed = speed * (1f / Time.timeScale);

        // handles crosshair change on enemy acquired
        CrosshairChange();
    }

    void FixedUpdate()
    {
        // gets user movement input
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");
        input = finalSpeed * (transform.right * moveHorizontal + transform.forward * moveVertical).normalized;

        // handles jumping and double jumping
        if (grounded)
        {
            moveDirection = input;
            airJumpsLeft = numAirJumps;
            if (isJumpPressed)
            {
                isJumpPressed = false;
                moveDirection.y = Mathf.Sqrt(2 * jumpHeight * gravity);
                AudioSource.PlayClipAtPoint(jumpSFX, transform.position);
            }
            else
            {
                moveDirection.y = 0f;
            }
        }
        else
        {
            if (isJumpPressed && airJumpsLeft > 0)
            { //handles double jumping
                isJumpPressed = false;
                moveDirection.y = Mathf.Sqrt(2 * jumpHeight * gravity);
                airJumpsLeft--;
                AudioSource.PlayClipAtPoint(jumpSFX, transform.position);
            }
            else
            {
                input.y = moveDirection.y;
                moveDirection = Vector3.Lerp(moveDirection, input, airControl * Time.deltaTime);
            }
        }

        // handles blinking. Currently player blinks in the direction they are moving and not the direction the are facing.
        // This means that blinking is always horizontal.
        if (isBlinkPressed || isBlinkActive)
        {
            if(!isBlinkActive) 
            {
                AudioSource.PlayClipAtPoint(blinkSFX, transform.position);
                isBlinkActive = true;
                isBlinkPressed = false;
                if(moveHorizontal == 0 && moveVertical == 0)
                {
                    blinkDirection = transform.forward;
                }
                else
                {
                    blinkDirection = (transform.right * moveHorizontal + transform.forward * moveVertical).normalized;
                }
            }
            if(currentBlinkStep == numBlinkSteps) 
            {
                isBlinkActive = false;
                currentBlinkStep = 0;
            }
            else
            {
                controller.Move(blinkStep * blinkDirection);
                currentBlinkStep++;
            }
        }

        // handles shooting fireball. 
        if (isFireballPressed)
        {
            isFireballPressed = false;
            // camera = transform.GetComponent<Camera>();
            // GameObject cam = FindGameObjectWithTag("MainCamera");
            Instantiate(fireball, transform.position + (transform.forward * 1f) + new Vector3(0f, 1.2f, 0f), Camera.main.transform.rotation);
        }

        // applies gravity and moves player
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }

    void CrosshairChange()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject.tag == "MeleeEnemy" || hit.collider.gameObject.tag == "Enemy")
            {
                crosshairImage.color = Color.Lerp(crosshairImage.color, Color.red, Time.deltaTime * 5);
                crosshairImage.transform.localScale = Vector3.Lerp(crosshairImage.transform.localScale, new Vector3(.7f, .7f, .7f), Time.deltaTime * 5);
            }
            else
            {
                crosshairImage.color = Color.Lerp(crosshairImage.color, crosshairColor, Time.deltaTime * 2);
                crosshairImage.transform.localScale = Vector3.Lerp(crosshairImage.transform.localScale, new Vector3(1, 1, 1), Time.deltaTime * 5);
            }
        }
        else
        {
            crosshairImage.color = Color.Lerp(crosshairImage.color, crosshairColor, Time.deltaTime * 2);
            crosshairImage.transform.localScale = Vector3.Lerp(crosshairImage.transform.localScale, new Vector3(1, 1, 1), Time.deltaTime * 5);
        }
    }

    void OnCollisionEnter(Collision hit)
    {
        if (hit.gameObject.tag == "MeleeEnemy")
        {
            AudioSource.PlayClipAtPoint(hitSFX, transform.position);
            health -= 15;
            Debug.Log("Player health: " + health);
        }
        if (hit.gameObject.tag == "Fireball")
        {
            health -= 20;
            Debug.Log("Player health: " + health);
        }
        if (health <= 0)
        {
            AudioSource.PlayClipAtPoint(deathSFX, transform.position);
            LevelManager.isGameOver = true;
            Invoke("PlayerDie", 1f);
        }
        slider = GameObject.Find("HealthBar").GetComponent<Slider>();
        slider.value = health;
    }

    void OnTriggerEnter(Collider hit)
    {
        if (hit.gameObject.tag == "Lava")
        {
            AudioSource.PlayClipAtPoint(deathSFX, transform.position);
            LevelManager.isGameOver = true;
            Invoke("PlayerDie", 1f);
        }
    }

    // Used to manually set if the player in grounded for moving platforms
    public void SetGrounded(bool g) 
    {
        grounded = g;
    }

    void PlayerDie()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
