using System;
using UnityEngine;



[CreateAssetMenu(fileName = "Ammo Config", menuName = "ScriptableObjects/Ammo", order = 1)]
public class AmmoScriptableObject : ScriptableObject
{
    public int maxAmmo;
    public float ReloadTime;
    private float lastReloadTime = 0;
    private int currentAmmo;


    public void Reload()
    {
        currentAmmo = maxAmmo;
        lastReloadTime = Time.time;
    }

    public bool IsReloading()
    {
        return Time.time < lastReloadTime + ReloadTime;
    }


    public bool TryShoot()
    {
        if (currentAmmo > 0 && !IsReloading())
        {
            currentAmmo--;
            return true;
        }

        return false;
    }

}