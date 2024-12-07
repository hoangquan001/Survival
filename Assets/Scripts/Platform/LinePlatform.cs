
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DG.Tweening;
using EditorAttributes;
using UnityEditor;
using UnityEngine;


public class LinePlatform : MovePlatform
{
    public List<Vector3> Points = new List<Vector3>();

    public float Distance 
    {
        get {
            float distance = 0;
            for (int i = 0; i < Points.Count - 1; i++)
            {
                distance += (Points[i + 1] - Points[i]).magnitude;
            }
            return distance;
        }
    }
    public int StartIdx = 0;

    private void OnEnable()
    {
        transform.position = Points[StartIdx];
    }

    void CalculateIndex(LoopType type)
    {
        switch (type)
        {
            case LoopType.LoopRepeat:
                StartIdx = (StartIdx + Direction) % Points.Count;
                break;
            case LoopType.LoopPingPong:
                // direction = IsValidIndex(StartIdx + direction, Points.Count) ? direction : -direction;
                StartIdx = (StartIdx + Direction) % Points.Count;
                break;
            case LoopType.LoopOnce:
                if (StartIdx + 1 >= Points.Count) IsRunning = false;
                StartIdx = (StartIdx + 1) % Points.Count;
                break;
            default:
                StartIdx = (StartIdx + 1) % Points.Count;
                break;

        }
    }
    public override void FixedUpdateMove()
    {
        if ((transform.position - Points[StartIdx]).magnitude < 0.01)
        {
            CalculateIndex(Type);
        }
        float detalDistance = Distance * (Time.fixedDeltaTime / Duration);
        var next = Vector3.MoveTowards(transform.position, Points[StartIdx], detalDistance);
        Vector3 detalMove = next - transform.position;
        transform.position = next;
        // UnityEngine.Debug.Log(detalMove.magnitude);

        otherCC?.Move(detalMove);
    }

    public void Resetposition()
    {
        transform.position = Points[StartIdx];
    }

}

[CustomEditor(typeof(LinePlatform))]
public class LinePlatformEditor : Editor
{

    void OnSceneGUI()
    {
        var t = target as LinePlatform;
        using (var cc = new EditorGUI.ChangeCheckScope())
        {
            var Points = new List<Vector3>(t.Points);
            for (int i = 0; i < t.Points.Count; i++)
            {
                var p = t.Points[i];
                Handles.color = Color.red;
                var pos = Handles.PositionHandle(p, Quaternion.identity);
                Points[i] = pos;
                Handles.color = Color.yellow;
                Handles.DrawDottedLine(t.Points[i], t.Points[(i + 1) % t.Points.Count], 5);
                // Handles.Label(Vector3.Lerp(start, end, 0.5f), "Distance:" + (end - start).magnitude);
            }

            if (cc.changed)
            {
                Undo.RecordObject(t, "Move Handles");
                t.Points = Points;
            }

        }
    
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(GUILayout.Button("Reset Position"))
        {
            (target as LinePlatform).Resetposition();
        }
    }
}