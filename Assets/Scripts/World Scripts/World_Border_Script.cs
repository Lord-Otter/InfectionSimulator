using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class World_Border_Script : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private PolygonCollider2D polygonCollider;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        polygonCollider = GetComponent<PolygonCollider2D>();

        // Ensure the line renderer can handle the number of points in the polygon
        lineRenderer.positionCount = polygonCollider.points.Length + 1; // +1 to close the loop
        DrawBorder();
    }

    void DrawBorder()
    {
        // Get the points of the polygon collider
        Vector2[] points = polygonCollider.points;

        // Convert the 2D points to 3D points (assuming z = 0 for 2D)
        for (int i = 0; i < points.Length; i++)
        {
            // Convert each 2D point into a 3D point and assign to LineRenderer
            lineRenderer.SetPosition(i, new Vector3(points[i].x, points[i].y, 0));
        }

        // Close the loop by setting the last point to the first point
        lineRenderer.SetPosition(points.Length, new Vector3(points[0].x, points[0].y, 0));
    }
}
