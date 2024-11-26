using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
public class CirclePlatform : Platform
{
    public Transform Center;

    public float radius = 1f;

    public Vector3 Axis = Vector3.up;

    private void OnDrawGizmosSelected()
    {
        if (Event.current.type == EventType.Repaint)
        {
            UnityEditor.Handles.color = Color.blue;
            UnityEditor.Handles.ArrowHandleCap(0, Center.position, Quaternion.LookRotation(Axis.normalized), radius,
                EventType.Repaint);
        }
        // Handles.ArrowHandleCap(0,Center.position, Center.position + Axis.normalized * radius);
        Handles.color = new Color(1.0f, 0.0f, 0.0f, 0.05f);
        Handles.DrawSolidArc(Center.position, Axis, Quaternion.LookRotation(Axis) * Vector3.right, 360, radius);
        
    }

    public override void FixedUpdateMove()
    {
        var v = transform.position - Center.position;
        Vector3 projectionOnNormal = Vector3.Dot(v, Axis.normalized) * Axis.normalized;
        Vector3 projectionOnPlane = v - projectionOnNormal;
        transform.position = Center.position + projectionOnPlane.normalized * radius;
        transform.RotateAround(Center.position, Axis, Time.fixedDeltaTime * Speed * Direction);

    }
}