using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItem : MonoBehaviour
{
    public bool isAvailable = false;
    public Weapon item;
    public GameObject itemGameObject;
    public Transform bulletHolder;
    public GameObject bulletGameObject;
    public GameObject impactVfx;
    public AudioSource gunAudio;

    public int currentAmmo = 0;
    public int maxAmmo = 20;
    public int ammoPerMagazine = 10;

    private void Start()
    {
        StartComplect();
        gunAudio = GetComponent<AudioSource>();
    }

    public void StartComplect()
    {
        maxAmmo = item.maxAmmo / 2;
        ammoPerMagazine = item.magazineAmmo;
        currentAmmo = ammoPerMagazine;
    }

    public void SetAccess()
    {
        isAvailable = true;
    }
}
