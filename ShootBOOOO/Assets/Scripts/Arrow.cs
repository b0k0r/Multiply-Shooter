using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : WeaponBullet
{
    bool hitSomething = false;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(BulletLife());
    }

    private void FixedUpdate()
    {
        if (!hitSomething)
        {
            transform.rotation = Quaternion.LookRotation(rb.velocity);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        hitSomething = true;
        rb.constraints = RigidbodyConstraints.FreezeAll;

        GameObject go1 = Instantiate(weaponItem.impactVfx, 
                                     collision.contacts[0].point, 
                                     Quaternion.LookRotation(collision.contacts[0].normal));
        Destroy(go1, 2.0f);

        if (collision.collider.CompareTag("Player"))
        {
            Health enemyHealth = collision.collider.GetComponent<Health>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(weaponItem.item.damage);
            }

            Destroy(gameObject);
        }
    }
}
