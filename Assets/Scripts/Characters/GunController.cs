using System.Collections;
using EditorAttributes;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;

public class GunController : MonoBehaviour
{
    public MonoEventReceiver eventReceiver;
    public ParticleSystem hitEffect;
    public Rig bodyAimRig;
    public Transform gunTransform;
    public GameObject bulletPrefab;
    public float bulletSpeed = 20f;
    public float fireRate = 0.5f;

    public Transform gunParent;

    private float nextFireTime = 0f;

    GunScriptableObject gunScriptableObject;

    private void Start()
    {
        eventReceiver.AddListener(EventDefine.StateChange, OnStateChange);
    }
    void OnStateChange(BaseEventArgs args)
    {
        StateChangeEventArgs stateChangeArgs = (StateChangeEventArgs)args;
        // if(stateChangeArgs.newState == EntityState.Run)
        // {
        //     StartCoroutine(SmoothWeight(bodyAimRig, 0, 0.5f));
        // }
        // else
        // {
        //     StartCoroutine(SmoothWeight(bodyAimRig, 1, 0.5f));
        // }
    }

    private IEnumerator SmoothWeight(Rig rig,float targetWeight, float duration)
    {   
        float startWeight = rig.weight;
        float time = 0;
        while(time < duration)
        {
            rig.weight = Mathf.Lerp(startWeight, targetWeight, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        rig.weight = targetWeight;
    }   

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && Time.time > nextFireTime)
        {
            FireGun();
            nextFireTime = Time.time + fireRate;
        }

    }

    void FireGun()
    {
        Vector3 screenPoint = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = Camera.main.ScreenPointToRay(screenPoint);
        if (Physics.Raycast(ray, out RaycastHit hit, 100))
        {
            hitEffect.transform.position = hit.point;
            hitEffect.transform.forward = hit.normal;
            hitEffect.Emit(1);
            Vector3 direction = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            transform.LookAt(direction); 
        }
    }

    [Button("Setparent")]
    public void Setparent()
    {
        gunTransform.SetParent(gunParent);

    }

}