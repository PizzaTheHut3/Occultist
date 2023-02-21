using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public float speed = 5;
    public GameObject liftTo;
    private bool lift = false;
   
    void Start()
    {
        
    }

    
    void Update()
    {
        
        if (lift){
            transform.position = Vector3.MoveTowards(transform.position, liftTo.transform.position,speed* Time.deltaTime);
        }

         
    }

    private void OnTriggerEnter(Collider collision){
        if (collision.gameObject.CompareTag("Player")){
            lift = true;
        }
    }
}
