using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip1 : Enemy
{
    public float speed = 3;
    public float directionChangeDuration = 2f;

    private float directionChangedCounter = 0f;

    private float moveDirection = -1f;

    public override void Init(float leftBound, float rightBound, PlayerShip playerShip)
    {
        base.Init(leftBound, rightBound, playerShip);
        directionChangedCounter = directionChangeDuration / 2;
        moveDirection = -1f;
    }

    private void Update()
    {
        if (isDeactivated) return;
        transform.Translate(moveDirection * speed * Time.deltaTime, 0f, 0f);
        directionChangedCounter += Time.deltaTime;
        if (directionChangedCounter > directionChangeDuration)
        {
            directionChangedCounter = 0f;
            moveDirection *= -1f;
        }
    }
}
