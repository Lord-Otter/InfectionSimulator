using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent_Script : MonoBehaviour
{
    public float moveSpeed;
    public float defaultMoveSpeed;
    public float idleTime;
    public float defaultIdleTime;
    private Vector3 targetPosition;

    private SpriteRenderer spriteRenderer;

    private enum HealthState
    {
        Healthy,
        Immune,
        Compromised,
        Infected,
        Symptomatic,
        Dead
    }

    private enum MoveState
    {
        Idle,
        Moving
    }

    private HealthState currentHealth = HealthState.Healthy;
    private MoveState currentMove = MoveState.Idle;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 1.3f + Random.Range(-0.3f, 0.3f);
        defaultMoveSpeed = moveSpeed;
        idleTime = Random.Range(0.5f, 1.5f);
        defaultIdleTime = idleTime;

        transform.position = GetTargetLocation();
        spriteRenderer = GetComponent<SpriteRenderer>();

        ChangeHealth(HealthState.Healthy);
        ChangeMove(MoveState.Idle);
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Change Color of Agent
    void ChangeColor(string hexColor)
    {
        if (ColorUtility.TryParseHtmlString(hexColor, out Color newColor))
        {
            spriteRenderer.color = newColor;
        }
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

//--Health States -------------------------------------------------------------------------------------------------------
    private void ChangeHealth(HealthState newState)
    {
        currentHealth = newState;
        switch (currentHealth)
        {
            case HealthState.Healthy:
                ChangeColor("#00FF00");
                break;

            case HealthState.Immune:
                ChangeColor("#FFFFFF");
                break;

            case HealthState.Compromised:
                ChangeColor("#FFFF00");
                break;

            case HealthState.Infected:
                ChangeColor("#FF8000");
                break;

            case HealthState.Symptomatic:
                ChangeColor("#0000FF");
                break;

            case HealthState.Dead:
                ChangeColor("#000000");
                break;
        }
    }
}

