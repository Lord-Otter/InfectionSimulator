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

        lineRenderer.positionCount = polygonCollider.points.Length + 1;
        DrawBorder();
    }

    void DrawBorder()
    {
        Vector2[] points = polygonCollider.points;

        for (int i = 0; i < points.Length; i++)
        {
            lineRenderer.SetPosition(i, new Vector3(points[i].x, points[i].y, 0));
        }

        lineRenderer.SetPosition(points.Length, new Vector3(points[0].x, points[0].y, 0));
    }
}
