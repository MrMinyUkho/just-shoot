using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Pistol : GunShotImplementation
{
    public Pistol(
        GameObject weaponHolder,
        GameObject[] bPos, 
        float damage, 
        float recoil, 
        float range, 
        float fireRate,
        bool[] modes) : base(weaponHolder, bPos, damage, recoil, range, fireRate, modes)
    {
        
    }
}

public class pistol_shoot : MonoBehaviour
{
    
    public float damage = 10f;
    public float range = 100f;

    public GameObject weaponHolder;
    
    private IGunShot _shot;

    private void Start()
    {
        GameObject barrel = new GameObject();
        barrel.transform.parent = weaponHolder.transform;
        barrel.transform.localPosition = new Vector3(0.112999998f, 0.112499997f, 0.744000018f);

        
        _shot = new Pistol(
            weaponHolder,
            new [] {barrel},
            damage,
            0.0f,
            range,
            0.05f,
            new [] { true, true }
            );
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
            _shot.Shot("just_pressed");
        else if(Input.GetButton("Fire1"))
            _shot.Shot("held");

        if (Input.GetButtonDown("FireMode"))
            _shot.ChangeMode();
    }
}
