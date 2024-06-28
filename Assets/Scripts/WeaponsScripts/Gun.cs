using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public interface IGunShot
{
    public GameObject[] BarrelPos { get; set; }
    public int CurrentBarrel { get; set; }
    void Shot(string type);
    public void ChangeMode();
}

public interface IGunHit
{
    void Hit();
}

public interface IGunReload
{
    void Reload();
}

public abstract class GunShotImplementation : IGunShot
{
    public GameObject WeaponHolder { get; set; }
    public GameObject[] BarrelPos { get; set; }
    public int CurrentBarrel { get; set; }
    private readonly float _damage;
    private readonly float _impact;
    private readonly float _range;
    private readonly float _fireRate;
    private readonly string[] _modes;
    private int _currentMode = 0;
    private float _shootTimer = Time.realtimeSinceStartup;

    private GameObject _weaponDrag;
    private GameObject _cameraDrag;
    
    protected GunShotImplementation(
        GameObject weaponHolder,
        GameObject[] bPos, 
        float damage, 
        float impact, 
        float range, 
        float fireRate,
        bool[] modes,
        GameObject cameraDrag,
        GameObject weaponDrag)
    {
        WeaponHolder = weaponHolder;
        BarrelPos = bPos;
        _damage = damage;
        _impact = impact;
        _range = range;
        _fireRate = fireRate;
        _cameraDrag = cameraDrag;
        _weaponDrag = weaponDrag;
        _modes = Array.Empty<string>();
        if (modes[0]) _modes = _modes.Append("single").ToArray();
        if (modes[1]) _modes = _modes.Append("auto").ToArray();
        if (_modes.Length == 0) _modes = new[] { "single", "auto" };

    }

    public void ChangeMode()
    {
        _currentMode++;
        if (_currentMode >= _modes.Length) _currentMode = 0;
    }
    
    public void Shot(string type)
    {
        // Разрешаем стрелять одиночными при нажатии, а в авто-режиме насрать
        if ((type == "just_pressed" && _modes[_currentMode] == "single") || _modes[_currentMode] != "single")
        {
            // Таймер на стрельбу, разрешаем выстрел, если
            // время с предыдущего выстрела + скорость стрельбы стало меньше текущего
            if (_shootTimer + _fireRate < Time.realtimeSinceStartup)
            {
                GameObject trace = new GameObject();
                trace.AddComponent<LineRenderer>();
                trace.AddComponent<TraceDrawerDestroyer>();
                
                _weaponDrag.transform.position += new Vector3((Random.value-0.5f) * _impact * 2, Random.value * _impact * 2, 0);;
                _cameraDrag.transform.localRotation = Quaternion.Euler(Random.value * _impact * 100f, (Random.value-0.5f) * _impact * 20f, 0);
                
                if (Physics.Raycast(BarrelPos[CurrentBarrel].transform.position,
                                    WeaponHolder.transform.forward, out var hit, _range))
                {
                    testDamageScript test = hit.transform.GetComponent<testDamageScript>();
                    if (test != null) test.TakenDamage(_damage);
                    
                    trace.GetComponent<LineRenderer>().SetPositions(new Vector3[]{BarrelPos[CurrentBarrel].transform.position, hit.point});
                    
                }
                else
                {
                    trace.GetComponent<LineRenderer>().SetPositions(new Vector3[]{BarrelPos[CurrentBarrel].transform.position, BarrelPos[CurrentBarrel].transform.forward * _range});
                }
                
                
                // Выбираем следующий ствол
                NextBarrel();
                // "Обнуляем" таймер
                _shootTimer = Time.realtimeSinceStartup;
            }
        }
    }

    /// <summary>
    /// Функция, которая определяет какой ствол будет следующим в случае многоствольного оружия
    /// </summary>
    protected virtual void NextBarrel()
    {
        
    }
}


