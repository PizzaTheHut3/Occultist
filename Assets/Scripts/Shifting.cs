using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shifting : MonoBehaviour
{

    public float shift = 3f;
    private Vector3 origin;

    // Start is called before the first frame update
    void Start()
    {
        origin = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = origin + new Vector3(Mathf.Sin(Time.time)*shift, 0, 0);
    }
}
