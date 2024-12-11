using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SkillType
{
    None,
    Melee,
    Ranged
}
[CreateAssetMenu(fileName = "NewSkill", menuName = "ScriptableObjects/NewSkill")]
public class SkillData : ScriptableObject
{
    public SkillType skillType;
    public AnimationClip animationClip;
    public float damage;
    public float angle = 0;
    public bool isProjectile = false;
    public GameObject projectile;

    // Start is called before the first frame update
    
}
