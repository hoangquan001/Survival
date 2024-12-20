using UnityEngine;
using UnityEngine.PlayerLoop;



[CreateAssetMenu(fileName = "Trail Config", menuName = "ScriptableObjects/TrailGun", order = 1)]
public class TrailGunScriptableObject : ScriptableObject
{

}

[CreateAssetMenu(fileName = "Gun Config", menuName = "ScriptableObjects/Gun", order = 1)]
public class GunScriptableObject : ScriptableObject
{
    public GunType gunType;
    public GameObject ModelPrefab;
    public ParticleSystem hitEffectPrefab;
    public Vector3 SpanwPosition;
    public Vector3 SpanwRotation;
    public GameObject bulletPrefab;
    public ParticleSystem flashEffectPrefab;
    private GameObject Model;

    private MonoBehaviour activeMonoBehaviour;
    private Transform barrel;
    private float lastShotTime = 0f;

    public float FireRate = 0.1f;
    private ParticleSystem hitEffect;
    private ParticleSystem flashEffect;


    public float Spread = 0.01f;


    public TrailRenderer trailRenderer;
    public float trailTime = 0.1f;

    public void Spawn(Transform parrent, MonoBehaviour monoBehaviour)
    {
        activeMonoBehaviour = monoBehaviour;
        Model = Instantiate<GameObject>(ModelPrefab);
        Model.transform.SetParent(parrent,false);
        Model.transform.localPosition = SpanwPosition;
        Model.transform.localEulerAngles = SpanwRotation;
        barrel = Model.transform.Find("Barrel");
        lastShotTime = 0;
        hitEffect = Instantiate<ParticleSystem>(hitEffectPrefab);
        hitEffect.Stop();
        flashEffect = Instantiate<ParticleSystem>(flashEffectPrefab,Vector3.zero,Quaternion.identity, barrel);

        flashEffect.Stop();
    }

    public void TryShot()
    {
        if(Time.time > lastShotTime + FireRate)
        {
            lastShotTime = Time.time;

            Vector3 randomSpread = new Vector3(Random.Range(-Spread, Spread), Random.Range(-Spread, Spread), Random.Range(-Spread, Spread));
            Vector3 direction = barrel.forward + randomSpread;
            flashEffect.transform.localPosition = Vector3.zero;
            flashEffect.Emit(1);
            if (Physics.Raycast(barrel.position, direction, out RaycastHit hit, 100, 1))
            {
                if(hitEffect != null)
                {
                    hitEffect.transform.position = hit.point;
                    hitEffect.transform.forward = hit.normal;
                    hitEffect.Emit(1);

                }
            }
        }
    }



}