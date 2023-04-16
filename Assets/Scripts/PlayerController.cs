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
    public int health = 100;
    public bool hasBlink = true;
    public bool hasSuperJump = false;
    public float jumpHeight = 4f;
    public float superJumpHeight = 20f;
    public float knockback = 2f;
    public float gravity = 40f;
    public float airControl = 3;
    public int numAirJumps = 1;
    public float blinkDistance = 10f;
    public AudioClip blinkSFX;
    public AudioClip jumpSFX;
    public AudioClip walkSFX;
    public AudioClip deathSFX;
    public AudioClip hitSFX;
    public AudioClip soulCollectSFX;
    private int airJumpsLeft;
    private bool isJumpPressed;
    private bool isBlinkPressed;
    private bool isBlinkActive = false;
    private bool isFireballPressed;
    private int fireballCountdown;
    public int fireballCooldown = 60;
    private float blinkStep = 1.3f;
    private int numBlinkSteps;
    private int currentBlinkStep = 0;
    private Vector3 blinkDirection = Vector3.forward;
    private Slider slider;
    private Image crosshairImage;
    private Color crosshairColor;
    private GameObject blinkUI;
    public GameObject superJumpUI;

    //bullet time
    public AudioClip timeSlowSFX;
    public AudioClip timeSpeedupSFX;
    public float timeScaleSpeed = 0.001f;
    public float bulletTimeDrainSpeed = 0.001f;
    public float minTimeScale = 0.2f;
    float bulletTimeMeterPerTick;
    float finalSpeedMultiplier = 1f;
    float maxBulletTime;
    float bulletTime;
    float originalFixedDeltaTime;
    Slider bulletTimeUI;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        airJumpsLeft = numAirJumps;
        crosshairImage = GameObject.Find("Crosshair").GetComponent<Image>();
        crosshairColor = crosshairImage.color;
        hasBlink = true;
        blinkUI = GameObject.Find("Blink");
        numBlinkSteps = (int)(blinkDistance / blinkStep);

        //bullettime
        originalFixedDeltaTime = Time.fixedDeltaTime;
        bulletTimeUI = GameObject.Find("BulletTime").GetComponent<Slider>();
        maxBulletTime = bulletTimeUI.maxValue;
        bulletTime = maxBulletTime;
        bulletTimeUI.value = maxBulletTime;
        bulletTimeMeterPerTick = bulletTimeDrainSpeed * maxBulletTime;

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
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isBlinkPressed = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            isFireballPressed = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isFireballPressed = false;
        }

        if (Input.GetKey(KeyCode.Mouse1) && bulletTime > 0)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                AudioSource.PlayClipAtPoint(timeSlowSFX, transform.position);
            }
            bulletTime -= 1;
            if (bulletTime <= 0)
            {
                AudioSource.PlayClipAtPoint(timeSpeedupSFX, transform.position);
            }
            bulletTimeUI.value = bulletTime;
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
            if (Input.GetKeyUp(KeyCode.LeftControl) && bulletTime > 0)
            {
                AudioSource.PlayClipAtPoint(timeSpeedupSFX, transform.position);
            }
            if (Time.timeScale < 1f)
            {
                Time.timeScale += timeScaleSpeed;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }

        if (fireballCountdown > 0)
        {
            fireballCountdown -= 1;
        }

        if (hasBlink)
        {
            blinkUI.SetActive(true);
        }
        else
        {
            blinkUI.SetActive(false);
        }

        if (hasSuperJump)
        {
            superJumpUI.SetActive(true);
        }
        else
        {
            superJumpUI.SetActive(false);
        }

        BulletTime();

        // handles crosshair change on enemy acquired
        CrosshairChange();
    }

    void FixedUpdate()
    {
        if (LevelManager.isGameOver)
        {
            return;
        }
        // gets user movement input
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");
        input = speed * (transform.right * moveHorizontal + transform.forward * moveVertical).normalized;

        // handles jumping and double jumping
        if (controller.isGrounded)
        {
            moveDirection = input;
            airJumpsLeft = numAirJumps;
            if (hasSuperJump && isJumpPressed)
            {
                isJumpPressed = false;
                hasSuperJump = false;
                moveDirection.y = Mathf.Sqrt(2 * superJumpHeight * gravity);
                AudioSource.PlayClipAtPoint(jumpSFX, transform.position);
            }
            else if (isJumpPressed)
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
                moveDirection = Vector3.Lerp(moveDirection, input, airControl * Time.deltaTime * finalSpeedMultiplier);
            }
        }

        // handles blinking. Currently player blinks in the direction they are moving and not the direction the are facing.
        // This means that blinking is always horizontal.
        // if (isBlinkPressed && hasBlink)
        // {
        //     isBlinkPressed = false;
        //     hasBlink = false;
        //     controller.Move(blinkDistance * (transform.right * moveHorizontal + transform.forward * moveVertical).normalized);
        //     AudioSource.PlayClipAtPoint(blinkSFX, transform.position);
        // }

        if ((isBlinkPressed && hasBlink) || isBlinkActive)
        {
            if (!isBlinkActive)
            {
                AudioSource.PlayClipAtPoint(blinkSFX, transform.position);
                isBlinkActive = true;
                isBlinkPressed = false;
                hasBlink = false;
                if (moveHorizontal == 0 && moveVertical == 0)
                {
                    blinkDirection = transform.forward;
                }
                else
                {
                    blinkDirection = (transform.right * moveHorizontal + transform.forward * moveVertical).normalized;
                }
            }
            if (currentBlinkStep == numBlinkSteps)
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
        if (isFireballPressed & fireballCountdown <= 0)
        {
            isFireballPressed = false;
            fireballCountdown = fireballCooldown;
            // camera = transform.GetComponent<Camera>();
            // GameObject cam = FindGameObjectWithTag("MainCamera");
            Instantiate(fireball, transform.position + (transform.forward * 1f) + new Vector3(0f, 1.2f, 0f), Camera.main.transform.rotation);
        }

        // applies gravity and moves player
        moveDirection.y -= gravity * Time.deltaTime * finalSpeedMultiplier;
        controller.Move(moveDirection * Time.deltaTime * finalSpeedMultiplier);
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
            FindObjectOfType<LevelManager>().LevelLost();
        }
        slider = GameObject.Find("HealthBar").GetComponent<Slider>();
        slider.value = health;
    }

    void OnTriggerEnter(Collider hit)
    {
        if (hit.gameObject.tag == "Lava")
        {
            AudioSource.PlayClipAtPoint(deathSFX, transform.position);
            FindObjectOfType<LevelManager>().LevelLost();
        }

        if (hit.gameObject.tag == "Soul")
        {
            SoulCollect();
        }

        if (hit.gameObject.tag == "SuperJumpSoul")
        {
            hasSuperJump = true;
            SoulCollect();
        }
    }

    void SoulCollect()
    {
        bulletTime = maxBulletTime;
        hasBlink = true;
        AudioSource.PlayClipAtPoint(soulCollectSFX, transform.position);
    }

    void BulletTime()
    {

        // Slow down time
        if (Input.GetMouseButton(1) && bulletTime > 0)
        {
            // Play timeslowdown SFX
            if (Input.GetMouseButtonDown(1))
            {
                AudioSource.PlayClipAtPoint(timeSlowSFX, transform.position);
            }

            // Reduce timescale if above minTimeScale
            if (Time.timeScale > minTimeScale)
            {
                Time.timeScale -= timeScaleSpeed;
            }
            else
            {
                Time.timeScale = minTimeScale;
            }

            // Reduce bullettime meter
            bulletTime -= bulletTimeMeterPerTick;
        }
        else // Speed up time back to normal
        {
            if (Input.GetMouseButtonUp(1) && bulletTime > 0)
            {
                AudioSource.PlayClipAtPoint(timeSpeedupSFX, transform.position);
            }
            // Return timescale back to normal
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
        Time.fixedDeltaTime = originalFixedDeltaTime * Time.timeScale;
        finalSpeedMultiplier = 1f / Time.timeScale;
        bulletTimeUI.value = bulletTime;
    }
}
