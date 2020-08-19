using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Collider2D))]
public class Bullet : MonoBehaviour
{
    public int damage = 1;
    public Action<Bullet> OnCleanSelf;
    public virtual void Init(Vector3 direction)
    {
        
    }

    protected virtual void HitEnemy()
    {
        Debug.Log("Hit Enemy");
    }

    protected void CleanSelf()
    {
        OnCleanSelf?.Invoke(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            HitEnemy();
            collision.SendMessage("GetHit", this);
        }
    }
}
