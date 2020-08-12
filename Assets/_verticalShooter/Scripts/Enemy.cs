using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected PlayerShip playerShip;
    protected float xBoundaryLeft;
    protected float xBoundaryRight;

    public enum EnemyType
    {
        Asteroid = 0,
        Enemy1 = 1
    }
    public EnemyType enemyType;

    //custom functions
    public void Init(float leftBound, float rightBound, PlayerShip playerShip)
    {
        this.playerShip = playerShip;
        xBoundaryLeft = leftBound;
        xBoundaryRight = rightBound;
    }
}
