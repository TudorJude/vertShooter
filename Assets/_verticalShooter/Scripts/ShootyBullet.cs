using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootyBullet : Bullet
{
    public float speed;
    public float lifeTime = 3f;
    private Vector3 normalizedDirection;
    private float lifeTimeCounter;

    public override void Init(Vector3 direction)
    {
        this.normalizedDirection = direction.normalized;
        lifeTimeCounter = 0f;
    }

    protected override void HitEnemy()
    {
        base.HitEnemy();
        //add explosion
        CleanSelf();
    }

    private void Update()
    {
        transform.Translate(normalizedDirection * speed * Time.deltaTime);
        lifeTimeCounter += Time.deltaTime;
        if(lifeTimeCounter > lifeTime)
        {
            CleanSelf();
        }
    }
}
