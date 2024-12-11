using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


public class CircleBoundary : Boundary
{
    public float Radius = 10;
    public override bool IsInside(Vector3 position)
    {
        return Vector3.Distance(transform.position, position) < Radius;
    }

    void OnDrawGizmosSelected()
    {
        // points = points.Select(p => transform.TransformPoint(p)).ToList();
        Handles.color = new Color(1.0f, 0.0f, 0.0f, 0.2f);
        Handles.DrawSolidDisc(transform.position, Vector3.up, Radius);
    }
    public override Vector3 RandomPointInBoundary()
    {
        var point = Random.insideUnitCircle* Radius;
        float y = transform.position.y;
        if(Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, Radius*2f, 1, QueryTriggerInteraction.Ignore))
        {
            y = hit.point.y;
        }

        return new Vector3(transform.position.x + point.x, y, transform.position.z + point.y);
    }
}
