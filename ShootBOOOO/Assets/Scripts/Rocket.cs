using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : WeaponBullet
{
    [SerializeField] private GameObject affectedArea;
    public float radiusAffect = 1.0f;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(BulletLife());
    }

    private void OnCollisionEnter(Collision collision)
    {
        Explosion();
    }
    
    public override IEnumerator BulletLife()
    {
        while (currentLifeTime < lifeTime)
        {
            currentLifeTime += 1f;
            yield return new WaitForSeconds(1f);
        }

        Explosion();
    }

    public void Explosion()
    {
        GameObject go = Instantiate(affectedArea, transform.position, Quaternion.identity);
        Collider[] cols = Physics.OverlapSphere(transform.position, radiusAffect);

        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].CompareTag("Player"))
            {
                Health enemyHealth = cols[i].GetComponent<Health>();

                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(weaponItem.item.damage);
                }
            }
        }
        Destroy(this.gameObject);
    }
}
