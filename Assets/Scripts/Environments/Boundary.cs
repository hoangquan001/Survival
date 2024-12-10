using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Boundary : MonoBehaviour
{
    public virtual bool IsInside(Vector3 position)
    {   
        return true;
    }
}
