using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Script : MonoBehaviour
{
    public int numberOfAgents;
    public float spawnDelay;

    public GameObject Agent;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnAgent());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnAgent()
    {
        for (int i = 1; i <= numberOfAgents; i++)
        {
            Instantiate(Agent);
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
