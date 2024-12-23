using DesignPatterns.Utilities;
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
    public GameObject gunModelPrefab;
    public ParticleSystem hitEffectPrefab;
    public Vector3 spanwPosition;
    public Vector3 spanwRotation;
    public GameObject bulletPrefab;
    public ParticleSystem flashEffectPrefab;

    public float Spread = 0.01f;
    public float FireRate = 0.1f;
    public TrailRenderer trailRenderer;
    public float trailTime = 0.1f;
    public DamageScriptableObject damageConfig;
    public AmmoScriptableObject ammoConfig;
    private GameObject gunModel;
    private PlayerController entityController;
    private Transform barrel;
    private float lastShotTime = 0f;
    private ParticleSystem hitEffect;
    private ParticleSystem flashEffect;

    public bool IsReloading
    {
        get => ammoConfig.IsReloading();
    }


    public void Spawn(Transform parrent, PlayerController monoBehaviour)
    {
        entityController = monoBehaviour;
        gunModel = Instantiate<GameObject>(gunModelPrefab);

        gunModel.transform.SetParent(parrent,false);
        gunModel.transform.localPosition = spanwPosition;
        gunModel.transform.localEulerAngles = spanwRotation;
        barrel = gunModel.transform.Find("Barrel");
        lastShotTime = 0;
        hitEffect = Instantiate<ParticleSystem>(hitEffectPrefab);
        hitEffect.Stop();
        flashEffect = Instantiate<ParticleSystem>(flashEffectPrefab,Vector3.zero,Quaternion.identity, barrel);

        flashEffect.Stop();

        ammoConfig.Reload();
    }

    public void TryShot()
    {
        if(Time.time > lastShotTime + FireRate && !IsReloading)
        {
            lastShotTime = Time.time;
            bool canShoot = ammoConfig.TryShoot();

            if(canShoot)
            {
                Shot();

            }
            else
            {
                ammoConfig.Reload();
                entityController.animator.CrossFade("Reload",1,1);
            }

          
        }
    }

    private void Shot()
    {
        Vector3 randomSpread = new Vector3(Random.Range(-Spread, Spread), Random.Range(-Spread, Spread), Random.Range(-Spread, Spread));
        Vector3 direction = barrel.forward + randomSpread;
        flashEffect.transform.localPosition = Vector3.zero;
        flashEffect.Emit(1);
        if (Physics.Raycast(barrel.position, direction, out RaycastHit hit, 100, 1))
        {
            if (hitEffect != null)
            {
                hitEffect.transform.position = hit.point;
                hitEffect.transform.forward = hit.normal;
                hitEffect.Emit(1);

            }
            var msg = new Damageable.DamageMessage();
            msg.damager = entityController;
            msg.damageValue = damageConfig.GetDamage(10f);
            hit.collider.GetComponent<Damageable>()?.ApplyDamage(msg);
        }
    }







}