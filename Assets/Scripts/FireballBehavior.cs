using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballBehavior : MonoBehaviour
{
    Animator animator;
    Renderer renderer;
    public Color startColor;
    public Color endColor;
    public float speed = 10f;
    private bool die = false;
    private float deathTimer = 1f;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        if(die) {
            renderer.material.color = Color.Lerp(endColor, startColor, deathTimer);
            deathTimer -= 0.03f;
            if (deathTimer <= 0.1f) {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag != "Player" && other.tag != "Fireball") {
            die = true;
            animator.SetTrigger("FireballDie");
            speed = 0f;
        }
    }
}
