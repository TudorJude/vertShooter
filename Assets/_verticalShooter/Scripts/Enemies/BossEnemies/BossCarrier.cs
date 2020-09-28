using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCarrier : MonoBehaviour
{
    public float finalBossPosition = 5f;
    public float speed = 2;

    private void Awake()
    {
        StartCoroutine(StartSequence());
    }

    //coroutines
    private IEnumerator StartSequence()
    {
        while(transform.position.y > finalBossPosition)
        {
            transform.Translate(0, Time.deltaTime * -speed, 0f);
            yield return null;
        }

        //taunt the player
        BusSystem.General.CallDisplayGenericMessage("Welcome to die!", 3f);
        yield return new WaitForSecondsRealtime(3f);

        //start hurting the player
    }
}
