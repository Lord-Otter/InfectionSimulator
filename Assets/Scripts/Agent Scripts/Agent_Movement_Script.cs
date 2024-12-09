using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent_Movement_Script : MonoBehaviour
{
    public float moveSpeed;
    public float defaultMoveSpeed;
    public float idleTime;
    public float defaultIdleTime;
    private Vector3 targetPosition;

    private enum MoveState
    {
        Idle,
        Moving
    }
    private MoveState currentMove = MoveState.Idle;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 1.3f + Random.Range(-0.3f, 0.3f);
        defaultMoveSpeed = moveSpeed;
        idleTime = Random.Range(0.5f, 1.5f);
        defaultIdleTime = idleTime;

        transform.position = GetTargetLocation();
       
        ChangeMove(MoveState.Idle);
    }

    // Update is called once per frame
    void Update()
    {

    }

    //------Movement States ------------------------------------------------------------------------------------
    private void ChangeMove(MoveState newState)
    {
        StopAllCoroutines();
        currentMove = newState;
        switch (currentMove)
        {
            case MoveState.Idle:
                StartCoroutine(IdleStateRoutine());
                Debug.Log("Agent is idle");
                break;

            case MoveState.Moving:
                StartCoroutine(MoveToTarget());
                Debug.Log("Agent is moving");
                break;
        }
    }

    IEnumerator IdleStateRoutine()
    {
        yield return new WaitForSeconds(idleTime);
        ChangeMove(MoveState.Moving);
    }

    IEnumerator MoveToTarget()
    {
        targetPosition = GetTargetLocation();

        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                ChangeMove(MoveState.Idle);
            }

            yield return null;
        }
    }

    Vector3 GetTargetLocation()
    {
        Camera camera = Camera.main;
        Vector3 bottomLeft = camera.ViewportToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));
        Vector3 topRight = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));

        float randomX = Random.Range(bottomLeft.x, topRight.x);
        float randomY = Random.Range(bottomLeft.y, topRight.y);

        return new Vector3(randomX, randomY, 0);
    }
}

