using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGateTrigger : MonoBehaviour
{

    public bool hasTriggered = false;
    public GameObject EnemiesFinalCluster;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            GameObject gate1 = GameObject.FindWithTag("gate1");
            Destroy(gate1, 2f);
            Invoke("SpawnCluster", 3f);
            hasTriggered = true;
        }
    }

    void SpawnCluster()
    {
        EnemiesFinalCluster.SetActive(true);
    }
}
