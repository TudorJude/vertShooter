using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip1 : Enemy
{
    enum EnemyShipStates
    {
        spawn,
        goToPlayer,
        tryKillPlayer
    }

    public float distanceToPlayer = 10f;
    public float speed = 3.5f;

    private EnemyShipStates shipStates = EnemyShipStates.spawn;    

    private PlayerShip playerShip;

    private float xBoundaryLeft;
    private float xBoundaryRight;

    private bool goingRight = false;

    private void Awake()
    {
        GameObject shipObject = GameObject.FindGameObjectWithTag("Player");
        playerShip = shipObject.GetComponent<PlayerShip>();

        GameObject[] boundaries = GameObject.FindGameObjectsWithTag("Boundary");
        if(boundaries[0].transform.position.x < boundaries[1].transform.position.x)
        {
            xBoundaryLeft = boundaries[0].transform.position.x;
            xBoundaryRight = boundaries[1].transform.position.x;
        }
        else
        {
            xBoundaryLeft = boundaries[1].transform.position.x;
            xBoundaryRight = boundaries[0].transform.position.x;
        }
    }

    private void Start()
    {
        shipStates = EnemyShipStates.goToPlayer;
    }

    // Update is called once per frame
    private void Update()
    {
        switch(shipStates)
        {
            case EnemyShipStates.goToPlayer:
                {
                    float currentDistance = Vector3.Distance(transform.position, playerShip.transform.position);
                    if(currentDistance <= distanceToPlayer)
                    {
                        shipStates = EnemyShipStates.tryKillPlayer;
                    }
                    else
                    {
                        transform.Translate(0f, -speed * Time.deltaTime, 0f);
                    }
                }break;
            case EnemyShipStates.tryKillPlayer:
                {
                    if(transform.position.x < xBoundaryLeft)
                    {
                        goingRight = true;
                    }
                    if (transform.position.x > xBoundaryRight)
                    {
                        goingRight = false;
                    }
                    if(goingRight)
                    {
                        transform.Translate(speed * Time.deltaTime, 0f, 0f);
                    }
                    else
                    {
                        transform.Translate(-speed * Time.deltaTime, 0f, 0f);
                    }
                }
                break;
        }
    }
}
