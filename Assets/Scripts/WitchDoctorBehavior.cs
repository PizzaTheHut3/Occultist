using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchDoctorBehavior : MonoBehaviour
{
    public GameObject player;
    public float speed = 2f;
    public GameObject projectile;
    public GameObject firePoint;
    public float attackCooldown = .9f;
    public int health = 20;

    Animator anim;
    float cooldown = 0f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // if player is too far away, skip interaction
        if (Vector3.Distance(transform.position, player.transform.position) > 50f) {
            return;
        }
        transform.LookAt(player.transform);
        if (Vector3.Distance(transform.position, player.transform.position) > 15f)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
        else
        {
            if (cooldown <= 0)
            {
                Instantiate(projectile, firePoint.transform.position + transform.forward, transform.rotation * Quaternion.Euler(5, 0, 0));
                cooldown = 1f;
                anim.SetBool("Shoot_b", true);
            }
            else
            {
                cooldown -= Time.deltaTime;
                anim.SetBool("Shoot_b", false);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Fireball")
        {
            health -= 10;
        }
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
