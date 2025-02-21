using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpOrb : MonoBehaviour
{

    public Vector2 destination;
    public Vector2 initialPos;
    public int collectionStage = 0;
    public float expValue;
    public bool spawning = false;
    public void Collect()
    {
        initialPos = transform.position;
        destination = transform.position + Random.insideUnitSphere.normalized * 2f;
        collectionStage = 1;
    }

    public void FixedUpdate()
    {
        switch (collectionStage)
        {
            case 0: //uncollected
                break;
            case 1: //is collecting, first animation stage
                transform.position = Vector2.Lerp(transform.position, destination, Time.deltaTime *4f);
                if (Vector2.Distance(transform.position, destination) <= 0.1f)
                {
                    collectionStage = 2;
                    initialPos = transform.position;
                }
                break;
            case 2: //leaping towards player
                transform.position = Vector2.Lerp(transform.position, EnemyManager.instance.player.transform.position, Time.deltaTime *4f);
                if (Vector2.Distance(transform.position, EnemyManager.instance.player.transform.position) <= 0.5f)
                {
                    collectionStage = 3;
                    EnemyManager.instance.player.CollectOrb(this);
                    ProgressionManager.instance.markedForDeathOrbs.Add(this);
                    gameObject.SetActive(false);
                }
                break;
        }
    }

    public void Spawn(Vector2 location, float value)
    {
        transform.position = location;
        spawning = true;
        SetupOrb(value);
        gameObject.SetActive(true);
    }

    public void SetupOrb(float value)
    {
        collectionStage = 0;
        expValue = value;
    }
}
