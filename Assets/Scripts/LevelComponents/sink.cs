using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sink : MonoBehaviour
{

    public float speed = 2;
    private bool s = false;
    private Vector3 origin;
    private Vector3 low;
    // Start is called before the first frame update
    void Start()
    {
        origin = transform.position;
        low = new Vector3(origin.x, origin.y-10, origin.z);
    }

    // Update is called once per frame
    void Update()
    {
        if(s){
            transform.position = Vector3.MoveTowards(transform.position, low,speed* Time.deltaTime);
        } else {
            transform.position = Vector3.MoveTowards(transform.position, origin,speed* Time.deltaTime);
        }
        
    }

    private void OnTriggerEnter(Collider collision){
        s = true;
    }

    private void OnTriggerExit(Collider collision){
        s = false;
    }


}
