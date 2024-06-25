using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class pistol_shoot : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;

    [SerializeField] private LineRenderer lineRenderer;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            PistolShoot();
        }
    }
    
    void PistolShoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, range))
        {
            Debug.Log(hit. transform.name);
            
            testDamageScript test = hit.transform.GetComponent<testDamageScript>();
            if (test != null)
            {
                test.TakenDamage(damage);
            }
            lineRenderer.enabled = true;
            var firstPosition = transform.position;
            var secondPosition = hit.point;
            lineRenderer.SetPosition(0, firstPosition);
            lineRenderer.SetPosition(1, secondPosition);

        }
    }
}
