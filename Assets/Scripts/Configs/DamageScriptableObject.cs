using System;
using UnityEngine;



[CreateAssetMenu(fileName = "Damage Config", menuName = "ScriptableObjects/Damage", order = 1)]
public class DamageScriptableObject : ScriptableObject
{
    public float MinDamage = 10;
    public float MaxDamage = 15;

    public float GetDamage(float distance)
    {
        return UnityEngine.Random.Range(MinDamage, Mathf.Lerp(MinDamage, MaxDamage, 10/distance));
    }

}