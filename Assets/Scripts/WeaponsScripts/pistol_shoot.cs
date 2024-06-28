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
        float impact, 
        float range, 
        float fireRate,
        bool[] modes,
        GameObject cameraDrag,
        GameObject weaponDrag) : base(weaponHolder, bPos, damage, impact, range, fireRate, modes, cameraDrag, weaponDrag)
    {
        
    }
}

public class pistol_shoot : MonoBehaviour
{
    
    public float damage = 10f;
    public float range = 100f;

    public GameObject weaponHolder;
    public GameObject weaponDrag;
    public GameObject cameraDrag;
    
    private IGunShot _shot;

    private void Start()
    {
        GameObject barrel = new GameObject();
        barrel.transform.parent = weaponDrag.transform;
        barrel.transform.localPosition = new Vector3(0.112999998f, 0.112499997f, 0.744000018f);
        
        _shot = new Pistol(
            weaponHolder,
            new [] {barrel},
            damage,
            0.05f,
            range,
            0.05f,
            new [] { true, true },
            cameraDrag,
            weaponDrag);
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

    private void FixedUpdate()
    {
        weaponDrag.transform.localPosition = Vector3.Slerp(weaponDrag.transform.localPosition, Vector3.zero, 0.5f);
        weaponDrag.transform.localRotation = Quaternion.Slerp(weaponDrag.transform.localRotation, Quaternion.identity, 0.5f);
        cameraDrag.transform.localPosition = Vector3.Slerp(weaponDrag.transform.localPosition, Vector3.zero, 0.5f);
        cameraDrag.transform.localRotation = Quaternion.Slerp(weaponDrag.transform.localRotation, Quaternion.identity, 0.5f);
    }
}
