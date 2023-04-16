using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingBehavior : MonoBehaviour
{
    private GameObject player;
    public GameObject firePoint;
    public float speed = 2f;
    public GameObject projectile;
    float cooldown = 0f;
    public int health = 50;
    public GameObject soul;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player.transform);
        Attack();
    }

    void Attack()
    {
        // if player is too far away, skip interaction
        if (Vector3.Distance(transform.position, player.transform.position) > 50f)
        {
            return;
        }
        transform.LookAt(player.transform);
        if (Vector3.Distance(transform.position, player.transform.position) > 25f)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
        else
        {
            if (cooldown <= 0)
            {
                Instantiate(projectile, firePoint.transform.position + transform.forward, transform.rotation * Quaternion.Euler(10, 0, 0));
                cooldown = .8f;
                //anim.SetBool("Shoot_b", true);
            }
            else
            {
                cooldown -= Time.deltaTime;
                //anim.SetBool("Shoot_b", false);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Lava")
        {
            Die();
        }
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
        FindObjectOfType<LevelManager>().EnemyKilled();
        Destroy(gameObject);
        Instantiate(soul, transform.position + new Vector3(0f, 1f, 0f), transform.rotation);
    }
}
