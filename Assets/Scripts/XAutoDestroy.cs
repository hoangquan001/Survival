using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XAutoDestroy : MonoBehaviour
{

    [Range(0f, 10f)]
    public float destroyTime = 5f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, destroyTime);
    }

    // Update is called once per frame

}
