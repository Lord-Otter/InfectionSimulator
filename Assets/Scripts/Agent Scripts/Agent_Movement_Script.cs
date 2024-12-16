using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Agent_Movement_Script : MonoBehaviour
{
    public float moveSpeed;
    public float baseMoveSpeed;
    public float turnSpeed;
    public float baseTurnSpeed;
    public float idleTime;
    public float baseIdleTime;

    public bool pathBlocked = false;
    public float rayDistance = 2f;

    private Vector3 targetPosition;
    private Rigidbody2D rb;

    GameObject worldBorderObject;

    private enum MoveState
    {
        Idle,
        Moving,
        PlayerControl
    }
    private MoveState currentMove = MoveState.Idle;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  // Get the Rigidbody2D component
        worldBorderObject = GameObject.Find("World_Border");
        transform.position = GetTargetLocation();

        moveSpeed = Mathf.Round(Random.Range(0.7f, 1.3f) * 100f) / 100f;
        baseMoveSpeed = moveSpeed;
        turnSpeed = Mathf.Round(Random.Range(40f, 60f) * 100f) / 100f;
        baseTurnSpeed = turnSpeed;
        idleTime = Mathf.Round(Random.Range(0.5f, 1.5f) * 100f) / 100f;
        baseIdleTime = idleTime;

        ChangeState(MoveState.Idle);
    }

    // Update is called once per frame
    void Update()
    {

    }

    //--Movement States------------------------------------------------------------------------------------
    private void ChangeState(MoveState newState)
    {
        StopAllCoroutines();
        currentMove = newState;
        switch (currentMove)
        {
            case MoveState.Idle:
                StartCoroutine(IdleStateRoutine());
                break;

            case MoveState.Moving:
                StartCoroutine(MoveToTarget());
                break;
        }
    }

    IEnumerator IdleStateRoutine()
    {
        yield return new WaitForSeconds(idleTime);
        ChangeState(MoveState.Moving);
    }

    IEnumerator MoveToTarget()
    {
        targetPosition = GetTargetLocation();

        var initialTarget = targetPosition;

        while (true)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;

            if(pathBlocked == true)
            {
                RaycastHit2D hitRight = Physics2D.Raycast(transform.position, transform.right, rayDistance);
                RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, -transform.right, rayDistance);

                Debug.DrawRay(transform.position, transform.right * rayDistance, Color.green);
                Debug.DrawRay(transform.position, -transform.right * rayDistance, Color.red); 

                if (hitRight.collider != null && hitLeft.collider != null)
                {
                    if (hitRight.distance < hitLeft.distance)
                    {
                        transform.Rotate(0, 0, -turnSpeed * Time.deltaTime);
                    }
                    else
                    {
                        transform.Rotate(0, 0, turnSpeed * Time.deltaTime);


                    }
                }
                else if (hitRight.collider != null)
                {
                    transform.Rotate(0, 0, turnSpeed * Time.deltaTime);
                }
                else if (hitLeft.collider != null)
                {
                    transform.Rotate(0, 0, -turnSpeed * Time.deltaTime);
                }               
            }
            else
            {
                Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
            }

            Vector2 velocity = Vector2.up;
            float angle = transform.eulerAngles.z;
            velocity = Quaternion.Euler(0, 0, angle) * Vector2.up;
            rb.velocity = velocity;

            if (Vector3.Distance(transform.position, targetPosition) < 2f)
            {
                rb.velocity = Vector2.zero;
                ChangeState(MoveState.Idle);
            }

            yield return null;
        }
    }

    public Vector3 GetTargetLocation()
    {
        PolygonCollider2D worldBorder = worldBorderObject.GetComponent<PolygonCollider2D>();

        Vector2[] colliderPoints = worldBorder.points;

        Vector2 randomPoint = GetRandomPointInsidePolygon(colliderPoints);

        Vector3 worldPoint = worldBorder.transform.TransformPoint(randomPoint);

        return worldPoint;
    }

    private Vector2 GetRandomPointInsidePolygon(Vector2[] polygon)
    {
        Rect bounds = GetBounds(polygon);
        Vector2 randomPoint;

        do
        {
            randomPoint = new Vector2(
                Random.Range(bounds.xMin, bounds.xMax),
                Random.Range(bounds.yMin, bounds.yMax)
            );
        } while (!IsPointInsidePolygon(randomPoint, polygon));

        return randomPoint;
    }

    private bool IsPointInsidePolygon(Vector2 point, Vector2[] polygon)
    {
        int intersectionCount = 0;
        for (int i = 0; i < polygon.Length; i++)
        {
            Vector2 p1 = polygon[i];
            Vector2 p2 = polygon[(i + 1) % polygon.Length];
            if (IsRayIntersectingSegment(point, p1, p2))
            {
                intersectionCount++;
            }
        }
        return (intersectionCount % 2 != 0);
    }

    private bool IsRayIntersectingSegment(Vector2 point, Vector2 p1, Vector2 p2)
    {
        if (p1.y > p2.y)
        {
            Vector2 temp = p1;
            p1 = p2;
            p2 = temp;
        }

        if (point.y == p1.y || point.y == p2.y)
            point.y += 0.0001f;

        if (point.y < p1.y || point.y > p2.y)
            return false;

        float xIntersection = (point.y - p1.y) * (p2.x - p1.x) / (p2.y - p1.y) + p1.x;
        return point.x < xIntersection;
    }

    private Rect GetBounds(Vector2[] polygon)
    {
        float minX = Mathf.Infinity;
        float maxX = -Mathf.Infinity;
        float minY = Mathf.Infinity;
        float maxY = -Mathf.Infinity;

        foreach (Vector2 point in polygon)
        {
            minX = Mathf.Min(minX, point.x);
            maxX = Mathf.Max(maxX, point.x);
            minY = Mathf.Min(minY, point.y);
            maxY = Mathf.Max(maxY, point.y);
        }

        return new Rect(minX, minY, maxX - minX, maxY - minY);
    }
}
