using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentMove : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision){
        collision.transform.SetParent(transform);
    }
    
    private void OnTriggerExit(Collider collision){
        collision.transform.SetParent(null);
    }

}
