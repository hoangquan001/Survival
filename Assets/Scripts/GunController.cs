using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    // Start is called before the first frame update
    public float force = 100f;
    public GameObject bulletPrefab;

    Animator m_Animator;
    void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }

    public PlayerController host;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Shot()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletPrefab.transform.position, bulletPrefab.transform.rotation);
        bullet.GetComponent<Rigidbody>().AddForce(host.transform.forward * force, ForceMode.Force);
        m_Animator.SetTrigger("Shot");
        bullet.AddComponent<XAutoDestroy>();

    }
}
