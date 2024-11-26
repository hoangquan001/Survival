using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(ObjectMovement))]
[CanEditMultipleObjects]
public class ObjectMovementEditor : Editor
{
    private SerializedProperty Type;
    private SerializedProperty StartIdx;
    private SerializedProperty Speed;
    private SerializedProperty direction;
    private SerializedProperty Center;
    private SerializedProperty radius;
    private SerializedProperty Points;
    private SerializedProperty Axis;
    private void OnEnable()
    {
        Type = serializedObject.FindProperty("Type");
        StartIdx = serializedObject.FindProperty("StartIdx");
        Center = serializedObject.FindProperty("Center");
        radius = serializedObject.FindProperty("radius");
        Speed = serializedObject.FindProperty("Speed");
        direction = serializedObject.FindProperty("direction");
        Points = serializedObject.FindProperty("Points");
        Axis = serializedObject.FindProperty("Axis");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();

        EditorGUILayout.PropertyField(Type, true);
        if (Type.enumValueIndex == (int)MovementType.Circle)
        {
            EditorGUILayout.PropertyField(Center);
            EditorGUILayout.PropertyField(radius);
            EditorGUILayout.PropertyField(Axis);
        }
        EditorGUILayout.PropertyField(Speed);
        if (Type.enumValueIndex != (int)MovementType.Circle)
        {
            EditorGUILayout.PropertyField(StartIdx);

            EditorGUILayout.PropertyField(direction);
            EditorGUILayout.PropertyField(Points);

        }
        serializedObject.ApplyModifiedProperties();

    }
}

public enum MovementType { Loop, PingPong, OneWay, OneWayPingPong, Random, Circle }
public class ObjectMovement : MonoBehaviour
{
    public void onSerializedPropertyChange(SerializedProperty property)
    {

    }
    // Start is called before the first frame update
    [HideInInspector]

    public MovementType Type = MovementType.Loop;
    [HideInInspector]

    public List<Transform> Points = new List<Transform>();

    [HideInInspector]

    public int StartIdx = 0;
    [HideInInspector]

    public float Speed = 1;
    [HideInInspector]


    public int direction = 1;
    [HideInInspector]


    public Transform Center;

    [HideInInspector]

    public float radius = 1f;

    [HideInInspector]
    public Vector3 Axis = Vector3.up;


    void Start()
    {
        // transform.position = Points[StartIdx].position;
        transform.position = Center.position + Quaternion.LookRotation(Axis) * Vector3.right * radius;

    }
    private void OnDrawGizmos()
    {
        for (int i = 0; i < Points.Count; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(Points[i].position, 0.2f);
        }
        if (Type != MovementType.Circle)
        {
            for (int i = 0; i < Points.Count; i++)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(Points[i].position, Points[(i + 1) % Points.Count].position);
            }

        }
        if (Type == MovementType.Circle)
        {

            Handles.DrawWireDisc(Center.position, Axis, radius);
        }
    }
    bool validIndex(int idx, int count)
    {
        return idx >= 0 && idx < count;
    }
    void CalculateIndex(MovementType type)
    {
        switch (type)
        {
            case MovementType.Loop:
                StartIdx = (StartIdx + 1) % Points.Count;
                break;
            case MovementType.PingPong:
                direction = validIndex(StartIdx + direction, Points.Count) ? direction : -direction;
                StartIdx = (StartIdx + direction) % Points.Count;
                break;
            case MovementType.OneWay:
                StartIdx = (StartIdx + 1) % Points.Count;
                break;
            case MovementType.OneWayPingPong:
                StartIdx = (StartIdx + 1) % Points.Count;
                break;
            case MovementType.Random:
                StartIdx = (StartIdx + 1) % Points.Count;
                break;
            default:
                StartIdx = (StartIdx + 1) % Points.Count;
                break;

        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Type != MovementType.Circle)
        {

            if ((transform.position - Points[StartIdx].position).magnitude < 0.01)
            {
                CalculateIndex(Type);
            }
            transform.position = Vector3.MoveTowards(transform.position, Points[StartIdx].position, Time.deltaTime * Speed);
        }
        else
        {
            // Center.position += Vector3.up * Time.deltaTime * Time.deltaTime;
            // Quaternion angle = Quaternion.LookRotation(Axis);
            // var v = (transform.position - Center.position).normalized;
            // angle.eulerAngles
            // transform.position = Center.position + angle * Vector3.right * radius;

            transform.RotateAround(Center.position, Axis, Time.deltaTime * Speed);
        }

    }
}
