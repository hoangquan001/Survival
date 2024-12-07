
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DG.Tweening;
using EditorAttributes;
using UnityEditor;
using UnityEngine;


public class MovePlatform : Engine
{
    // [SerializeField]
    protected CharacterController otherCC;

    private void OnTriggerEnter(Collider other)
    {
        otherCC = other.GetComponent<CharacterController>();
    }
    private void OnTriggerExit(Collider other)
    {
        otherCC = null;
    }
}
