using System.Collections;
using System.Collections.Generic;
using EditorAttributes;
using UnityEditor;
using UnityEngine;

public class SkillPreview : MonoBehaviour
{
    public SkillType skillType;
    public AnimationClip animationClip;
    public float Radius;
    public float damage;
    public float angle = 0;
    public bool isProjectile = false;
    public GameObject projectile;

    [Button]
    public void PlayAnimation()
    {
        GetComponent<Animator>().Play(animationClip.name);
    }

    /// <summary>
    /// Callback to draw gizmos that are pickable and always drawn.
    /// </summary>
    private void OnDrawGizmos()
    {
        Handles.color = Color.red;
        //Draw cirle by angle and radius
        Vector3 rotatedForward = Quaternion.Euler(0, -angle * 0.5f, 0) * transform.forward;
        Handles.DrawSolidArc(transform.position, Vector3.up, rotatedForward, angle, Radius);
    }

    // Start is called before the first frame update
    
}
