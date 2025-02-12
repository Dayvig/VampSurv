using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedHitbox : HitboxScript
{
    public float lifeTimer = 0.0f;
    public float duration = 0.0f;
    public override void Reset()
    {
        lifeTimer = 0.0f;
        gameObject.SetActive(true);
    }
    public override void OnTriggerStay2D(Collider2D other)
    {
        
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {

    }

    public override void hitboxUpdate()
    {
        base.hitboxUpdate();
        if (lifeTimer > duration)
        {
            gameObject.SetActive(false);
            HitboxManager.instance.markedForDeathHitboxes.Add(this);
        }
        else
        {
            lifeTimer += Time.deltaTime;
        }
    }
}
