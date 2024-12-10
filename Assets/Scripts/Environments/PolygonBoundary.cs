using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


public class PolygonBoundary : MonoBehaviour
{
    public List<Vector3> points = new List<Vector3>();
    
    public List<Vector3> worldPoints { get { return points.Select(p => transform.TransformPoint(p)).ToList();}  }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void OnDrawGizmos()
    {
        // points = points.Select(p => transform.TransformPoint(p)).ToList();
        Handles.color = new Color(1.0f, 0.0f, 0.0f, 0.2f);
        
        Handles.DrawAAConvexPolygon(worldPoints.ToArray());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


[CustomEditor(typeof(PolygonBoundary))]
public class BoundaryEditor : Editor
{
    /// <summary>
    /// Enables the Editor to handle an event in the scene view.
    /// </summary>
    private void OnSceneGUI()
    {
        var boundary = target as PolygonBoundary;
        for (int i = 0; i < boundary.worldPoints.Count; i++)
        {
            var point = boundary.worldPoints[i];
            Handles.color = Color.red;
            Handles.Label(point, "Point " + i);
            point = Handles.PositionHandle(point, Quaternion.identity);
            Handles.color = Color.yellow;
            Handles.DrawDottedLine(point, point + Vector3.up, 5);
            boundary.points[i] = boundary.transform.InverseTransformPoint(point);
        }
        Handles.Label(boundary.transform.position, "Root");
    }
}