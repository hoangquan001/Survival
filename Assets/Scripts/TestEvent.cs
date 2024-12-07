using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEvent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Test());
        GlobalEventManager.Instance.AddListener(EventDefine.EntityDied, (args) => { Debug.Log("EntityDied"); }, this.gameObject);
    }

    IEnumerator Test()
    {
        
        yield return new WaitForSeconds(5f);
        EntityDeathEventArgs e = new EntityDeathEventArgs();
        e.Sender = this.gameObject;
        GlobalEventManager.Instance.Fire(EventDefine.EntityDied, e);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
