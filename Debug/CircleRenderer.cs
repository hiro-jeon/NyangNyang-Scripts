using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CircleRenderer : MonoBehaviour
{
    public float radius;
    public float duration = 5f; 
    public int segments = 50;

    private LineRenderer line;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = segments + 1;
        line.useWorldSpace = false;
    }

    public void Initialize(float radius)
    {
        this.radius = radius;
        DrawCircle();
    }

    void DrawCircle()
    {
        float angleStep = 360f / segments;
        for (int i = 0; i <= segments; i++)
        {
            float angle = Mathf.Deg2Rad * (i * angleStep);
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;
            Vector3 pos = new Vector3(x, y, 0);
            line.SetPosition(i, pos);
            // Debug.Log($"Point {i}: {pos}");
            Destroy(gameObject, duration);
        }
    }
}