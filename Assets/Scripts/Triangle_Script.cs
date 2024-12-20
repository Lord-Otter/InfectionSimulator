using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle_Script : MonoBehaviour
{
    private Agent_Movement_Script agentMoveScript;
    public Camera mainCamera;
    private BoxCollider2D boxCollider;

    void Start()
    {
        agentMoveScript = transform.parent.GetComponent<Agent_Movement_Script>();
        mainCamera = Camera.main;
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        //Check if the Agent is in view of the camera so we can disable the collider to save on performance
        if (!IsInCameraView())
        {
            if (boxCollider != null)
            {
                boxCollider.enabled = false;
            }
            agentMoveScript.pathBlocked = false;
        }
        else
        {
            if (boxCollider != null)
            {
                boxCollider.enabled = true;
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Nose") || other.CompareTag("Agent"))
        {
            if (agentMoveScript != null)
            {
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
                agentMoveScript.pathBlocked = false;
            }
        }
    }

    /*public void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Nose") || other.CompareTag("Agent"))
        {
            if (agentMoveScript != null)
            {
                agentMoveScript.pathBlocked = true;
            }
        }
        else
        {
            if (agentMoveScript != null)
            {
                agentMoveScript.pathBlocked = false;
            }
        }
    }*/

    private bool IsInCameraView()
    {
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);

        return viewportPosition.x >= 0f && viewportPosition.x <= 1f && viewportPosition.y >= 0f && viewportPosition.y <= 1f;
    }
}
