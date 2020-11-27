using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 2.0f;
    public float currentLifeTime = 0;

    private void Start()
    {
        StartCoroutine(BulletLife());
    }
    
    public virtual IEnumerator BulletLife()
    {
        while (currentLifeTime < lifeTime)
        {
            currentLifeTime += 1f;
            yield return new WaitForSeconds(1f);
        }

        Destroy(this.gameObject);
    }
}
