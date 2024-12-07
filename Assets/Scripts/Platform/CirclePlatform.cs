
using EditorAttributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;


[CustomEditor(typeof(CirclePlatform))]
public class CirclePlatformEditor : Editor
{

    void OnSceneGUI()
    {
        var t = target as CirclePlatform;
        using (var cc = new EditorGUI.ChangeCheckScope())
        {

            Handles.color = Color.blue;
            Handles.color = new Color(1.0f, 0.0f, 0.0f, 0.05f);
            Handles.DrawSolidArc(t.Center, t.Axis, Quaternion.LookRotation(t.Axis) * Vector3.right, 360, t.radius);
            var Center = Handles.PositionHandle(t.Center, Quaternion.Euler(t.Rotation));
            Quaternion rotation = Handles.RotationHandle(Quaternion.Euler(t.Rotation), t.Center);
            if (cc.changed)
            {
                t.Center = Center;
                t.Rotation = rotation.eulerAngles;
                t.FixedUpdateMove();
            }
            Handles.color = Color.yellow;
            Handles.Label(t.Center + Quaternion.LookRotation(t.Axis) * Vector3.right * t.radius, "Radius:" + t.radius);
            Handles.DrawDottedLine(t.Center, t.Center + Quaternion.LookRotation(t.Axis) * Vector3.right * t.radius, 5);
        }

    }

}
public class CirclePlatform : MovePlatform
{
    public Vector3 Center;

    public float radius = 1f;

    public Vector3 Axis { 
        get
        {
            return Quaternion.Euler(Rotation) * Vector3.up;
        }
        
    }
    public Vector3 Rotation = Vector3.zero;

    private void OnDrawGizmosSelected()
    {


    }

    public override void FixedUpdateMove()
    {
        var v = transform.position - Center;
        Vector3 projectionOnNormal = Vector3.Dot(v, Axis.normalized) * Axis.normalized;
        Vector3 projectionOnPlane = v - projectionOnNormal;
        transform.position = Center + projectionOnPlane.normalized * radius;
        transform.RotateAround(Center, Axis, (Time.fixedDeltaTime / Duration) * 360 * Direction);

    }
    [Button("Reset Position")]
    public void Resetposition()
    {
        FixedUpdateMove();
    }
}
