using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle_Script : MonoBehaviour
{
    private Agent_Movement_Script agentMoveScript;

    void Start()
    {
        agentMoveScript = transform.parent.GetComponent<Agent_Movement_Script>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Nose") || other.CompareTag("Agent"))
        {
            if (agentMoveScript != null)
            {
                Debug.Log("Collision Detected");
                agentMoveScript.pathBlocked = true;
            }
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Nose") || other.CompareTag("Agent"))
        {
            if (agentMoveScript != null)
            {
                Debug.Log("Collision Ended");
                agentMoveScript.pathBlocked = false;
            }
        }
    }
}
