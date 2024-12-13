using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle_Script : MonoBehaviour
{
    private Agent_Movement_Script agentMoveScript;

    void Start()
    {
        // Get reference to the Agent_Movement_Script on the parent object
        agentMoveScript = transform.parent.GetComponent<Agent_Movement_Script>();
    }

    // Detect collision with other objects
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Nose") || other.CompareTag("Agent"))
        {
            // Set pathBlocked to true when a collision with "Nose" or "Agent" occurs
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
            // Set pathBlocked to false when the collision ends
            if (agentMoveScript != null)
            {
                Debug.Log("Collision Ended");
                agentMoveScript.pathBlocked = false;
            }
        }
    }
}
