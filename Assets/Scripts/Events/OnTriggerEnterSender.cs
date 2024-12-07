
using System;
using EditorAttributes;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;


public class OnTriggerEnterSender : MonoEventSender
{
    public LayerMask layer;
   
    private void OnTriggerEnter(Collider other) {
        if (layer == (layer | (1 << other.gameObject.layer)))
        {
            SendEvent();
        }
    }


}

