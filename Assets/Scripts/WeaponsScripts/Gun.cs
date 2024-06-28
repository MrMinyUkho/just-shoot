using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGunShot
{
    public GameObject[] BarrelPos { get; set; }
    public int CurrentBarrel { get; set; }
    void shot();
}

public interface IGunHit
{
    void hit();
}

public interface IGunReload
{
    void reload();
}

public abstract class IGunShotImplementation : IGunShot
{
    public GameObject[] BarrelPos { get; set; }
    public int CurrentBarrel { get; set; }
    private readonly float _damage;
    private readonly float _recoil;
    private readonly float _range;
    private readonly float _firerate;
    private readonly string[] _modes;
    
    protected IGunShotImplementation(
        GameObject[] bPos, 
        float damage, 
        float recoil, 
        float range, 
        float firerate,
        bool[] single_semi_auto)
    {
        BarrelPos = bPos;
        this._damage = damage;
        this._recoil = recoil;
        this._range = range;
        this._firerate = firerate;
        _modes = Array.Empty<string>();
        
    }
    
    public void shot()
    {
        RaycastHit hit;
        if (Physics.Raycast(BarrelPos[CurrentBarrel].transform.position, 
                            BarrelPos[CurrentBarrel].transform.forward, out hit, _range))
        {
            Debug.Log(hit. transform.name);
            
            testDamageScript test = hit.transform.GetComponent<testDamageScript>();
            if (test != null)
            {
                test.TakenDamage(_damage);
            }
        }
        NextBarrel();
    }

    protected virtual void NextBarrel()
    {
        
    }
}


