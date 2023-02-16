using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndeadHeavyBehavior : MonoBehaviour
{
    public GameObject player;
    public float speed = 3f;

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
}
