using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Pistol,
    Bow,
    RocketLauncher
}
public enum BulletType
{
    Patrons,
    Arrow,
    Rocket
}

[CreateAssetMenu(menuName = "ShootBOOO/New weapon")]
public class Weapon : Item
{
    public WeaponType weaponType;
    public BulletType bulletType;
    public float damage = 1.0f;
    public float fireRate = 0.5f;
    public float range = 100;
    public float force = 50;
    public int maxAmmo = 150;
    public int magazineAmmo = 10;
    public float reloadTime = 2f;
}
