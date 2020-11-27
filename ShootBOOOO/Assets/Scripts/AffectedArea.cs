using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AffectedArea : Bullet
{
    public float radius = 1f;
    [SerializeField] private GameObject graphics;
    [SerializeField] private float deltaSize = 0.25f;

    private void Start()
    {
        StartCoroutine(BulletLife());
        StartCoroutine(IncreaseSize());
    }

    public override IEnumerator BulletLife()
    {
        while (currentLifeTime < lifeTime)
        {
            currentLifeTime += deltaSize;
            yield return new WaitForSeconds(deltaSize);
        }

        Destroy(this.gameObject);
    }
    private IEnumerator IncreaseSize()
    {
        while (graphics.transform.localScale.x < 2*radius)
        {
            graphics.transform.localScale = new Vector3(graphics.transform.localScale.x + 2 * deltaSize,
                                                        graphics.transform.localScale.y + 2 * deltaSize,
                                                        graphics.transform.localScale.z + 2 * deltaSize);
            yield return null;
        }

    }
}
