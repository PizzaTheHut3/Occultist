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
        transform.LookAt(player.transform);
        if (Vector3.Distance(transform.position, player.transform.position) > 15f)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
        else
        {
            if (cooldown <= 0)
            {
                Instantiate(projectile, firePoint.transform.position + transform.forward, transform.rotation);
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
}
