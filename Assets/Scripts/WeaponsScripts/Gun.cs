using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

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
    private readonly float _recoil;
    private readonly float _range;
    private readonly float _fireRate;
    private readonly string[] _modes;
    private int _currentMode = 0;
    private float _shootTimer = Time.realtimeSinceStartup;
    
    protected GunShotImplementation(
        GameObject weaponHolder,
        GameObject[] bPos, 
        float damage, 
        float recoil, 
        float range, 
        float fireRate,
        bool[] modes)
    {
        WeaponHolder = weaponHolder;
        BarrelPos = bPos;
        _damage = damage;
        _recoil = recoil;
        _range = range;
        _fireRate = fireRate;
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
            Debug.Log("Piu");
            // Таймер на стрельбу, разрешаем выстрел, если
            // время с предыдущего выстрела + скорость стрельбы стало меньше текущего
            if (_shootTimer + _fireRate < Time.realtimeSinceStartup)
            {
                if (Physics.Raycast(BarrelPos[CurrentBarrel].transform.position,
                                    WeaponHolder.transform.forward, out var hit, _range))
                {
                    testDamageScript test = hit.transform.GetComponent<testDamageScript>();
                    if (test != null) test.TakenDamage(_damage);

                    GameObject trace = new GameObject();
                    
                    trace.AddComponent<LineRenderer>();
                    trace.GetComponent<LineRenderer>().SetPositions(new Vector3[]{BarrelPos[CurrentBarrel].transform.position, hit.point});
                    
                    trace.GetComponent<LineRenderer>().enabled = true;

                    trace.AddComponent<TraceDrawerDestroyer>();
                    
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


