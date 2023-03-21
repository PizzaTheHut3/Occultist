using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndeadHeavyBehavior : MonoBehaviour
{
    public GameObject player;
    public float speed = 3f;
    public int health = 30;
    public GameObject soul;

    Animator anim;
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
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, player.transform.position) < 2f)
        {
            anim.SetBool("Shoot_b", true);
        }
        else
        {
            anim.SetBool("Shoot_b", false);
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
