using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent_Movement_Script : MonoBehaviour
{
    public float moveSpeed;
    public float baseMoveSpeed;
    public float turnSpeed;
    public float baseTurnSpeed;
    public float idleTime;
    public float baseIdleTime;

    private Vector3 targetPosition;
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
        worldBorderObject = GameObject.Find("World_Border");
        transform.position = GetTargetLocation();

        moveSpeed = 1.3f + Random.Range(-0.3f, 0.3f);
        baseMoveSpeed = moveSpeed;
        turnSpeed = 50f + Random.Range(-10f, 10f);
        baseTurnSpeed = turnSpeed;
        idleTime = Random.Range(0.5f, 1.5f);
        baseIdleTime = idleTime;

        ChangeState(MoveState.Idle);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //------Movement States ------------------------------------------------------------------------------------
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

        while (true)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;

            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 2f)
            {
                ChangeState(MoveState.Idle);
            }

            yield return null;
        }
    }

    public Vector3 GetTargetLocation()
    {
        // Get the PolygonCollider2D component from the referenced GameObject
        PolygonCollider2D worldBorder = worldBorderObject.GetComponent<PolygonCollider2D>();

        // Get the points of the polygon
        Vector2[] colliderPoints = worldBorder.points;

        // Find a random point inside the polygon
        Vector2 randomPoint = GetRandomPointInsidePolygon(colliderPoints);

        // Convert the random point to world space (since the collider points are local to the object)
        Vector3 worldPoint = worldBorder.transform.TransformPoint(randomPoint);

        return worldPoint;
    }

    // Method to get a random point inside a polygon
    private Vector2 GetRandomPointInsidePolygon(Vector2[] polygon)
    {
        // Create a random point within the bounds of the polygon's bounding box
        Rect bounds = GetBounds(polygon);
        Vector2 randomPoint;

        // Ensure the random point lies inside the polygon
        do
        {
            randomPoint = new Vector2(
                Random.Range(bounds.xMin, bounds.xMax),
                Random.Range(bounds.yMin, bounds.yMax)
            );
        } while (!IsPointInsidePolygon(randomPoint, polygon));

        return randomPoint;
    }

    // Method to check if a point is inside a polygon (raycasting technique)
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
        // If the number of intersections is odd, the point is inside the polygon
        return (intersectionCount % 2 != 0);
    }

    // Check if a horizontal ray from the point intersects a segment of the polygon
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

    // Get the bounding box of the polygon
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

