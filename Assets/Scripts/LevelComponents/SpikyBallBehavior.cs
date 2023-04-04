using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikyBallBehavior : MonoBehaviour
{
    public float speed = 3f;
    public float distance = 5f;
    public float offset = 0f;
    Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = transform.position;
        newPos.z = startPos.z + (Mathf.Sin((Time.time + offset) * speed) * distance) + distance;

        transform.position = newPos;
    }
}
