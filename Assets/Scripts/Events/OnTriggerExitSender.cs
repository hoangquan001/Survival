
using System;
using EditorAttributes;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;


public class OnTriggerExitSender : MonoEventSender
{
    public LayerMask layer;
   
    private void OnTriggerExit(Collider other) {
        if (layer == (layer | (1 << other.gameObject.layer)))
        {
            SendEvent();
        }
    }


}

