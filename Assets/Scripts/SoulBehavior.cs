using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulBehavior : MonoBehaviour
{
    Animator animator;
    Renderer renderer;
    public Color startColor;
    public Color endColor;
    private GameObject player;
    public float detectDistance = 30f;
    public float speed = 5f;
    private float dist = 1f;
    private bool die = false;
    private float deathTimer = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
        player = GameObject.Find("Player");
        animator = GetComponent<Animator>();
        renderer = GetComponent<Renderer>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if(die) {
            renderer.material.color = Color.Lerp(endColor, startColor, deathTimer);
            deathTimer -= 0.03f;
            if (deathTimer <= 0.1f) {
                Destroy(gameObject);
            }
        }

        dist = Vector3.Distance(transform.position, player.transform.position);

        if (dist > detectDistance) {
            return;
        }

        // added negligible value to avoid divide by zero
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime * (1.0f - (dist / detectDistance)));

    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            die = true;
            animator.SetTrigger("Collected");
        }
    }
}
