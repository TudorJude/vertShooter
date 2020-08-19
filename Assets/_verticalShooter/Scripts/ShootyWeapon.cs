using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootyWeapon : Weapon
{
    public Bullet bulletPrefab;
    public int weaponLevel = 1;
    private ObjectPool<Bullet> bulletPool;

    private List<Bullet> activeBullets;

    public override void Init()
    {
        bulletPool = new ObjectPool<Bullet>(bulletPrefab, 200, 200);
        bulletPool.Init();
        weaponLevel = 1;
        activeBullets = new List<Bullet>(200);
    }

    public void Shoot()
    {
        //number of bullets is 2*(weapon level) - 1
        for(int i = -(2 * weaponLevel - 1) / 2; i <= (2 * weaponLevel - 1) / 2; i++)
        {
            var bullet = bulletPool.Instantiate();
            Vector3 direction = new Vector3(Mathf.Sin(Mathf.Deg2Rad * (i * 10)), Mathf.Cos(Mathf.Deg2Rad * (i * 10)), 0f);
            bullet.transform.position = transform.position;
            bullet.Init(direction);
            activeBullets.Add(bullet);
            bullet.OnCleanSelf = (Bullet bulletToBeCleaned) =>
            {
                activeBullets.Remove(bullet);
                bulletPool.Destroy(bullet);
            };
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            weaponLevel++;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            weaponLevel--;
            if (weaponLevel < 1)
                weaponLevel = 1;
        }
    }
}
