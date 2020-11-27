using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBullet : Bullet
{
    public PlayerItem weaponItem;

    public void SetWeapon(PlayerItem _weapon)
    {
        weaponItem = _weapon;
    }
}
