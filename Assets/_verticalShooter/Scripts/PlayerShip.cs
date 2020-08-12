using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : MonoBehaviour
{
    public int maxHealth = 3;

    private int currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mosePos2D = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mosePos2D.x, mosePos2D.y, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision detected: " + collision.gameObject.name);
        GetHit();
    }

    //get hit
    private void GetHit()
    {
        currentHealth--;
        
        BusSystem.General.ShipGotHit(currentHealth, maxHealth);
        StartCoroutine(GetHitCoroutine());
        if(currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        BusSystem.General.ShipGotDead(gameObject);
    }

    IEnumerator GetHitCoroutine()
    {
        float maxDuration = 0.2f;
        float currentDuration = 0f;
        while(currentDuration < maxDuration)
        {
            transform.localScale *= 1.01f;
            currentDuration += 0.01f;
            yield return new WaitForSecondsRealtime(0.01f);
        }
        transform.localScale = Vector3.one;// new Vector(1f,1f,1f);
    }
}
